using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class Repository
    {
        private string Path { get; set; }
        private LibGit2Sharp.Repository InternalRepository { get; set; }

        public Repository(string path)
        {
            Path = path;

            try
            {
                InternalRepository = new LibGit2Sharp.Repository(path);
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
                return !InternalRepository.RetrieveStatus().IsDirty;
            }
        }
    }
}
