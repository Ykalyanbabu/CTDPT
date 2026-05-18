using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

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
            if (Session["Userid"] == null)
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
            if (Session["Userid"] == null)
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
        public JsonResult GetPaymentPendingReturns(string ptin, string OwnerType)
        {
            var response = _ptdal.GetPaymentPendingMonthsYears(ptin, OwnerType);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReturnDetailsByReturnId(string ReturnId)
        {
            var response = _ptdal.GetReturnDetails(ReturnId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePTMonthlyReturn(PTReturnModel model)
        {
            if (Session["Tin"] == null)
            {
                RedirectToAction("Home", "PTHome");
            }
            if (model == null)
            {
                return Json(new SaveResponse
                {
                    success = false,
                    message = "Invalid Data"
                }, JsonRequestBehavior.AllowGet);
            }


            var response = _ptdal.SavePTMonthlyReturn(model);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePTYearlyPmt(PTReturnModel model)
        {
            if (Session["Tin"] == null)
            {
                RedirectToAction("Home", "PTHome");
            }
            if (model == null)
            {
                return Json(new SaveResponse
                {
                    success = false,
                    message = "Invalid Data"
                }, JsonRequestBehavior.AllowGet);
            }

            var response = _ptdal.SavePTYearlyPmtDetails(model);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateEmployees(string ptin, string below_15000, string between_15001_20000, string above_20000, string tot_emp)
        {
            try
            {
                string LoginUser = Session["Userid"].ToString();

                StatusResponse response = _ptdal.UpdateEmployees(ptin, below_15000, between_15001_20000, above_20000, tot_emp);

                if (response.Status == "Success")
                {
                    return Json(new { success = true, message = response.Message });
                }
                else
                {
                    return Json(new { success = false, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

    }
}