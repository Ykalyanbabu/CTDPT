using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGCTDPT.Controllers
{
    public class PTOReportsController : Controller
    {
        // GET: PTOReports
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PaymentsReport()
        {
            return View();
        }
    }
}