using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Caros.CI.API
{
    public class Ftp
    {
        private FileInfo _sourceFile;
        private NetworkCredential _credentials;
        private int _versionNumber;

        private const string Host = "103.9.171.165";
        private const string Password = "_=nMqH!m@naV";
        private const string Username = "caros@jay-wick.com";
        private const string VersionPointerFileName = "version.pointer";

        public bool Result { get; set; }

        public Ftp(string sourceFilePath, int versionNumber)
        {
            _sourceFile = new FileInfo(sourceFilePath);
            _credentials = new System.Net.NetworkCredential(Username, Password);
            _versionNumber = versionNumber;
        }

        public void Upload()
        {
            Result = UploadFile("/updates/" + _sourceFile.Name, _sourceFile.FullName);
            Result &= UploadFile("/updates/" + VersionPointerFileName, CreateVersionPointerFile(VersionPointerFileName, _versionNumber.ToString()));
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

            return response.StatusDescription.StartsWith("226-File successfully transferred");
        }

        public string CreateVersionPointerFile(string filename, string contents)
        {
            var path = Path.Combine(Path.GetTempPath(), filename);
            File.WriteAllText(path, contents);

            return path;
        }
    }
}
