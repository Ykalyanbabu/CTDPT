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
    public class ReturnsDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
        public List<Returns> GetSlabs()
        {
            var slabs = new List<Returns>();
            using (var con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("pr_getPtReturnTaxRateMst_bind_dropdown", con);
                cmd.CommandType = CommandType.StoredProcedure;
                /*cmd.Parameters.AddWithValue("@USERID", UserId);*/
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    slabs.Add(new Returns
                    {
                        SlabCode = dr["slabcode_taxrate"].ToString(),
                        SlabDetails = dr["sal_slab_dtl"].ToString(),
                        TaxAmount = dr["TaxAmount"].ToString()
                    });
            }
            return slabs;
        }
        public List<ReturnDetails> GetReturnDetails(string ptin)
        {
            var dtls = new List<ReturnDetails>();
            using (var con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("GetReturnDtils", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@strPtin", ptin);
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    dtls.Add(new ReturnDetails
                    {
                        PTIN = dr["prof_tin"].ToString(),
                        OwnerType = dr["owner_type"].ToString(),
                        Form1Reg = dr["Form1_reg"].ToString()
                    });
            }
            return dtls;
        }
        public List<ReturnDetails> GetPTGstReturnDetails(string ptin,string type)
        {
            var dtls = new List<ReturnDetails>();
            using (var con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("pr_pt_gst_rtn_dtls", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prof_tin", ptin);
                cmd.Parameters.AddWithValue("@Dealer_type", type);
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    dtls.Add(new ReturnDetails
                    {
                        MonthYear = dr["mon_yr"].ToString()
                    });
            }
            return dtls;
        }
        public List<ReturnDetails> GetPTTaxReturnDetails(string ptin, string flag, string type)
        {
            var dtls = new List<ReturnDetails>();
            using (var con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("pr_get_no_PtRet_new", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tin_grn", ptin);
                cmd.Parameters.AddWithValue("@Flag", flag);
                cmd.Parameters.AddWithValue("@Dealertype", type);
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    dtls.Add(new ReturnDetails
                    {
                        MonthYear = dr["mon_yr"].ToString()
                    });
            }
            return dtls;
        }
        public List<EnterpriseDetails> GetPTEnterpriseDetails(string ptin)
        {
            var dtls = new List<EnterpriseDetails>();
            using (var con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("pr_getPtTinDetails_Combined", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ptin", ptin);
                con.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    dtls.Add(new EnterpriseDetails
                    {
                        PTIN = dr["prof_tin"].ToString(),
                        EnterPriseName = dr["enterprise_name"].ToString(),
                        InsertedDate = dr["inserted_date"].ToString(),
                        Division = dr["division_name"].ToString(),
                        Circle = dr["circle_name"].ToString(),
                        ProfessionType = dr["profession_type"].ToString(),
                        DealerId = dr["dealer_Id"].ToString(),
                        PtRegDate = dr["pt_reg_date"].ToString(),
                        EmailId = dr["email_id"].ToString(),
                        ProfType = dr["prof_type"].ToString(),
                        ProfTypeBranch = dr["prof_type_branch"].ToString(),
                        ProfTypePartnar = dr["prof_type_partnar"].ToString()
                    });
            }
            return dtls;
        }
        public List<EnitityDetails> GetPTEntityDetails(string ptin)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<EnitityDetails>(
                    "PR_PT_ENTITY_DTLS_COMBINED",
                    new { ptin = ptin },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public SaveResponse SavePTReturnDetails(PTReturnModel model)
        {
            var response = new SaveResponse();

            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_save_pt_return_new", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

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
        public SaveResponse SavePTReturnYearlyDetails(PTReturnModel model)
        {
            var response = new SaveResponse();

            try
            {
                using (var con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand("pr_save_pt_yearly_return_new", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

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
    }
}