using System;
using System.Configuration;
using System.IO;
using NewLib.Security;

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
            FileUrl = SensitiveDataSafetyProvider.Decrypt(config.AppSettings.Settings["FileUrl"].Value);
            RedisConnection = SensitiveDataSafetyProvider.Decrypt(config.AppSettings.Settings["RedisConnection"].Value);
            RedisPrefix = SensitiveDataSafetyProvider.Decrypt(config.AppSettings.Settings["RedisPrefix"].Value);
        }

        public static String FileUrl { get; private set; }

        public static String RedisConnection { get; private set; }

        public static String RedisPrefix { get; private set; }
    }
}
