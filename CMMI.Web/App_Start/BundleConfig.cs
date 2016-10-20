using System.Web;
using System.Web.Optimization;

namespace CMMI.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/spa/libs").Include(
                "~/bower_components/angular/angular.js",
                "~/bower_components/angular-bootstrap/ui-bootstrap-tpls.js",
                "~/bower_components/angular-route/angular-route.js"
                ));

            bundles.Add(new StyleBundle("~/spa/styles").Include(
                "~/bower_components/angular/angular-csp.css",
                "~/bower_components/angular-bootstrap/ui-bootstrap-csp.css",
                "~/Content/bootstrap.css",
                "~/Content/site.css"
                ));

            bundles.Add(new ScriptBundle("~/spa/components").IncludeDirectory("~/Components", "*.js", true));
            bundles.Add(new ScriptBundle("~/spa/app").Include("~/app.js"));
        }
    }
}
