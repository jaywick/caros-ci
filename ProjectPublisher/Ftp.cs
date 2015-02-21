using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Publisher
{
    class Ftp
    {
        private FileInfo _sourceFile;
        private NetworkCredential _credentials;
        private string _versionName;

        private const string Host = "103.9.171.165";
        private const string Password = "_=nMqH!m@naV";
        private const string Username = "caros@jay-wick.com";
        private const string VersionPointerFileName = "version.pointer";

        public Ftp(string sourceFilePath, string versionName)
        {
            _sourceFile = new FileInfo(sourceFilePath);
            _credentials = new System.Net.NetworkCredential(Username, Password);
            _versionName = versionName;
        }

        public void Upload()
        {
            UploadFile("/update/" + _sourceFile.Name, _sourceFile.FullName);
            UploadFile("/update/" + VersionPointerFileName, CreateVersionPointerFile(VersionPointerFileName, _versionName));
        }

        private bool UploadFile(string remotePath, string filePath)
        {
            var request = WebRequest.Create("ftp://" + Host + remotePath) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = _credentials;

            var contents = File.ReadAllBytes(filePath);
            request.ContentLength = contents.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(contents, 0, contents.Length);
            }

            var response = (FtpWebResponse)request.GetResponse();

            return response.StatusCode == FtpStatusCode.CommandOK;
        }

        public string CreateVersionPointerFile(string filename, string contents)
        {
            var path = Path.Combine(Path.GetTempPath(), filename);
            File.WriteAllText(path, contents);

            return path;
        }

        public bool Result { get; set; }
    }
}
