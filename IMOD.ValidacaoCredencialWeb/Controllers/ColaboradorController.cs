using IMOD.ValidacaoCredencialWeb.Models;
using System;
using System.Web.Mvc;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class ColaboradorController : Controller
    {
        // GET: Colaborador
        private readonly IColaboradorCredencialService objColaboradorCredencialService = new ColaboradorCredencialService();
        private readonly IConfiguraSistemaService objConfiguraSistema = new ConfiguraSistemaService();


        // GET: Veiculo/Credential/5
        public ActionResult Credential(string id, string param)
        {

            if (id == null || (string.IsNullOrEmpty(id))) return HttpNotFound();
            if (param == null || (string.IsNullOrEmpty(param))) return HttpNotFound();

            var paramDescodificado = Helper.CriptografiaHelper.Decodificar(param);
            var identificador = Helper.CriptografiaHelper.Decodificar(id);

            var credencialView = objColaboradorCredencialService.ObterCredencialView(Convert.ToInt16(identificador));

            if (credencialView != null)
            {
                CredencialViewModel objCredencialMapeado = Mapper.Map<CredencialViewModel>(credencialView);

                if (objCredencialMapeado.Ativa)
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

                ViewBag.ocultaCcam = !objCredencialMapeado.FlagCcam ? "hidden='Hidden'" : "";
                ViewBag.ocultaBagagem = !objCredencialMapeado.ManuseioBagagem ? "hidden='Hidden'" : "";
                ViewBag.ocultaOperaEmbarque = !objCredencialMapeado.OperadorPonteEmbarque ? "hidden='Hidden'" : "";

                ViewBag.ImgOperador = GetImagemOperadorAereo();

                return View(objCredencialMapeado);
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
