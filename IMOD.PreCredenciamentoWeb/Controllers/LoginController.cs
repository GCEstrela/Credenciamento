using IMOD.PreCredenciamentoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;
using System.Web.Security;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class LoginController : Controller
    {

        IEmpresaService service = new EmpresaService();

        // GET: Login
        public ActionResult Index()
        {
            return View(new EmpresaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmpresaViewModel empresa)
        {
            // esta action trata o post (login)
            if (ModelState.IsValid) //verifica se é válido
            {
                var lista = service.Listar();
                var empresaLogada = lista.Where(e => e.Cnpj.Equals(empresa.CNPJ) && e.Senha.Equals(empresa.Senha)).FirstOrDefault();

                if (empresaLogada != null)
                {
                    empresa.Codigo = empresaLogada.EmpresaId;
                    empresa.Nome = empresaLogada.Nome;
                    empresa.Senha = string.Empty;
                    Session["EmpresaLogada"] = empresa;
                    FormsAuthentication.SetAuthCookie(empresa.Nome, empresa.Lembreme);
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "O login está incorreto!");
                }

            }
            return View(empresa);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Login");
        }

    }
}