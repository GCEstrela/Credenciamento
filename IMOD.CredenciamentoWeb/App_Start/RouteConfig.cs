// ***********************************************************************
// Project: IMOD.CredenciamentoWeb
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 10 - 2019
// ***********************************************************************

#region

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace IMOD.CredenciamentoWeb
{
    public class RouteConfig
    {
        #region  Metodos

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

            routes.MapRoute (
                "Default",
                "{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }

        #endregion
    }
}