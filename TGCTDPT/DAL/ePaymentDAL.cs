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
    public class ePaymentDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;

        public List<ReturnDetails> GetPaymentPendingReturns(string ptin,string OwnerType)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<ReturnDetails>(
                    "pr_get_payment_pending_returns",
                    new { ptin = ptin, type= OwnerType },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<ePayment> GetReturnDetails(string ReturnId)
        {
            using (var con = new SqlConnection(conStr))
            {
                var result = con.Query<ePayment>(
                    "pr_get_returnid_details",
                    new { ReturnId = ReturnId },
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return result;
            }
        }
    }
}