using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Infrastructure.CommonTools
{
    public sealed class PasswordUtil
    {
        private const Int32 _saltLength = 4;

        public static Boolean ComparePasswords(String dbPassword, String userPassword)
        {
            var dbPwd = Convert.FromBase64String(dbPassword);
            var hashedPwd = HashString(userPassword);

            if (dbPwd.Length == 0 || hashedPwd.Length == 0 || dbPwd.Length != hashedPwd.Length + _saltLength)
            {
                return false;
            }

            var saltValue = new Byte[_saltLength];
            var saltOffset = hashedPwd.Length;
            for (var i = 0; i < _saltLength; i++)
            {
                saltValue[i] = dbPwd[saltOffset + i];
            }

            var saltedPassword = CreateSaltedPassword(saltValue, hashedPwd);
            var result = CompareByteArray(dbPwd, saltedPassword);

            return result;


        }

        public static String CreateDbPassword(String userPassword)
        {
            var unsaltedPassword = HashString(userPassword);
            var saltValue = new Byte[_saltLength];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltValue);

            var saltedPassword = CreateSaltedPassword(saltValue, unsaltedPassword);
            return Convert.ToBase64String(saltedPassword);
        }

        #region 私有函数
        /// <summary>
        /// 将一个字符串哈希化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static Byte[] HashString(String str)
        {
            var pwd = System.Text.Encoding.UTF8.GetBytes(str);

            var sha1 = SHA1.Create();
            var saltedPassword = sha1.ComputeHash(pwd);
            return saltedPassword;
        }

        private static Boolean CompareByteArray(ICollection<Byte> array1, IList<Byte> array2 = null)
        {
            if (array2 == null)
            {
                throw new BusinessException("array2");
            }
            if (array1.Count != array2.Count)
            {
                return false;
            }
            return !array1.Where((t, i) => t != array2[i]).Any();
        }


        private static Byte[] CreateSaltedPassword(Byte[] saltValue, Byte[] unsaltedPassword)
        {
            var rawSalted = new Byte[unsaltedPassword.Length + saltValue.Length];
            unsaltedPassword.CopyTo(rawSalted, 0);
            saltValue.CopyTo(rawSalted, unsaltedPassword.Length);

            var sha1 = SHA1.Create();
            var saltedPassword = sha1.ComputeHash(rawSalted);

            var dbPassword = new Byte[saltedPassword.Length + saltValue.Length];
            saltedPassword.CopyTo(dbPassword, 0);
            saltValue.CopyTo(dbPassword, saltedPassword.Length);

            return dbPassword;
        }
        #endregion

    }
}
