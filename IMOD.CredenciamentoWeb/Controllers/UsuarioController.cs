 
 

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using IMOD.CredenciamentoWeb.Models;
using IMOD.CredenciamentoWeb.ViewModel;
using IMOD.CrossCutting;

namespace IMOD.CredenciamentoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LoginViewModel {UrlRetorno = returnUrl};
            return View("Login", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel login)
        {
            if (!ModelState.IsValid) return View("Login");
            var usuarioModel = new UsuarioModel();
            var loginSuccess = usuarioModel.Login(login.Email, login.Senha);
            if (!loginSuccess)
            {
                ViewBag.Error = "Usuário ou senha inválidos";
                return View("Login");
            }
            var user = usuarioModel.ObterUsuario(login.Email.ToLower(),login.Email.ToLower());
            var declaracoes = usuarioModel.ObterIdentidade(user);

            var clientAuth = Request.GetOwinContext().Authentication;
            clientAuth.SignIn(declaracoes);
            return Redirect(ObterUrlRetorno(login.UrlRetorno));
        }

        public ActionResult LogOut()
        {
            var clientAuth = Request.GetOwinContext().Authentication;
            clientAuth.SignOut("cookieAutentication");
            return RedirectToAction("LogIn");
        }

        /// <summary>
        ///     Obter o retorno da url
        /// </summary>
        /// <param name="urlRetorno"></param>
        /// <returns></returns>
        private string ObterUrlRetorno(string urlRetorno)
        {
            if (string.IsNullOrWhiteSpace(urlRetorno) | !Url.IsLocalUrl(urlRetorno))
                return Url.Action("Index", "Home");
            return urlRetorno;
        }

        //[Authorize(Roles = "Admin")]
        //[ValidateAntiForgeryToken]
        public ActionResult Editar(int idUser)
        {
            var usuarioModel = new UsuarioModel();
            var model = usuarioModel.BuscarPelaChave(idUser);
            var model2 = Mapper.Map<UsuarioViewModel>(model);
            model2.Senha = Utils.DecryptAes(model2.Senha);
            return View("Editar", model2);
        }

        //[HandleError]
        //public ActionResult Conta()
        //{
        //    ////Exceto usuario master, o usuario deve acessar apenas as suas licenças
        //    //var user = (ClaimsIdentity)User.Identity;
        //    //var key = user.FindFirst("id");
        //    //var usuarioModel = new UsuarioModel();
        //    //var id = int.Parse(key.Value);
        //    ////Obter usuario logado
        //    //var model = usuarioModel.Conta(id); 
        //    var usuarioModel = new UsuarioModel();
        //    var model = usuarioModel.ObterUsuario(User);
        //    return View("Conta",model);
        //}

        [ValidateAntiForgeryToken]
        [HttpPost]
        public void Editar(UsuarioViewModel usuarioViewModel)
        {
            var usuarioModel = new UsuarioModel();
            var model2 = Mapper.Map<Usuario>(usuarioViewModel);
            usuarioModel.Alterar(model2);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public void EditarConta(UsuarioProfileViewModel usuarioViewModel)
        {
            var usuarioModel = new UsuarioModel(); 
            var usuario = usuarioModel.BuscarPelaChave(usuarioViewModel.IdUser);
            usuario.Nome = usuarioViewModel.Nome;
            usuario.Senha = usuarioViewModel.Senha;
            usuario.Endereco = usuarioViewModel.Endereco;
            usuario.Cep = usuarioViewModel.Cep;
            usuarioModel.Alterar(usuario);
        }

       

        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public void Novo(UsuarioViewModel usuarioViewModel)
        {
            try
            {
                var usuarioModel = new UsuarioModel();
                var user = Mapper.Map<Usuario>(usuarioViewModel);
                usuarioModel.Criar(user); 
            }
            catch (Exception ex)
            {
                throw new HttpException(400, ex.Message);
            }
          
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Novo()
        { 
            return View("Novo");
        }


        /// <summary>
        ///     Listar Usuários
        /// </summary>
        /// <returns></returns>
        public ActionResult Listar()
        {
            var usuarioModel = new UsuarioModel();
            //Listar todos os usuarios, exceto o administrador
            var model = usuarioModel.ListarAdministrador();
            var lstmodel = Mapper.Map<IEnumerable<UsuarioViewModel>>(model);
            return View("Listar", lstmodel);
        }
    }
}