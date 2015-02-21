using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Caros.Publisher
{
    class Zip
    {
        private string _sourcePath;
        private Versioning _versioning;

        private readonly static string PackageNameFormat = "caros4_package_r{0}.zip";

        public Zip(string sourcePath, Versioning versioning)
        {
            _sourcePath = sourcePath;
            _versioning = versioning;

            PackageFile = String.Format(PackageNameFormat, _versioning.NewRelease);
        }

        public void Compress()
        {
            var archive = ZipFile.Create(PackageFile);
            
            foreach (var item in new DirectoryInfo(_sourcePath).EnumerateFileSystemInfos())
	        {
                if (item is DirectoryInfo)
                    archive.AddDirectory(item.FullName);
                else
                    archive.Add(item.FullName);
	        }

            archive.CommitUpdate();

            Result = true;
        }

        public bool Result { get; set; }
        public string PackageFile { get; set; }
    }
}
