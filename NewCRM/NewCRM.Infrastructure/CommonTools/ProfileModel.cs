using NewCRM.Infrastructure.CommonTools.CustomException;
using System;
using System.Configuration;
using System.IO;

namespace NewCRM.Infrastructure.CommonTools
{
    public class ProfileManager
    {
        public static void Init()
        {
            var watcher = new FileSystemWatcher
            {
                Path = AppDomain.CurrentDomain.BaseDirectory,
                Filter = "WebSite.config"
            };

            watcher.Changed += (obj, sender) =>
            {
                Reload();
            };

            Reload();
            watcher.EnableRaisingEvents = true;
        }

        private static void Reload()
        {
            var config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = $@"{AppDomain.CurrentDomain.BaseDirectory}/WebSite.config" }, ConfigurationUserLevel.None);
            FileUrl = config.AppSettings.Settings["FileUrl"].Value;
        }

        public static String FileUrl { get; private set; }
    }
}
