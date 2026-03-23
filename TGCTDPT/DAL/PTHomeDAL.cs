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
    public class PTHomeDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
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