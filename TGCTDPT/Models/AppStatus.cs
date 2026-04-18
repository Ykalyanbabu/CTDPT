using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class AppStatus
    {
        public string DisplayRNR { get; set; }
        public bool ShowQueryPanel { get; set; }
        public string RNR2 { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public bool ShowVATSection { get; set; }
        public List<QueryModel> Queries { get; set; }
        public string Remarks { get; set; }
        public string RNR1 { get; set; }
        public DateTime QueryDate { get; set; }
        public bool ShowModifyButton { get; set; }
    }
    public class QueryModel
    {
        public string query_name { get; set; }
        public string query_code { get; set; }
        public string rnr { get; set; }
    }
    public class StatusResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}