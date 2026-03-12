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
                SqlCommand cmd = new SqlCommand("proc_PTIN_Tracking", con);
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
                        PTIN = dr["PTIN"].ToString()
                    });
            }
            return dtls;
        }
    }
}