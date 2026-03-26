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
    public class mail_registation
    {
        public string email_id { get; set; }
        public string mobile_number { get; set; }
        public string designation { get; set; }
        public string contact_person { get; set; }
        public string role_id { get; set; } = "7";

    }
    public class Business_dtls
    {

        public string application_id { get; set; }
        public string enterprise_name { get; set; }
        public string division_code { get; set; }
        public string division_name { get; set; }
        public string circle_code { get; set; }
        public string circle_name { get; set; }
        public string business_pan { get; set; }
        public string cobz { get; set; }
        public string email_id { get; set; }
        public string mobile_no { get; set; }
        public string door_no { get; set; }
        public string road_street { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string mandal { get; set; }
        public string district_code { get; set; }
        public string district_name { get; set; }
        public string pincode { get; set; }
    }
    public class employee_dtls
    {
        public string application_id { get; set; }
        public string emp_below_15000 { get; set; }
        public string emp_between_15001_20000 { get; set; }
        public string emp_above_20000 { get; set; }
        public string tot_emp { get; set; }

    }
    public class ownr_md_dtls
    {
        public string application_id { get; set; }
        public string O_owner_name { get; set; }
        public string O_father_name { get; set; }
        public string O_mobile_no { get; set; }
        public string O_email_id { get; set; }
        public string O_pan { get; set; }
        public string O_status_of_individual { get; set; }
        public string O_aadhaar { get; set; }
        public string O_door_no { get; set; }
        public string O_road_street { get; set; }
        public string O_locality { get; set; }
        public string O_city { get; set; }
        public string O_mandal { get; set; }
        public string O_district { get; set; }
        public string O_state_name { get; set; }
        public string O_country { get; set; }
        public string O_pincode { get; set; }
    }
    public class auth_prsn_dtls
    {
        public string application_id { get; set; }
        public string is_Authorised_person { get; set; }
        public string auth_prsn_name { get; set; }
        public string auth_prsn_father_name { get; set; }
        public string auth_prsn_mobile_no { get; set; }
        public string email_id { get; set; }
        public string auth_prsn_pan { get; set; }
        public string auth_prsn_aadhaar { get; set; }
        public string auth_prsn_door_no { get; set; }
        public string auth_prsn_road_street { get; set; }
        public string auth_prsn_locality { get; set; }
        public string auth_prsn_city { get; set; }
        public string auth_prsn_district { get; set; }
        public string auth_prsn_pincode { get; set; }
    }
    public class dir_prtnr_dtls
    {
        public string application_id { get; set; }
        public string dir_prtnr_name { get; set; }
        public string dir_prtnr_remunrtn { get; set; }
        public string dir_prtnr_type { get; set; }
        public string dir_prtnr_mobile_no { get; set; }

        public string dir_prtnr_pan { get; set; }
        public string dir_prtnr_aadhaar { get; set; }
        public string dir_prtnr_door_no { get; set; }
        public string dir_prtnr_road_street { get; set; }
        public string dir_prtnr_locality { get; set; }
        public string dir_prtnr_city { get; set; }
        public string dir_prtnr_mandal { get; set; }
        public string dir_prtnr_state { get; set; }
        public string dir_prtnr_country { get; set; }
        public string dir_prtnr_district { get; set; }
        public string dir_prtnr_pincode { get; set; }
    }
    public class DirPrtnrWrapper
    {
        public List<dir_prtnr_dtls> model { get; set; }
    }

    public class addl_plc_dtls
    {
        public string application_id { get; set; }
        public string is_Additional_place { get; set; }
        public string addl_plc_door_no { get; set; }
        public string addl_plc_road_street { get; set; }
        public string addl_plc_city { get; set; }
        public string addl_plc_locality { get; set; }
        public string addl_plc_mandal { get; set; }
        public string addl_plc_state { get; set; }
        public string addl_plc_country { get; set; }
        public string addl_plc_district { get; set; }
        public string addl_plc_pincode { get; set; }
    }
    public class AddlPlaceWrapper
    {
        public List<addl_plc_dtls> model { get; set; }
    }


    public class bnk_dtls
    {
        public string application_id { get; set; }
        public string account_number { get; set; }
        public string account_holder_name { get; set; }
        public string bank_name { get; set; }
        public string bank_id { get; set; }
        public string ifsc_code { get; set; }
        public string branch_address { get; set; }
        public string min_digits { get; set; }
        public string max_digits { get; set; }

    }
    public class BankdtlsWrapper
    {
        public List<bnk_dtls> model { get; set; }
    }
    public class documents_dtls
    {
        public string application_id { get; set; }
        public string master_doc_id { get; set; }
        public string document_type { get; set; }
        public string document_path { get; set; }
        public DateTime uploaded_date { get; set; }

    }
}