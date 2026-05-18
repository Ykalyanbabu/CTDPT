using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;
namespace TGCTDPT.Controllers
{
    public class PTOAmendController : Controller
    {
        private AmendmentsDAL _dal = new AmendmentsDAL();
        // GET: PTOAmend
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Requests()
        {
            return View();
        }
        public ActionResult ViewApplication()
        {
            return View();
        }
        public ActionResult ViewRequest()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPendingApplications(string Circle)
        {
            Circle = Session["CircleCode"].ToString();
            var data = _dal.GetPendingRequests(Circle);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}