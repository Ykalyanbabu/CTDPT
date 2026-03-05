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

namespace TGCTDPT.Controllers
{   
    public class PTHomeController : Controller
    {
        private PTHomeDAL dal = new PTHomeDAL();
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
            return View();
        }
        public ActionResult ChangePassword()
        {
            if (Session["Tin"] == null)
            {
                RedirectToAction("Login", "PTHome");
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
        public ActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
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
                mailResult = await SendLoginCredentials(Email, password);

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
        public ActionResult ChangeUserPassword(string UserId, string OldPassword,string NewPassword)
        {
            var RegDtls = _User.GetTINDtlsforRCPrinting(UserId);
            if (RegDtls == null || RegDtls.Count == 0)
            {
                ModelState.AddModelError("", "Not a valid PTIN");
                return View();
            }
            var users = _User.CheckTinRegistration(UserId);
            if (users == null || users.Count == 0)
            {
                ModelState.AddModelError("", "Not a registered PTIN");
                return View();
            }
            else
            {
                if (OldPassword != users[0].Password)
                {
                    ModelState.AddModelError("", "You are wrongly entered Current Password");
                    return View();
                }
                else
                {
                    string clientIPAddress = Request.UserHostAddress;
                    string result = _User.InsertPasswordForTrack(UserId, clientIPAddress, OldPassword, NewPassword, Session["Userid"].ToString());
                    if (result == "SUCCESS") 
                    {
                    
                    }
                }
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View();
        }
    }
}