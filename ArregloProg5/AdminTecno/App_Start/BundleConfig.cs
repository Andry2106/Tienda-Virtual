using System.Web;
using System.Web.Optimization;

namespace AdminTecno
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/fontawesome/all.min.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.responsive.js",
            "~/Scripts/jquery-{version}.js"));



            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js",
                      "~/Scripts/fontawesome/all.min.js",
                      "~/Scripts/loadingoverlay.min.js",
                      "~/Scripts/sweetalert.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(

                "~/Content/site.css",
                "~/Content/CssBot.css",
                "~/Content/sweetalert.css"

                ));
        }
    }
}
