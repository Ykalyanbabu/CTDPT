using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;

namespace TGCTDPT.Controllers
{
    public class HomeController : Controller
    {
        private CommonDAL dal = new CommonDAL();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult checkRC()
        {
            return View();
        }

        public ActionResult PrintCertificate()
        {
            return View();
        }
        public ActionResult SearchARN()
        {
            return View();
        }

        public JsonResult GetRNRDetails(string RNR)
        {
            var response = dal.GetRNRDetails(RNR);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}