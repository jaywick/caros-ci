using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Caros.CI.API
{
    public static class Zip
    {
        private readonly static string PackageNameFormat = "r{0}.caros-update";
        private readonly static string Key = "YyEvuWz7yMYTXtjUaKC4MA7Xlhe1TZNmJ3NcpCJunUk1H0EUj00iNL20yYaUb9F6yqijpFovu/QmXV1ZGj7S9Q==";

        public static string Compress(string fileName, string sourcePath)
        {
            var packageFile = Path.Combine(Path.GetTempPath(), String.Format(PackageNameFormat, fileName));

            var archive = ZipFile.Create(packageFile);
            archive.BeginUpdate();

            foreach (var item in new DirectoryInfo(sourcePath).EnumerateFiles("*", SearchOption.AllDirectories))
            {
                var relativePath = item.FullName.Substring(sourcePath.Length + 1);
                archive.Add(item.FullName, relativePath);
            }

            archive.Password = Key;
            archive.CommitUpdate();
            archive.Close();

            return packageFile;
        }

        public static void Uncompress(string sourcePath, string destinationFolder)
        {
            var zip = new FastZip();
            zip.Password = Key;
            zip.ExtractZip(sourcePath, destinationFolder, null);
        }
    }
}
