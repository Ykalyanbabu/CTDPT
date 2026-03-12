using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class Refunds
    {
        public string id { get; set; }
        public string doc_name { get; set; }
    }
    public class RefundUploadDoc
    {
        public string ProfTin { get; set; }
        public int DocId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
        public string OrderNumber { get; set; }
    }
    public class RefundFormModel
    {
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ServedDate { get; set; }
        public decimal? ClaimAmount { get; set; }

        public string NoOrderNumber { get; set; }
        public DateTime? NoOrderDate { get; set; }
        public DateTime? NoServedDate { get; set; }
        public decimal? NoClaimAmount { get; set; }

        public string AssessmentType { get; set; }
    }
    public class PtRefundClaimModel
    {
        public string prof_tin { get; set; }
        public string enterprise_name { get; set; }
        public string order_assessment { get; set; }
        public DateTime? date_order_assessmnt { get; set; }
        public string number_order_assessmnt { get; set; }
        public DateTime? date_notice_final_assessmnt { get; set; }
        public DateTime? date_notice_final_assessmnt_served { get; set; }
        public decimal refund_amount_ordered { get; set; }
        public string created_by { get; set; }
    }
}