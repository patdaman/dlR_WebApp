using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SignalAPILib;
using ViewModel;
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
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_DailyStatusReport")]
#endif
    public class DailyStatusReportController : BaseController
    {
        // GET: DailyStatusReport
        public async Task<ActionResult> DailyStatusReport()
        {
            ViewBag.Message = "Daily Status Report";
            return PartialView();
        }

        public async Task<JsonResult> GetStatusReportItems([DataSourceRequest] DataSourceRequest request, DateTime StartDate, DateTime EndDate)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["fromdate"] = StartDate.Date.ToShortDateString();
            parameters["todate"] = EndDate.AddDays(1).ToShortDateString();
            parameters["datetype"] = "orderdate";
            IEnumerable<BillingStatusCase> reportItems = null;

            try
            {
                reportItems = await SignalAPILib.ClientApi<BillingStatusCase>.GetAsync(ClientApi<BillingStatusCase>.ArgType.TypeParamsFromURI, parameters.ToString());
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }

            return Json(reportItems.ToList<BillingStatusCase>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private async Task<BillingStatusCase> GetLatestValidCaseAsync()
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["latest"] = "true";
            var result = await SignalAPILib.ClientApi<BillingStatusCase>.GetSingleAsync(
                ClientApi<BillingStatusCase>.ArgType.TypeParamsFromURI, parameters.ToString());
            return result;
        }

    }
}