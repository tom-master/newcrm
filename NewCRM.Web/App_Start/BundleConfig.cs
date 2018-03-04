using System.Web.Optimization;

namespace NewCRM.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //js
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-3.2.1.min.js",
                "~/Scripts/HoorayJs/jquery-browser.js",
                "~/Scripts/HoorayLibs/hooraylibs.js",
                "~/Scripts/bootstrap/js/bootstrap.min.js",
                "~/Scripts/sugar/sugar-1.4.1.min.js",
                "~/Scripts/HoorayJs/hros.min.js",
                "~/Scripts/validate/Validform_v5.3.2/Validform_v5.3.2_min.js",
                "~/Scripts/webuploader-0.1.5/webuploader.js"
                ));

            // 将 EnableOptimizations 设为 false 以进行调试。有关详细信息，
            // 请访问 http://go.microsoft.com/fwlink/?LinkId=301862

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#else
            BundleTable.EnableOptimizations = false;
#endif
        }
    }
}
