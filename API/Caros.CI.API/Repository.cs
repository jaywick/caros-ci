using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caros.CI.API
{
    public class Repository
    {
        private string Path { get; set; }
        private LibGit2Sharp.Repository Repo { get; set; }

        public Repository(string path)
        {
            Path = path;

            try
            {
                Repo = new LibGit2Sharp.Repository(path);
                Exists = true;
            }
            catch (RepositoryNotFoundException)
            {
                Exists = false;
            }
        }

        public bool Exists { get; private set; }

        public bool IsClean
        {
            get
            {
                return !Repo.RetrieveStatus().IsDirty;
            }
        }

        public IEnumerable<string> Tags
        {
            get { return Repo.Tags.Select(x => x.Name); }
        }

        public void TagCurrent(string name)
        {
            if (!IsClean)
                throw new Exception("Repository is not clean");

            Repo.Tags.Add(name, Repo.Commits.First());
        }
    }
}
