using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModel;
using Kendo.Mvc.Extensions;

namespace BillingSuiteWebApp.Controllers
{
#if DEBUG || STAGING || DEVAPP
    [Authorize(Roles = "BillingSuite_Developer")]
#else
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_PhysicianEditor")]
#endif
    public class PhysicianEditorController : BaseController
    {

    
        public async Task<ActionResult> PhysicianEditor()
        {
            ViewBag.Message = "Physician Editor";
            return PartialView();
        }
        public async Task<JsonResult> GetPhysicians([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<Physician> physicians = null;
            try
            {
                physicians = await SignalAPILib.ClientApi<Physician>.GetAsync();
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("RetrieveError", ex.Message);
            }
            return Json(physicians.ToList<Physician>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdatePhysician([DataSourceRequest] DataSourceRequest request, Physician ph)
        {
            try
            {
                ph = await SignalAPILib.ClientApi<Physician>.PutAsync(ph);
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("UpdateError", ex.Message);
            }
            return Json(new[] { ph }.ToDataSourceResult(request, ModelState));
        }


        /// do not allow insert for Physician editor page
        //[AcceptVerbs(HttpVerbs.Post)]
        //public async Task<JsonResult> InsertPhysician([DataSourceRequest] DataSourceRequest request, Physician ph)
        //{

        //    try
        //    {
        //        ph = await SignalAPILib.ClientApi<Physician>.PostAsync(ph);
        //    }
        //    catch (SignalException ex)
        //    {
        //        ModelState.AddModelError("CreationError", ex.Message);
        //    }
        //    return Json(new[] { ph }.ToDataSourceResult(request, ModelState));
        //}

    }
}