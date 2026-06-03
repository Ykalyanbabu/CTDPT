using Dapper;
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
    public class CommonDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;

        public List<Common> GetRNRDetails(string RNR)
        {
            var dtls = new List<Common>();
            using (var con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("proc_PTIN_Tracking_new", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rnr", RNR);
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    dtls.Add(new Common
                    {
                        Rnr = dr["rnr"].ToString(),
                        EnterpriseName = dr["Enterprise Name"].ToString(),
                        PendingWith = dr["Pending with"].ToString(),
                        AssignedTo = dr["Assigned to"].ToString(),
                        ApplicationEntryStatus = dr["application_entry_status"].ToString(),
                        QueryStatus = dr["query_status"].ToString(),
                        Status = dr["Status"].ToString(),
                        QueryClosedDate = dr["query_closed_date"].ToString(),
                        Query = dr["Query"].ToString(),
                        AppliedOn = dr["Applied on"].ToString(),
                        Delay = dr["Delay (Days)"].ToString(),
                        PTIN = dr["PTIN"].ToString(),
                        division_name = dr["division_name"].ToString(),
                        circle_name = dr["circle_name"].ToString()
                    });
            }
            return dtls;
        }
        public List<Masters> GetCountryState(string Type)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Masters>(
                    "usp_get_country_state_master", new { type = Type },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<Codes> GetDdoCodes(string Ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Codes>(
                    "pr_get_ddocodes", new { strPtin = Ptin },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
    }
}