using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IMOD.CredenciamentoWeb.Filters
{
    public class ClaimsAuthorizeAttribute:AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;

            if (user.Claims.Where(c => c.Type == ClaimTypes.Name).Any(x => x.Value == "Master"))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary
                                   {
                                         { "action", "Login" },
                                         { "controller", "Home" }
                                   });
            }
        }

    }
}