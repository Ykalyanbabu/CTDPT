using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

namespace TGCTDPT.Controllers
{
    public class Modify_PTIN_StatusController : Controller
    {
        private RegistrationDAL dal = new RegistrationDAL();

        // GET: Modify_PTIN_Status
        public ActionResult Cancel_PTIN()
        {
            string StrTIN = Session["TIn"].ToString();
            RC_Cancel_ReActivate_Details rcd = dal.CanPTEntityDetails(StrTIN);
            return View(rcd);
        }

        public ActionResult Reactivate_PTIN(string StrTIN)
        {
             
            RC_Cancel_ReActivate_Details rcd = dal.CanPTEntityDetails(StrTIN);
            return View(rcd);
        }

        [HttpPost]
        public ActionResult SubmitRequest(RC_Cancel_ReActivate_Details model, HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    TempData["ErrorMessage"] = "Please upload file";
                    return RedirectToAction("Cancel_PTIN");
                }

                string fileName = Session["TIn"].ToString() + "_" + Path.GetFileName(file.FileName);
                /*string path = Server.MapPath("~/Uploads/Documents/Requests/RegCancel/") + fileName;*/
                string path = ConfigurationManager.AppSettings["DocumentsPathPTCancelReq"] + fileName;

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                file.SaveAs(path);

                model.doc_path = "/Requests/Cancel/" + fileName;
                model.created_by = Session["TIn"].ToString();
                model.FileName = fileName;

                if (model.registration_status == "REGD")
                {
                    model.request_status = "P";
                    model.new_status = "CNCL";

                    string json = JsonConvert.SerializeObject(model);

                    int result = dal.SaveCancelReactivateDetails(json);
                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Request Submitted Successfully.";
                        return RedirectToAction("Cancel_PTIN");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Cancellation Request Already Submitted Pending for Approval";
                        return RedirectToAction("Cancel_PTIN");

                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "TIN is already cancelled.";
                    return RedirectToAction("Cancel_PTIN");
                }


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Cancel_PTIN");
            }
        }
        //[HttpPost]
        //public ActionResult SubmitRequest(RC_Cancel_ReActivate_Details model, HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        if (file == null)
        //            return Json(new { success = false, message = "File required" });

        //        string fileName = Session["TIn"].ToString() + "_" + Path.GetFileName(file.FileName);
        //        string path = Server.MapPath("~/Uploads/Documents/Requests/RegCancel/") + fileName;

        //        Directory.CreateDirectory(Path.GetDirectoryName(path));
        //        file.SaveAs(path);

        //        model.doc_path = "/Uploads/Documents/Requests/RegCancel/" + fileName;
        //        model.created_by = Session["TIn"].ToString();//|| Session["user_id"].ToString();

        //        if (model.registration_status == "REGD")
        //        {
        //            model.request_status = "P";
        //            model.new_status = "CNCL";
        //        }
        //        else
        //        {
        //            //model.request_status = "REQ";
        //            //model.new_status = "REGD";
        //            return Json(new { success = false, message = "TIN is Already Cancelled." });
        //        }

        //        string json = JsonConvert.SerializeObject(model);

        //        string result = dal.SaveCancelReactivateDetails(json);

        //        return Json(new { success = true, message = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}



    }
}