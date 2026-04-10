using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TGCTDPT.Models
{
    public class QueryItem
    {
        public bool IsSelected { get; set; }
        public int SNo { get; set; }
        public string query_name { get; set; }
        public string query_code { get; set; }
    }
    public class QueryViewModel
    {
        public List<QueryItem> Queries { get; set; }

        [Required(ErrorMessage = "Reasons are required")]
        [Display(Name = "Reasons")]
        [DataType(DataType.MultilineText)]
        public string Reasons { get; set; }
    }
    public class SubmitQueryModel
    {
        public List<SelectedQuery> SelectedQueries { get; set; }
        public string Reasons { get; set; }
        public string RNR { get; set; }
        public string UserId { get; set; }
    }
    public class SelectedQuery
    {
        public string QueryCode { get; set; }
        public string QueryName { get; set; }
    }
    public class AjaxResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}