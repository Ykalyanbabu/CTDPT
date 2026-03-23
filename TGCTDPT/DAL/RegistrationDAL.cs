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
    }
}