using IMOD.PreCredenciamentoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;

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
                lista.Where(e => e.Cnpj.Equals(empresa.CNPJ));




                //var v = dc.Usuarios.Where(a => a.NomeUsuario.Equals(u.NomeUsuario) && a.Senha.Equals(u.Senha)).FirstOrDefault();
                //if (v != null)
                //{
                //    Session["usuarioLogadoID"] = v.Id.ToString();
                //    Session["nomeUsuarioLogado"] = v.NomeUsuario.ToString();
                //    return RedirectToAction("Index");
                //}

            }
            return View(empresa);
        }

    }
}