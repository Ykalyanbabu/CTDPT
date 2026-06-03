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
            return RedirectToAction("Login", "PTHome");
        }

        return View();
    }
    public ActionResult DealerReturns()
    {
        if (Session["Tin"] == null)
        {
            return RedirectToAction("Login", "PTHome");
        }
        string ptin = Session["Tin"].ToString();

        DealerReturnsModel model = _dal.GetDealerDtls(ptin);
        Session["enterprise_name"] = model.enterprise_name;
        return View(model);
    }

    [HttpPost]
    public ActionResult DealerReturns(string SelectedYear)
    {

        if (Session["Tin"] == null)
        {
            return RedirectToAction("Login", "PTHome");
        }
        string ptin = Session["Tin"].ToString();

        DealerReturnsModel model = new DealerReturnsModel();

        model.ptin = ptin;
        model.enterprise_name = Session["enterprise_name"]?.ToString();
        model.f_year = SelectedYear;

        model.dlr_ret_dtls = _dal.GetReturnDataByYear(ptin, SelectedYear);
        
        return View(model);
    }

    [HttpGet]
    public ActionResult ViewReturn(string returnid)
    {
        if (Session["Tin"] == null)
        {
            return RedirectToAction("Login", "PTHome");
        }
        string ptin = Session["Tin"].ToString();
        dlr_ret_dtls model = new dlr_ret_dtls();
        model.ptin = ptin;
        model.enterprise_name = Session["enterprise_name"]?.ToString();
        var response = _dal.GetReturnByReturnId(ptin, returnid);

        return View(response);
    }



    public ActionResult DealerDCB()
    {
        if (Session["Tin"] == null)
        {
            return RedirectToAction("Login", "PTHome");
        }
        string ptin = Session["Tin"].ToString();
        DealerReturnsModel model = _dal.GetDealerDtls(ptin);


        return View(model);
    }

    [HttpPost]
    public ActionResult DealerDCB(string SelectedYear)
    {
        if (Session["Tin"] == null)
        {
            return RedirectToAction("Login", "PTHome");
        }
        string ptin = Session["Tin"].ToString();

        DealerReturnsModel model = new DealerReturnsModel();

        model.ptin = ptin;
        model.enterprise_name = Session["enterprise_name"]?.ToString();
        model.f_year = SelectedYear;

        model.dlr_ret_dtls = _dal.GetReturnDataByYear(ptin, SelectedYear);

        return View(model);
    }


}
