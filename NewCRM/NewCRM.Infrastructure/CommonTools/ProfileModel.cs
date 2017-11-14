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
        private static String _fileUrl = "";

        public static void Init(String profilePath = default(String))
        {
            if (profilePath != default(String))
            {
                _profilePath = profilePath;
            }

            WatcherStrat(profilePath, "*.ini");
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
                var a = 0;
            };
            watcher.EnableRaisingEvents = true;
        }

        public String GetFileUrl
        {
            get
            {
                byte[] buffer = new byte[2048];
                Int32 length = GetPrivateProfileString("FileStorage", "FileUrl", "发生错误", buffer, 999, _profilePath);
                String result = Encoding.Default.GetString(buffer, 0, length);
                if (!String.IsNullOrEmpty(result))
                {
                    throw new BusinessException("FileUrl配置参数获取失败");
                }

                return result;
            }
        }
    }
}
