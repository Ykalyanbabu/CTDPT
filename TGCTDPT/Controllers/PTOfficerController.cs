using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

namespace TGCTDPT.Controllers
{
    public class PTOfficerController : Controller
    {
        // GET: PTOfficer
        private readonly FormIISummaryDAL _dal = new FormIISummaryDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PTOHome()
        {
            return View();
        }
        public ActionResult PendingApprovals()
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            return View();
        }
        public ActionResult ViewApplication(string rnr)
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            ViewBag.rnr = rnr;
            return View();
        }
        
        [HttpGet]
        public JsonResult GetPendingApplications(string Circle)
        {
            Circle = Session["CircleCode"].ToString();
            var data = _dal.GetPendingApplications(Circle);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFullSummary(string rnr)
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            if (string.IsNullOrWhiteSpace(rnr))
                return JsonError("RNR is required.");

            try
            {
                var data = _dal.GetFullSummary(rnr);

                if (data.BusinessDetails == null)
                    return JsonError($"No record found for RNR: {rnr}");

                return JsonSuccess(data);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        private ActionResult JsonSuccess(object data)
        {
            var json = JsonConvert.SerializeObject(
                new { success = true, data },
                new JsonSerializerSettings
                {  
                    DateFormatString = "dd-MM-yyyy",
                    NullValueHandling = NullValueHandling.Include
                });

            return Content(json, "application/json");
        }

        private ActionResult JsonError(string message)
        {
            var json = JsonConvert.SerializeObject(new { success = false, message });
            return Content(json, "application/json");
        }

        public ActionResult Error(string msg)
        {
            ViewBag.ErrorMessage = msg;
            return View();
        }

        [HttpPost]
        public JsonResult ApprooveApplication(string rnr)
        {   
            if (rnr == null)
            {
                return Json(new Response
                {
                    success = false,
                    message = "Invalid Data"
                }, JsonRequestBehavior.AllowGet);
            }
            var response = _dal.GeneratePTIN("PT",Session["Userid"].ToString());
            /* var response = "";
             if (strPtin != "" && strPtin != null)
             {
                  response = _dal.SavePTReturnYearlyDetails(rnr);
             }*/
            return Json(new { success = true, data = response });
        }
    }
    
}