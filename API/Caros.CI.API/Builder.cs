using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System.IO;

namespace Caros.CI.API
{
    public static class Builder
    {
        public static string Build(string solutionPath)
        {
            var projectPath = new DirectoryInfo(solutionPath)
                .EnumerateFiles("Caros.csproj", SearchOption.AllDirectories)
                .First()
                .FullName;

            var outputPath = Path.Combine(Path.GetTempPath(), "caros4-build-" + Guid.NewGuid().ToString());

            var collection = new ProjectCollection();

            var targets = new Dictionary<string, string>();
            targets.Add("Configuration", "RELEASE");
            targets.Add("Platform", "x86");
            targets.Add("OutputPath", outputPath);

            var parameters = new BuildParameters(collection);
            var request = new BuildRequestData(projectPath, targets, null, new string[] { "Build" }, null);

            var buildResult = BuildManager.DefaultBuildManager.Build(parameters, request);

            if (buildResult.OverallResult != BuildResultCode.Success)
                return null;

            return outputPath;
        }

    }
}
