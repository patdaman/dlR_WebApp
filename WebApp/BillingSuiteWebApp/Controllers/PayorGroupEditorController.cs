using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_PayorGroupEditor")]
#endif
    public class PayorGroupEditorController : BaseController
    {
        public async Task<ActionResult> PayorGroupEditor()
        {
            ViewBag.Message = "Billing Payor Group Editor";
            return PartialView();
        }

        public async Task<JsonResult> GetPayorGroups([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<PayorGroup> payorGroups = null;
            try
            {
                payorGroups = await SignalAPILib.ClientApi<PayorGroup>.GetAsync();
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(payorGroups.ToList<PayorGroup>().ToDataSourceResult(request));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdatePayorGroup([DataSourceRequest] DataSourceRequest request, PayorGroup pg)
        {
#region Custom Validation
            IEnumerable<PayorGroup> payorGroups = await SignalAPILib.ClientApi<PayorGroup>.GetAsync();
            if (payorGroups.Where(p => p.PayorGroupName.Equals(pg.PayorGroupName) & p.StartDate >= pg.StartDate  & p.EndDate == null).Count() > 0)
            {
                ModelState.AddModelError("Invalid Start Date", "Start Date cannot be earlier or equal to the Start Date of previous contractual allowance for this Payor Group. Changes will be cancelled.");
            }
#endregion

            try
            {
                if (ModelState.IsValid)
                {
                    pg = await SignalAPILib.ClientApi<PayorGroup>.PutAsync(pg);
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("UpdateError", ex.Message);
            }
            return Json(new[] { pg }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> InsertPayorGroup([DataSourceRequest] DataSourceRequest request, PayorGroup pg)
        {
#region Custom Validation
            IEnumerable<PayorGroup> payorGroups = await SignalAPILib.ClientApi<PayorGroup>.GetAsync();
            if (payorGroups.Where(p => p.PayorGroupName.Equals(pg.PayorGroupName)).Count() > 0)
            {
                ModelState.AddModelError("Duplicate", "Duplicate Payor Group Name. Changes will be cancelled.");
            }
#endregion

            try
            {
                if (ModelState.IsValid)
                {
                    pg = await SignalAPILib.ClientApi<PayorGroup>.PostAsync(pg);
                }
            }
            catch (CustomException ex)
            {
               ModelState.AddModelError("CreationError", ex.Message);
            }
            return Json(new[] { pg }.ToDataSourceResult(request, ModelState));
        }

    }
}