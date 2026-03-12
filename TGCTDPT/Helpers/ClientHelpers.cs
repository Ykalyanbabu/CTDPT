using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

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
    }
}