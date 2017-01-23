using System.Web;
using System.Web.Optimization;

namespace BillingSuiteWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string kendoScriptPath = "~/Scripts/kendo/2016.1.112/";

            //System.Web.Optimization.BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bool MinifyLibs = false;
            bool MinifyCustom = false;

            var angularBundle = new ScriptBundle("~/bundles/angular").Include(
               "~/Scripts/angular.js",
               "~/Scripts/angular-route.js",
               "~/Scripts/moment.js"
               );
            if (!MinifyLibs) angularBundle.Transforms.Clear();// no minification 
            bundles.Add(angularBundle);


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/custom.css",
                      "~/Content/FinalStyles.css"));

            var kendoBundle = new ScriptBundle("~/bundles/kendo").Include(            
             kendoScriptPath +  "kendo.all.min.js",
             kendoScriptPath +  "kendo.aspnetmvc.min.js",
             kendoScriptPath +  "kendo.angular.min.js"
            );
            if (!MinifyLibs) kendoBundle.Transforms.Clear();
            bundles.Add(kendoBundle);

            // need to be able to load these independently in order
            var modelsBundle = new ScriptBundle("~/bundles/app/Models").IncludeDirectory(
                          "~/AppScripts/Models/",
                          "*.js",
                          true
                          );

            if (MinifyCustom) modelsBundle.Transforms.Clear();
            bundles.Add(modelsBundle);

            var servicesBundle = new ScriptBundle("~/bundles/app/Services").IncludeDirectory(
              "~/AppScripts/Services/",
              "*.js",
              true
              );
            if (MinifyCustom) servicesBundle.Transforms.Clear();
            bundles.Add(servicesBundle);

            var controllersBundle = new ScriptBundle("~/bundles/app/Controllers").IncludeDirectory(
                "~/AppScripts/Controllers/",
                "*.js",
                true
                );
            if (MinifyCustom) controllersBundle.Transforms.Clear();
            bundles.Add(controllersBundle);

            var appBundle = new ScriptBundle("~/bundles/app").IncludeDirectory(
               "~/AppScripts/",
               "*.js",
               false
               );
            ///Looks like only the app loader needs to not be minified
            if (MinifyCustom) appBundle.Transforms.Clear();
            bundles.Add(appBundle);


            //BundleTable.EnableOptimizations = false;


        }
    }
}
