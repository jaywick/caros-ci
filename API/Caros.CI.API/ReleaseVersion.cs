using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Caros.CI.API
{
    public class ReleaseVersion
    {
        public string ReleaseName { get; set; }
        public int ReleaseNumber { get; set; }

        public ReleaseVersion(string releaseName)
        {
            ReleaseName = releaseName;
            ReleaseNumber = int.Parse(new Regex(@"r(\d+)").Match(releaseName).Groups[1].Value);
        }

        public ReleaseVersion(int releaseNumber)
        {
            ReleaseNumber = releaseNumber;
            ReleaseName = "r" + ReleaseNumber;
        }
    }
}
