using System.Web.Optimization;

namespace Sb.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Vendor styles
            bundles.Add(new StyleBundle("~/styles/vendor").Include(
                "~" + Links.Assets.Vendor.normalize_css,
                "~" + Links.Assets.Vendor.foundation_5_5_3.foundation_min_css,
                "~" + Links.Assets.Vendor.font_awesome_4_6_1.css.font_awesome_min_css
                ));

            // Our styles
            bundles.Add(new StyleBundle("~/styles/app").Include(
                "~" + Links.Assets.app_min_css
                ));

            // Scripts to load first
            bundles.Add(new ScriptBundle("~/scripts/head").Include(
                "~" + Links.Assets.Vendor.modernizr.modernizr_custom_js
                ));

            // Scripts to load later
            bundles.Add(new ScriptBundle("~/scripts/foot").Include(
                "~" + Links.Assets.Vendor.foundation_5_5_3.foundation_min_js,
                "~" + Links.Assets.App.Js.startup_js,
                "~" + Links.Assets.App.Js.webchat_js
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
