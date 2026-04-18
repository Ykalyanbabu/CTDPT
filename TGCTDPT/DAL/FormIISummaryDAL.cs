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

        public FormIISummaryViewModel GetFullSummary(string rnr)
        {
            var vm = new FormIISummaryViewModel();

            using (var con = CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@RNR", rnr, DbType.String);

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
    }
}