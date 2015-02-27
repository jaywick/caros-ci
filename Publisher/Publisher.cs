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
        private DeployVersion _versioning;

        private string ZipPackage { get; set; }
        private string OutputPath { get; set; }

        public Publisher(string solutionPath)
        {
            _path = solutionPath;
        }

        public void Start()
        {
            Info("Checking repository");

            if (checkRepository())
                Success("Repository looks good", 10);
            else
                return;

            Info("Rebuilding solution");

            if (rebuildRelease())
                Success("Built release", 20);
            else
                return;

            Info("Updating version info");

            if (stampVersion())
                Success("Updated version info", 50);
            else
                return;

            Info("Starting compression of binaries");

            if (updateZip())
                Success("Compressed binaries", 60);
            else
                return;

            Info("Starting FTP upload");

            if (uploadFtp())
                Success("Uploaded to FTP", 75);
            else
                return;

            Finish("Publish complete. " + _versioning.NewReleaseName);
        }

        private bool checkRepository()
        {
            _repo = new Repository(_path);

            if (!_repo.Exists)
            {
                Fail("Repository not found");
                return false;
            }

            if (!_repo.IsClean)
            {
                Fail("Repository is dirty");
                return false;
            }

            return true;
        }

        private bool rebuildRelease()
        {
            OutputPath = Builder.Build(_path);

            if (OutputPath != null)
                Fail("Rebuild failed");

            return OutputPath != null;
        }

        private bool stampVersion()
        {
            _versioning = new DeployVersion(_repo);
            _versioning.Update(OutputPath);

            if (!_versioning.Result)
                Fail("Versioning failed");

            return _versioning.Result;
        }

        private bool updateZip()
        {
            ZipPackage = Zip.Compress(_versioning.NewRelease.ToString(), OutputPath);

            if (ZipPackage == null)
                Fail("Compression failed");

            return ZipPackage != null;
        }

        private bool uploadFtp()
        {
            var result = Ftp.Upload(ZipPackage);

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

        public void Success(string message, float progress)
        {
            if (OnSuccess != null)
                OnSuccess.Invoke(message);

            if (OnUpdateProgress != null)
                OnUpdateProgress.Invoke(progress);
        }

        public void Finish(string message)
        {
            if (OnFinishedAll != null)
                OnFinishedAll.Invoke(message);
        }
    }
}
