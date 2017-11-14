using NewCRM.Infrastructure.CommonTools.CustomException;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NewCRM.Infrastructure.CommonTools
{
    public class ProfileManager
    {
        private static String _profilePath = "";
        public static String GetFileUrl { get; private set; }


        public static void Init(String profilePath = default(String))
        {
            if (profilePath != default(String))
            {
                _profilePath = profilePath;
            }

            WatcherStrat(profilePath, "ConsoleApp1.exe.config");
            GetFileUrl = ConfigurationManager.AppSettings["FileStorage"];
        }

        private static void WatcherStrat(string path, string filter)
        {
            var watcher = new FileSystemWatcher
            {
                Path = path,
                Filter = filter
            };

            watcher.Changed += (obj, sender) =>
            {
                GetFileUrl = ConfigurationManager.AppSettings["FileStorage"];
            };
            watcher.EnableRaisingEvents = true;
        }
    }
}
