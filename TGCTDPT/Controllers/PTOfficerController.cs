using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;
using TGCTDPT.BSNL_SMS;
using TGCTDPT.Mail_Services;
using System.IO;

namespace TGCTDPT.Controllers
{
    public class PTOfficerController : Controller
    {
        // GET: PTOfficer
        private readonly FormIISummaryDAL _dal = new FormIISummaryDAL();
        private readonly RegistrationDAL dal = new RegistrationDAL();
        private readonly PaymentsDAL pdal = new PaymentsDAL();
        private readonly CommonDAL cdal = new CommonDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PTOHome()
        {
            if (Session["Userid"] == null)
            {
                TempData["SessionExpired"] = "Your session has expired. Please login again.";
                return RedirectToAction("Home", "PTHome");
            }
            return View();
        }
        public ActionResult PendingApprovals()
        {
            if (Session["Userid"] == null)
            {
                TempData["SessionExpired"] = "Your session has expired. Please login again.";
                return RedirectToAction("Home", "PTHome");
            }
            return View();
        }
        public ActionResult ViewApplication(string rnr)
        {
            if (Session["Userid"] == null)
            {
                TempData["SessionExpired"] = "Your session has expired. Please login again.";
                return RedirectToAction("Home", "PTHome");
            }
            ViewBag.rnr = rnr;
            return View();
        }
        public ActionResult ChallanEntry()
        {
            if (Session["Userid"] == null)
            {
                TempData["SessionExpired"] = "Your session has expired. Please login again.";
                return RedirectToAction("Home", "PTHome");
            }
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
                string Circle = Session["CircleCode"].ToString();
                var data = _dal.GetFullSummary(rnr, Circle);

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
            string errorMsg;
            string userId = Session["UserID"]?.ToString();
            if (rnr == null)
            {
                return Json(new Response
                {
                    success = false,
                    message = "Invalid Data"
                }, JsonRequestBehavior.AllowGet);
            }
            var strPtin = _dal.GeneratePTIN("PT", userId);
            var response = ""; bool res = false;
            if (strPtin != "" && strPtin != null)
            {
                response = _dal.ApproveRNR(rnr, strPtin, userId, out errorMsg);
                
                if (response == "Success")
                {
                    var data = _dal.GetRNR_PT_userid_pwd(rnr);
                    string ptin = data.prof_tin;
                    string password = data.password;
                    string email = data.email;


                    if (response.ToString() != null)
                    {
                        send_mail send_mail = new send_mail();
                        send_mail.PT_Send_Application_Approved(ptin, password, email);
                    }
                    response = strPtin;
                    res = true;
                }
                else {
                    res = false;
                }
            }
            return Json(new { success = res, data = response });
        }

        [HttpPost]
        public JsonResult ChangeApplicationStatus(string rnr,string comments)
        {
            try
            {
                string LoginUser = Session["Userid"].ToString();

                StatusResponse response = _dal.ChangeApplicationStatus(rnr,comments,"R",LoginUser);

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
        [HttpPost]
        public JsonResult TransferApplication(string rnr, string division,string circle)
        {
            try
            {
                string LoginUser = Session["Userid"].ToString();

                StatusResponse response = _dal.TransferApplication(rnr, division, circle, LoginUser);

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
        public ActionResult Pending_Cancel_Revoke_Requests()
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            string user_id = Session["UserID"]?.ToString();
            var data = _dal.GetPendingRequests(user_id);
            return View(data);
        }

        public ActionResult RequestDetails(int id)
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            var data = _dal.GetRequestDetails(id); 
            return View(data);
        }
        [HttpPost]
        public JsonResult ApprovePTINCancelRequest(int id, string r_status)
        {
            try
            {
                string user_id = Session["UserID"]?.ToString();
                var result = _dal.ApproveCancellation(id, r_status, user_id);
                if (result.result == 1)
                {
                    return Json(new { success = true, message = result.message });
                }
                else
                {
                    return Json(new { success = false, message = result.message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        public ActionResult Reactivate_PTIN()
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            //string StrTIN = Session["Tin"].ToString();
            //RC_Cancel_ReActivate_Details rcd = _dal.ReactivatePTEntityDetails(StrTIN);
            RC_Cancel_ReActivate_Details rcd = new RC_Cancel_ReActivate_Details();
            return View(rcd);
        }

        [HttpGet]
        public JsonResult ReactivatePTIN(string ptin)
        {
            var rcd = _dal.ReactivatePTEntityDetails(ptin);
            Session["TIn"] = ptin;
            return Json(new
            {
                enterprise_name = rcd.enterprise_name,
                division_name = rcd.division_name,
                circle_name = rcd.circle_name,
                request_id = rcd.request_id,
                registration_status = rcd.registration_status,
                edr = rcd.edr.HasValue ? rcd.edr.Value.ToString("dd/MM/yyyy") : ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReactivationRequest(RC_Cancel_ReActivate_Details model, HttpPostedFileBase file)
        {
            if (Session["UserId"] == null || Session["TIn"] == null)
            {
                return RedirectToAction("DeptLogin","PTHome");
            }

            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    TempData["ErrorMessage"] = "Please upload file";
                    return RedirectToAction("Reactivate_PTIN");
                }

                string tin = Session["TIn"].ToString();
                string userId = Session["UserId"].ToString();

                string fileName = tin + "_" + Path.GetFileName(file.FileName);

                string folderPath = Server.MapPath("~/Uploads/Documents/Requests/RegRevoke/");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(folderPath, fileName);

                file.SaveAs(fullPath);

                model.doc_path = "/Uploads/Documents/Requests/RegRevoke/" + fileName;

                model.created_by = userId;

                if (model.registration_status == "CNCL")
                {
                    model.request_status = "A";
                    model.new_status = "REGD";
                    model.registration_status = "REGD";
                      

                    string json = JsonConvert.SerializeObject(model);

                    int result = _dal.SaveReactivateDetails(json);

                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Request Submitted Successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Cancellation Request Already Submitted Pending for Approval";
                    }

                    return RedirectToAction("Reactivate_PTIN");
                }
                else
                {
                    TempData["ErrorMessage"] = "TIN is already cancelled.";
                    return RedirectToAction("Reactivate_PTIN");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Reactivate_PTIN");
            }
        }

        [HttpGet]
        public JsonResult GetProftinDetails(string ptin)
        {
            var response = pdal.GetProftinDetails(ptin, Session["Userid"].ToString());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReturnDetails(string Ptin,string Type)
        {
            var response = pdal.GetReturnDetails(Ptin, Type);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDDOCodes(string Ptin)
        {
            var response = cdal.GetDdoCodes(Ptin);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SubmitChallanDetails(string Ptin, string FromDate, string ToDate, string InsType, string ChallanNo, string ChallanDate,
            string InsNo, string InsDate,string Purpose, string Amount, string Bank, string DdoCode, string StoCode, string FormType, string ReturnId)
        {
            try
            {
                if (Session["Userid"] == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Session expired. Please login again.",
                        redirectToLogin = true
                    });
                }

                string paymentID = DateTime.Now.ToString("yyyyMMddssffff");

                var paymentId = pdal.InsertChallanPaymentDetails(
                    paymentID,
                    Ptin,
                    FromDate,
                    ToDate,
                    InsType,
                    ChallanNo,
                    ChallanDate,
                    InsNo,
                    InsDate, Purpose, Amount, Bank, DdoCode, StoCode, FormType, ReturnId, Session["Userid"].ToString()
                );

                if (string.IsNullOrEmpty(paymentId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to Save Details. Please try again."
                    });
                }

                return Json(new
                {
                    success = true,
                    message = paymentID
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error processing : " + ex.Message
                });
            }
        }
    }

}