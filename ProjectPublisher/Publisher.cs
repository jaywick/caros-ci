using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Publisher;

namespace Publisher
{
    public class Publisher
    {
        public enum EventTypes { Success, Failure, Info }

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
        private Versioning _versioning;
        private Ftp _ftp;

        public Publisher(string solutionPath)
        {
            _path = solutionPath;
        }

        public void Start()
        {
            if (checkRepository())
                Success("Repository looks good", 10);
            else
                return;

            if (rebuildRelease())
                Success("Built release", 20);
            else
                return;

            if (updateTags())
                Success("Updated release tag", 50);
            else
                return;

            if (updateZip())
                Success("Compressing binaries", 60);
            else
                return;

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

            return _builder.Result;
        }

        private bool updateTags()
        {
            _versioning = new Versioning(_repo);
            _versioning.Update();

            return _versioning.Result;
        }

        private bool updateZip()
        {
            _zip = new Zip(_builder.OutputPath, _versioning);
            _zip.Compress();

            return _zip.Result;
        }

        private bool uploadFtp()
        {
            _ftp = new Ftp(_zip.PackageFile, "r" + _versioning.NewRelease);
            _ftp.Upload();

            return _ftp.Result;
        }

        private void Fail(string message)
        {
            if (OnFailure != null)
                OnFailure.Invoke(message);
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
