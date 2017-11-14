using NewCRM.Infrastructure.CommonTools.CustomException;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NewCRM.Infrastructure.CommonTools
{
    public class ProfileManager
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string sectionName, string key, string defaultValue, byte[] returnBuffer, int size, string filePath);

        private static String _profilePath = "";
        public static String GetFileUrl { get; private set; }


        public static void Init(String profilePath = default(String))
        {
            if (profilePath != default(String))
            {
                _profilePath = profilePath;
            }

            WatcherStrat(profilePath, "*.ini");
            GetFileUrl = GetSectionValue("FileStorage", "FileUrl");
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
                GetFileUrl = GetSectionValue("FileStorage", "FileUrl");
            };
            watcher.EnableRaisingEvents = true;
        }

        private static String GetSectionValue(String sectionName, String key)
        {
            Byte[] buffer = new Byte[2048];
            Int32 length = GetPrivateProfileString(sectionName, key, "发生错误", buffer, 999, _profilePath);
            String result = Encoding.Default.GetString(buffer, 0, length);
            if (!String.IsNullOrEmpty(result))
            {
                throw new BusinessException("FileUrl配置参数获取失败");
            }
            return result;
        }
    }
}
