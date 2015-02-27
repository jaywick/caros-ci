using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Caros.CI.API
{
    public static class Ftp
    {
        private static NetworkCredential _credentials;

        private const string Host = "103.9.171.165";
        private const string Password = "_=nMqH!m@naV";
        private const string Username = "caros@jay-wick.com";
        private const string VersionPointerFileName = "version.pointer";

        static Ftp()
        {
            _credentials = new System.Net.NetworkCredential(Username, Password);
        }

        public static bool Upload(string sourceFile)
        {
            var file = new FileInfo(sourceFile);

            var request = WebRequest.Create("ftp://" + Host + "/updates/" + file.Name) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = _credentials;

            var contents = File.ReadAllBytes(file.FullName);
            request.ContentLength = contents.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(contents, 0, contents.Length);
            }

            var response = (FtpWebResponse)request.GetResponse();

            return response.StatusDescription.StartsWith("226-File successfully transferred");
        }
    }
}
