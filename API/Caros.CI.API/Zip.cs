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
        private readonly static string PackageNameFormat = "{0}.caros-update";
        private readonly static string Key = "YyEvuWz7yMYTXtjUaKC4MA7Xlhe1TZNmJ3NcpCJunUk1H0EUj00iNL20yYaUb9F6yqijpFovu/QmXV1ZGj7S9Q==";

        public static string Compress(string fileName, string sourcePath)
        {
            var packageFile = Path.Combine(Path.GetTempPath(), String.Format(PackageNameFormat, fileName));

            var zip = new FastZip();
            zip.Password = Key;
            zip.CreateZip(packageFile, sourcePath, true, null);

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
