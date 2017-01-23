///-------------------------------------------------------------------------------------------------
// <copyright file="CaseEditorController.cs" company="Signal Genetics Inc.">
// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Dtorres</author>
// <date>20151027</date>
// <summary>Implements the case editor controller class</summary>
///-------------------------------------------------------------------------------------------------

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SignalAPILib;
using System.Data;
using PathCentralApiLib;

namespace BillingSuiteWebApp.Controllers
{
#if DEBUG || STAGING || DEVAPP
    [Authorize(Roles = "BillingSuite_Developer")]
#else
    [Authorize(Roles = "BillingSuite_Admin, BillingSuite_CaseEditor, BillingSuite_DailyStatusReport")]
#endif
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A controller for handling the case editor. </summary>
    ///
    /// <remarks>   Dtorres, 20151027. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CaseEditorController : BaseController
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Case editor. </summary>
        ///
        /// <remarks>   Dtorres, 20151027. </remarks>
        ///
        /// <returns>   A Task&lt;ActionResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public async Task<ActionResult> CaseEditor()
        {  

#if DEBUG || STAGING || DEVAPP
            bool isBillingAdminRole = true;
#else
            bool isBillingAdminRole = User.IsInRole("BillingSuite_Billing_Admin");
#endif

            ViewBag.IsBillingAdminRole = isBillingAdminRole;
            ViewBag.Message = "Billing Case Editor";
            return PartialView();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the cases. </summary>
        ///
        /// <remarks>   Dtorres, 20151027. </remarks>
        ///
        /// <param name="request">      The request. </param>
        /// <param name="StartDate">    The start date. </param>
        /// <param name="EndDate">      The end date. </param>
        ///
        /// <returns>   The cases. </returns>
        ///-------------------------------------------------------------------------------------------------

        public async Task<JsonResult> GetCases([DataSourceRequest] DataSourceRequest request, DateTime StartDate, DateTime EndDate)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["fromdate"] = StartDate.Date.ToShortDateString();
            parameters["todate"] = EndDate.AddDays(1).ToShortDateString();
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the case. </summary>
        ///
        /// <remarks>   Dtorres, 20151027. </remarks>
        ///
        /// <param name="request">  The request. </param>
        /// <param name="theCase">  the case. </param>
        ///
        /// <returns>   A Task&lt;JsonResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<JsonResult> UpdateCase([DataSourceRequest] DataSourceRequest request, BillingStatusCase theCase)
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets requisition form. </summary>
        ///
        /// <remarks>   Dtorres, 20151027. </remarks>
        ///
        /// <param name="request">  The request. </param>
        /// <param name="accId">    Identifier. </param>
        ///
        /// <returns>   The requisition form. </returns>
        ///-------------------------------------------------------------------------------------------------

        public ActionResult GetRequisitionForm([DataSourceRequest] DataSourceRequest request, int accId)
        {
            try
            {
                //SignalAPILib.PathCentral.SoapAPI pcapi = new SignalAPILib.PathCentral.SoapAPI(SignalAPILib.PathCentral.PathCentralSoapType.Soap12, SignalAPILib.PathCentral.PathCentralReleaseConfiguration.Production);
                //DataSet ds = pcapi.AccessRequisitionForm_ByAccessionID(accId);
                //FileHelper fh = ds.GetPathCentralPDFFile();
                // 
                PathCentralApi pcapi = new PathCentralApi(PathCentralSoapType.Soap12, PathCentralReleaseConfiguration.Production);
                List<PathCentralPdf> list = pcapi.AccessRequisitionForm_ByAccessionID(accId);
                FileHelper fh = list.GetPathCentralPDFFile();                
                if (fh.Status != FileHelper.FileHelperStatus.OK)
                    return Content("<script language='javascript' type='text/javascript'>alert('Requisition not found for accession id:" + accId.ToString()+ "!');</script>");
                return File(fh.FileBytes, fh.ContentType, fh.FileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }            
        }
    }
}