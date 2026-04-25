using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class Common
    {
        public string Rnr { get; set; }
        public string EnterpriseName { get; set; }
        public string PendingWith { get; set; }
        public string AssignedTo { get; set; }
        public string ApplicationEntryStatus { get; set; }
        public string QueryStatus { get; set; }
        public string Status { get; set; }
        public string QueryClosedDate { get; set; }
        public string Query { get; set; }
        public string AppliedOn { get; set; }
        public string Delay { get; set; }
        public string PTIN { get; set; }
        public string division_name { get; set; }
        public string circle_name { get; set; }
    }
    public class Masters 
    {
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string state_code { get; set; }
        public string state_name { get; set; }
        public string state_short_code { get; set; }
    }
}