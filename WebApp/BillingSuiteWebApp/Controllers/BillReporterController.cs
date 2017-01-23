using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SignalAPILib;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BillingSuiteWebApp.Controllers
{

#if DEBUG || STAGING || DEVAPP
    [Authorize(Roles = "BillingSuite_Developer")]
#else
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_BillingReporter")]
#endif
    public class BillReporterController : BaseController
    {
        public async Task<ActionResult> BillReporter()
        {
            ViewBag.Message = "Billing Reporter";            
            return PartialView();
        }

        public async Task<JsonResult> GetBillReports([DataSourceRequest] DataSourceRequest request, DateTime StartDate, DateTime EndDate)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["fromdate"] = StartDate.Date.ToShortDateString();
            parameters["todate"] = EndDate.AddDays(1).Date.ToShortDateString();
            parameters["datetype"] = "completeddate";
            IEnumerable<BillingStatusCase> cases = null;
            try
            {
                cases = await SignalAPILib.ClientApi<BillingStatusCase>.GetAsync(ClientApi<BillingStatusCase>.ArgType.TypeParamsFromURI, parameters.ToString());
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }

            return Json(cases.ToList<BillingStatusCase>().ToDataSourceResult(request));
        }

        public async Task<JsonResult> GetUnbilled([DataSourceRequest] DataSourceRequest request, DateTime StartDate, DateTime EndDate, string BillingAggregate)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["fromdate"] = StartDate.Date.ToShortDateString();
            parameters["todate"] = EndDate.AddDays(1).Date.ToShortDateString();
            parameters["billingAggregate"] = BillingAggregate;
            IEnumerable<BillingStatusCase> cases = null;
            try
            {
                cases = await SignalAPILib.ClientApi<BillingStatusCase>.GetAsync(ClientApi<BillingStatusCase>.ArgType.TypeParamsFromURI, parameters.ToString());
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }

            return Json(cases.ToList<BillingStatusCase>().ToDataSourceResult(request));
        }


        public async Task<JsonResult> GetBillReportsUnbilled([DataSourceRequest] DataSourceRequest request, DateTime StartDate, DateTime EndDate)
        {
            string parameters = "filter=unbilled";

            IEnumerable<BillingStatusCase> cases = null;
            try
            {
                cases = await SignalAPILib.ClientApi<BillingStatusCase>.GetAsync(ClientApi<BillingStatusCase>.ArgType.TypeParamsFromURI, parameters);
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }

            return Json(cases.ToList<BillingStatusCase>().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdateBillingStatusCase([DataSourceRequest] DataSourceRequest request, BillingStatusCase theCase)
        {
            try
            {
                theCase = await SignalAPILib.ClientApi<BillingStatusCase>.PutAsync(theCase); //put is for updates
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("UpdateError", ex.Message);
            }
            return Json(new[] { theCase }.ToDataSourceResult(request, ModelState));
        }


        public async Task<ActionResult> GetBillingEvents([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<BillingEvent> billingEvents = null;
            try
            {
                billingEvents = await SignalAPILib.ClientApi<BillingEvent>.GetAsync();
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(billingEvents.ToList<BillingEvent>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBillingEvent([DataSourceRequest] DataSourceRequest request,
            DateTime dateFrom,
            DateTime dateTo,
            string billingAggregate,
            string comment
        )
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["dateFrom"] = dateFrom.Date.ToShortDateString();
            parameters["dateTo"] = dateTo.AddDays(1).Date.ToShortDateString();
            parameters["billingAggregate"] = billingAggregate;
            parameters["comment"] = comment;

            BillingEvent billingEvent = null;
            try
            {
                billingEvent = await SignalAPILib.ClientApi<BillingEvent>.GetSingleAsync(
                    ClientApi<BillingEvent>.ArgType.TypeParamsFromURI,
                    parameters.ToString());
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(new[] { billingEvent }.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public async Task<FileResult> GetReportDownload([DataSourceRequest] DataSourceRequest request, string reporttype, string billingid)
        {
            string parameters = "requestType=Report&reportType=" + reporttype + "&billingEventCode=" + billingid;
            FileHelper fh = await ClientApi<CaseBillingReport>.GetFileAsync(ClientApi<CaseBillingReport>.ArgType.TypeParamsFromURI, parameters);
            UTF8Encoding encoder = new UTF8Encoding();
            return File(fh.FileBytes, fh.ContentType, fh.FileName);
        }

        public async Task<FileResult> GetRangeSummaryReportDownload([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to)
        {

            string fdatestr = from.Date.ToShortDateString();
            string tdatestr = to.AddDays(1).Date.ToShortDateString();
            string parameters = "requestType=billingsummary&fromdate=" + fdatestr+ "&todate=" + tdatestr;
            FileHelper fh = await ClientApi<CaseBillingReport>.GetFileAsync(ClientApi<CaseBillingReport>.ArgType.TypeParamsFromURI, parameters);
            UTF8Encoding encoder = new UTF8Encoding();
            return File(fh.FileBytes, fh.ContentType, fh.FileName);
        }
    }


}