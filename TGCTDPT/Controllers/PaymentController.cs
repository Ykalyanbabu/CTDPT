using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGCTDPT.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult PaymentPage()
        {
            return View();
        }
        public ActionResult Payment_Rates()
        {
            return View();
        }
        public ActionResult PaymentConfirmation()
        {
            return View();
        }
    }
}