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
        private PTHomeDAL dal = new PTHomeDAL();
        UserDetails _User = new UserDetails();
        // GET: PTHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult checkRC()
        {
            return View();
        }
        public JsonResult GetPTEntityDetails(string ptin)
        {
            var response = dal.GetPTEntityDetails(ptin);
            return Json(response, JsonRequestBehavior.AllowGet);
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
            }
            
            return RedirectToAction("Dashboard", "PTHome");
        }
    }
}