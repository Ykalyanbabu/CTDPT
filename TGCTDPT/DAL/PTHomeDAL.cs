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
        public List<EnitityDetails> GetPTEntityDetails(string ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<EnitityDetails>(
                    "",
                    new { ptin = ptin },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

    }
}