using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class RC_Details
    {
        public string prof_tin { get; set; }
        public string enterprise_name { get; set; }
        public string division_name { get; set; }
        public string circle_name { get; set; }
        public string door_no { get; set; }
        public string city { get; set; }
        public string mandal_code { get; set; }
        public string district_code { get; set; }
        public string pin { get; set; }
        public string mobile_number { get; set; }
        public string phone_number { get; set; }
        public string Profession_Type { get; set; }
        public string DealerId { get; set; }
        public string pt_reg_date { get; set; }
        public string edr { get; set; }
        public string Email_Id { get; set; }
        public string address { get; set; }
        public string isemp { get; set; }

    }
    public class RC_Cancel_ReActivate_Details
    {
        public int request_id { get; set; }
        public string prof_tin { get; set; }
        public string enterprise_name { get; set; }
        public DateTime edr { get; set; }
        public string division_name { get; set; }
        public string circle_name { get; set; }
        public string registration_status { get; set; }
        public string request_status { get; set; }
        public string new_status { get; set; }
        public DateTime effective_date { get; set; }
        public string reason { get; set; }
        public string doc_path { get; set; }
        public string created_by { get; set; }

    }
}