using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BillingSuiteWebApp.Controllers
{
#if DEBUG || STAGING || DEVAPP
    [Authorize(Roles = "BillingSuite_Developer")]
#else
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_Tracker")]
#endif
    public class AccessionTrackingController : BaseController
    {
        // GET: AccessionTracking
        public async Task<ActionResult> AccessionTracking()
        {         
            return PartialView();
        }
    }
}