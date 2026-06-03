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

        public List<ePayment> GetPaymentPendingMonthsYears(string ptin,string ownertype)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<ePayment>(
                    "pr_pt_gst_rtn_dtls_payment_pending",
                    new { prof_tin = ptin, Dealer_type = ownertype },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public SaveResponse SavePTMonthlyReturn(PTReturnModel model)
        {
            var response = new SaveResponse();

            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_save_pt_return_new_epayment", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ReturnId", model.ReturnId);
                        cmd.Parameters.AddWithValue("@Ptin", model.Ptin);
                        cmd.Parameters.AddWithValue("@ReturnMonth", model.ReturnMonth);
                        cmd.Parameters.AddWithValue("@TotalPayable", model.TotalPayable);
                        cmd.Parameters.AddWithValue("@FiledBy", model.FiledBy);
                        cmd.Parameters.AddWithValue("@DealerId", model.DealerId);
                        cmd.Parameters.AddWithValue("@FormType", model.FormType);

                        cmd.Parameters.AddWithValue("@SalSlabCode1", (object)model.SalSlabCode1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp1", (object)model.NoEmp1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble1", (object)model.TotPble1 ?? DBNull.Value);

                        cmd.Parameters.AddWithValue("@SalSlabCode2", (object)model.SalSlabCode2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp2", (object)model.NoEmp2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble2", (object)model.TotPble2 ?? DBNull.Value);

                        cmd.Parameters.AddWithValue("@SalSlabCode3", (object)model.SalSlabCode3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp3", (object)model.NoEmp3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble3", (object)model.TotPble3 ?? DBNull.Value);

                        con.Open();

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string status = dr["Status"].ToString();

                                if (status == "SUCCESS")
                                {
                                    response.success = true;
                                    response.returnid = dr["ReturnId"].ToString();
                                    response.message = "Saved Successfully";
                                }
                                else
                                {
                                    response.success = false;
                                    response.message = dr["ErrorMessage"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }
        public SaveResponse SavePTYearlyPmtDetails(PTReturnModel model)
        {
            var response = new SaveResponse();

            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_save_pt_yearly_return_Epayment", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ReturnId", model.ReturnId);
                        cmd.Parameters.AddWithValue("@Ptin", model.Ptin);
                        cmd.Parameters.AddWithValue("@ReturnMonth", model.ReturnMonth);
                        cmd.Parameters.AddWithValue("@TotalPayable", model.TotalPayable);
                        cmd.Parameters.AddWithValue("@FiledBy", model.FiledBy);
                        cmd.Parameters.AddWithValue("@DealerId", model.DealerId);
                        cmd.Parameters.AddWithValue("@FormType", model.FormType);

                        cmd.Parameters.AddWithValue("@SalSlabCode1", (object)model.SalSlabCode1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp1", (object)model.NoEmp1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble1", (object)model.TotPble1 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type1", model.Type1);

                        cmd.Parameters.AddWithValue("@SalSlabCode2", (object)model.SalSlabCode2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp2", (object)model.NoEmp2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble2", (object)model.TotPble2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type2", model.Type2);

                        cmd.Parameters.AddWithValue("@SalSlabCode3", (object)model.SalSlabCode3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp3", (object)model.NoEmp3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble3", (object)model.TotPble3 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type3", model.Type3);

                        cmd.Parameters.AddWithValue("@SalSlabCode4", (object)model.SalSlabCode4 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NoEmp4", (object)model.NoEmp4 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotPble4", (object)model.TotPble4 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type4", model.Type4);

                        con.Open();

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string status = dr["Status"].ToString();

                                if (status == "SUCCESS")
                                {
                                    response.success = true;
                                    response.returnid = dr["ReturnId"].ToString();
                                    response.message = "Saved Successfully";
                                }
                                else
                                {
                                    response.success = false;
                                    response.message = dr["ErrorMessage"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return response;
        }
        public StatusResponse UpdateEmployees(string ptin, string below_15000, string between_15001_20000, string above_20000, string tot_emp)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_update_pt_employees", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ptin", ptin);
                        cmd.Parameters.AddWithValue("@Below", below_15000);
                        cmd.Parameters.AddWithValue("@Between", between_15001_20000);
                        cmd.Parameters.AddWithValue("@Above", above_20000);
                        cmd.Parameters.AddWithValue("@Total", tot_emp);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                response.Status = reader["Sts"].ToString();
                            }
                        }
                        if (response.Status == "Success")
                        {
                            response.Message = "Updated successfully.";
                        }
                        else
                        {
                            response.Message = "Failed to Update.";
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                response.Status = "Failed";
                response.Message = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }
        public StatusResponse UpdateEmpDirector(emp_dir_prtnr model)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_upd_empdirector_dtls", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@prof_tin", model.Prof_tin);
                        cmd.Parameters.AddWithValue("@emp_15000", model.emp_15000);
                        cmd.Parameters.AddWithValue("@emp_150001", model.emp_150001);
                        cmd.Parameters.AddWithValue("@emp_20000", model.emp_20000);
                        cmd.Parameters.AddWithValue("@no_of_emp", model.no_of_emp);
                        cmd.Parameters.AddWithValue("@no_of_atm", model.no_of_atm);
                        cmd.Parameters.AddWithValue("@no_of_director", model.no_of_director);
                        cmd.Parameters.AddWithValue("@no_of_Branches", model.no_of_Branches);
                        cmd.Parameters.AddWithValue("@no_of_partners", model.no_of_partners);
                        cmd.Parameters.AddWithValue("@inserted_userid", model.inserted_userid);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                response.Status = reader["Sts"].ToString();
                            }
                        }
                        if (response.Status == "Success")
                        {
                            response.Message = "Updated successfully.";
                        }
                        else
                        {
                            response.Message = "Failed to Update.";
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                response.Status = "Failed";
                response.Message = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }
        public emp_dir_prtnr GetEntitydetails(string StrTIN)
        {
            using (var con = new SqlConnection(conStr))
            {
                var result = con.QueryFirstOrDefault<emp_dir_prtnr>(
                    "Proc_PT_GetTINDetails_1",
                    new { @StrTIN = StrTIN },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
    }
}