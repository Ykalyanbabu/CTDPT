using System;
using System.Collections.Generic;
using System.Configuration;
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

        public ActionResult PaymentConfirmation(string returnId, string tin)
        {
            if (tin != null && tin != "")
            {
                Session["Tin"] = tin;
            }
            if (Session["Tin"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            if (Session["Userid"] == null)
            {
                ViewBag.Layout = "~/Views/Shared/_OuterLayout.cshtml";
            }
            else
            {
                ViewBag.Layout = "~/Views/Shared/_InnerLayout1.cshtml";
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
        public ActionResult OtherPaymentConfirmation(string ptin, string purpose, string FromDate, string ToDate, string Amount, string Remarks)
        {
            if (ptin != null && ptin != "")
            {
                Session["Tin"] = ptin;
            }
            if (Session["Tin"] == null)
            {
                return RedirectToAction("Home", "PTHome");
            }
            var data = new OtherPaymentConfirmationModel();
            var ddo = dal.GetddocodebyTin(ptin);
            if (ddo != null)
            {
                var rnrno = dal.GetNextRnr();
                Session["CTDTId"] = rnrno;
                Session["TAmount"] = Amount;
                data.CTDTransactionId = rnrno;
                data.Ptin = ptin;
                data.TypeofTax = "Profession Tax";
                data.TaxPurpose = purpose;
                data.FromDate = FromDate;
                data.ToDate = ToDate;
                data.EnterpriseName = ddo.EnterpriseName;
                data.Ddocode = ddo.Ddocode;
                data.Circlecode = ddo.Circlecode;
                data.Amount = Convert.ToInt32(Amount);
                data.Remarks = Remarks;
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult ProcessPaymentOld(string returnId)
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
                    Session["Tin"].ToString(),
                    Session["ReturnId"].ToString()
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

                string ddocode = data.Ddocode?.ToString();
                string hoa = data.Hoa?.ToString();
                string deptCode = data.DeptId?.ToString();
                string amount = Convert.ToDecimal(data.Amount).ToString("0.00");

                string dru = ConfigurationManager.AppSettings["ReturnUrl"];

                string deptTransId = Session["CTDTId"].ToString();

                var paymentId = dal.InsertPTPaymentDetails(
                    Session["CTDTId"].ToString(),
                    Session["Tin"].ToString(),
                    data.TypeofTax,
                    data.ReturnPeriod,
                    data.ReturnPeriod,
                    Convert.ToInt32(data.Amount),
                    data.TaxPurpose,
                    Session["Tin"].ToString(),
                    Session["ReturnId"].ToString()
                );

                if (string.IsNullOrEmpty(paymentId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to initialize payment. Please try again."
                    });
                }

                string paymentGatewayUrl = ConfigurationManager.AppSettings["PaymentUrl"];

                return Json(new
                {
                    success = true,
                    paymentUrl = paymentGatewayUrl,

                    formData = new
                    {
                        dru = dru,
                        deptcode = deptCode,
                        depttransid = deptTransId,
                        ddocode = ddocode,
                        hoa = hoa,
                        remittersname = Session["Tin"].ToString(),
                        amount = Convert.ToDecimal(amount).ToString("0.00")
                    }
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
        [HttpPost]
        public ActionResult ProcessPaymentOther(string ptin, string type, string purpose, string fdate, string tdate, string remarks)
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

                var ddo = dal.GetddocodebyTin(ptin);

                if (ddo == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid payment transaction"
                    });
                }

                string ddocode = ddo.Ddocode?.ToString();
                string hoa = ddo.Hoa?.ToString();
                string deptCode = ddo.DeptId?.ToString();
                string amount = Convert.ToDecimal(Session["TAmount"].ToString()).ToString("0.00");

                string dru = ConfigurationManager.AppSettings["ReturnUrl"];

                string deptTransId = Session["CTDTId"].ToString();

                var paymentId = dal.InsertPTPaymentDetailsOther(
                    Session["CTDTId"].ToString(),
                    Session["Tin"].ToString(),
                    type,
                    fdate,
                    tdate,
                    Convert.ToInt32(Session["TAmount"].ToString()),
                    purpose,
                    Session["Tin"].ToString(),
                    remarks
                );

                if (string.IsNullOrEmpty(paymentId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to initialize payment. Please try again."
                    });
                }

                string paymentGatewayUrl = ConfigurationManager.AppSettings["PaymentUrl"];

                return Json(new
                {
                    success = true,
                    paymentUrl = paymentGatewayUrl,

                    formData = new
                    {
                        dru = dru,
                        deptcode = deptCode,
                        depttransid = deptTransId,
                        ddocode = ddocode,
                        hoa = hoa,
                        remittersname = Session["Tin"].ToString(),
                        amount = Convert.ToDecimal(amount).ToString("0.00")
                    }
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
        [HttpPost]
        public ActionResult PaymentResponse()
        {
            try
            {
                var response = new IfmisPaymentResponse();

                response.bankstatus = Request.Form["bankstatus"] ?? "";
                response.challanno = Request.Form["challanno"] ?? "";
                response.depttransid = Request.Form["depttransid"] ?? "";
                response.bankname = Request.Form["bankname"] ?? "";
                response.bankdate = Request.Form["bankdate"] ?? "";
                response.hoa = Request.Form["hoa"] ?? "";
                response.remittersname = Request.Form["remittersname"] ?? "";
                response.ddocode = Request.Form["ddocode"] ?? "";
                response.trydate = Request.Form["trydate"] ?? "";
                response.banktransid = Request.Form["banktransid"] ?? "";

                decimal amount = 0;
                decimal bankamount = 0;

                decimal.TryParse(
                    Request.Form["amount"],
                    out amount);

                decimal.TryParse(
                    Request.Form["bankamount"],
                    out bankamount);

                response.amount = amount.ToString();
                response.bankamount = bankamount.ToString();

                var transId = dal.InsertPaymentResponseLog(
                    response.bankstatus,
                    response.challanno,
                    response.depttransid,
                    response.bankname,
                    response.bankdate,
                    response.amount,
                    response.hoa,
                    response.remittersname,
                    response.ddocode,
                    response.bankamount,
                    response.trydate,
                    response.banktransid
                );

                var data = dal.GetDeatailsbyPaymentId(response.depttransid);

                if (data != null)
                {
                    response.enterprisename = data.EnterpriseName;
                    response.typeoftax = data.TypeofTax;
                    response.taxpurpose = data.TaxPurpose;
                    response.remittersname = data.Ptin;
                    response.returnperiod = data.ReturnPeriod;
                }

                dal.UpdatePTPaymentResponse(
                    response.depttransid,
                    response.challanno,
                    response.hoa,
                    response.bankname,
                    response.bankstatus,
                    response.banktransid
                );

                string status =
                    (response.bankstatus ?? "")
                    .Trim()
                    .ToUpper();

                if (status == "SUCCESS")
                {
                    return View("PaymentResponse", response);
                }
                else if (status == "PENDING")
                {
                    return View("PaymentResponse", response);
                }
                else
                {
                    return View("PaymentResponse", response);
                }
            }
            catch (Exception ex)
            {
                /*dal.InsertErrorLog(
                    "PaymentResponse",
                    ex.Message,
                    ex.StackTrace
                );*/

                return View("PaymentResponse");
            }
        }
    }
}