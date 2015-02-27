using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caros.CI.API
{
    public class Deployment
    {
        public void Deploy(UpdateInfo update, string binariesFolder)
        {
            var temp = update.Download();
            var newFolder = Path.Combine(binariesFolder, update.Version.ReleaseName);

            Directory.CreateDirectory(newFolder);

            Zip.Uncompress(temp, newFolder);

            File.WriteAllText(Path.Combine(binariesFolder, "version.pointer"), update.Version.ReleaseName);
        }

        public void Launch(string binariesFolder)
        {
            var currentVersion = Path.Combine(binariesFolder, "version.pointer");
            var executable = new DirectoryInfo(currentVersion).EnumerateFiles("caros.exe").Single();

            Process.Start(executable.FullName);
        }
    }
}
