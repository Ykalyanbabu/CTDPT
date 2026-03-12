using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;

namespace TGCTDPT.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        private PaymentsDAL dal = new PaymentsDAL();
        public ActionResult PaymentPage()
        {
            return View();
        }
        public ActionResult Payment_Rates()
        {
            return View();
        }
       
        public ActionResult PaymentConfirmation(string returnId)
        {
            if (Session["Tin"] == null)
            {
                RedirectToAction("Home", "PTHome");
            }
            var data = dal.GetReturnById(returnId);
            Session["ReturnId"] = data.ReturnId; ;
            if (data != null)
            {
                var rnrno = dal.GetNextRnr();
                Session["CTDTId"] = rnrno;
                data.CTDTransactionId = rnrno;
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult ProcessPayment(string returnId)
        {
            try
            {
                if (Session["Tin"] == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Session expired. Please login again.",
                        redirectToLogin = true
                    });
                }

                var data = dal.GetReturnById(returnId);

                if (data == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid payment transaction"
                    });
                }

                string ddocode = data.Ddocode.ToString();
                string hoa = data.Hoa.ToString();
                string deptCode = data.DeptId.ToString();
                string amount = data.Amount.ToString();

                string returnUrl = "https://tgct.gov.in/tgportal/DlrServices/Payments/e-PaymentConfirm_pt.aspx";
                var paymentId = dal.InsertPTPaymentDetails(
                    Session["CTDTId"].ToString(),
                    Session["Tin"].ToString(),
                    data.TypeofTax,
                    data.ReturnPeriod,
                    data.ReturnPeriod,
                    Convert.ToInt32(amount),
                    data.TaxPurpose,
                    Session["Tin"].ToString()
                );

                if (string.IsNullOrEmpty(paymentId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to initialize payment. Please try again."
                    });
                }

                // Log payment attempt only if paymentId is valid
                //LogPaymentAttempt(data.CTDTransactionId, paymentUrl);

                string paymentUrl = string.Format(
                    "https://treasury.telangana.gov.in/tg_cybertry/deptrequest.php?amount={0}&depttransid={1}&ddocode={2}&hoa={3}&deptcode={4}&remittersname={5}&tin={6}&RU={7}",
                    Uri.EscapeDataString(amount),
                    Uri.EscapeDataString(Session["CTDTId"].ToString()),
                    Uri.EscapeDataString(ddocode),
                    Uri.EscapeDataString(hoa),
                    Uri.EscapeDataString(deptCode),
                    Uri.EscapeDataString(data.EnterpriseName),
                    Uri.EscapeDataString(data.Ptin),
                    Uri.EscapeDataString(returnUrl)
                );

                return Json(new
                {
                    success = true,
                    redirectUrl = paymentUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error processing payment: " + ex.Message
                });
            }
        }

        [HttpGet]
        public ActionResult PaymentReturn()
        {
            try
            {
                var model = new PaymentReceiptViewModel();

                if (Request.HttpMethod == "POST")
                {
                    model.CTDTransactionId = Request.Form["depttransid"] ?? Request.QueryString["depttransid"];
                    model.Amount = Request.Form["bankamount"]?.Trim() ?? Request.QueryString["bankamount"];
                    model.BankName = Request.Form["bankname"]?.Trim() ?? Request.QueryString["bankname"];
                    model.BankAckNo = Request.Form["banktransid"]?.Trim() ?? Request.QueryString["banktransid"];
                    model.ChallanNo = Request.Form["challanno"]?.Trim() ?? Request.QueryString["challanno"];
                    model.PaymentDate = Request.Form["trydate"]?.Trim() ?? Request.QueryString["trydate"];
                    model.Status = Request.Form["bankstatus"]?.ToUpper().Trim() ?? Request.QueryString["bankstatus"]?.ToUpper();
                    model.HOA = Request.Form["hoa"]?.Trim() ?? Request.QueryString["hoa"];
                }
                else
                {
                    /*model.CTDTransactionId = Request.QueryString["depttransid"];
                    model.Amount = Request.QueryString["bankamount"];
                    model.BankName = Request.QueryString["bankname"];
                    model.BankAckNo = Request.QueryString["banktransid"];
                    model.ChallanNo = Request.QueryString["challanno"];
                    model.PaymentDate = Request.QueryString["trydate"];
                    model.Status = Request.QueryString["bankstatus"]?.ToUpper();
                    model.HOA = Request.QueryString["hoa"];*/

                    model.CTDTransactionId = "3611425242422";
                    model.Amount = "2000";
                    model.BankName = "Axis Bank";
                    model.BankAckNo = "961812345867876";
                    model.ChallanNo = "65032156799";
                    model.PaymentDate = "28-Feb-2026";
                    model.Status = "Success";
                    model.HOA = "36GHZ56776489999";
                    model.TypeofTax = "PT";
                    model.Ptin = "361814757567";
                    model.EnterpriseName = "Sai Ram Jingri Pvt.Ltd";
                    model.TaxPurpose = "Return Tax";
                    model.ReturnPeriod = "2024-2025";
                }

                //if (!string.IsNullOrEmpty(model.CTDTransactionId))
                //{
                //    var paymentDetails = dal.GetReturnById(model.CTDTransactionId);
                //    if (paymentDetails != null)
                //    {
                //        model.TypeofTax = paymentDetails.TypeofTax;
                //        model.Ptin = paymentDetails.Ptin;
                //        model.EnterpriseName = paymentDetails.EnterpriseName;
                //        model.TaxPurpose = paymentDetails.TaxPurpose;
                //        model.ReturnPeriod = paymentDetails.ReturnPeriod;
                //    }
                //}

                //LogPaymentReturn(model);

                if (model.Status == "SUCCESS" || model.Status == "SUCCESSFUL")
                {
                    //UpdatePaymentStatus(model.CTDTransactionId, "Success", model.BankAckNo, model.ChallanNo);

                    TempData["PaymentSuccess"] = true;
                    TempData["SuccessMessage"] = "Payment processed successfully!";
                }
                else
                {
                    //UpdatePaymentStatus(model.CTDTransactionId, "Failed", model.BankAckNo, model.ChallanNo);
                    TempData["PaymentSuccess"] = false;
                    TempData["ErrorMessage"] = "Payment failed. Please try again.";
                }

                //return View("PaymentReceipt", model);
                return View(model);
            }
            catch (Exception ex)
            {
               
                TempData["ErrorMessage"] = "Error processing payment return: " + ex.Message;
                return RedirectToAction("Home", "PTHome");
            }
        }

        private void LogPaymentAttempt(string transactionId, string paymentUrl)
        {
            
        }
    }
}