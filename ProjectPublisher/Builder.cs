using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System.IO;

namespace Publisher
{
    public class Builder
    {
        private string _solutionPath;

        public string OutputPath { get; set; }

        public Builder(string solutionPath)
        {
            _solutionPath = solutionPath;
            OutputPath = Path.GetTempPath();
        }

        public bool Build(string platform = "x86")
        {
            var collection = new ProjectCollection();

            Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
            GlobalProperty.Add("Configuration", "Release");
            GlobalProperty.Add("Platform", platform);
            GlobalProperty.Add("OutputPath", OutputPath);

            var parameters = new BuildParameters(collection);
            var request = new BuildRequestData(_solutionPath, GlobalProperty, "4.0", new string[] { "Build" }, null);
            
            var buildResult = BuildManager.DefaultBuildManager.Build(parameters, request);

            return (buildResult.OverallResult == BuildResultCode.Success);
        }
    }
}
