using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

namespace TGCTDPT.Controllers
{
    public class QueryController : Controller
    {
        // GET: Query
        private QueryDAL dal = new QueryDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RaiseQuery(string rnr)
        {
            if (Session["Userid"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            var model = new QueryViewModel();
            Session["RNR"] = ViewBag.rnr = rnr;
            return View();
        }
        [HttpGet]
        public JsonResult GetQueryList()
        {
            try
            {
                var queries = dal.GetQueries();
                return Json(new { success = true, data = queries }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SubmitQuery(string[] selectedQueryCodes, string reasons)
        {
            try
            {
                if (Session["RNR"] == null)
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = "Session expired. Please login again."
                    });
                }

                string rnr = Session["RNR"].ToString();
                string userId = Session["Userid"]?.ToString() ?? "System";

                if (selectedQueryCodes == null || selectedQueryCodes.Length == 0)
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = "Please select at least one query."
                    });
                }

                if (string.IsNullOrWhiteSpace(reasons))
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = "Please enter reasons for raising the query."
                    });
                }

                var selectedQueries = selectedQueryCodes.Select(code => new SelectedQuery
                {
                    QueryCode = code,
                    QueryName = ""
                }).ToList();

                var result = dal.SaveQueryList(rnr, userId, reasons, selectedQueries);
                return Json(new AjaxResponse
                {
                    Success = true,
                    Message = "Query raised successfully and has been sent to dealer's login/email.",
                    Data = new { redirectUrl = Url.Action("ApproveRegistration", "Registration") }
                });

                /* if (result.Item1)
                 {
                     try
                     {
                         dal.SendEmailToQueryDealer(rnr);
                     }
                     catch (Exception ex)
                     {
                         System.Diagnostics.Debug.WriteLine("Email error: " + ex.Message);
                     }

                     return Json(new AjaxResponse
                     {
                         Success = true,
                         Message = "Query raised successfully and has been sent to dealer's login/email.",
                         Data = new { redirectUrl = Url.Action("ApproveRegistration", "Registration") }
                     });
                 }
                 else
                 {
                     return Json(new AjaxResponse
                     {
                         Success = false,
                         Message = result.Item2
                     });
                 }*/
            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse
                {
                    Success = false,
                    Message = "Error: " + ex.Message
                });
            }
        }
    }
}