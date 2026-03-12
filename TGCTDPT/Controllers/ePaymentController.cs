using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;

namespace TGCTDPT.Controllers
{
    public class ePaymentController : Controller
    {
        // GET: ePayment
        private ReturnsDAL _rtdal = new ReturnsDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ePaymentGen()
        {
            return View();
        }

        public ActionResult ePaymentGenMonthly()
        {
            return View();
        }
        public ActionResult ePaymentGenYearly()
        {
            return View();
        }

        public JsonResult GetReturnDetails(string ptin)
        {
            var response = _rtdal.GetReturnDetails(ptin);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}