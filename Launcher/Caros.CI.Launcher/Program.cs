using Caros.CI.API;
using System;

namespace Caros.CI.Launcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Deployment.Launch(@"c:\caros\system\binaries");
        }
    }
}
