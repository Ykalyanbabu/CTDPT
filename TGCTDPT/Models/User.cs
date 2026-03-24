using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class User
    {
        [Required(ErrorMessage = "Enter User Id")]

        public string User_id { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Tin { get; set; }
    }

    public class PTOfficer
    {
        [Required(ErrorMessage = "Enter User Id")]

        public string User_id { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string HierarchyCode { get; set; }
        public string Hierarchy { get; set; }
        public string Designation { get; set; }
        public string ShortDesignationCode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string CircleName { get; set; }
    }
}