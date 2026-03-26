using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;


namespace TGCTDPT.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        private RegistrationDAL dal = new RegistrationDAL();
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
        public ActionResult eRegistration()
        {
            return View();
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
        public JsonResult GetBanks()
        {

            var response = dal.Loadbanks();
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

                if (files.Count == 0)
                {
                    return Json(new { success = false, message = "No files uploaded" });
                }

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
                    var file = files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        if (!file.FileName.ToLower().EndsWith(".pdf"))
                        {
                            return Json(new { success = false, message = "Only PDF files allowed" });
                        }
                        string fileName = Path.GetFileName(file.FileName);



                        string fullPath = Path.Combine(appFolder, fileName);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        file.SaveAs(fullPath);

                        finalList.Add(new documents_dtls
                        {
                            application_id = appId,
                            master_doc_id = model[i].master_doc_id,
                            document_type = model[i].document_type,
                            document_path = "/Uploads/Documents/Registration_docs/" + appId + "/" + fileName,

                        });
                    }
                }

                // ✅ Convert to JSON
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(finalList);

                // ✅ Call stored procedure (bulk insert)
                var response = dal.SaveDocuments(jsonData);

                return Json(new { success = true, message= response });
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
    }
}