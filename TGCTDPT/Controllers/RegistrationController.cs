using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;

namespace TGCTDPT.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        private RegistrationDAL dal = new RegistrationDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Landing()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult eRegistration()
        {
            return View();
        }

        public JsonResult CheckPantoPT(string PAN)
        {
            var response = dal.CheckPantoPT(PAN);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}