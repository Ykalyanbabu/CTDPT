using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;
using TGCTDPT.Services;
using TGCTDPT.Helpers;
using System.Web.Security;
using TGCTDPT.BSNL_SMS;
using TGCTDPT.Mail_Services;

namespace TGCTDPT.Controllers
{   
    public class PTHomeController : Controller
    {
        private PTHomeDAL dal = new PTHomeDAL();
        private ClientHelpers _help = new ClientHelpers();
        UserDetails _User = new UserDetails();
        private readonly IEmailService _emailService;

        public PTHomeController()
        {
            _emailService = new EmailService();
        }
        // GET: PTHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            if (Request["expired"] == "true")
            {
                ViewBag.Message = "Your session has expired. Please login again.";
            }
            Session.Clear();
            Session.Abandon();
            return View();
        }
        public ActionResult PaymentType()
        {
            if (Session["Tin"] == null)
            {
                ViewBag.Layout = "~/Views/Shared/_OuterLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_InnerLayout.cshtml";
            }
            return View();
        }
        public ActionResult ChangePassword()
        {
            if (Session["Tin"] == null)
            {
                return RedirectToAction("Login", "PTHome");
            }
            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        public ActionResult Login(string ptin)
        {
            ViewBag.username = ptin;
            return View();
        }
        public ActionResult DeptLogin()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Login", "PTHome");
            }
            return View();
        }
        public ActionResult ViewPaymentReceipt()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeptLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Username and password are required.";
                ModelState.AddModelError("", "Username and password are required");
                return View();
            }

            string EncryptdPwd = _help.EncryptPwd(password);

            PTOfficer usd = _User.GetPTOfficerData(username, EncryptdPwd);

            if (usd.User_id != null)
            {   
                Session["Userid"] = usd.User_id;
                Session["CircleCode"] = usd.CircleCode;
                Session["DivisionCode"] = usd.DivisionCode;
                Session["Circle"] = usd.CircleName;
                Session["Division"] = usd.DivisionName;
                Session["DisplayName"] = usd.DisplayName;
                Session["Hierarchy"] = usd.Hierarchy;
                Session["ShortDesignationCode"] = usd.ShortDesignationCode;
                Session["LoginTime"] = DateTime.Now;
                return RedirectToAction("PTOHome", "PTOfficer");
            }
            TempData["ErrorMessage"] = "Invalid login attempt";
            ModelState.AddModelError("", "Invalid login attempt");
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            }

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Login", "PTHome");
        }
        public ActionResult DeptLogout()
        {
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            }

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("DeptLogin", "PTHome");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Username and password are required.";
                ModelState.AddModelError("", "Username and password are required");
                return View();
            }

            User usd = _User.GetUserData(username, password);

            if (usd.User_id != null)
            {
                Session["Tin"] = usd.User_id;
                Session["Userid"] = usd.User_id;
                Session["LoginTime"] = DateTime.Now;

                return RedirectToAction("Dashboard", "PTHome"); 
            }

            TempData["ErrorMessage"] = "Invalid login attempt";
            ModelState.AddModelError("", "Invalid login attempt");
            return View(); 
        }
        [HttpPost]
        public async Task <bool> SendLoginCredentials(string email, string password)
        {
            string subject = "Login Credentials for TG Profession Tax Registration";

            string body =
                "TG COMMERCIAL TAXES DEPARTMENT\n\n" +
                "Login Credentials for Dealer Services\n\n" +
                "Email: " + email + "\n" +
                "Password: " + password + "\n\n" +
                "Please do not reply to this email.";
            return await _emailService.SendAsync(email, subject, body);
        }
        public JsonResult GetPTINDetails(string PTIN)
        {
            var response = _User.GetPTINDtls(PTIN);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(string PTIN, string Email)
        {
            string n = Guid.NewGuid().ToString();
            string password = n.Substring(n.Length - 5);

            int Updresponse = _User.UpdatePassword(PTIN, password);
            bool mailResult = false;
            

            if (Updresponse > 0)
            {
                // mailResult = await SendLoginCredentials("bsairam3108@gmail.com", password);

                send_mail send_mail = new send_mail();
                mailResult = send_mail.PT_Send_Reset_Password(PTIN, password,Email);
                if (mailResult)
                {
                    return Json(new { success = true, message = "Password has been sent to your EmailID "+ Email });
                }
                else
                {
                    return Json(new { success = true, message = "Password Reset But Sending e-Mail Failed, Please contact CTD Department for Credentials" });
                }
            }
            return Json(new { success = false, message = "Password Update Failed" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string UserId, string OldPassword,string NewPassword,string ConfirmPassword)
        {
            if (Session["Tin"] == null)
            {
                return RedirectToAction("Login", "PTHome");
            }
            if (string.IsNullOrEmpty(UserId))
            {
                TempData["ErrorMessage"] = "Please enter PTIN/Username.";
                return View();
            }
            if (string.IsNullOrEmpty(OldPassword))
            {
                TempData["ErrorMessage"] = "Please enter Old Password";
                return View();
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                TempData["ErrorMessage"] = "Please enter New Password";
                return View();
            }
            if (string.IsNullOrEmpty(NewPassword)!= string.IsNullOrEmpty(ConfirmPassword))
            {
                TempData["ErrorMessage"] = "New Password and Confirm Password must match";
                return View();
            }
            var RegDtls = _User.GetTINDtlsforRCPrinting(UserId);
            if (RegDtls == null || RegDtls.Count == 0)
            {
                TempData["ErrorMessage"] = "The entered PTIN is invalid. Please check and try again.";
                return View();
            }
            var users = _User.CheckTinRegistration(UserId);
            if (users == null || users.Count == 0)
            {
                TempData["ErrorMessage"] = "The entered PTIN is not registered in the system.";
                return View();
            }
            else
            {
                if (OldPassword != users[0].Password)
                {
                    TempData["ErrorMessage"] = "The current password you entered is incorrect. Please try again.";
                    return View();
                }
                else
                {
                    string clientIPAddress = Request.UserHostAddress;
                    string result = _User.InsertPasswordForTrack(UserId, clientIPAddress, OldPassword, NewPassword, Session["Userid"].ToString());
                    if (result == "SUCCESS")
                    {
                        int response = _User.ChangePassword(UserId, NewPassword);
                        if (response > 0)
                        {
                            TempData["SuccessMessage"] = "Password changed successfully.";
                            return View();
                        }
                        else 
                        {
                            TempData["ErrorMessage"] = "Action Failed please try after sometime or Contact System Administrator.";
                            return View();
                        }
                    }
                    else 
                    {
                        TempData["ErrorMessage"] = "Action Failed please try after sometime or Contact System Administrator.";
                        return View();
                    }
                }
            }
        }
        public ActionResult checkRC()
        {
            string StrTIN = Session["TIn"].ToString();
            RC_Details rcd = dal.GetPTEntityDetails(StrTIN);
            return View(rcd);
        }
        public ActionResult PrintRC()
        {
            string StrTIN = Session["TIn"].ToString();
            RC_Details model = dal.GetPTEntityDetails(StrTIN);

            string fileName = "PT_Registration_Certificate_" + model.prof_tin + ".pdf";

            Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);

            return View("profession_tax_certificate", model);
            //return new ViewAsPdf("profession_tax_certificate", model)
            //{
            //    FileName = fileName,
            //    PageSize = Size.A4,
            //    PageMargins = new Margins(10, 10, 10, 10)
            //};
        }
        public JsonResult GetSessionStatus()
        {
            string IsSessionExpired = "N";

            if (Session["Userid"] != null)
            {
                IsSessionExpired = "N";
            }
            else 
            {
                IsSessionExpired = "Y";
            }
            
            return Json(IsSessionExpired, JsonRequestBehavior.AllowGet);
        }
    }
}