using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class Amendments
    {
        public string amendment_type_id { get; set; }
        public string amendment_type_name { get; set; }
        public string amendment_order { get; set; }
        public string UserId { get; set; }
    }

    public class SaveAmendRequest
    {
        public BusinessModel Business { get; set; }
        public EmployeeModel Emp { get; set; }
        public KeyPersonModel KeyPerson { get; set; }
        public AuthModel Auth { get; set; }
        public List<PartnerModel> Partners { get; set; }
        public List<BranchModel> Branches { get; set; }
        public List<BankModel> Bank { get; set; }
        public List<DocumentsModel> Documents { get; set; }
    }
    public class BusinessModel
    {
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
    public class EmployeeModel
    {
        public string emp_below_15000 { get; set; }
        public string emp_between_15001_20000 { get; set; }
        public string emp_above_20000 { get; set; }
        public string tot_emp { get; set; }
        public string isemp { get; set; }

    }
    public class KeyPersonModel
    {
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
        public string O_district_code { get; set; }
        public string O_district_name { get; set; }
        public string O_state_name { get; set; }
        public string O_country { get; set; }
        public string O_pincode { get; set; }
    }
    public class AuthModel
    {
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
    public class PartnerModel
    {
        public string dir_prtnr_name { get; set; }
        public string dir_prtnr_remunrtn { get; set; }
        public string dir_prtnr_type { get; set; }
        public string dir_prtnr_mobile_no { get; set; }
        public string dir_prtnr_email { get; set; }
        public string dir_prtnr_pan { get; set; }
        public string dir_prtnr_aadhaar { get; set; }
        public string dir_prtnr_door_no { get; set; }
        public string dir_prtnr_road_street { get; set; }
        public string dir_prtnr_locality { get; set; }
        public string dir_prtnr_city { get; set; }
        public string dir_prtnr_mandal { get; set; }
        public string dir_prtnr_state { get; set; }
        public string dir_prtnr_country { get; set; }
        public string dir_prtnr_district_code { get; set; }
        public string dir_prtnr_district_name { get; set; }
        public string dir_prtnr_pincode { get; set; }
    }
    public class BranchModel
    {
        public string is_Additional_place { get; set; }
        public string addl_plc_door_no { get; set; }
        public string addl_plc_road_street { get; set; }
        public string addl_plc_city { get; set; }
        public string addl_plc_locality { get; set; }
        public string addl_plc_mandal { get; set; }
        public string addl_plc_state { get; set; }
        public string addl_plc_country { get; set; }
        public string addl_plc_district_code { get; set; }
        public string addl_plc_district_name { get; set; }
        public string addl_plc_pincode { get; set; }
    }
    public class BankModel
    {
        public string account_number { get; set; }
        public string account_holder_name { get; set; }
        public string bank_name { get; set; }
        public string bank_id { get; set; }
        public string ifsc_code { get; set; }
        public string branch_address { get; set; }
        public string min_digits { get; set; }
        public string max_digits { get; set; }

    }

    public class DocumentsModel
    {
        public string application_id { get; set; }
        public string master_doc_id { get; set; }
        public string document_type { get; set; }
        public string document_path { get; set; }
    }
    public class AmendApplicationData
    {
        public string ApplicationNumber { get; set; }
        public string Ptin { get; set; }
        public string EnterpriseName { get; set; }
        public string Circle { get; set; }
        public string ApplicationDate { get; set; }
    }
}