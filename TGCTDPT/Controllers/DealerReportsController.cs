using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using TGCTDPT.DAL;
using TGCTDPT.Models;
 

public class DealerReportsController : Controller
{
    ReturnsDAL _dal = new ReturnsDAL();

    public ActionResult DealerReportType()
    {
        if (Session["Tin"] == null)
        {
            ViewBag.Layout = "~/Views/Shared/_OuterLayout.cshtml";
        }
        else
        {
            ViewBag.Layout = "~/Views/Shared/_InnerLayout.cshtml";
        }
        return View();
    }
    public ActionResult DealerReturns()
    {
        if (Session["Tin"] == null)
        {
            return RedirectToAction("Home", "PTHome");
        }
        string ptin = Session["Tin"].ToString();

        DealerReturnsModel model = _dal.GetDealerDtls(ptin);
        Session["enterprise_name"] = model.enterprise_name;
        return View(model);
    }

    [HttpPost]
    public ActionResult DealerReturns(string SelectedYear)
    {
        string ptin = Session["Tin"].ToString();

        DealerReturnsModel model = new DealerReturnsModel();

        model.ptin = ptin;
        model.enterprise_name = Session["enterprise_name"]?.ToString();
        model.f_year = SelectedYear;

        model.dlr_ret_dtls = _dal.GetReturnDataByYear(ptin, SelectedYear);

        return View(model);
    }



    public ActionResult DealerDCB()
    {
        if (Session["Tin"] == null)
        {
            return RedirectToAction("Home", "PTHome");
        }
        string ptin = Session["Tin"].ToString();
        DealerReturnsModel model = _dal.GetDealerDtls(ptin);
        return View(model);
    }

    [HttpPost]
    public ActionResult DealerDCB(string SelectedYear)
    {
        string ptin = Session["Tin"].ToString();

        DealerReturnsModel model = new DealerReturnsModel();

        model.ptin = ptin;
        model.enterprise_name = Session["enterprise_name"]?.ToString();
        model.f_year = SelectedYear;

        model.dlr_ret_dtls = _dal.GetReturnDataByYear(ptin, SelectedYear);

        return View(model);
    }



}
