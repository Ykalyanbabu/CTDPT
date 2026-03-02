using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

namespace TGCTDPT.Controllers
{   
    public class PTHomeController : Controller
    {
        UserDetails _User = new UserDetails();
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
        public ActionResult Logout()
        {
           
            return RedirectToAction("Home", "PTHome");
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
            if (usd != null)
            {
                Session["Tin"] = usd.User_id;
                Session["Userid"] = usd.User_id;
                Session["LoginTime"] = DateTime.Now;
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return RedirectToAction("Dashboard", "PTHome");
        }
    }
}