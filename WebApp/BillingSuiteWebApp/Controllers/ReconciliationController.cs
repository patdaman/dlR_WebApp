using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BillingSuiteWebApp.Controllers
{
#if DEBUG || STAGING || DEVAPP
    [Authorize(Roles = "BillingSuite_Developer")]
#else
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_Reconciliation")]
#endif
    public class ReconciliationController : BaseController
    {
        public async Task<ActionResult> Reconciliation()
        {            
            return PartialView();
        }
    }
}
