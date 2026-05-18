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
    public class ReportsDAL
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;

        private IDbConnection CreateConnection() => new SqlConnection(_connStr);


        public List<DivisionReportModel> GetDivisionReport(int year)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.Query<DivisionReportModel>(
                    "pr_division_return_report",
                    new { year },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public List<CircleReportModel> GetCircleReport(int year, string division)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.Query<CircleReportModel>(
                    "pr_circle_return_report",
                    new
                    {
                        year,
                        division
                    },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public List<PTOReportModel> GetPTOReport(int year, string division, string circle)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.Query<PTOReportModel>(
                    "pr_pto_return_report",
                    new
                    {
                        year,
                        division,
                        circle
                    },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
        public List<DealerReportModel> GetDealerReport(int year, string division, string circle, string pto)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.Query<DealerReportModel>(
                    "pr_dealer_return_report",
                    new
                    {
                        year,
                        division,
                        circle,
                        pto
                    },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
         
        public List<PTO_Reports> GetPTOReturnReport(string user_id)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                List<PTO_Reports> list = new List<PTO_Reports>();
                var parameters = new DynamicParameters();
                parameters.Add("@user_id", user_id);
                return db.QueryFirstOrDefault<dynamic>
                ("",
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );

            }
        }
        public List<PTO_Reports> GetCirPTOReturnReport(int yr, string user_id)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                List<PTO_Reports> list = new List<PTO_Reports>();
                var parameters = new DynamicParameters();
                parameters.Add("@year", yr);
                parameters.Add("@user_id", user_id);
                return db.QueryFirstOrDefault<dynamic>
                ("pr_pt_ret_pto_dlr_lvl",
                    parameters,
                    commandType: CommandType.StoredProcedure
                    );

            }
        }

        public DealerMailModel GetDealerMailDetails(string ptin, int year)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.QueryFirstOrDefault<DealerMailModel>(
                    "pr_get_dealer_mail_details",
                    new { ptin, year },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public List<ReturnDetails> GetReturnDetails(string ptin, int year)
        {
            using (IDbConnection db = new SqlConnection(_connStr))
            {
                return db.Query<ReturnDetails>(
                    "sp_GetReturnDetailsByPTIN",
                    new
                    {
                        prof_tin = ptin,
                        year = year
                    },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

    }
}