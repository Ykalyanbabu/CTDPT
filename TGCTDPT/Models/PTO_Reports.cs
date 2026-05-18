using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class PTO_Reports
    {
        public string division_name { get; set; }
        public string circle_name { get; set; }


        public string PTO_name { get; set; }
        public string prof_tin { get; set; }
        public string enterprise_name { get; set; }
        public int active_tps { get; set; }
        public int TotReturns { get; set; }
        public int ReturnsFiled { get; set; }
        public int BalReturns { get; set; }
        public string LstFiledMnth { get; set; }
        public string mobile { get; set; }
        public string email_id { get; set; }
        public string filingprcntg { get; set; }
        public string pndngprcntg { get; set; }
    }


    public class DivisionReportModel
    {
        public string division_name { get; set; }

        public int ActiveTPs { get; set; }

        public int ReturnsFiled { get; set; }

        public decimal FiledPercentage { get; set; }

        public int ToBeFiled { get; set; }

        public decimal PendingPercentage { get; set; }
    }
    public class CircleReportModel
    {
        public string division_name { get; set; }

        public string circle_name { get; set; }

        public string PTO_name { get; set; }

        public int ActiveTPs { get; set; }

        public int ReturnsFiled { get; set; }

        public decimal FiledPercentage { get; set; }

        public int ToBeFiled { get; set; }

        public decimal PendingPercentage { get; set; }
    }
    public class PTOReportModel
    {
        public string division_name { get; set; }

        public string circle_name { get; set; }

        public string PTO_name { get; set; }

        public int NoOfDealers { get; set; }

        public int ReturnsFiled { get; set; }

        public int PendingReturns { get; set; }
    }
    public class DealerReportModel
    {
        public string division_name { get; set; }

        public string circle_name { get; set; }

        public string PTO_name { get; set; }

        public string prof_tin { get; set; }

        public string enterprise_name { get; set; }

        public int TotReturns { get; set; }

        public int ReturnsFiled { get; set; }

        public int BalReturns { get; set; }

        public string mobile { get; set; }

        public string email_id { get; set; }
    }

    public class DealerMailModel
    {
        public string prof_tin { get; set; }
        public string enterprise_name { get; set; }
        public int ReturnsFiled { get; set; }
        public int BalReturns { get; set; }
        public string email_id { get; set; }
    }



  

    
    public class DlrReturnDetails
    {
        public int SlNo { get; set; }
        public string Month { get; set; }
        public string ReturnID { get; set; }
        public DateTime FilingDate { get; set; }
        public string Status { get; set; }
        public decimal AmountPaid { get; set; }
    }
}