using System;
using System.Runtime.InteropServices;

namespace NewCRM.Infrastructure.CommonTools
{
    public class ProfileModel
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string sectionName, string key, string defaultValue, byte[] returnBuffer, int size, string filePath);

        private static String _profilePath = "";

        public static void Init(String profilePath = default(String))
        {
            if (profilePath != default(String))
            {
                _profilePath = profilePath;
            }


        }
    }
}
