using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TGCTDPT.Models;

namespace TGCTDPT.DAL
{
    public class RegistrationDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
        public List<Registration> CheckPantoPT(string PAN)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Registration>(
                    "PR_CHECK_PAN_NUMBER_EXISTS_PT",
                    new { PAN = PAN },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<Business_dtls> LoadDivisions()
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Business_dtls>(
                    "pr_PT_FillDropDownLists_Div",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }  
        public List<Business_dtls> Loadistricts()
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Business_dtls>(
                    "pr_PT_FillDropDownLists_Dis",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        } 
        
        public List<bnk_dtls> Loadbanks()
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<bnk_dtls>(
                    "pr_PT_Fillbankddl_Dis",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<Business_dtls> LoadCircles(string divisionId)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Business_dtls>(
                    "pr_PT_FillDropDownLists_Cir", new { DivisionCode = divisionId },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        
        public Business_dtls SaveBusinessDetails(Business_dtls m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<Business_dtls>(
                    //"pr_savebusinessdetails",
                    "pr_savebusinessdetails1",
                    new
                    {
                        JsonData = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public employee_dtls SaveEmployeeDetails(employee_dtls m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<employee_dtls>(
                    //"pr_savebusinessdetails",
                    "pr_save_employee_details",
                    new
                    {
                        JsonData = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public ownr_md_dtls Saveownr_mdDetails(ownr_md_dtls m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<ownr_md_dtls>(
                    "pr_save_owner_details",
                    new
                    {
                        JsonData = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }

        public auth_prsn_dtls Save_AuthrsedPrsn_Details(auth_prsn_dtls m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<auth_prsn_dtls>(
                    "pr_save_authorised_person_details",
                    new
                    {
                        JsonData = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public int Save_dir_prtnr_Details(List<dir_prtnr_dtls> m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<int>(
                    "pr_save_dir_prtnr_details",
                    new
                    {
                        @jsondata = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public int Save_addl_place_Details(List<addl_plc_dtls> m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<int>(
                    "pr_save_addl_plc_dtls",
                    new
                    {
                        @jsondata = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }


        public int Save_Bank_Details(List<bnk_dtls> m)
        {
            using (var con = new SqlConnection(conStr))
            {
                var jsonData = JsonConvert.SerializeObject(m);

                var result = con.QueryFirstOrDefault<int>(
                    "pr_save_bnk_dtls",
                    new
                    {
                        @jsondata = jsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public string SaveDocuments(string mjsonData)
        {
            using (var con = new SqlConnection(conStr))
            {
                var result = con.QueryFirstOrDefault<string>(
                    "pr_save_bz_documents",
                    new
                    {
                        @jsondata = mjsonData
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public string GenerateRNR(string AppId)
        {
            using (var con = new SqlConnection(conStr))
            {
                var result = con.QueryFirstOrDefault<string>(
                    "usp_generate_rnr_number",
                    new
                    {
                        @ApplicationId = AppId
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public string ReSubmitApplication(string AppId)
        {
            using (var con = new SqlConnection(conStr))
            {
                var result = con.QueryFirstOrDefault<string>(
                    "usp_applicant_query_response",
                    new
                    {
                        @ApplicationId = AppId
                    },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public FormIISummaryViewModel GetFullSummary(string appid)
        {
            var vm = new FormIISummaryViewModel();

            using (var con = new SqlConnection(conStr))
            {
                var param = new DynamicParameters();
                param.Add("@ApplicationId", appid, DbType.String);

                using (var multi = con.QueryMultiple(
                    "USP_GETFORMII_ENROLMENTSUMMARY_APPLICANT",
                    param,
                    commandType: CommandType.StoredProcedure))
                {
                    vm.BusinessDetails = multi.ReadFirstOrDefault<BusinessDetails>();

                    vm.EmployeeDetails = multi.Read<EmployeeDetail>().ToList();

                    vm.OwnerDetails = multi.Read<OwnerDetail>().ToList();

                    vm.AuthPersonDetails = multi.Read<AuthPersonDetail>().ToList();

                    vm.DirectorPartners = multi.Read<DirectorPartner>().ToList();

                    vm.AddlPlacesOfBiz = multi.Read<AddlPlaceOfBiz>().ToList();

                    vm.BankDetails = multi.Read<BankDetail>().ToList();

                    vm.DocumentDetails = multi.Read<DocumentDetail>().ToList();
                }
            }

            return vm;
        }
        public FormIISummaryViewModel GetApplicationDtls(string appid)
        {
            var vm = new FormIISummaryViewModel();

            using (var con = new SqlConnection(conStr))
            {
                var param = new DynamicParameters();
                param.Add("@ApplicationId", appid, DbType.String);

                using (var multi = con.QueryMultiple(
                    "USP_APPLICANT_ALL_DETAILS",
                    param,
                    commandType: CommandType.StoredProcedure))
                {
                    vm.BusinessDetails = multi.ReadFirstOrDefault<BusinessDetails>();

                    vm.EmployeeDetails = multi.Read<EmployeeDetail>().ToList();

                    vm.OwnerDetails = multi.Read<OwnerDetail>().ToList();

                    vm.AuthPersonDetails = multi.Read<AuthPersonDetail>().ToList();

                    vm.DirectorPartners = multi.Read<DirectorPartner>().ToList();

                    vm.AddlPlacesOfBiz = multi.Read<AddlPlaceOfBiz>().ToList();

                    vm.BankDetails = multi.Read<BankDetail>().ToList();

                    vm.DocumentDetails = multi.Read<DocumentDetail>().ToList();
                }
            }

            return vm;
        }


        public application_status Getapplicant(application_status a)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.QueryFirstOrDefault<application_status>(
                    "pr_get_applicant",
                    new
                    {
                        applicationid = a.application_id,
                        mobile = a.Mobile_No
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public DataSet GetLastOTPSpan(string a, string b)
        {
            using (var con = new SqlConnection(conStr))
            using (var cmd = new SqlCommand("get_pt_applicant_last_otp", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@application_id", a);
                cmd.Parameters.AddWithValue("@mobile", b);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
        }
        //public dynamic GetLastOTPSpan( string a,string b)
        //{
        //    using (var con = new SqlConnection(conStr))
        //    {
        //        return con.QueryFirstOrDefault<DataSet>(
        //            "get_pt_applicant_last_otp",
        //            new { application_id=a , mobile=b },
        //            commandType: CommandType.StoredProcedure
        //        );
        //    }
        //}

        public int Save_PT_OTP(string appid, string mobile, string rnd_num)
        {
            using (var con = new SqlConnection(conStr))
            {
                int rowsAffected = con.Execute(
                                "pr_Save_OTP_For_PT_applicant_Login",
                            new
                            {
                                application_id = appid,
                                mobile = mobile,
                                otp = rnd_num
                            },
                            commandType: CommandType.StoredProcedure
                        );

                return rowsAffected;
            }
        }
        public List<ApplicationStatus> GetApplicationStatus(string AppId)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<ApplicationStatus>(
                    "usp_get_application_status",
                    new { ApplicationId = AppId },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<QueryModel> GetQueryDetails(string rnr)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<QueryModel>(
                    "Pr_pt_GetQueryDocuments",
                    new { rnr = rnr },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public RC_Details GetPTEntityDetails(string ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.QueryFirstOrDefault<RC_Details>(
                    "Proc_PT_GetTINDetails_1",
                    new { StrTIN = ptin },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

    }
}