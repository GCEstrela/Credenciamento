// ***********************************************************************
// Project: IMOD.CredenciamentoWeb
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 10 - 2019
// ***********************************************************************

#region

using System.Web.Http;

#endregion

namespace IMOD.CredenciamentoWeb
{
    public static class WebApiConfig
    {
        #region  Metodos

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute (
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
                );
        }

        #endregion
    }
}