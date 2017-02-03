using System.Web;
using System.Web.Optimization;

namespace eFormFrontendDotNet
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/wow.min.js",
                      "~/Scripts/jquery.ujs.js",
                      "~/Scripts/jquery.ui.widget.js",
                      "~/Scripts/jquery.fileupload.js",
                      "~/Scripts/jquery.fileupload-process.js",
                      "~/Scripts/jquery.fileupload-validate.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/buttons.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-switch.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/jquery.pnotify.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/trumbowyg.js",
                      "~/Scripts/mt_layout.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/mt_layout.css",
                      "~/Content/trumbowyg.min.css",
                      "~/Content/docs.min.css",
                      "~/Content/mt_overrides_trumbowyg.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/jquery.pnotify.default.css",
                      "~/Content/jquery.pnotify.default.icons.css",
                      "~/Content/jquery.fileupload.css",
                      "~/Content/site.css"));
        }
    }
}
