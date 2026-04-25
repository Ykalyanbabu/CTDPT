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
    }
}