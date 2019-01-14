using System;
using System.Security.Claims;
using System.Web.Mvc;
using IMOD.CredenciamentoWeb.Models;
using IMOD.CredenciamentoWeb.ViewModel;

namespace IMOD.CredenciamentoWeb.Controllers
{
    
    public class NavbarController : Controller
    {
        // GET: Navbar
       
        public ActionResult Index()
        {
            try
            {
               // return null;
                //if(!User.Identity.IsAuthenticated) throw  new Exception("Usuário não autenticado.");
                //var user = (ClaimsIdentity)User.Identity; 
                //var key = user.FindFirst("id");
                //var usuarioModel = new UsuarioModel();
                //var id = int.Parse(key.Value);
                //var usuario = usuarioModel.BuscarPelaChave(id);

                var data = new MenuNavBarModel();
                //return PartialView("_Navbar", data.ObterMenuItem(usuario.Perfil.ToLower()).ToList());
                return PartialView("_Navbar", data.ObterMenuItem("adm"));
            }
            catch (Exception ex)
            {
               //throw  new Exception($"Não foi possível criar menu Razão<br>{ex.Message}");
               return RedirectToAction("LogIn","Usuario");
            }
           
        }
    }
}