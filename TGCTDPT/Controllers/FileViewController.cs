using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace TGCTDPT.Controllers
{
    public class FileViewController : Controller
    {
        // GET: FileView
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FetchBase64FileUSINGN(string fileName, string Type)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return new HttpStatusCodeResult(400, "Filename is not provided.");
                }
                var folderPath = "";

                if (Type == "Reg")
                {
                    folderPath = ConfigurationManager.AppSettings["DocumentsPathPTReg"];
                }
                else if (Type == "Amend")
                {
                    folderPath = ConfigurationManager.AppSettings["PTAmend"];
                }
                else if (Type == "Cancel")
                {
                    folderPath = ConfigurationManager.AppSettings["DocumentsPathPTCancelReq"];
                }
                else if (Type == "Revoke")
                {
                    folderPath = ConfigurationManager.AppSettings["DocumentsPathPTRevokeReq"];
                }
                else if (Type == "Refunds")
                {
                    folderPath = ConfigurationManager.AppSettings["DocumentsPathPTRefunds"];
                }
                var filePath = Path.Combine(folderPath, fileName);

                string domain = ConfigurationManager.AppSettings["Impersonation:Domain"];
                string username = ConfigurationManager.AppSettings["Impersonation:Username"];
                string password = ConfigurationManager.AppSettings["Impersonation:Password"];

                using (new Impersonation(domain, username, password))
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        return HttpNotFound("File not found.");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    string base64FileContent = Convert.ToBase64String(fileBytes);

                    var response = new Base64FileResponse
                    {
                        ResponseCode = Convert.ToInt32(ConfigurationManager.AppSettings["ResponsesCodes:SuccessCode"]),
                        FileName = fileName,
                        Base64FileContent = base64FileContent
                    };

                    return new JsonResult
                    {
                        Data = response,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
public class Base64FileResponse
{
    public int ResponseCode { get; set; }
    public string FileName { get; set; }
    public string Base64FileContent { get; set; }
}
public class Impersonation : IDisposable
{
    private IntPtr _userToken = IntPtr.Zero;
    private IntPtr _dupeTokenHandle = IntPtr.Zero;
    private bool _disposed = false;

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
                                        int dwLogonType, int dwLogonProvider, out IntPtr phToken);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int ImpersonationLevel, out IntPtr DuplicateTokenHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static bool SetThreadToken(IntPtr Thread, IntPtr Token);

    public Impersonation(string domain, string username, string password)
    {
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        const int SecurityImpersonation = 2;

        if (!LogonUser(username, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, out _userToken))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (!DuplicateToken(_userToken, SecurityImpersonation, out _dupeTokenHandle))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (!SetThreadToken(IntPtr.Zero, _dupeTokenHandle))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            SetThreadToken(IntPtr.Zero, IntPtr.Zero);

            if (_userToken != IntPtr.Zero)
            {
                CloseHandle(_userToken);
            }

            if (_dupeTokenHandle != IntPtr.Zero)
            {
                CloseHandle(_dupeTokenHandle);
            }

            _disposed = true;
        }
    }
}