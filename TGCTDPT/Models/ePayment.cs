using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGCTDPT.Models
{
    public class ePayment
    {
        public string RDetailId { get; set; }
        public string ReturnId { get; set; }
        public string SlabCode { get; set; }
        public string Range { get; set; }
        public string TaxAmount { get; set; }
        public string Quantity { get; set; }
        public string PayableTaxAmount { get; set; }
        public string Type { get; set; }
        public string TotalAmount { get; set; }

    }
}