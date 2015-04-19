using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Caros.CI.API;

namespace Caros.CI.Publisher
{
    public class Publisher
    {
        public enum EventTypes { Success, Failure, Info }

        public event PublishEventHandler OnInfo;
        public event PublishEventHandler OnFailure;
        public event PublishProgressEventHandler OnUpdateProgress;
        public event PublishEventHandler OnSuccess;
        public event PublishEventHandler OnFinishedAll;

        public delegate void PublishEventHandler(string message);
        public delegate void PublishProgressEventHandler(float percentage);

        private string _path;
        private Repository _repo;
        private bool _ignoreDirtyRepo;

        private string ZipPackage { get; set; }
        private string OutputPath { get; set; }
        private ReleaseVersion NewRelease { get; set; }

        public Publisher(string solutionPath, bool ignoreDirtyRepo)
        {
            _path = solutionPath;
            _ignoreDirtyRepo = ignoreDirtyRepo;
        }

        public async void Start()
        {
            Info("Checking repository");

            if (await checkRepositoryAsync())
                Success("Repository looks good", 10);
            else
                return;

            Info("Rebuilding solution");

            if (await rebuildReleaseAsync())
                Success("Built release", 20);
            else
                return;

            Info("Updating version info");

            if (await stampVersionAsync())
                Success("Updated version info", 50);
            else
                return;

            Info("Starting compression of binaries");

            if (await updateZipAsync())
                Success("Compressed binaries", 60);
            else
                return;

            Info("Starting FTP upload");

            if (await uploadFtpAsync())
                Success("Uploaded to FTP", 75);
            else
                return;

            Finish("Publish complete. " + NewRelease.ReleaseName);
        }

        private Task<bool> checkRepositoryAsync()
        {
            return Task.Run(() => checkRepository());
        }

        private Task<bool> rebuildReleaseAsync()
        {
            return Task.Run(() => rebuildRelease());
        }

        private Task<bool> stampVersionAsync()
        {
            return Task.Run(() => stampVersion());
        }

        private Task<bool> updateZipAsync()
        {
            return Task.Run(() => updateZip());
        }

        private Task<bool> uploadFtpAsync()
        {
            return Task.Run(() => uploadFtp());
        }

        private bool checkRepository()
        {
            _repo = new Repository(_path);

            if (!_repo.Exists)
            {
                Fail("Repository not found");
                return false;
            }
            else
                Success("Repository exists");

            if (!_repo.IsClean)
            {
                if (_ignoreDirtyRepo)
                {
                    Info("Ignoring dirty repo");
                    return true;
                }
                else
                {
                    Fail("Repository is dirty");
                    return false;
                }
            }
            else
                Success("Repository is clean");

            return true;
        }

        private bool rebuildRelease()
        {
            OutputPath = Builder.Build(_path);

            if (OutputPath == null)
                Fail("Rebuild failed");

            return OutputPath != null;
        }

        private bool stampVersion()
        {
            NewRelease = DeployVersion.Update(_repo, OutputPath);
            return true;
        }

        private bool updateZip()
        {
            ZipPackage = Zip.Compress(NewRelease.ReleaseName, OutputPath);

            if (ZipPackage == null)
                Fail("Compression failed");

            return ZipPackage != null;
        }

        private bool uploadFtp()
        {
            var result = Ftp.Upload(ZipPackage, NewRelease);

            if (!result)
                Fail("FTP Failed");

            return result;
        }

        private void Fail(string message)
        {
            if (OnFailure != null)
                OnFailure.Invoke(message);
        }

        public void Info(string message)
        {
            if (OnInfo != null)
                OnInfo.Invoke(message);
        }

        public void Success(string message, float? progress = null)
        {
            if (OnSuccess != null)
                OnSuccess.Invoke(message);

            if (OnUpdateProgress != null && progress.HasValue)
                OnUpdateProgress.Invoke(progress.Value);
        }

        public void Finish(string message)
        {
            if (OnFinishedAll != null)
                OnFinishedAll.Invoke(message);
        }
    }
}
