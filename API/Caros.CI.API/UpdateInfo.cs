using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caros.CI.API
{
    public class UpdateInfo
    {
        public bool Exists { get; private set; }
        public string DownloadAddress { get; private set; }
        public ReleaseVersion Version { get; private set; }

        private static UpdateInfo _none = new UpdateInfo();
        public static UpdateInfo None { get { return _none; } }

        private const string DownloadAddressFormat = "http://internal.jay-wick.com/caros/updates/{0}.caros-update";

        public UpdateInfo(int releaseNumber)
        {
            Exists = true;
            Version = new ReleaseVersion(releaseNumber);
            DownloadAddress = String.Format(DownloadAddressFormat, Version.ReleaseName);
        }

        private UpdateInfo()
        {
            Exists = false;
        }
    }
}
