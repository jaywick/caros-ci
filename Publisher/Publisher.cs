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
        private Builder _builder;
        private Zip _zip;
        private DeployVersion _versioning;
        private Ftp _ftp;

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
            _builder = new Builder(_path);
            _builder.Build();

            if (!_builder.Result)
                Fail("Rebuild failed");

            return _builder.Result;
        }

        private bool stampVersion()
        {
            _versioning = new DeployVersion(_repo, _builder);
            _versioning.Update();

            if (!_versioning.Result)
                Fail("Versioning failed");

            return _versioning.Result;
        }

        private bool updateZip()
        {
            _zip = new Zip(_builder.OutputPath, _versioning);
            _zip.Compress();

            if (!_zip.Result)
                Fail("Compression failed");

            return _zip.Result;
        }

        private bool uploadFtp()
        {
            _ftp = new Ftp(_zip.PackageFile, _versioning.NewRelease);
            _ftp.Upload();

            if (!_ftp.Result)
                Fail("FTP Failed");

            return _ftp.Result;
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
