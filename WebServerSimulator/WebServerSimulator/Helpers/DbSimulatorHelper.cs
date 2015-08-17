using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerSimulator.Helpers
{
    public static class DbSimulatorHelper
    {
        public static string[] LoadInFromTextFile(string fileName)
        {
            return !File.Exists(fileName) ? null : File.ReadAllLines(fileName);
        }
    }
}
