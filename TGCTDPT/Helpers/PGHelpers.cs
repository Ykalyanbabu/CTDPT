using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace TGCTDPT.Helpers
{
    public class PGHelpers
    {
        public static string GetMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
        public static string Encrypt(string plainText, string keyFilePath)
        {
            byte[] keyBytes = File.ReadAllBytes(keyFilePath);
            byte[] key = new byte[16];

            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));

            using (RijndaelManaged rij = new RijndaelManaged())
            {
                rij.Mode = CipherMode.CBC;
                rij.Padding = PaddingMode.PKCS7;
                rij.KeySize = 128;
                rij.BlockSize = 128;
                rij.Key = key;
                rij.IV = key;

                var encryptor = rij.CreateEncryptor();

                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                return Convert.ToBase64String(encrypted);
            }
        }
        public static string Decrypt(string cipherText, string keyFilePath)
        {
            byte[] keyBytes = File.ReadAllBytes(keyFilePath);
            byte[] key = new byte[16];

            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));

            using (RijndaelManaged rij = new RijndaelManaged())
            {
                rij.Mode = CipherMode.CBC;
                rij.Padding = PaddingMode.PKCS7;
                rij.KeySize = 128;
                rij.BlockSize = 128;
                rij.Key = key;
                rij.IV = key;

                var decryptor = rij.CreateDecryptor();

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}