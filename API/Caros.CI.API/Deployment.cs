﻿using System;
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
        public static async Task Deploy(UpdateInfo update, string binariesFolder)
        {
            var downloadPackage = await update.Download();
            await Deploy(downloadPackage, update.Version, binariesFolder);
        }

        public static async Task Deploy(string downloadPackage, ReleaseVersion version, string binariesFolder)
        {
            var newFolder = Path.Combine(binariesFolder, version.ReleaseName);

            Directory.CreateDirectory(newFolder);

            await Task.Run(() => Zip.Uncompress(downloadPackage, newFolder));

            File.WriteAllText(Path.Combine(binariesFolder, "version.pointer"), version.ReleaseName);
        }

        public static void Launch(string binariesFolder)
        {
            var currentVersion = File.ReadAllText(Path.Combine(binariesFolder, "version.pointer"));
            var binaries = Path.Combine(binariesFolder, currentVersion);

            var executable = new DirectoryInfo(binaries)
                .EnumerateFiles()
                .Single(x => x.Name.ToLower() == "caros.exe");

            Process.Start(executable.FullName);
        }
    }
}
