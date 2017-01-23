using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CommonUtils.Build;

namespace BillingSuiteWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<JsonResult> GetCases([DataSourceRequest] DataSourceRequest request)
        {

            IEnumerable<Case> cases = await SignalAPILib.ClientApi<Case>.GetAsync();

            return Json(cases.ToList<Case>().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdateCase([DataSourceRequest] DataSourceRequest request, Case cs)
        {
            cs = await SignalAPILib.ClientApi<Case>.PutAsync(cs);
            return Json(new[] { cs }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Signal Genetics Billing Suite";
            return View();
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)] 
        public async Task<ActionResult> About()
        {
            ViewBag.Message = " Signal Genetics Billing Suite";

            Configuration cf =await SignalAPILib.ClientApi<Configuration>.GetSingleAsync(SignalAPILib.ClientApi<Configuration>.ArgType.TypeExactString, "");
            string buildTag = BuildUtils.CurrentConfiguration;

            Version v = typeof(MvcApplication).Assembly.GetName().Version;
            cf.AppAssemblyVersion = v.Major.ToString() + "." + v.Minor.ToString() + "." + v.Build.ToString() + "." + v.Revision.ToString();
            cf.AppBuildTag = buildTag;
            cf.AppServer = AssemblyUtils.MachineName;
            Response.Cache.SetCacheability(HttpCacheability.NoCache); // for ie
            Response.Cache.SetNoStore(); // for ff
            return PartialView(cf);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "For help or feedback.";

            return View();
        }

        public ActionResult Home()
        {
            ViewBag.Message = "Signal Genetics Billing Suite";
            return PartialView("Home");
        }


    }
}
