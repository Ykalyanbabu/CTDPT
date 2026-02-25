using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TGCTDPT.Helpers;
using TGCTDPT.Models;

namespace TGCTDPT.DAL
{
    public class UserDetails
    {
        DB_context dB_Context = new DB_context();
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
    }
}