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
    public class RefundsDAL
    {
        private string conStr = ConfigurationManager.ConnectionStrings["portalConnectionString"].ConnectionString;
        public List<Refunds> GetDocumetsCheckList(string type)
        {
            using (var con = new SqlConnection(conStr))
            {
                return con.Query<Refunds>(
                    "sp_get_pt_refunds_docs_checklist_mst_new",
                    new { Type = type },
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public void InsertPtRefundClaim(PtRefundClaimModel model)
        {
            using (IDbConnection db = new SqlConnection(conStr))
            {
                db.Execute(
                    "sp_insert_pt_refunds_claim",
                    model,
                    commandType: CommandType.StoredProcedure
                );
            }
        }
        public void InsertPTRefundUploadDocs(RefundUploadDoc model)
        {
            using (IDbConnection db = new SqlConnection(conStr))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@prof_tin", model.ProfTin);
                parameters.Add("@doc_id", model.DocId);
                parameters.Add("@file_name", model.FileName);
                parameters.Add("@file_path", model.FilePath);
                parameters.Add("@order_number", model.OrderNumber);
                parameters.Add("@created_by", model.CreatedBy);

                db.Execute("sp_insert_pt_refunds_upload_docs_new",
                           parameters,
                           commandType: CommandType.StoredProcedure);
            }
        }
    }
}