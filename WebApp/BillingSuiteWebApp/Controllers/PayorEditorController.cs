using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
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
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_PayorEditor")]
#endif
    public class PayorEditorController : BaseController
    {

        public async Task<ActionResult> PayorEditor()
        {
            ViewBag.Message = "Billing Payor Editor";
            return PartialView("PayorEditor");
        }
        public async Task<JsonResult> GetPayors([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<Payor> payors = null;
            try
            {
                payors = await SignalAPILib.ClientApi<Payor>.GetAsync();
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }

            return Json(payors.Where(a => !a.Name.Equals("Unassigned")).ToList<Payor>().ToDataSourceResult(request));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdatePayor([DataSourceRequest] DataSourceRequest request, Payor pp)
        {
            try
            {
                pp = await SignalAPILib.ClientApi<Payor>.PutAsync(pp); //put is for updates
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("UpdateError", ex.Message);
            }
            return Json(new[] { pp }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> InsertPayor([DataSourceRequest] DataSourceRequest request, Payor pp)
        {
            try
            {
                pp.PayorCode = pp.PayorCode.ToUpper();
                pp = await SignalAPILib.ClientApi<Payor>.PostAsync(pp); //post is for inserts
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("CreationError", ex.Message);
            }
            return Json(new[] { pp }.ToDataSourceResult(request, ModelState));
        }

    }
}