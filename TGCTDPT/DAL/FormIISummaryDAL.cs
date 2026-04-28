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
    public class FormIISummaryDAL
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;

        private IDbConnection CreateConnection() => new SqlConnection(_connStr);

        public FormIISummaryViewModel GetFullSummary(string rnr,string Circle)
        {
            var vm = new FormIISummaryViewModel();

            using (var con = CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@RNR", rnr, DbType.String);
                param.Add("@CIRCLE", Circle, DbType.String);

                using (var multi = con.QueryMultiple(
                    "USP_GETFORMII_ENROLMENTSUMMARY",
                    param,
                    commandType: CommandType.StoredProcedure))
                {
                    vm.BusinessDetails = multi.ReadFirstOrDefault<BusinessDetails>();

                    vm.EmployeeDetails = multi.Read<EmployeeDetail>().ToList();

                    vm.OwnerDetails = multi.Read<OwnerDetail>().ToList();

                    vm.AuthPersonDetails = multi.Read<AuthPersonDetail>().ToList();

                    vm.DirectorPartners = multi.Read<DirectorPartner>().ToList();

                    vm.AddlPlacesOfBiz = multi.Read<AddlPlaceOfBiz>().ToList();

                    vm.BankDetails = multi.Read<BankDetail>().ToList();

                    vm.DocumentDetails = multi.Read<DocumentDetail>().ToList();
                }
            }

            return vm;
        }
        public string GeneratePTIN(string act,string UserId)
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_GenerateNewTin_PT_New", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@act", act);
                        cmd.Parameters.AddWithValue("@inserted_userid", UserId);

                        con.Open();

                        var result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ApplicationData> GetPendingApplications(string circle)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Circle", circle);

                return db.Query<ApplicationData>(
                    "USP_GET_PENDING_APPROVALS",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).AsList();
            }
        }
        public StatusResponse ChangeApplicationStatus(string appId, string comments, string status, string userId)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_change_application_status", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@AppId", appId);
                        cmd.Parameters.AddWithValue("@Comments", string.IsNullOrEmpty(comments) ? (object)DBNull.Value : comments);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        SqlParameter outputParam = new SqlParameter("@ReturnStatus", SqlDbType.VarChar, 50);
                        outputParam.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        conn.Close();

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
                            response.Message = "Application status updated successfully.";
                        }
                        else
                        {
                            response.Message = "Failed to update application status.";
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
        public StatusResponse TransferApplication(string appId, string division, string circle, string userId)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_transfer_application", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AppId", appId);
                        cmd.Parameters.AddWithValue("@Division", division);
                        cmd.Parameters.AddWithValue("@Circle", circle);
                        cmd.Parameters.AddWithValue("@UserID", userId);
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
                            response.Message = "Application Transferred successfully.";
                        }
                        else
                        {
                            response.Message = "Failed to Transfer application.";
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
        public string ApproveRNR(string rnr, string tin, string userId, out string errorMsg)
        {
            errorMsg = string.Empty;

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_approve_rnr_new", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RNR", rnr);
                    cmd.Parameters.AddWithValue("@StrTIN", tin);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    try
                    {
                        con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            string result = "";

                            if (dr.Read())
                            {
                                result = dr["TinSts"].ToString();
                            }

                            if (dr.NextResult() && dr.Read())
                            {
                                errorMsg = dr[0].ToString();
                            }

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                        return "Failed";
                    }
                }
            }
        }

        public List<RC_Cancel_ReActivate_Details> GetPendingRequests(string user_id)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user_id);

                return db.Query<RC_Cancel_ReActivate_Details>(
                    @"select a.request_id,a.prof_tin,a.request_status,a.From_date as edr,a.To_date as effective_date,a.reasons as reason,
                a.doc_path,c.division_name,c.circle_name,c.user_id from pt_enterprise_regd_status_temp a
              inner join   Fn_PT_Division_circle_link() b on a.prof_tin = b.prof_tin inner join  Fn_user_reg_mstr() c on b.division_name = c.division_name and b.circle_name = c.circle_name where a.request_status = 'P' and c.user_id = @UserId order by a.inserted_date desc",
                    parameters,
                    commandType: CommandType.Text
                ).ToList();
            }
        }
        public RC_Cancel_ReActivate_Details GetRequestDetails(int id)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.QueryFirstOrDefault<RC_Cancel_ReActivate_Details>(
                    @"select a.request_id,a.prof_tin,a.request_status,a.From_date as edr,a.To_date as effective_date,a.reasons as reason,
                a.doc_path  from pt_enterprise_regd_status_temp a
              inner join   Fn_PT_Division_circle_link() b on a.prof_tin = b.prof_tin  where a.request_id = @id",
                    new { id });
            }
        }
        public dynamic ApproveCancellation(int id, string r_status, string user_id)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@r_status", r_status);
                parameters.Add("@user_id", user_id);
                return db.QueryFirstOrDefault<dynamic>(
                    "pr_ptin_cancel_revoke",
                     parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

    }
}