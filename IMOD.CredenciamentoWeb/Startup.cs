 

using System;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using IMOD.CredenciamentoWeb.AutoMapper;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace IMOD.CredenciamentoWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperConfig.RegisterMappings();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

            #region Config. Formatações JSON
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;

            var settings = jsonFormatter.SerializerSettings;
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            #endregion


            //Routes
            var config = new HttpConfiguration();
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            //Formatar Date para pt-br, web.config
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
          
            ConfigRoutes(config);
            //Ativando CORS
            app.UseCors(CorsOptions.AllowAll);
                    //Permitir que qualquer sistema solicite acessar serviços a este webApi, caso autorizado
            ConfigOAuth(app);
            //Configurar autenticação por cookie
            ConfigCookieAutentication(app);
            //Ativando configuração WebApi
            app.UseWebApi(config);
            
        }
        /// <summary>
        /// Configuração de autorização por Cookie
        /// </summary>
        /// <param name="app"></param>
        private static void ConfigCookieAutentication(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {

                AuthenticationType = "cookieAutentication",
                CookieName = "_GEstrela_AUTH",
                LoginPath = new PathString("/Home/Login"),
                LogoutPath = new PathString("/Home/index")
            });

        }

        /// <summary>
        /// Configurações de Rota
        /// </summary>
        /// <param name="config"></param>
        private static void ConfigRoutes(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();
            config.
                    Routes.
                    MapHttpRoute("credenciamento",
                            "api/v1/{controller}/{id}",
                            new {id = RouteParameter.Optional});
        }

        /// <summary>
        /// Configuração de autorização por Token
        /// </summary>
        /// <param name="app"></param>
        private static void ConfigOAuth(IAppBuilder app)
        {
            var oauthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true, //permite logins sem o HTTPS
                TokenEndpointPath = new PathString("/credenciamento/api/token"),
                //endereco onde serão realizadas as requisiçõe para obtenção do token
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20), //tempo em que o token irá expirá
                Provider = new AutorizaProvider()
                //O provedor de acesso ao token pode ser google ou qualquer outro, neste caso será um provider customizado
            };
            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}