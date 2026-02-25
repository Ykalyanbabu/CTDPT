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
}