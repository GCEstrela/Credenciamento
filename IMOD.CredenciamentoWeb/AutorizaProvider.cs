#region

using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using IMOD.CredenciamentoWeb.Models;
using Microsoft.Owin.Security.OAuth;

#endregion

namespace IMOD.CredenciamentoWeb
{
    public class AutorizaProvider : OAuthAuthorizationServerProvider
    {
        #pragma warning disable 1998
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        #pragma warning restore 1998
        {
            //O método ValidateClientAuthentication realiza as validações 
            //quando o usuário se autenticar usando o token de acesso gerado;
            //Valida se token é valido, se expirou
            context.Validated();
        }

        /// <summary>
        ///     Responsavel pela autenticação
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Vai e  volta de uma origem  
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin",new[] {"*"});
            
            //Obter usuario
            var usuarioModel = new UsuarioModel();
            var email = context.UserName;
            var senha = context.Password;
            //TODO: Login aqui...
            //if (usuarioModel.Login(email, senha))
            //{
            //    var usuario = usuarioModel.Listar().Single(k => k.Email == email);
              var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nome));
            // identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            //identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Perfil));

            identity.AddClaim(new Claim(ClaimTypes.Name, "Valnei"));
            identity.AddClaim(new Claim(ClaimTypes.Email, "v_marinpietri@yahoo.com.br"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "adm"));
            context.Validated(identity);
            //    //Manager da coneao
               var roles = identity.Claims.Where(m => m.Type == "Role").Select(n => n.Value).ToArray();
            //    //O metodo GenericPrincipal serve para passar a Thread, para poder recuperar os dados de identidade no controller
                var principal = new GenericPrincipal(identity, roles);
               Thread.CurrentPrincipal = principal;
            //}
            //else
            //{
            //    context.SetError("acesso inválido", "As credenciais do usuário não conferem");
            //}
        }
    }
}