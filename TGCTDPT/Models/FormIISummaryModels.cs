using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class FormIISummaryViewModel
    {
        public BusinessDetails BusinessDetails { get; set; }
        public List<EmployeeDetail> EmployeeDetails { get; set; }
        public List<OwnerDetail> OwnerDetails { get; set; }
        public List<AuthPersonDetail> AuthPersonDetails { get; set; }
        public List<DirectorPartner> DirectorPartners { get; set; }
        public List<AddlPlaceOfBiz> AddlPlacesOfBiz { get; set; }
        public List<BankDetail> BankDetails { get; set; }
        public List<DocumentDetail> DocumentDetails { get; set; }
    }

    public class BusinessDetails
    {
        public string ApplicationId { get; set; }
        public string EnterPriseName { get; set; }
        public string BusinessPan { get; set; }
        public string BusinessConstitution { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string DoorNo { get; set; }
        public string RoadStreet { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string Mandal { get; set; }
        public string District { get; set; }
        public string Pincode { get; set; }
        public string FullAddress { get; set; }
        public string ApplicationDate { get; set; }
        public string EmpBelow_15000 { get; set; }
        public string EmpBetween_15001_20000 { get; set; }
        public string EmpAbove_20000 { get; set; }
        public string TotalEmployees { get; set; }
        public string division_code { get; set; }
        public string division_name { get; set; }
        public string circle_code { get; set; }
        public string circle_name { get; set; }
        public string district_code { get; set; }
        public string progress_step { get; set; }
        public string Nominated_Auth_Person { get; set; }
        public string rnr_number { get; set; }
        public string DataEntryDate { get; set; }

    }

    public class EmployeeDetail
    {
        public string application_id { get; set; }
        public string emp_below_15000 { get; set; }
        public string emp_between_15001_20000 { get; set; }
        public string emp_above_20000 { get; set; }
        public string total_emp { get; set; }
    }

    public class OwnerDetail
    {
        public string application_id { get; set; }
        public string owner_name { get; set; }
        public string father_name { get; set; }
        public string mobile_no { get; set; }
        public string email_id { get; set; }
        public string pan { get; set; }
        public string status_of_individual { get; set; }
        public string aadhaar { get; set; }
        public string door_no { get; set; }

        public string road_street { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string mandal { get; set; }
        public string district { get; set; }
        public string state_name { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string FullAddress { get; set; }
    }

    public class AuthPersonDetail
    {
        public string application_id { get; set; }
        public string is_Authorised_person { get; set; }
        public string auth_prsn_name { get; set; }
        public string auth_prsn_father_name { get; set; }
        public string email_id { get; set; }
        public string auth_prsn_door_no { get; set; }
        public string auth_prsn_road_street { get; set; }
        public string auth_prsn_locality { get; set; }
        public string auth_prsn_city { get; set; }
        public string auth_prsn_district { get; set; }
        public string auth_prsn_pan { get; set; }
        public string auth_prsn_aadhaar { get; set; }
        public string auth_prsn_mobile_no { get; set; }
        public string auth_prsn_pincode { get; set; }
        public string FullAddress { get; set; }
    }

    public class DirectorPartner
    {
        public string application_id { get; set; }
        public string dir_name { get; set; }
        public string type_drp { get; set; }
        public string drawing_remuneration { get; set; }
        public string door_no { get; set; }
        public string road_street { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string mandal { get; set; }
        public string district { get; set; }
        public string state_name { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string pan { get; set; }
        public string aadhaar { get; set; }
        public string email_id { get; set; }
        public string mobile_no { get; set; }
        public string FullAddress { get; set; }
        public string district_name { get; set; }
    }

    public class AddlPlaceOfBiz
    {
        public string application_id { get; set; }
        public string is_Additional_place { get; set; }
        public string door_no { get; set; }
        public string road_street { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string mandal { get; set; }
        public string district { get; set; }
        public string state_name { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string FullAddress { get; set; }
        public string district_name { get; set; }
    }

    public class BankDetail
    {
        public string application_id { get; set; }
        public string account_number { get; set; }
        public string account_holder_name { get; set; }
        public string bank_name { get; set; }
        public string ifsc_code { get; set; }
        public string branch_address { get; set; }
        public string bank_id { get; set; }
    }

    public class DocumentDetail
    {
        public int doc_id { get; set; }
        public string application_id { get; set; }
        public string document_type { get; set; }
        public string document_path { get; set; }
        public string uploaded_date { get; set; }
        public string master_doc_id { get; set; }
    }
    public class Response
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string returnid { get; set; }
    }
    public class ApplicationData
    {
        public string ApplicationNumber { get; set; }
        public string EnterpriseName { get; set; }
        public string Circle { get; set; }
        public string Address { get; set; }
        public string ModeOfSubmission { get; set; }
        public string TypeOfSubmission { get; set; }
        public string ApplicationDate { get; set; }
        public string DelayInDays { get; set; }
    }
}