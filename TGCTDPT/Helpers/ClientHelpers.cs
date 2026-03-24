using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace TGCTDPT.Helpers
{
    public class ClientHelpers
    {
        string DocumentPathPT = ConfigurationManager.AppSettings["DocumentsPathPTRefunds"].ToString();
        public static string GetClientIp()
        {
            return HttpContext.Current?.Request?.UserHostAddress;
        }
        public void CreateDirectoryIfNotExists(string basePath,string ptin)
        {
            string path = Path.Combine(basePath, ptin);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public  string EncryptPwd(string pwd)
        {
            using (TripleDESCryptoServiceProvider tdcsp = new TripleDESCryptoServiceProvider())
            {
                byte[] key1 = new byte[]
                {
            0x99, 0x22, 0x95, 0x93, 0x90, 0x89, 0x35, 0x85,
            0x83, 0x81, 0x79, 0x37, 0x75, 0x73, 0x20, 0x69
                };

                byte[] key2 = new byte[]
                {
            0x93, 0x28, 0x95, 0x93, 0x91, 0x89, 0x34, 0x85,
            0x81, 0x83, 0x79, 0x38, 0x75, 0x72, 0x69, 0x20
                };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream encryptcs = new CryptoStream(
                        ms,
                        tdcsp.CreateEncryptor(key1, key2),
                        CryptoStreamMode.Write))
                    {
                        byte[] b = Encoding.ASCII.GetBytes(pwd);
                        encryptcs.Write(b, 0, pwd.Length);
                        encryptcs.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}