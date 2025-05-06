using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Banking
{
    public static class Logger
    {
        private static readonly string LogFile = "log.txt";

        public static void Log(string action)
        {
            string entry = $"{DateTime.Now:G} – {action}";
            File.AppendAllText(LogFile, entry + Environment.NewLine);
        }
    }
}
