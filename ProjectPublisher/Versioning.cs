using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Publisher
{
    class Versioning
    {
        private Repository _repo;

        private static readonly string ReleaseTagFormat = @"release/{0}";
        private static readonly string NumberGroup = @"(\d+)";

        public Versioning(Repository repo)
        {
            _repo = repo;
        }

        public void Update()
        {
            var nextReleaseTag = String.Format(ReleaseTagFormat, GetNextReleaseNumber().ToString());
            _repo.ApplyTagToCurrent(nextReleaseTag);
        }

        private int GetNextReleaseNumber()
        {
            var releaseTagPattern = String.Format(ReleaseTagFormat, NumberGroup);

            var latestReleaseTag = _repo.Tags
                .Last(x => x.Matches(releaseTagPattern));

            if (!latestReleaseTag.Any())
                return 1;

            int latestRevision;
            string latestRevisionText = latestReleaseTag.Extract(releaseTagPattern).First();

            var parseResult = Int32.TryParse(latestRevisionText, out latestRevision);

            if (!parseResult)
                return 1;

            return latestRevision;
        }

        public bool Result { get; set; }
    }
}
