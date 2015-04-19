using System;
using System.Collections.Generic;
using System.IO;
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

        private const string DownloadAddressFormat = "http://inhouse.jaywick.io/caros/updates/{0}.caros-update";

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

        public string Download()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            
            var web = new System.Net.WebClient();
            web.DownloadFile(DownloadAddress, tempPath);

            return tempPath;
        }
    }
}
