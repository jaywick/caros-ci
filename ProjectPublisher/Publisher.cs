using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string p;

        public Publisher(string solutionPath)
        {
            _path = solutionPath;
        }

        public void Start()
        {
            if (checkRepository())
                Success("", 10);
            else
                return;

            if (rebuildRelease())
                Success("", 20);
            else
                return;

            // list all tags
            // tag with next increment
            // zip
            // upload to ftp
        }

        private bool checkRepository()
        {
            var repo = new Repository(_path);

            if (!repo.Exists)
            {
                Fail("Repository not found");
                return false;
            }
            
            if (!repo.IsClean)
            {
                Fail("Repository is dirty");
                return false;
            }

            return true;
        }

        private bool rebuildRelease()
        {
            //var repo = new Builder(_path);
            return false;
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
