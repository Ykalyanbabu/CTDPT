using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class Returns
    {
        public string SlabCode { get; set; }
        public string SlabDetails { get; set; }
        public string TaxAmount { get; set; }
        
    }

    public class EnitityDetails
    {
        public string branches { get; set; }
        public string partners { get; set; }
        public string directors { get; set; }

    }
    public class ReturnDetails
    {
        public string PTIN { get; set; }
        public string OwnerType { get; set; }
        public string Form1Reg { get; set; }
        public string MonthYear { get; set; }
        public string ReturnId { get; set; }
        public string Amount { get; set; }
    }
    public class EnterpriseDetails
    {
        public string PTIN { get; set; }
        public string EnterPriseName { get; set; }
        public string InsertedDate { get; set; }
        public string Division { get; set; }
        public string Circle { get; set; }
        public string ProfessionType { get; set; }
        public string DealerId { get; set; }
        public string PtRegDate { get; set; }
        public string EmailId { get; set; }
        public string ProfType { get; set; }
        public string ProfTypeBranch { get; set; }
        public string ProfTypePartnar { get; set; }
        public string OwnerType { get; set; }
    }
    public class PTReturnModel
    {
        public string ReturnId { get; set; }
        public string Ptin { get; set; }
        public string ReturnMonth { get; set; }
        public decimal TotalPayable { get; set; }
        public string FiledBy { get; set; }
        public int DealerId { get; set; }
        public string FormType { get; set; }

        public int? SalSlabCode1 { get; set; }
        public int? NoEmp1 { get; set; }
        public decimal? TotPble1 { get; set; }
        public string Type1 { get; set; }

        public int? SalSlabCode2 { get; set; }
        public int? NoEmp2 { get; set; }
        public decimal? TotPble2 { get; set; }
        public string Type2 { get; set; }

        public int? SalSlabCode3 { get; set; }
        public int? NoEmp3 { get; set; }
        public decimal? TotPble3 { get; set; }
        public string Type3 { get; set; }

        public int? SalSlabCode4 { get; set; }
        public int? NoEmp4 { get; set; }
        public decimal? TotPble4 { get; set; }
        public string Type4 { get; set; }
    }
    public class ApplicationStatus
    {
        public string rnr_number { get; set; }
        public string email_id { get; set; }
        public string AppStatus { get; set; }
        public string query_status { get; set; }
        public string application_id { get; set; }
    }
    public class SaveResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string returnid { get; set; }
    }
}