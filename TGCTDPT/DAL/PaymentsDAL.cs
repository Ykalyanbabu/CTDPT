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
    public class PaymentsDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;

        public List<EnitityDetails> GetReturnByIda(string returnId)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<EnitityDetails>(
                    "PR_PT_ENTITY_DTLS_COMBINED",
                    new { returnId = returnId },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public PaymentConfirmationViewModel GetReturnById(string returnId)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.QueryFirstOrDefault<PaymentConfirmationViewModel>(
                    "pr_get_pt_return_details",
                    new { returnId = returnId },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public string GetNextRnr()
        {
            using (var con = new SqlConnection(conStr))
            {
                var parameters = new DynamicParameters();

                parameters.Add(
                    "@rnr",
                    dbType: DbType.String,
                    size: 100, 
                    direction: ParameterDirection.Output
                );
                con.Execute(
                    "pr_get_next_rnr_new",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return parameters.Get<string>("@rnr");
            }
        }
        public string InsertPTPaymentDetails(string CTDTransactionId,string PTIN,string Act,string FromTaxPeriod, string ToTaxPeriod,int Amount,string TaxPurpose,
            string UserId,string ReturnId)
        {
            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_insert_pt_payment_details", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@paymentID", CTDTransactionId);
                        cmd.Parameters.AddWithValue("@tin", PTIN);
                        cmd.Parameters.AddWithValue("@act", Act);
                        cmd.Parameters.AddWithValue("@taxperiodFrom", FromTaxPeriod);
                        cmd.Parameters.AddWithValue("@taxperiodTo", ToTaxPeriod);
                        cmd.Parameters.AddWithValue("@amount", Amount);
                        cmd.Parameters.AddWithValue("@purpose", TaxPurpose);
                        cmd.Parameters.AddWithValue("@inserted_userid", UserId);
                        cmd.Parameters.AddWithValue("@returnid", ReturnId);

                        con.Open();

                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? CTDTransactionId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}