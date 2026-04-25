using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TGCTDPT.Models;

namespace TGCTDPT.DAL
{
    public class QueryDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;

        public List<QueryItem> GetQueries()
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<QueryItem>(
                    "usp_get_pt_query_master",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public Tuple<bool, string> SaveQueryList(string rnr, string userId, string queryReason, List<SelectedQuery> selectedQueries)
        {
            try
            {
                XElement xmlQueries = new XElement("Queries",
                    selectedQueries.Select(q => new XElement("Query",
                        new XAttribute("Code", q.QueryCode)))
                );

                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_Save_Query_List", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@RNR", rnr);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@QueryReason", queryReason);
                        cmd.Parameters.AddWithValue("@SelectedQueries", xmlQueries.ToString());

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int result = Convert.ToInt32(reader["Result"]);
                                string message = reader["Message"].ToString();

                                return Tuple.Create(result == 1, message);
                            }
                        }
                    }
                }

                return Tuple.Create(false, "Unknown error occurred");
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Error: " + ex.Message);
            }
        }
        public void SendEmailToQueryDealer(string rnr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_SendQueryEmail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RNR", rnr);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email sending failed: " + ex.Message);
            }
        }

    }
}