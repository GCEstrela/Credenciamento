using IMOD.ValidacaoCredencialWeb.Models;
using System;
using System.Web.Mvc;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using System.Net;
using System.Collections.Generic;
using IMOD.Domain.Entities;
using System.Linq;
using IMOD.CredenciamentoDeskTop.Views.Model;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class ColaboradorController : Controller
    {
        // GET: Colaborador
        private readonly IColaboradorCredencialService objColaboradorCredencialService = new ColaboradorCredencialService();
        private readonly IColaboradorCursoService objColaboradorCursoService = new ColaboradorCursosService();
        private readonly IConfiguraSistemaService objConfiguraSistema = new ConfiguraSistemaService();

        private List<ColaboradorCurso> cursos = new List<ColaboradorCurso>();

        // GET: Veiculo/Credential/5
        public ActionResult Credential(string id)
        {

            if (id == null || (string.IsNullOrEmpty(id))) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var identificador = Helper.CriptografiaHelper.Decriptar(id);

            var credencialView = objColaboradorCredencialService.ObterCredencialView(Convert.ToInt16(identificador));
            cursos = objColaboradorCursoService.Listar(Convert.ToInt16(identificador), null, null, null, null).ToList();

            var colaboradorCursos = "";


            foreach (var item in cursos)
            {
                if (item.Cracha)
                {
                    colaboradorCursos += item.Descricao + ", ";
                }

            }

            ViewBag.Cursos = colaboradorCursos.Substring(0, colaboradorCursos.Length - 2);

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
