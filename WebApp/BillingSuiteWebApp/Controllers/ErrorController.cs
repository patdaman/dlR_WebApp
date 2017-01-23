using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BillingSuiteWebApp.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.DeveloperRole = "BillingSuite_Developer";
            return View();
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}