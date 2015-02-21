using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Publisher
{
    public class Builder
    {
        private string _solutionPath;

        public Builder(string solutionPath)
        {
            _solutionPath = solutionPath;
        }

        public void Start()
        {
            var processInfo = new ProcessStartInfo(@"C:\WINDOWS\Microsoft.NET\Framework\v3.5\MsBuild.exe");

            var arguments = new StringBuilder();
            arguments.AppendLine(_solutionPath);
            arguments.AppendLine(" /t:Rebuild");
            arguments.AppendLine(" /p:Configuration=RELEASE");
            arguments.AppendLine(" /p:Platform=\"x86\"");
            processInfo.Arguments = arguments.ToString();

            Process.Start(processInfo);
        }
    }
}
