using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System.IO;

namespace Caros.Publisher
{
    public class Builder
    {
        private string _solutionPath;
        private string _projectPath;

        public string OutputPath { get; set; }

        public Builder(string solutionPath)
        {
            _solutionPath = solutionPath;

            _projectPath = new DirectoryInfo(_solutionPath)
                .EnumerateFiles("Caros.csproj", SearchOption.AllDirectories)
                .First()
                .FullName;

            OutputPath = Path.GetTempPath();
        }

        public void Build(string platform = "x86")
        {
            var collection = new ProjectCollection();

            var targets = new Dictionary<string, string>();
            targets.Add("Configuration", "RELEASE");
            targets.Add("Platform", platform);
            targets.Add("OutputPath", OutputPath);

            var parameters = new BuildParameters(collection);
            var request = new BuildRequestData(_projectPath, targets, "4.0", new string[] { "Build" }, null);

            var buildResult = BuildManager.DefaultBuildManager.Build(parameters, request);

            Result = buildResult.OverallResult == BuildResultCode.Success;
        }

        public bool Result { get; set; }
    }
}
