using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;
using TGCTDPT.Helpers;

namespace TGCTDPT.Controllers
{
    public class RefundsController : Controller
    {
        // GET: Refunds
        private RefundsDAL dal = new RefundsDAL();
        private ClientHelpers hlp = new ClientHelpers();
        public ActionResult Refunds()
        {
            if (Session["Tin"] == null)
            {
               return RedirectToAction("Login", "PTHome");
            }
            return View();
        }
        public JsonResult GetDocumetsCheckList(string Type)
        {
            var response = dal.GetDocumetsCheckList(Type);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SubmitRefund(PtRefundClaimModel model,List<HttpPostedFileBase> files,List<int> docIds)
        {
            try
            {
                string ptin = Session["Tin"].ToString();
                model.prof_tin = ptin;
                string basePath = Server.MapPath(ConfigurationManager.AppSettings["DocumentsPathPTRefunds"]);

                hlp.CreateDirectoryIfNotExists(basePath, ptin);

                if (files != null && files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var fileUpload = files[i];
                        int docId = docIds[i];

                        if (fileUpload != null && fileUpload.ContentLength > 0)
                        {
                            string fileExt = Path.GetExtension(fileUpload.FileName).ToLower();

                            if (fileExt == ".jpeg" || fileExt == ".jpg" || fileExt == ".png" || fileExt == ".pdf")
                            {
                                if (fileUpload.ContentLength < 500000)
                                {
                                    string fileName = docId + "_" + Path.GetFileName(fileUpload.FileName);
                                    string FilePath = Path.Combine(ptin, fileName);
                                    string fullPath = Path.Combine(basePath, ptin, fileName);

                                    fileUpload.SaveAs(fullPath);
                                    RefundUploadDoc filemodel = new RefundUploadDoc();
                                    filemodel.ProfTin = ptin;
                                    filemodel.DocId = docId;
                                    filemodel.FileName = fileName;
                                    filemodel.FilePath = FilePath;
                                    filemodel.CreatedBy = ptin;
                                    filemodel.OrderNumber = model.number_order_assessmnt;
                                    dal.InsertPTRefundUploadDocs(filemodel);
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        success = false,
                                        message = "File size should be less than 500 KB"
                                    });
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = "Only PDF, JPG, JPEG, PNG files are allowed"
                                });
                            }
                        }
                    }
                }
                dal.InsertPtRefundClaim(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase fileUpload, string lblPtin, string checklistCode)
        {
            string DocumentPathPT= ConfigurationManager.AppSettings["DocumentsPathPTRefunds"].ToString();
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string fileExt = Path.GetExtension(fileUpload.FileName).ToLower();

                if (fileExt == ".jpeg" || fileExt == ".jpg" || fileExt == ".png" || fileExt == ".pdf")
                {
                    if (fileUpload.ContentLength < 500000)
                    {
                        string folderPath = Path.Combine(DocumentPathPT, lblPtin);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string fileName = checklistCode + Path.GetFileName(fileUpload.FileName);

                        string fullPath = Path.Combine(folderPath, fileName);

                        fileUpload.SaveAs(fullPath);

                        return Json(new { success = true, message = "File uploaded successfully" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "File size should be less than 500KB" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid file format" });
                }
            }

            return Json(new { success = false, message = "No file selected" });
        }
    }
}