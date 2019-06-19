using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.ValidacaoCredencialWeb.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class VeiculoController : Controller
    {
        
        private readonly IVeiculoCredencialService objVeiculoCredencialService = new VeiculoCredencialService(); 
        private readonly IConfiguraSistemaService objConfiguraSistema = new ConfiguraSistemaService(); 

        // GET: Veiculo/Credential/5
        public ActionResult Credential(string id)
        {
            if (id == null || (string.IsNullOrEmpty(id))) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var identificador = Helper.CriptografiaHelper.Decriptar(id);

            var credencialView = objVeiculoCredencialService.ObterCredencialView(Convert.ToInt16(identificador));

            if (credencialView != null)
            {
                AutorizacaoViewModel objAutorizacaoMapeado = Mapper.Map<AutorizacaoViewModel>(credencialView);

                if (objAutorizacaoMapeado.Ativa)
                {
                    ViewBag.ClasseAlerta = "alert alert-success";
                    ViewBag.ClasseIcone = "glyphicon glyphicon-ok";
                    ViewBag.ClasseTexto = "ATIVA";
                }
                else
                {
                    ViewBag.ClasseAlerta = "alert alert-danger";
                    ViewBag.ClasseIcone = "glyphicon glyphicon-remove";
                    ViewBag.ClasseTexto = "INATIVA";
                }

                ViewBag.ImgOperador = GetImagemOperadorAereo();

                return View(objAutorizacaoMapeado); 
            }

            return View(); 
        }

        #region Método(s) Interno(s)

        private string GetImagemOperadorAereo()
        {
            var configSistema = objConfiguraSistema.BuscarPelaChave(1);
            if (configSistema == null || configSistema.EmpresaLOGO == null || String.IsNullOrEmpty(configSistema.EmpresaLOGO)) return string.Empty;

            return string.Format("data:image/png;base64,{0}", configSistema.EmpresaLOGO);
        }

        #endregion

    }
}
