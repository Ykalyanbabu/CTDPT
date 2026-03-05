using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class Registration
    {
        public string PAN { get; set; }
        public string PTIN { get; set; }
    }
    public class TinDetails
    {
        public string PTIN { get; set; }
        public string enterprise_name { get; set; }
        public string location { get; set; }
        public string Address { get; set; }
        public string phone_number { get; set; }
        public string mobile_number { get; set; }
        public string division_name { get; set; }
        public string circle_name { get; set; }
        public string email_id { get; set; }
        public string registration_status { get; set; }
        public string prof_tin { get; set; }
        public string rc_effect_date { get; set; }

    }
}