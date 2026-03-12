using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

namespace TGCTDPT.Controllers
{
    public class ReturnController : Controller
    {
        // GET: Return

        private ReturnsDAL dal = new ReturnsDAL();
        public ActionResult Return()
        {
            if (Session["Tin"] == null) 
            {
                return RedirectToAction("Home", "PTHome");
            }
            return View();
        }
        public JsonResult GetSlabs()
        {
            var response = dal.GetSlabs();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReturnDetails()
        {
            if (Session["Tin"] == null)
            {
                 RedirectToAction("Home", "PTHome");
            }
            var response = dal.GetReturnDetails(Session["Tin"].ToString());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPTGstReturnDetails(string ptin,string type)
        {
            var response = dal.GetPTGstReturnDetails(ptin, type);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPTTaxReturnDetails(string ptin,string flag, string type)
        {
            var response = dal.GetPTTaxReturnDetails(ptin, flag, type);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPTEnterpriseDetails(string ptin)
        {
            var response = dal.GetPTEnterpriseDetails(ptin);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPTEntityDetails(string ptin)
        {
            var response = dal.GetPTEntityDetails(ptin);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePTReturn(PTReturnModel model)
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


            var response = dal.SavePTReturnDetails(model);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePTReturnYearly(PTReturnModel model)
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

            var response = dal.SavePTReturnYearlyDetails(model);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}