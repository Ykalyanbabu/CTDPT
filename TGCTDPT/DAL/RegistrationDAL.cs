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
    }
}