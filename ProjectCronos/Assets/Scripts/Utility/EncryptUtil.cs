using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.Analytics;

namespace ProjectCronos
{
    /// <summary>
    /// 暗号化関連の便利クラス
    /// </summary>
    public static class EncryptUtil
    {
        // 初期化ベクトル(半角16文字)
        const string AES_IV_256 = @"zsoaq%ko&gocthi)";

        // 暗号化鍵(半角32文字)
        const string AES_KEY_256 = @"feoal&tewp@+iowta1$#mi93%+iw:0$f";

        /// <summary>
        /// stringを暗号化する
        /// </summary>
        /// <param name="text"></param>
        /// <returns>暗号化されたbyteデータを返す</returns>
        public static byte[] EncryptStringToBytesAes(string text)
        {
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
                aesAlg.Key = Encoding.UTF8.GetBytes(AES_KEY_256);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt  = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static string EncryptStringToStringAes(string text)
        {
            var encrypted = EncryptStringToBytesAes(text);
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptStringFromBytesAes(byte[] text)
        {
            string plainText = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
                aesAlg.Key = Encoding.UTF8.GetBytes(AES_KEY_256);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(text))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        public static string DecryptStringFromStringAes(string text)
        {
            string plainText = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
                aesAlg.Key = Encoding.UTF8.GetBytes(AES_KEY_256);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(text)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }
    }
}
