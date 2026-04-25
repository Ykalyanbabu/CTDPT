using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.BSNL_SMS;
using TGCTDPT.DAL;
using TGCTDPT.Models;


namespace TGCTDPT.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        private RegistrationDAL dal = new RegistrationDAL();
        private CommonDAL cdal = new CommonDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Landing()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult RegistrationStatus()
        {
            return View();
        }
        public ActionResult eRegistration()
        {
            return View();
        }
        public ActionResult checkRC()
        {
            string StrTIN = Session["TIn"].ToString();
            RC_Details rcd = dal.GetPTEntityDetails(StrTIN);
            return View(rcd);
        }
        public ActionResult PrintCertificate()
        {
            string StrTIN = Session["TIn"]?.ToString();

            if (string.IsNullOrEmpty(StrTIN))
            {
                return RedirectToAction("Login");
            }

            RC_Details model = dal.GetPTEntityDetails(StrTIN);

            if (model == null)
            {
                return Content("No data found");
            }

            string fileName = "PT_Registration_Certificate_" + model.prof_tin + ".pdf";
            return View("PrintCertificate", model);
            
        }

        public JsonResult CheckPantoPT(string PAN)
        {
            var response = dal.CheckPantoPT(PAN);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDivisions()
        {

            var response = dal.LoadDivisions();
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetDistricts()
        {

            var response = dal.Loadistricts();
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetCirclesByDivision(string divisionId)
        {

            var response = dal.LoadCircles(divisionId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetCountryStates(string Type)
        {
            var response = cdal.GetCountryState(Type);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBanks()
        {

            var response = dal.Loadbanks();
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        
        //public ActionResult Mail_Registration()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SubmitMailRegistration(mail_registation m)
        //{
        //    Random generator = new Random();
        //    string rnd= generator.Next(1000,9999).ToString(); 
        //    return null;
        //}
        [HttpPost]
        public JsonResult SaveBusinessDetails(Business_dtls model)
        {
            try
            {
                if (Session["application_id"] != null)
                {
                    model.application_id = Session["application_id"].ToString();
                }
                var response = dal.SaveBusinessDetails(model);
                if (Session["application_id"] == null)
                {
                    Session["application_id"] = response.application_id.ToString();
                }
                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveEmployeeDetails(employee_dtls model)
        {
            try
            {
                if (Session["application_id"] != null)
                {
                    model.application_id = Session["application_id"].ToString();
                }
                else {
                    return Json(new { success = false, message = "Unable to Save Data" });
                }
                var response = dal.SaveEmployeeDetails(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Saveownr_mdDetails(ownr_md_dtls model)
        {
            try
            {
                if (Session["application_id"] != null)
                {
                    model.application_id = Session["application_id"].ToString();
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Save Data" });
                }
                var response = dal.Saveownr_mdDetails(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult Save_AuthrsedPrsn_Details(auth_prsn_dtls model)
        {
            try
            {
                if (Session["application_id"] != null)
                {
                    model.application_id = Session["application_id"].ToString();
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Save Data" });
                }
                var response = dal.Save_AuthrsedPrsn_Details(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        //[HttpPost]
        //public JsonResult Save_DirPrtnr_Details(DirPrtnrWrapper model)
        //{
        //    try
        //    {
        //        if (Session["application_id"] != null)
        //        {
        //            string appId = Session["application_id"].ToString();
        //            foreach (var item in model)
        //            {
        //                item.application_id = "36180326125765";//appId;
        //            }
        //        }
        //        var response = dal.Save_dir_prtnr_Details(model);

        //        return Json(new { success = true, data = response });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}
        [HttpPost]
        public JsonResult Save_DirPrtnr_Details(DirPrtnrWrapper request)
        {
            try
            {
                var model = request.model;

                if (Session["application_id"] != null)
                {
                    string appId = Session["application_id"].ToString();

                    foreach (var item in model)
                    {
                        item.application_id = appId;
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Save Data" });
                }

                var response = dal.Save_dir_prtnr_Details(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Save_Addl_Place_Details(AddlPlaceWrapper request)
        {
            try
            {
                var model = request.model;

                if (Session["application_id"] != null)
                {
                    string appId = Session["application_id"].ToString();

                    foreach (var item in model)
                    {
                        item.application_id = appId;
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Save Data" });
                }

                var response = dal.Save_addl_place_Details(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Save_bank_Details(BankdtlsWrapper request)
        {
            try
            {
                var model = request.model;

                if (Session["application_id"] != null)
                {
                    string appId = Session["application_id"].ToString();

                    foreach (var item in model)
                    {
                        item.application_id = appId;
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Save Data" });
                }

                var response = dal.Save_Bank_Details(model);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Save_Documents(List<documents_dtls> model)
        {
            string basePath = "";

            try
            {
                if (model == null || model.Count == 0)
                {
                    return Json(new { success = false, message = "No data received" });
                }

                if (Session["application_id"] == null)
                {
                    return Json(new { success = false, message = "Session expired" });
                }

                string appId = Session["application_id"].ToString();

                var files = Request.Files;

                basePath = Server.MapPath("~/Uploads/Documents/Registration_docs/");

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                string appFolder = Path.Combine(basePath, appId);

                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                }

                List<documents_dtls> finalList = new List<documents_dtls>();

                for (int i = 0; i < model.Count; i++)
                {
                    HttpPostedFileBase file = null;

                    if (files.Count > i)
                    {
                        file = files[i];
                    }

                    string filePath = "";

                    if (file != null && file.ContentLength > 0)
                    {
                        if (!file.FileName.ToLower().EndsWith(".pdf"))
                        {
                            return Json(new { success = false, message = "Only PDF files allowed" });
                        }

                        string masterdocFolder = model[i].master_doc_id.ToString(); 

                        string subFolderPath = Path.Combine(appFolder, masterdocFolder);

                        if (!Directory.Exists(subFolderPath))
                        {
                            Directory.CreateDirectory(subFolderPath);
                        }

                        string fileName = Path.GetFileName(file.FileName);
                        string fullPath = Path.Combine(subFolderPath, fileName);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        file.SaveAs(fullPath);

                        filePath = "/Uploads/Documents/Registration_docs/"
                                   + appId + "/"
                                   + masterdocFolder + "/"
                                   + fileName;
                    }
                    else
                    {
                        //  Existing File 
                        filePath = model[i].document_path;
                    }

                    finalList.Add(new documents_dtls
                    {
                        application_id = appId,
                        master_doc_id = model[i].master_doc_id,
                        document_type = model[i].document_type,
                        document_path = filePath
                    });
                }

                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(finalList);

                var response = dal.SaveDocuments(jsonData);

                return Json(new { success = true, message = response });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error: " + ex.Message + " | Path: " + basePath
                });
            }
        }

        [HttpPost]
        public JsonResult SubmitApplication(string AppId)
        {
            try
            {
               
                if (Session["application_id"] != null)
                {
                     AppId = Session["application_id"].ToString();
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Submit the Application" });
                }

                var response = dal.GenerateRNR(AppId);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ReSubmitApplication(string AppId)
        {
            try
            {

                if (Session["application_id"] != null)
                {
                    AppId = Session["application_id"].ToString();
                }
                else
                {
                    return Json(new { success = false, message = "Unable to Submit the Application" });
                }

                var response = dal.ReSubmitApplication(AppId);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetFullSummary(string ApplicationId)
        {
            if (string.IsNullOrWhiteSpace(ApplicationId))
                return JsonError("ApplicationId is required.");

            try
            {
                var data = dal.GetFullSummary(ApplicationId);

                if (data.BusinessDetails == null)
                    return JsonError($"No record found for RNR: {ApplicationId}");

                return JsonSuccess(data);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult GetApplicationDtls(string ApplicationId)
        {
            if (string.IsNullOrWhiteSpace(ApplicationId))
                return JsonError("ApplicationId is required.");

            try
            {
                var data = dal.GetApplicationDtls(ApplicationId);

                if (data.BusinessDetails == null)
                    return JsonError($"No record found for ApplicaionId: {ApplicationId}");

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


        public ActionResult New_applicant_Login()
        {
            return View();
        }

        [Route("Registration/Mail_Registration")]
        public ActionResult Mail_Registration()
        {
            return View("~/Views/Registration/Mail_Registration.cshtml");
        }



        [HttpPost]
        public string PT_SendOtp(application_status u)
        {
            string response = "";
            try
            {
                if (u == null || string.IsNullOrWhiteSpace(u.application_id) || string.IsNullOrWhiteSpace(u.Mobile_No))
                {
                    return response = "Application ID and Mobile Number are Required";
                }

                var u1 = dal.Getapplicant(u);

                if (u1 == null || string.IsNullOrWhiteSpace(u1.application_id))
                {
                    return response = "Invalid Application ID / Mobile Number";
                }

                var dtls = dal.GetLastOTPSpan(u1.application_id, u1.Mobile_No);

                if (dtls != null && dtls.Tables.Count > 0 && dtls.Tables[0].Rows.Count > 0)
                {
                    return response = "OTP already sent. Please wait.";
                }
                string rndnum = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 4).ToUpper();

                int rowsEffected = SaveOTP(u1.application_id, u1.Mobile_No, rndnum);

                if (rowsEffected <= 0)
                {
                    return response = "Failed to generate OTP";
                }

                 SMS_BSNL sm_service = new SMS_BSNL();
             string smsResponse = sm_service.Track_Reports_Dashboard_Login_OTP(u1.Mobile_No, rndnum, "5");
                //string smsResponse = sm_service.Track_Reports_Dashboard_Login_OTP(u1.Mobile_No, rndnum );
                //string smsResponse = sm_service.smstest1(u1.Mobile_No, rndnum, "5");-- need to call this method
                //string smsResponse = "Success";
                if (smsResponse == "Success")
                {
                    return response = "OTP sent successfully";
                }
                else
                {
                    return response = "SMS sending failed";
                }
            }
            catch (Exception ex)
            {
                return response = "Something went wrong";
            }
        }
        public int SaveOTP(string application_id, string Mobile_No, string rndnum)
        {
            try
            {
                var response = dal.Save_PT_OTP(application_id, Mobile_No, rndnum);

                return response;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public JsonResult GetApplicationStatus(string ApplicationId)
        {
            var response = dal.GetApplicationStatus(ApplicationId);
            Session["Email"] = response[0].email_id;
            Session["RNR"] = response[0].rnr_number;
            Session["AppStatus"] = response[0].AppStatus;
            Session["UserID"] = "Online";
            Session["QuerySts"] = response[0].query_status;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQueryDetails(string rnr)
        {
            var response = dal.GetQueryDetails(rnr);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string PT_Applicant_Login(application_status u)
        {
            string response = "";
            try
            {
                if (!string.IsNullOrEmpty(u.application_id) && !string.IsNullOrEmpty(u.Mobile_No) && !string.IsNullOrEmpty(u.otp))
                {

                    application_status u2 = dal.Getapplicant(u);
                    if (u2 != null)
                    {
                        string mobile = u.Mobile_No;
                        if (u2.Mobile_No == mobile)
                        {
                            DataSet dtls = dal.GetLastOTPSpan(u2.application_id, u2.Mobile_No);
                            if (dtls != null && dtls.Tables[0].Rows.Count > 0)
                            {
                                string otp = dtls.Tables[0].Rows[0]["otp"].ToString();
                                if (otp == u.otp)
                                {
                                    Session["application_id"] = u2.application_id;
                                    response = "success";
                                    return response;
                                }
                                else
                                {
                                    response = "Please Enter Valid OTP";
                                    return response;
                                }
                            }
                            else
                            {

                                response = "OTP Expired Please Try to Generate OTP again";
                                return response;
                            }

                        }
                        else
                        {
                            response = "Please Enter Registrered Mobile Number";
                            return response;
                        }
                    }
                    else
                    {
                        response = "Please Enter Valid Application Number and Registered Mobile Number";
                        return response;
                    }
                }
                else
                {
                    response = "Application Number and Registered Mobile Number are Required";
                    return response;
                }
            }
            catch (Exception ex)
            {
                return response;
            }
            //ViewData["status"] = "InValid Details";

        }


        public string Send_Sms_PT(string mobile, string otp, string minutes)
        {
            try
            {
                Content_Template_cls t = new Content_Template_cls();

                t.Header = "TGCCTD";

                List<object> s = new List<object>();

                s.Add(mobile);
                t.Target = mobile;
                t.Is_Unicode = "0";
                t.Is_Flash = "0";
                t.Message_Type = "SI";
                t.Entity_Id = "1401565020000014412";
                t.Content_Template_Id = "1407173918038651698";

                Template_Keys_and_Values tt = new Template_Keys_and_Values();

                tt.Key = "otp";
                tt.Value = otp;

                t.Template_Keys_and_Values.Add(tt);

                tt = new Template_Keys_and_Values();

                tt.Key = "module";
                tt.Value = "Dashboard Login";

                t.Template_Keys_and_Values.Add(tt);


                tt = new Template_Keys_and_Values();

                tt.Key = "min";
                tt.Value = minutes;
                t.Template_Keys_and_Values.Add(tt);

                string jsonResponse = JsonConvert.SerializeObject(t);

                string finalUrl = "https://bulksms.bsnl.in:5010/api/Send_SMS";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(finalUrl);
                request.Method = "POST";

                string skey = ConfigurationManager.AppSettings["Skey"].ToString();
                //string skey ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEwMzc1IDEiLCJuYmYiOjE3MTYyOTI3NzQsImV4cCI6MTc0NzgyODc3NCwiaWF0IjoxNzE2MjkyNzc0LCJpc3MiOiJodHRwczovL2J1bGtzbXMuYnNubC5pbjo1MDEwIiwiYXVkIjoiMTAzNzUgMSJ9.QHsSIMce1kqy167howAzDZ_ves87FMAU13braZwIj74";
                request.Headers.Add("Authorization", skey);

                request.ContentType = "application/json ; charset=utf-8";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonResponse);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}


public class Template_Keys_and_Values
{
    public string Key { get; set; }
    public string Value { get; set; }
}


public class Content_Template_cls
{
    public string Content_Template_Id { get; set; }
    // public string Content_Template_Name { get; set; }
    public string Header { get; set; }
    public string Target { get; set; }

    public string Is_Unicode { get; set; }
    public string Is_Flash { get; set; }
    public string Message_Type { get; set; }
    public string Entity_Id { get; set; }

    public List<Template_Keys_and_Values> Template_Keys_and_Values = new List<Template_Keys_and_Values>();


}