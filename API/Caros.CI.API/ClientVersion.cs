using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Caros.CI.API
{
    public class ClientVersion
    {
        static ClientVersion()
        {
            ReadCurrentVersion();
        }

        public static void ReadCurrentVersion()
        {
            var xdoc = XDocument.Load("meta.xml");
            var releaseNumber = int.Parse(xdoc.Element("Meta").Element("Release").Value);

            CurrentVersion = new ReleaseVersion(releaseNumber);
        }

        public static ReleaseVersion CurrentVersion { get; private set; }
    }
}
