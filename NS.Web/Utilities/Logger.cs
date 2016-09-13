using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NS.Web.Utilities
{
    public static class Logger
    {
        public static void Write(string msg)
        {
            var logPath = OpenAndCreateDir();
            using (var fileStream = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    streamWriter.WriteLine($"{time}-{msg}");
                }
            }
        }

        public static string OpenAndCreateDir()
        {
            var relatePath = $"~/Log/{DateTime.Now.ToString("yyyy-MM-dd")}";
            var absolutePath = HttpContext.Current.Server.MapPath(relatePath);

            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            return $"{absolutePath}/Logger.log";
        }
    }
}