// ***********************************************************************
// Project: IMOD.CredenciamentoWeb
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 10 - 2019
// ***********************************************************************

#region

using System.Collections.Generic;
using System.Web.Optimization;

#endregion

namespace IMOD.CredenciamentoWeb
{
    public class BundleConfig
    {
        #region  Metodos

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add (new ScriptBundle ("~/bundles/jquery").Include (
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js"));
            // "~/Scripts/jquery.validate*",

            var valBundle = new ScriptBundle ("~/bundles/jqueryval").Include (
                "~/Scripts/jquery.validate.js",
                "~/Scripts/globalize.js",
                "~/Scripts/jquery.validate.globalize.js",
                "~/Scripts/methods_pt.js"
                );

            valBundle.Orderer = new AsIsBundleOrderer();
            bundles.Add (valBundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add (new ScriptBundle ("~/bundles/modernizr").Include (
                "~/Scripts/modernizr-*"));

            bundles.Add (new ScriptBundle ("~/bundles/bootstrap").Include (
                "~/browser_components/bootstrap/dist/js/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/jquery.maskMoney.min.js",
                "~/Scripts/locales/bootstrap-datepicker.pt-BR.min.js",
                "~/Scripts/toastr.js",
                "~/Scripts/utils.js"
                ));

            bundles.Add (new StyleBundle ("~/Content/css").Include (
                "~/browser_components/bootstrap/dist/css/bootstrap.css",
                "~/browser_components/metisMenu/dist/metisMenu.min.css",
                "~/Content/timeline.css",
                "~/Content/sb-admin-2.css",
                "~/Content/bootstrap-datepicker.css",
                "~/Content/validate-summary.css",
                "~/Content/toastr.css"
                ));
        }

        #endregion
    }

    public class AsIsBundleOrderer : IBundleOrderer

    {
        #region  Metodos

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)

        {
            return files;
        }

        #endregion
    }
}