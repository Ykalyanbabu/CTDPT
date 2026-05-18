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
        public PaymentConfirmationViewModel GetddocodebyTin(string _ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.QueryFirstOrDefault<PaymentConfirmationViewModel>(
                    "pr_get_pt_ddo_code",
                    new { ptin = _ptin },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public PaymentConfirmationViewModel GetDeatailsbyPaymentId(string paymentId)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.QueryFirstOrDefault<PaymentConfirmationViewModel>(
                    "pr_get_pt_payment_details",
                    new { PaymentId = paymentId },
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
        public string InsertPTPaymentDetails(string CTDTransactionId, string PTIN, string Act, string FromTaxPeriod, string ToTaxPeriod, int Amount, string TaxPurpose,
            string UserId, string ReturnId)
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
        public string InsertPTPaymentDetailsOther(string CTDTransactionId, string PTIN, string Act, string FromTaxPeriod, string ToTaxPeriod, int Amount, string TaxPurpose,
            string UserId, string remarks)
        {
            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_insert_pt_other_payment_details", con))
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
                        cmd.Parameters.AddWithValue("@remarks", remarks);

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
        public string UpdatePTPaymentResponse(string CTDTransactionId, string ChallanNo, string Hoa, string BankName, string Status, string BankTransactionId)
        {
            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_update_pt_payment_response", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@paymentID", CTDTransactionId);
                        cmd.Parameters.AddWithValue("@ChallanNo", ChallanNo);
                        cmd.Parameters.AddWithValue("@hoa", Hoa);
                        cmd.Parameters.AddWithValue("@BankName", BankName);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@BankTransactionId", BankTransactionId);

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
        public string InsertPaymentResponseLog(string bankstatus, string challanno, string depttransid, string bankname, string bankdate, string amount,
            string hoa, string remittersname, string ddocode, string bankamount, string trydate, string banktransid)
        {
            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_insert_pt_payment_response_log", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@bankstatus", bankstatus);
                        cmd.Parameters.AddWithValue("@challanno", challanno);
                        cmd.Parameters.AddWithValue("@depttransid", depttransid);
                        cmd.Parameters.AddWithValue("@bankname", bankname);
                        cmd.Parameters.AddWithValue("@bankdate", bankdate);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@hoa", hoa);
                        cmd.Parameters.AddWithValue("@remittersname", remittersname);
                        cmd.Parameters.AddWithValue("@ddocode", ddocode);
                        cmd.Parameters.AddWithValue("@bankamount", bankamount);
                        cmd.Parameters.AddWithValue("@trydate", trydate);
                        cmd.Parameters.AddWithValue("@banktransid", banktransid);

                        con.Open();

                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? challanno;
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