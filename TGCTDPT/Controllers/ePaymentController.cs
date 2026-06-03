using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
        private PaymentsDAL dal = new PaymentsDAL();
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
                ViewBag.Layout = "~/Views/Shared/_InnerLayout1.cshtml";
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
                ViewBag.Layout = "~/Views/Shared/_InnerLayout1.cshtml";
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
            /*if (Session["Tin"] == null)
            {
                RedirectToAction("Home", "PTHome");
            }*/
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
            /*if (Session["Tin"] == null)
            {
                RedirectToAction("Home", "PTHome");
            }*/
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
        [HttpGet]
        public JsonResult GetCyberChallanDetails(string depttransid)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string deptcode = ConfigurationManager.AppSettings["deptcode"];

                string apiUrl =
                    "https://ifmis.telangana.gov.in/payment/get_cyber_challan_details" +
                    "?deptcode=" + deptcode +
                    "&depttransid=" + depttransid;

                HttpWebRequest request =
                    (HttpWebRequest)WebRequest.Create(apiUrl);

                request.Method = "GET";
                request.ContentType = "application/json";

                using (HttpWebResponse response =
                    (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader =
                        new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();

                        var apiResponse =
                         JsonConvert.DeserializeObject<CyberChallanResponse>(result);

                        var challan = apiResponse.challandetails[0];

                        var dbData = dal.GetDeatailsbyPaymentId(depttransid);

                        if (dbData != null)
                        {
                            challan.enterprisename = dbData.EnterpriseName;
                            challan.typeoftax = dbData.TypeofTax;
                            challan.taxpurpose = dbData.TaxPurpose;
                            challan.remittersname = dbData.Ptin;
                            challan.returnperiod = dbData.ReturnPeriod;
                        }

                        return Json(new
                        {
                            success = true,
                            data = challan
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                },
                JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPaymentTransactionDtls(string TransactionId)
        {
            var response = dal.GetTransactionDetails(TransactionId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateEmpDirector(string ptin)
        {

            try
            {
                emp_dir_prtnr model = new emp_dir_prtnr();
                string LoginUser = Session["Userid"].ToString();
                if (Session["Userid"] == null)
                {
                    return RedirectToAction("Login", "PTHome");
                }
                model = _ptdal.GetEntitydetails(LoginUser);

                return View(model);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Dashboard", "PTHome");
            }

        }
        [HttpPost]
        public ActionResult UpdateEmpDirector(emp_dir_prtnr model)
        {
            try
            {
                string LoginUser = Session["Userid"].ToString();
                model.inserted_userid = LoginUser;

                StatusResponse response = _ptdal.UpdateEmpDirector(model);

                if (response.Status == "Success")
                {
                    TempData["Message"] = response.Message;
                    TempData["MessageType"] = "Success";

                    return RedirectToAction("UpdateEmpDirector", "ePayment");
                }
                else
                {
                    TempData["Message"] = response.Message;
                    TempData["MessageType"] = "Error";

                    return RedirectToAction("UpdateEmpDirector", "ePayment");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                TempData["MessageType"] = "Error";

                return RedirectToAction("UpdateEmpDirector", "ePayment");
            }
        }

    }
}