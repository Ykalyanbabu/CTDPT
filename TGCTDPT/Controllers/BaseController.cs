using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TGCTDPT.Controllers
{
    [AllowAnonymous] 
    public abstract class BaseController : Controller
    {
        
        private readonly Dictionary<string, List<string>> _publicActions = new Dictionary<string, List<string>>
        {
            // Public payment callbacks
            /*{ "PTHome", new List<string> { "Home", "Index", "About", "Contact" } },
            { "Account", new List<string> { "Login", "Register", "ForgotPassword", "ResetPassword" } },
            { "Payment", new List<string> { "PaymentReturn", "PaymentCallback" } } */
        };

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get current controller and action names
            string currentController = filterContext.RouteData.Values["controller"]?.ToString() ?? "";
            string currentAction = filterContext.RouteData.Values["action"]?.ToString() ?? "";

            // Check if current page is public (doesn't require session)
            bool isPublicPage = IsPublicPage(currentController, currentAction);

            // For AJAX requests, handle differently
            bool isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();

            // Check if session exists and has Tin value
            bool hasValidSession = filterContext.HttpContext.Session != null &&
                                   filterContext.HttpContext.Session["Tin"] != null;

            // If no valid session and page is not public, redirect to home
            if (!hasValidSession && !isPublicPage)
            {
                if (isAjaxRequest)
                {
                    // For AJAX requests, return JSON with redirect info
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            sessionExpired = true,
                            redirectUrl = Url.Action("Home", "PTHome")
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    // For normal requests, redirect to home page
                    string returnUrl = filterContext.HttpContext.Request.RawUrl;

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                    { "controller", "PTHome" },
                    { "action", "Home" }
                        }
                    );
                }

                // IMPORTANT: This prevents the original action from executing
                return;
            }
        }

        private bool IsPublicPage(string controller, string action)
        {
            if (_publicActions.ContainsKey(controller))
            {
                if (_publicActions[controller].Count == 0)
                    return true;

                return _publicActions[controller].Contains(action, StringComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        public string CurrentUserTin
        {
            get
            {
                return Session["Tin"]?.ToString();
            }
        }
        public bool IsUserLoggedIn
        {
            get
            {
                return Session != null && Session["Tin"] != null;
            }
        }

        public void ClearSession()
        {
            if (Session != null)
            {
                Session.Clear();
                Session.Abandon();
            }
        }
    }
}