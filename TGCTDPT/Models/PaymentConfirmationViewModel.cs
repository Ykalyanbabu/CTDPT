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
}