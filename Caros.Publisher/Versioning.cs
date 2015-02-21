using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caros.Publisher
{
    class Versioning
    {
        private Repository _repo;

        private static readonly string ReleaseNameFormat = "r{0}";
        private static readonly string ReleaseTagFormat = "release/{0}";
        private static readonly string NumberGroupPattern = @"(\d+)";

        public Versioning(Repository repo)
        {
            _repo = repo;
        }

        public void Update()
        {
            NewRelease = GetNextReleaseNumber();
            var nextReleaseTag = String.Format(ReleaseTagFormat, NewRelease.ToString());
            _repo.ApplyTagToCurrent(nextReleaseTag);

            Result = true;
        }

        private int GetNextReleaseNumber()
        {
            var releaseTagPattern = String.Format(ReleaseTagFormat, NumberGroupPattern);

            var latestReleaseTag = _repo.Tags
                .Last(x => x.Matches(releaseTagPattern));

            if (!latestReleaseTag.Any())
                return 1;

            int latestRevision;
            string latestRevisionText = latestReleaseTag.Extract(releaseTagPattern).First();

            var parseResult = Int32.TryParse(latestRevisionText, out latestRevision);

            if (!parseResult)
                return 1;

            return latestRevision + 1;
        }

        public bool Result { get; set; }

        public int NewRelease { get; set; }

        public string NewReleaseName
        {
            get { return String.Format(ReleaseNameFormat, NewRelease.ToString()); }
        }
    }
}
