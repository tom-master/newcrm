using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace NewCRM.Infrastructure.CommonTools
{
    public sealed class PasswordUtil
    {
        #region field & constructor

        
        private const Int32 SaltLength = 4;

        #endregion

        /// <summary>
        /// 对比用户明文密码是否和加密后密码一致
        /// </summary>
        /// <param name="dbPassword">数据库中单向加密后的密码</param>
        /// <param name="userPassword">用户明文密码</param>
        /// <returns></returns>
        public static bool ComparePasswords(string dbPassword, string userPassword)
        {
            byte[] dbPwd = Convert.FromBase64String(dbPassword);

            byte[] hashedPwd = Hashstring(userPassword);

            if (dbPwd.Length == 0 || hashedPwd.Length == 0 || dbPwd.Length != hashedPwd.Length + SaltLength)
            {
                return false;
            }

            byte[] saltValue = new byte[SaltLength];
            Int32 saltOffset = hashedPwd.Length;
            for (Int32 i = 0; i < SaltLength; i++)
                saltValue[i] = dbPwd[saltOffset + i];

            byte[] saltedPassword = CreateSaltedPassword(saltValue, hashedPwd);

            // compare the values
            return CompareByteArray(dbPwd, saltedPassword);


        }

        /// <summary>
        /// 创建用户的数据库密码
        /// </summary>
        /// <returns></returns>
        public static string CreateDbPassword(string userPassword)
        {
            byte[] unsaltedPassword = Hashstring(userPassword);

            //Create a salt value
            byte[] saltValue = new byte[SaltLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltValue);

            byte[] saltedPassword = CreateSaltedPassword(saltValue, unsaltedPassword);

            return Convert.ToBase64String(saltedPassword);

        }

        #region 私有函数
        /// <summary>
        /// 将一个字符串哈希化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] Hashstring(string str)
        {
            byte[] pwd = System.Text.Encoding.UTF8.GetBytes(str);

            SHA1 sha1 = SHA1.Create();
            byte[] saltedPassword = sha1.ComputeHash(pwd);
            return saltedPassword;
        }

        private static bool CompareByteArray(ICollection<byte> array1, IList<byte> array2 = null)
        {
            if (array2 == null) throw new ArgumentNullException("array2");
            if (array1.Count != array2.Count)
                return false;
            return !array1.Where((t, i) => t != array2[i]).Any();
        }
        // create a salted password given the salt value
        private static byte[] CreateSaltedPassword(byte[] saltValue, byte[] unsaltedPassword)
        {
            // add the salt to the hash
            byte[] rawSalted = new byte[unsaltedPassword.Length + saltValue.Length];
            unsaltedPassword.CopyTo(rawSalted, 0);
            saltValue.CopyTo(rawSalted, unsaltedPassword.Length);

            //Create the salted hash			
            SHA1 sha1 = SHA1.Create();
            byte[] saltedPassword = sha1.ComputeHash(rawSalted);

            // add the salt value to the salted hash
            byte[] dbPassword = new byte[saltedPassword.Length + saltValue.Length];
            saltedPassword.CopyTo(dbPassword, 0);
            saltValue.CopyTo(dbPassword, saltedPassword.Length);

            return dbPassword;
        }
        #endregion

    }
}
