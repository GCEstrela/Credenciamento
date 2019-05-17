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

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class LoginController : Controller
    {

        IEmpresaService service = new EmpresaService();
        IEmpresaContratosService servicecontrato = new EmpresaContratoService();

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
                var empresaLogada = service.Listar(null,null,null,null, empresa.CNPJ, empresa.Senha).FirstOrDefault();
                //var empresaLogada = lista.Where(e => e.Cnpj.Equals(empresa.CNPJ) && e.Senha.Equals(empresa.Senha)).FirstOrDefault();
                if (empresaLogada != null)
                {
                    empresa.EmpresaID = empresaLogada.EmpresaId;
                    empresa.Nome = empresaLogada.Nome;
                    empresa.Apelido = empresaLogada.Apelido;
                    empresa.Senha = string.Empty;
                    empresa.Contratos = servicecontrato.ListarPorEmpresa(empresaLogada.EmpresaId).ToList();
                    empresa.CNPJ = Convert.ToUInt64(empresaLogada.Cnpj).ToString(@"00\.000\.000\/0000\-00");
                    //empresa.CNPJ = empresaLogada.Cnpj;
                    empresa.Logo = string.Format("data:image/png;base64,{0}", empresaLogada.Logo);
                    empresa.Email1= empresaLogada.Email1;
                    empresa.Contato1 = empresaLogada.Contato1;

                    Session["EmpresaLogada"] = empresa;
                    FormsAuthentication.SetAuthCookie(empresa.Nome, empresa.Lembreme);
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "O login está incorreto!");                    
                    return RedirectToAction("~/Login");
                }

            }
            return View(empresa);
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
            return RedirectToAction("Index","Login");
        }

    }
}