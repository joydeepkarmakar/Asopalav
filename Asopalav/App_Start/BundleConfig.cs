using System.Web;
using System.Web.Optimization;

namespace Asopalav
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Script/Common").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery-ui.js",
                "~/Scripts/respond.js",
                "~/Scripts/xzoom.min.js",
                "~/Scripts/main.js",
                "~/Scripts/jquery-1.11.3.min.js.download.js",
                "~/Scripts/bootstrap.min.js.download.js",
                "~/Scripts/jquery.smartmenus.js.download.js",
                "~/Scripts/jquery.smartmenus.bootstrap.js.download.js",
                "~/Scripts/plugins.js",
                "~/Scripts/dropzone.js",
                "~/Scripts/toastr.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Script/Layout").Include(
                "~/Scripts/setup.js"
                ));

            bundles.Add(new StyleBundle("~/Style/Common").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery-ui.structure.css",
                      "~/Content/jquery-ui.theme.css",
                      "~/Content/style.css",
                      "~/Content/xzoom.css",
                      "~/Content/jquery.smartmenus.bootstrap.css",
                      "~/Content/basic.css",
                      "~/Content/dropzone.css",
                      "~/Content/toastr.min.css"
                      ));
        }
    }
}