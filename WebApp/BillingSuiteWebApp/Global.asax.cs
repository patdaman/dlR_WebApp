﻿using BillingSuiteWebApp.Controllers;
using CommonUtils.Exception;
using CommonUtils.Logging;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BillingSuiteWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog iLog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoggingPatternUser logger = new LoggingPatternUser() { Logger = iLog };

        protected void Application_Start()
        {            
            AreaRegistration.RegisterAllAreas();            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);                        
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            LogError();
            HandleError(sender);
        }

        private void LogError()
        {
            // Get the exception object.
            Exception ex = Server.GetLastError();
            logger.Error("An unhandled exception occurred. Details follow.");
            logger.Error(String.Format("{0}\n\nStack Trace:\n{1}",
                ExamineException.GetInnerExceptionMessages(ex),
                ExamineException.GetInnerExceptionStackTraces(ex)));
        }

        private void HandleError(object sender)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "Index";

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    // catch for specific view
                    case 404:
                        action = "NotFound";
                        break;
                }
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}
