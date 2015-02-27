using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caros.CI.API
{
    public static class VersionPointer
    {
        public static string CreateVersionPointerFile(string filename, string contents)
        {
            var path = Path.Combine(Path.GetTempPath(), filename);
            File.WriteAllText(path, contents);

            return path;
        }
    }
}
