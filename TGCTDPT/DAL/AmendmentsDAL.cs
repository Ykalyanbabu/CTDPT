using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using TGCTDPT.Models;
namespace TGCTDPT.DAL
{
    public class AmendmentsDAL
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
        private IDbConnection CreateConnection() => new SqlConnection(_connStr);
        public List<Amendments> GetAmendmentsList(string UserId)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", UserId);
                return db.Query<Amendments>(
                    "usp_get_amendments_master",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).AsList();
            }
        }
        public FormIISummaryViewModel GetApplicationDtls(string ptin)
        {
            var vm = new FormIISummaryViewModel();

            using (var con = new SqlConnection(_connStr))
            {
                var param = new DynamicParameters();
                param.Add("@PTIN", ptin, DbType.String);

                using (var multi = con.QueryMultiple(
                    "USP_APPLICANT_ALL_DETAILS_BY_PTIN",
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
        public string SaveAmendDetails(SaveAmendRequest request, HttpFileCollectionBase files, string ptin)
        {
            //string basePath = HttpContext.Current.Server.MapPath("~/Uploads/Documents/Amendments/");
            string basePath = ConfigurationManager.AppSettings["PTAmend"];


            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            if (request.Documents != null && files.Count > 0)
            {
                for (int i = 0; i < request.Documents.Count; i++)
                {
                    var file = files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        string masterDocId = request.Documents[i].master_doc_id;

                        string ptinPath = Path.Combine(basePath, ptin);
                        if (!Directory.Exists(ptinPath))
                            Directory.CreateDirectory(ptinPath);

                        string docPath = Path.Combine(ptinPath, masterDocId);
                        if (!Directory.Exists(docPath))
                            Directory.CreateDirectory(docPath);

                        string fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);

                        string fullPath = Path.Combine(docPath, fileName);

                        file.SaveAs(fullPath);

                        request.Documents[i].document_path =
                            $"/Amendments/{ptin}/{masterDocId}/{fileName}";
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand("pr_save_pt_amendment_request", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Business", JsonConvert.SerializeObject(request.Business));
                cmd.Parameters.AddWithValue("@Emp", JsonConvert.SerializeObject(request.Emp));
                cmd.Parameters.AddWithValue("@KeyPerson", JsonConvert.SerializeObject(request.KeyPerson));
                cmd.Parameters.AddWithValue("@Auth", JsonConvert.SerializeObject(request.Auth));
                cmd.Parameters.AddWithValue("@Partners", JsonConvert.SerializeObject(request.Partners));
                cmd.Parameters.AddWithValue("@Branches", JsonConvert.SerializeObject(request.Branches));
                cmd.Parameters.AddWithValue("@Bank", JsonConvert.SerializeObject(request.Bank));
                cmd.Parameters.AddWithValue("@Docs", JsonConvert.SerializeObject(request.Documents));
                cmd.Parameters.AddWithValue("@Ptin", ptin);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["application_id"].ToString();
                    }
                }

                return null;
            }
        }
        public List<AmendApplicationData> GetPendingRequests(string circle)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Circle", circle);

                return db.Query<AmendApplicationData>(
                    "USP_GET_PENDING_AMENDMENTS",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).AsList();
            }
        }
    }
}