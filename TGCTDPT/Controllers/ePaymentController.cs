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
        private ePaymentDAL _ptdal = new ePaymentDAL();
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
        public ActionResult ePaymentGenYearly()
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

        public JsonResult GetReturnDetails(string ptin)
        {
            var response = _rtdal.GetReturnDetails(ptin);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPaymentPendingReturns(string ptin,string OwnerType)
        {
            var response = _ptdal.GetPaymentPendingReturns(ptin, OwnerType);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReturnDetailsByReturnId(string ReturnId)
        {
            var response = _ptdal.GetReturnDetails(ReturnId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}