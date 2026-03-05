using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TGCTDPT.Helpers;
using TGCTDPT.Models;

namespace TGCTDPT.DAL
{
    public class UserDetails
    {
        DB_context dB_Context = new DB_context();
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
        public User GetUserData(string username, string password)
        {
            User u = new User();

            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("@strPTIN", username);
            d.Add("@strPwd", password);
            DataSet ds = dB_Context.Get_datatable("pr_CheckTINPassword", d);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                u.User_id = dr["user_id"].ToString();
                u.Password = dr["password"].ToString();
            }
            return u;

        }
        public List<User> CheckTinRegistration(string ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<User>(
                    "pr_CheckTINRegistration",
                    new { strPTIN = ptin },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<TinDetails> GetPTINDtls(string ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<TinDetails>(
                    "pr_GetPTINDtls",
                    new { PTIN = ptin },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<TinDetails> GetTINDtlsforRCPrinting(string ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<TinDetails>(
                    "proc_pt_return_validatingcheck",
                    new { pt_tin = ptin },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public int UpdatePassword(string userId, string password)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Execute(
                    "pr_UpdateUserPassword",
                    new { UserId = userId, Password = password },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public string InsertPasswordForTrack(string ptTin, string ip,string oldPassword, string newPassword,string userId)
        {
            using (var con = new SqlConnection(conStr))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@pt_tin", ptTin, DbType.String, ParameterDirection.Input, 17);
                parameters.Add("@ip_add", ip, DbType.String, ParameterDirection.Input, 50);
                parameters.Add("@old_password", oldPassword, DbType.String, ParameterDirection.Input, 128);
                parameters.Add("@new_password", newPassword, DbType.String, ParameterDirection.Input, 128);
                parameters.Add("@inserted_user_id", userId, DbType.String, ParameterDirection.Input, 50);

                parameters.Add("@result", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

                con.Execute(
                    "proc_pt_enterprise_trackpassword",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return parameters.Get<string>("@result");
            }
        }

    }
}