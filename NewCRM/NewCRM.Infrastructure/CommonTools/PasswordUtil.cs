using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace NewCRM.Infrastructure.CommonTools
{
    public sealed class PasswordUtil
    {
        private const Int32 SaltLength = 4;

        public static Boolean ComparePasswords(String dbPassword, String userPassword)
        {
            Byte[] dbPwd = Convert.FromBase64String(dbPassword);

            Byte[] hashedPwd = HashString(userPassword);

            if (dbPwd.Length == 0 || hashedPwd.Length == 0 || dbPwd.Length != hashedPwd.Length + SaltLength)
            {
                return false;
            }

            Byte[] saltValue = new Byte[SaltLength];
            Int32 saltOffset = hashedPwd.Length;
            for (Int32 i = 0; i < SaltLength; i++)
                saltValue[i] = dbPwd[saltOffset + i];

            Byte[] saltedPassword = CreateSaltedPassword(saltValue, hashedPwd);

       
            return CompareByteArray(dbPwd, saltedPassword);


        }

        public static String CreateDbPassword(String userPassword)
        {
            Byte[] unsaltedPassword = HashString(userPassword);

            //Create a salt value
            Byte[] saltValue = new Byte[SaltLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltValue);

            Byte[] saltedPassword = CreateSaltedPassword(saltValue, unsaltedPassword);

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
            Byte[] pwd = System.Text.Encoding.UTF8.GetBytes(str);

            SHA1 sha1 = SHA1.Create();
            Byte[] saltedPassword = sha1.ComputeHash(pwd);
            return saltedPassword;
        }

        private static Boolean CompareByteArray(ICollection<Byte> array1, IList<Byte> array2 = null)
        {
            if (array2 == null) throw new ArgumentNullException("array2");
            if (array1.Count != array2.Count)
                return false;
            return !array1.Where((t, i) => t != array2[i]).Any();
        }


        private static Byte[] CreateSaltedPassword(Byte[] saltValue, Byte[] unsaltedPassword)
        {
            Byte[] rawSalted = new Byte[unsaltedPassword.Length + saltValue.Length];
            unsaltedPassword.CopyTo(rawSalted, 0);
            saltValue.CopyTo(rawSalted, unsaltedPassword.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] saltedPassword = sha1.ComputeHash(rawSalted);

            Byte[] dbPassword = new Byte[saltedPassword.Length + saltValue.Length];
            saltedPassword.CopyTo(dbPassword, 0);
            saltValue.CopyTo(dbPassword, saltedPassword.Length);

            return dbPassword;
        }
        #endregion

    }
}
