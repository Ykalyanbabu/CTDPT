using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{

    public class DealerReturnsModel
    {
        public string ptin { get; set; }
        public string enterprise_name { get; set; }
        public string f_year { get; set; }
        public DateTime? rc_effect_date { get; set; }
        public string circle { get; set; }
        public string division { get; set; }

        public List<dlr_ret_reports> dlr_ret_dtls { get; set; }
        public List<YearlyDCB> YearlyDCB { get; set; }
        public List<MonthlyDCB> MonthlyDCB { get; set; }

        public decimal TotalDemand { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalBalance { get; set; }
    }
     
    public class dlr_ret_reports
        {
            public int SlNo { get; set; }
            public string For_Month { get; set; }
            public string ReturnID { get; set; }
            public DateTime? FilingDate { get; set; }
            public string Status { get; set; }
            public decimal? AmountPaid { get; set; }
        }
    public class DealerDCB 
    {
        public string ptin { get; set; }
        public string enterprise_name { get; set; }
        public DateTime? rc_effect_date { get; set; }
        public string circle { get; set; }
        public string division { get; set; }
        public List<YearlyDCB> YearlyDCB { get; set; }
        public List<MonthlyDCB> MonthlyDCB { get; set; }

        public decimal TotalDemand { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class YearlyDCB
    {
        public int SlNo { get; set; }
        public string FY { get; set; }
        public decimal to_be_paid { get; set; }
        public decimal amount { get; set; }
        public decimal Balance { get; set; }
    }

    public class MonthlyDCB
    {
        public int SlNo { get; set; }
        public string FY { get; set; }
        public string tax_period { get; set; }
        public decimal to_be_paid { get; set; }
        public decimal amount { get; set; }
        public decimal Balance { get; set; }
    }
}