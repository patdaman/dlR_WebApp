using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rapid_e4473.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult atf_4473_Form()
        {
            return File(@"~\Files\atf-f-4473-1.pdf", System.Net.Mime.MediaTypeNames.Application.Pdf);
        }
    }
}