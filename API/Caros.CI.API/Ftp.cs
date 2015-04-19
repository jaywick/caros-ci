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

        private const string Host = "jaywick.io";
        private const string Password = "=v7ClNt%X8@&dlk)Xe";
        private const string Username = "caros@jaywick.io";
        private const string VersionPointerFileName = "version.pointer";

        static Ftp()
        {
            _credentials = new System.Net.NetworkCredential(Username, Password);
        }

        public static bool Upload(string sourceFile, ReleaseVersion version)
        {
            var file = new FileInfo(sourceFile);

            if (!UploadPackage(file))
                return false;

            return UploadVersionPointer(version);
        }

        private static bool UploadPackage(FileInfo file)
        {
            var request = WebRequest.Create("ftp://" + Host + "/updates/" + file.Name) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = _credentials;
            request.Timeout = -1;

            var contents = File.ReadAllBytes(file.FullName);
            request.ContentLength = contents.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(contents, 0, contents.Length);
            }

            var response = (FtpWebResponse)request.GetResponse();

            return response.StatusDescription.StartsWith("226-File successfully transferred");
        }

        private static bool UploadVersionPointer(ReleaseVersion version)
        {
            var request = WebRequest.Create("ftp://" + Host + "/updates/" + VersionPointerFileName) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = _credentials;
            request.Timeout = -1;

            var contents = Encoding.Default.GetBytes(version.ReleaseNumber.ToString());
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
