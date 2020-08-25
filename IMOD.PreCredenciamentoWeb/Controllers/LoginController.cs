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
using System.Drawing;
using System.IO;
using System.Text;
using IMOD.PreCredenciamentoWeb.Util;
using AutoMapper;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class LoginController : Controller
    {

        IEmpresaService service = new EmpresaService();
        IEmpresaContratosService servicecontrato = new EmpresaContratoService();
        IConfiguraSistemaService configuracaoService = new ConfiguraSistemaService();


        // GET: Login
        public ActionResult Index()
        {
            var config = configuracaoService.Listar().FirstOrDefault();
            Session.Add("ConfigSistema", config);
            Session.Add("Logo", config.EmpresaLOGO);
            ViewBag.Foto = config.EmpresaLOGO;
            ViewBag.Nome = config.NomeAeroporto;
            return View(new LoginViewModel());            
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login)
        {
            // esta action trata o post (login)
            if (ModelState.IsValid) 
            {
                var empresa = new EmpresaViewModel();
                var empresaLogada = service.Listar(null, null, null, null, login.Cnpj.Replace(".", "").Replace("/", "").Replace("-", ""), login.Senha).FirstOrDefault();
                //var empresaLogada = lista.Where(e => e.Cnpj.Equals(empresa.CNPJ) && e.Senha.Equals(empresa.Senha)).FirstOrDefault();
                if (empresaLogada != null)
                {
                    empresa.EmpresaId = empresaLogada.EmpresaId;
                    empresa.Nome = empresaLogada.Nome;
                    empresa.Apelido = empresaLogada.Apelido;
                    empresa.Senha = string.Empty;
                    empresa.Contratos = servicecontrato.ListarPorEmpresa(empresaLogada.EmpresaId).ToList();
                    empresa.Cnpj = Convert.ToUInt64(empresaLogada.Cnpj).ToString(@"00\.000\.000\/0000\-00");
                    //empresa.Sigla= empresaLogada.Sigla;

                    empresa.Logo = string.Format("data:image/png;base64,{0}", empresaLogada.Logo);
                    empresa.Email1 = empresaLogada.Email1;
                    empresa.Contato1 = empresaLogada.Contato1;
                    
                    Session["EmpresaLogada"] = empresa;
                    FormsAuthentication.SetAuthCookie(empresa.Nome, empresa.Lembreme);
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "O login está incorreto!");
                    return RedirectToAction("../Login");
                }
            }

            return RedirectToAction("../Login");

        }
        public static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }
        public Image LoadImage(string img)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            byte[] bytes = Convert.FromBase64String(img);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult Alterar(LoginViewModel model)
        {
            if (model.NovaSenha == model.ConfimacaoSenha)
            {
                var empresa = service.BuscarPelaChave(SessionUsuario.EmpresaLogada.EmpresaId);
                empresa.Senha = model.NovaSenha;
                var empresaMapeado = Mapper.Map<Empresa>(empresa);
                service.Alterar(empresaMapeado);

            }
            return RedirectToAction("TrocaSenha", "Login");
        }

        public ActionResult TrocaSenha()
        {
            return View();
        }

    }
}