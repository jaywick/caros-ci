using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Caros.CI.API
{
    public class Versioning
    {
        private Repository _repo;
        private Builder _builder;

        private static readonly string ReleaseNameFormat = "r{0}";
        private static readonly string ReleaseTagFormat = "release/{0}";
        private static readonly string NumberGroupPattern = @"(\d+)";

        public Versioning(Repository repo, Builder builder)
        {
            _repo = repo;
            _builder = builder;
        }

        public void Update()
        {
            NewRelease = GetNextReleaseNumber();

            var nextReleaseTag = String.Format(ReleaseTagFormat, NewRelease.ToString());
            _repo.TagCurrent(nextReleaseTag);

            CreateMetaXml();

            Result = true;
        }

        private void CreateMetaXml()
        {
            var path = System.IO.Path.Combine(_builder.OutputPath, "meta.xml");

            var xdoc = new XDocument();
            xdoc.Add(new XElement("Meta"));
            xdoc.Element("Meta").Add(new XElement("Release", NewRelease));
            xdoc.Element("Meta").Add(new XElement("ReleaseName", NewReleaseName));

            xdoc.Save(path);
        }

        private int GetNextReleaseNumber()
        {
            var releaseTagPattern = String.Format(ReleaseTagFormat, NumberGroupPattern);

            var releaseTags = _repo.Tags
                .Where(x => x.Matches(releaseTagPattern));

            if (!releaseTags.Any())
                return 1;

            var latestRevision = releaseTags
                .Select(x => x.Extract(releaseTagPattern).First())
                .Select(x => Int32.Parse(x))
                .OrderByDescending(x => x)
                .First();

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
