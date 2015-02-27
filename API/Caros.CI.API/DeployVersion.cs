using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Caros.CI.API
{
    public static class DeployVersion
    {
        private static readonly string ReleaseNameFormat = "r{0}";
        private static readonly string ReleaseTagFormat = "release/{0}";
        private static readonly string NumberGroupPattern = @"(\d+)";

        public static ReleaseVersion Update(Repository repo, string outputPath)
        {
            NewRelease = GetNextReleaseNumber(repo);

            var nextReleaseTag = String.Format(ReleaseTagFormat, NewRelease.ToString());
            repo.TagCurrent(nextReleaseTag);

            CreateMetaXml(outputPath);

            return new ReleaseVersion(NewRelease);
        }

        private static void CreateMetaXml(string outputPath)
        {
            var path = System.IO.Path.Combine(outputPath, "meta.xml");

            var xdoc = new XDocument();
            xdoc.Add(new XElement("Meta"));
            xdoc.Element("Meta").Add(new XElement("Release", NewRelease));
            xdoc.Element("Meta").Add(new XElement("ReleaseName", NewReleaseName));

            xdoc.Save(path);
        }

        private static int GetNextReleaseNumber(Repository repo)
        {
            var releaseTagPattern = String.Format(ReleaseTagFormat, NumberGroupPattern);

            var releaseTags = repo.Tags
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

        public static int NewRelease { get; set; }

        public static string NewReleaseName
        {
            get { return String.Format(ReleaseNameFormat, NewRelease.ToString()); }
        }
    }
}
