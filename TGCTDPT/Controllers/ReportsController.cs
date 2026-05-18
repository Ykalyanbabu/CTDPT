using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGCTDPT.DAL;
using PagedList.Mvc;
using PagedList;
using TGCTDPT.Mail_Services;

namespace TGCTDPT.Controllers
{

    public class ReportsController : Controller
    {
        private readonly ReportsDAL _dal = new ReportsDAL();

        // GET: Reports
        public ActionResult Reports()
        {

            return View();
        }
       


        //public ActionResult DivLvlRetReport()
        //{
        //    DivisionReportModel model = new DivisionReportModel();
        //    return View(model);
        //}

        public ActionResult DivisionReport(int year = 2025)
        {
            ViewBag.Year = year;

            var data = _dal.GetDivisionReport(year);

            return View("DivLvlRetReport", data);
        }

        public ActionResult CircleReport(string division, int year = 2025)
        {
            ViewBag.Year = year;
            ViewBag.Division = division;

            var data = _dal.GetCircleReport(year, division);

            return View("CirLvlRetReport", data);
        }

        public ActionResult PTOReport(string division, string circle, int year = 2025)
        {
            ViewBag.Year = year;
            ViewBag.Division = division;
            ViewBag.Circle = circle;

            var data = _dal.GetPTOReport(year, division, circle);

            return View("CirPtoLvlRetReport", data);
        }

        public ActionResult DealerReport(string division, string circle, string pto, int year = 2025, int page = 1)
        {
            ViewBag.Year = year;
            ViewBag.Division = division;
            ViewBag.Circle = circle;
            ViewBag.PTO = pto;

            var data = _dal.GetDealerReport(year, division, circle, pto);

            return View("PtoLvlRetReport", data.ToPagedList(page, 10));
        }

        [HttpPost]
        public  ActionResult SendReminderMail(string ptin,string enterprise_name,string filed,string tobefiled ,string email)
        {
            var mailResult = false;
                send_mail send_mail = new send_mail();
                mailResult = send_mail.PT_Return_Reminder_mail(ptin, enterprise_name, filed, tobefiled, email);
                if (mailResult)
                {
                    return Json(new { success = true, message = "Reminder Mail has been sent to your EmailID " + email });
                }
                else
                {
                    return Json(new { success = true, message = "Mail Sending Encountered an Issue." });
                }
        }

        //public ActionResult DivLvlRetReport()
        //{
        //    ViewBag.Year = year;

        //    if (String.IsNullOrEmpty(user_id))
        //    {
        //        return RedirectToAction("Home", "PTHome");
        //    }
        //    var data = _dal.GetPTOReturnReport(user_id);
        //    return View(data);
        //}
        //public ActionResult CirLvlRetReport()
        //{
        //    var user_id = Session["Userid"].ToString();
        //    if (String.IsNullOrEmpty(user_id))
        //    {
        //        return RedirectToAction("Home", "PTHome");
        //    }
        //    var data = _dal.GetPTOReturnReport(user_id);
        //    return View(data);
        //}
        //public ActionResult CirPtoLvlRetReport()
        //{
        //    var user_id = Session["Userid"].ToString();
        //    if (String.IsNullOrEmpty(user_id))
        //    {
        //        return RedirectToAction("Home", "PTHome");
        //    }
        //    var data = _dal.GetCirPTOReturnReport(yr,user_id);
        //    return View(data);
        //}
        //public ActionResult PtoLvlRetReport()
        //{
        //    var user_id = Session["Userid"].ToString();
        //    if(String.IsNullOrEmpty(user_id))
        //    {
        //         return RedirectToAction("Home", "PTHome");
        //    }
        //    var data = _dal.GetPTOReturnReport(user_id);  
        //    return View(data);
        //}
        //public ActionResult PtoTPLvlRetReport()
        //{
        //    var user_id = Session["Userid"].ToString();
        //    if (String.IsNullOrEmpty(user_id))
        //    {
        //        return RedirectToAction("Home", "PTHome");
        //    }
        //    var data = _dal.GetPTOReturnReport(user_id);
        //    return View(data);
        //}


        //


    }
}