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
                Success("Updated release tag", 20);
            else
                return;

            // zip
            // upload to ftp
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
            var builder = new Builder(_path);
            return builder.Build();
        }

        private bool updateTags()
        {
            var versioning = new Versioning(_repo);
            versioning.Update();

            return versioning.Result;
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
    }
}
