﻿using IMOD.PreCredenciamentoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using IMOD.Domain.Entities;
using IMOD.Application.Service;
using IMOD.PreCredenciamentoWeb.Util;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class ColaboradorController : Controller
    {
        // GET: Colaborador
        private readonly IMOD.Application.Interfaces.IColaboradorService objService = new IMOD.Application.Service.ColaboradorService();
        private readonly IMOD.Application.Interfaces.IDadosAuxiliaresFacade objAuxiliaresService = new IMOD.Application.Service.DadosAuxiliaresFacadeService();
        private readonly IMOD.Application.Interfaces.IEmpresaContratosService objContratosService = new IMOD.Application.Service.EmpresaContratoService();
        private readonly IMOD.Application.Interfaces.IColaboradorEmpresaService objColaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly IMOD.Application.Interfaces.IColaboradorCredencialService objColaboradorCredencialService = new IMOD.Application.Service.ColaboradorCredencialService();
        private readonly IMOD.Application.Interfaces.ICursoService objCursosService = new IMOD.Application.Service.CursoService();

        private List<Colaborador> colaboradores = new List<Colaborador>();
        private List<ColaboradorEmpresa> vinculos = new List<ColaboradorEmpresa>();

        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            List<ColaboradorViewModel> lstColaboradorMapeado = Mapper.Map<List<ColaboradorViewModel>>(ObterColaboradoresEmpresaLogada());
            return View(lstColaboradorMapeado);
        }

        private IList<Colaborador> ObterColaboradoresEmpresaLogada()
        {
            vinculos = objColaboradorEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.Codigo).ToList();
            vinculos.ForEach(v => { colaboradores.AddRange(objService.Listar(v.ColaboradorId)); });

            return colaboradores.OrderBy(c => c.Nome).ToList();
        }

        // GET: Colaborador/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Colaborador/Create
        public ActionResult Create()
        {
            PopularEstadosDropDownList();
            PopularDadosDropDownList();
            PopularContratoCreateDropDownList(SessionUsuario.EmpresaLogada.Codigo);
            PopularCursos(SessionUsuario.EmpresaLogada.Codigo);
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ColaboradorViewModel model, int[] EmpresaContratoId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    objService.Criar(colaboradorMapeado);

                    foreach (int contratoEmpresaId in EmpresaContratoId)
                    {
                        // Inclusão do vinculo
                        ColaboradorEmpresa colaboradorEmpresa = new ColaboradorEmpresa();
                        colaboradorEmpresa.ColaboradorId = colaboradorMapeado.ColaboradorId;
                        colaboradorEmpresa.EmpresaContratoId = Convert.ToInt32(contratoEmpresaId);
                        colaboradorEmpresa.Ativo = true;
                        colaboradorEmpresa.EmpresaId = SessionUsuario.EmpresaLogada.Codigo;
                        objColaboradorEmpresaService.Criar(colaboradorEmpresa);
                        objColaboradorEmpresaService.CriarNumeroMatricula(colaboradorEmpresa);
                    }
                    return RedirectToAction("Index", "Colaborador");
                }

                // Se ocorrer um erro retorna para pagina
                PopularEstadosDropDownList();
                PopularDadosDropDownList();
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Erro ao salvar registro");
                return View();
            }

        }

        // GET: Colaborador/Edit/5
        public ActionResult Edit(int? id)
        {
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();
            if (colaboradorEditado == null)
                return HttpNotFound();

            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);
            PopularEstadosDropDownList();
            PopularDadosDropDownList();
            ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            ViewBag.ContratosSelecionados = SessionUsuario.EmpresaLogada.Contratos;
            return View(colaboradorMapeado);
        }

        // POST: Colaborador/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, ColaboradorViewModel model)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();

                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    objService.Alterar(colaboradorMapeado);

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Colaborador/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Colaborador/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AdicionarContrato(int id)
        {
            ViewBag.ContratosSelecionados = new List<EmpresaContrato>();
            if (TempData["ContratosSelecionados"] != null)
            {
                ViewBag.ContratosSelecionados = TempData["ContratosSelecionados"];
            }
            var item = SessionUsuario.EmpresaLogada.Contratos.Where(c => c.EmpresaContratoId == id).FirstOrDefault();
            ((List<EmpresaContrato>)ViewBag.ContratosSelecionados).Add(item);
            TempData["ContratosSelecionados"] = ViewBag.ContratosSelecionados;
            return View();
        }

        // GET: Veiculo/Credential/5
        public ActionResult Credential(int id)
        {
            var credencialView = objColaboradorCredencialService.ObterCredencialView(id);

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

                return View(objCredencialMapeado);
            }

            return View();


        }

        #region Métodos internos carregar componentes

        private void PopularEstadosDropDownList()
        {
            var lstEstado = objAuxiliaresService.EstadoService.Listar();

            ViewBag.Estados = lstEstado;
            ViewBag.UfRg = lstEstado;
            ViewBag.UfCnh = lstEstado;
        }

        private void PopularMunicipiosDropDownList(String idEstado)
        {
            if (!string.IsNullOrEmpty(idEstado))
            {
                var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, idEstado);
                ViewBag.Municipio = lstMunicipio;
            }
        }

        private void PopularDadosDropDownList()
        {

            ViewBag.CategoriaCnh = new SelectList(new object[]
                {
                    new {Name = "A", Value = "A"},
                    new {Name = "B", Value = "B"},
                    new {Name = "AB", Value = "AB"},
                    new {Name = "C", Value = "C"},
                    new {Name = "D", Value = "D"},
                    new {Name = "E", Value = "E"}
                }, "Value", "Name");

            ViewBag.OrgaoEmissorRG = new SelectList(new object[]
                {
                                new {Name = "SSP", Value = "SSP"},
                                new {Name = "SJS", Value = "SJS"},
                                new {Name = "SESP", Value = "SESP"},
                                new {Name = "SJTC", Value = "SJTC"},
                                new {Name = "CGPI", Value = "CGPI"},
                                new {Name = "COMAE", Value = "COMAE"},
                                new {Name = "COMAE", Value = "COMAE"},
                                new {Name = "DPF", Value = "DPF"},
                                new {Name = "EST", Value = "EST"},
                                new {Name = "OAB", Value = "OAB"},
                                new {Name = "CRM", Value = "CRM"}
                                }, "Value", "Name");

            var contrato = objColaboradorEmpresaService.Listar(null, null, null, 12);

        }


        private void PopularContratoCreateDropDownList(int idEmpresa)
        {
            if (idEmpresa <= 0) return;

            var contratoEmpresa = objContratosService.Listar(idEmpresa);
            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao");
        }

        private void PopularCursos(int idEmpresa)
        {
            if (idEmpresa <= 0) return;

            var cursos = objCursosService.Listar();
            ViewBag.Cursos = new MultiSelectList(cursos, "CursoId", "Descricao");
        }

        #endregion
    }
}
