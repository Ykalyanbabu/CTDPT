using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;

namespace TGCTDPT.Controllers
{
    public class AmendmentsController : Controller
    {
        // GET: Amendments
        private AmendmentsDAL _dal = new AmendmentsDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AmendmentRequest()
        {
            return View();
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
        [HttpGet]
        public JsonResult GetAmendments()
        {
            string UserId = Session["Tin"].ToString();
            var data = _dal.GetAmendmentsList(UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetApplicationDtls(string ptin)
        {
            if (string.IsNullOrWhiteSpace(ptin))
                return JsonError("Ptin is required.");

            try
            {
                var data = _dal.GetApplicationDtls(ptin);

                if (data.BusinessDetails == null)
                    return JsonError($"No record found for Ptin: {ptin}");

                return JsonSuccess(data);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
    }
}