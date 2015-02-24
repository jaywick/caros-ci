﻿using ICSharpCode.SharpZipLib.Zip;
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

        private readonly static string PackageNameFormat = "r{0}.caros-update";
        private readonly static string Key = "YyEvuWz7yMYTXtjUaKC4MA7Xlhe1TZNmJ3NcpCJunUk1H0EUj00iNL20yYaUb9F6yqijpFovu/QmXV1ZGj7S9Q==";

        public Zip(string sourcePath, Versioning versioning)
        {
            _sourcePath = sourcePath;
            _versioning = versioning;

            PackageFile = Path.Combine(Path.GetTempPath(), String.Format(PackageNameFormat, _versioning.NewRelease));
        }

        public void Compress()
        {
            var archive = ZipFile.Create(PackageFile);
            archive.BeginUpdate();

            foreach (var item in new DirectoryInfo(_sourcePath).EnumerateFiles("*", SearchOption.AllDirectories))
            {
                var relativePath = item.FullName.Substring(_sourcePath.Length + 1);
                archive.Add(item.FullName, relativePath);
            }

            archive.Password = Key;
            archive.CommitUpdate();
            archive.Close();

            Result = true;
        }

        public bool Result { get; set; }
        public string PackageFile { get; set; }
    }
}
