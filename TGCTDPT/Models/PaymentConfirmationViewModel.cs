using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class PaymentConfirmationViewModel
    {
        public string ChallanNo { get; set; }
        public string DDOCode { get; set; }
        public string HOA { get; set; }
        public string EnterpriseName { get; set; }
        public int Amount { get; set; }
        public string Ptin { get; set; }
        public string ReturnPeriod { get; set; }
        public string TypeofTax { get; set; }
        public string TaxPurpose { get; set; }
        public string CTDTransactionId { get; set; }
        public string Circlecode { get; set; }
        public string Ddocode { get; set; }
        public string Hoa { get; set; }
        public string DeptId { get; set; }
        public string ReturnId { get; set; }

    }
    public class OtherPaymentConfirmationModel
    {
        public string ChallanNo { get; set; }
        public string DDOCode { get; set; }
        public string HOA { get; set; }
        public string EnterpriseName { get; set; }
        public int Amount { get; set; }
        public string Ptin { get; set; }
        public string ReturnPeriod { get; set; }
        public string TypeofTax { get; set; }
        public string TaxPurpose { get; set; }
        public string CTDTransactionId { get; set; }
        public string Circlecode { get; set; }
        public string Ddocode { get; set; }
        public string Hoa { get; set; }
        public string DeptId { get; set; }
        public string ReturnId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }

    }
    public class PaymentReceiptViewModel
    {
        public string CTDTransactionId { get; set; }
        public string Amount { get; set; }
        public string BankName { get; set; }
        public string BankAckNo { get; set; }
        public string ChallanNo { get; set; }
        public string PaymentDate { get; set; }
        public string Status { get; set; }
        public string HOA { get; set; }

        public string TypeofTax { get; set; }
        public string Ptin { get; set; }
        public string EnterpriseName { get; set; }
        public string TaxPurpose { get; set; }
        public string ReturnPeriod { get; set; }

        public bool IsSuccess => Status == "SUCCESS" || Status == "SUCCESSFUL";
        public string FormattedAmount => string.IsNullOrEmpty(Amount) ? "" : $"₹ {Convert.ToDecimal(Amount):N2}";
        public string DisplayStatus => IsSuccess ? "Payment Successful" : "Payment Failed";
        public string StatusClass => IsSuccess ? "status-success" : "status-failed";
        public string StatusIcon => IsSuccess ? "fa-check-circle" : "fa-exclamation-circle";
    }
    public class PaymentConfirmationResponse
    {
        public bool success { get; set; }
        public string paymentid { get; set; }
        public string message { get; set; }
    }
    public class IfmisPaymentResponse
    {
        public string bankstatus { get; set; }
        public string challanno { get; set; }
        public string depttransid { get; set; }
        public string bankname { get; set; }
        public string bankdate { get; set; }
        public string amount { get; set; }
        public string hoa { get; set; }
        public string remittersname { get; set; }
        public string ddocode { get; set; }
        public string bankamount { get; set; }
        public string trydate { get; set; }
        public string banktransid { get; set; }
        public string enterprisename { get; set; }
        public string returnperiod { get; set; }
        public string typeoftax { get; set; }
        public string taxpurpose { get; set; }
    }

    public class CyberChallanModel
    {
        public string challanno { get; set; }
        public string depttransid { get; set; }
        public string hoa { get; set; }
        public string bankamount { get; set; }
        public string banktransid { get; set; }
        public string ddocode { get; set; }
        public string deptcode { get; set; }
        public string bankcode { get; set; }
        public string remittersname { get; set; }
        public string scrolldate { get; set; }
        public string bankstatus { get; set; }
        public string status { get; set; }
        public string bankdate { get; set; }
        public string challan_date { get; set; }

        public string enterprisename { get; set; }
        public string typeoftax { get; set; }
        public string taxpurpose { get; set; }
        public string returnperiod { get; set; }


    }
    public class TransactionDetails
    {
        public string challanno { get; set; }
        public string depttransid { get; set; }
        public string hoa { get; set; }
        public string bankamount { get; set; }
        public string banktransid { get; set; }
        public string ddocode { get; set; }
        public string deptcode { get; set; }
        public string bankcode { get; set; }
        public string remittersname { get; set; }
        public string scrolldate { get; set; }
        public string bankstatus { get; set; }
        public string status { get; set; }
        public string bankdate { get; set; }
        public string challan_date { get; set; }

        public string enterprisename { get; set; }
        public string typeoftax { get; set; }
        public string taxpurpose { get; set; }
        public string returnperiod { get; set; }


    }
    public class CyberChallanResponse
    {
        public List<CyberChallanModel> challandetails { get; set; }
    }
}