﻿using AutoMapper;
using IMOD.Domain.Entities;
using IMOD.PreCredenciamentoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class VeiculoController : Controller
    {
        // GET: Colaborador
        private readonly IMOD.Application.Interfaces.IVeiculoService objService = new IMOD.Application.Service.VeiculoService();
        private readonly IMOD.Application.Interfaces.IDadosAuxiliaresFacade objAuxiliaresService = new IMOD.Application.Service.DadosAuxiliaresFacadeService();
        private readonly IMOD.Application.Interfaces.IEmpresaContratosService objContratosService = new IMOD.Application.Service.EmpresaContratoService();
        private readonly IMOD.Application.Interfaces.IVeiculoEmpresaService objVeiculoEmpresaService = new IMOD.Application.Service.VeiculoEmpresaService();

        // GET: Veiculo
        public ActionResult Index()
        {
            var lstVeiculo = objService.Listar(null, null, string.Empty);
            List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(lstVeiculo);

            return View(lstVeiculoMapeado); 
        }

        // GET: Veiculo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Veiculo/Create
        public ActionResult Create()
        {

            PopularEstadosDropDownList();
            PopularDadosDropDownList();

            PopularContratoCreateDropDownList(6476);

            return View();
        }

        // POST: Veiculo/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(VeiculoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var veiculoMapeado = Mapper.Map<Veiculo>(model);
                    objService.Criar(veiculoMapeado);

                    return RedirectToAction("Index", "Veiculo");
                }

                // Se ocorrer um erro retorna para pagina 
                PopularEstadosDropDownList();
                PopularDadosDropDownList();


                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar registro");
                return View();
            }
        }

        // GET: Veiculo/Edit/5
        public ActionResult Edit(int ? id)
        {
            var veiculoEditado = objService.Listar(null,null,null,null,null,id).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado); 

            PopularEstadosDropDownList(); 
            PopularDadosDropDownList(); 
            PopularContratoEditDropDownList(veiculoMapeado, 6476);

            return View(veiculoMapeado); 
        }

        // POST: Veiculo/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, VeiculoViewModel model)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                var idVeiculo = id;
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    //model.EquipamentoVeiculoId = id;

                    var veiculoMapeado = Mapper.Map<Veiculo>(model);
                    veiculoMapeado.EquipamentoVeiculoId = Convert.ToInt32(idVeiculo);
                    objService.Alterar(veiculoMapeado);

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Veiculo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Veiculo/Delete/5
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

        #region Métodos Internos Carregamento de Componentes

        private void PopularEstadosDropDownList()
        {
            var lstEstado = objAuxiliaresService.EstadoService.Listar();

            ViewBag.Estados = lstEstado;

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
            ViewBag.TipoVeiculo = new SelectList(new object[]
                {
                    new {Name = "VEICULO", Value = "1"},
                    new {Name = "EQUIPAMENTO", Value = "2"} 
                }, "Value", "Name"); 

            var lstTipoServico = objAuxiliaresService.TipoServico.Listar();
            ViewBag.TipoServicos = lstTipoServico;

            var lstTipoCombustivel = objAuxiliaresService.TipoCombustivelService.Listar();
            ViewBag.TipoCombustivel = lstTipoCombustivel;

            var lstTipoEquipamento = objAuxiliaresService.TipoEquipamentoService.Listar();
            ViewBag.TipoEquipamento = lstTipoEquipamento;
        }

        private void PopularContratoCreateDropDownList(int idEmpresa)
        {
            if (idEmpresa <= 0) return;

            var contratoEmpresa = objContratosService.Listar(idEmpresa);
            ViewBag.ContratoEmpresa = contratoEmpresa;
        }

        private void PopularContratoEditDropDownList(VeiculoViewModel veiculo, int idEmpresa)
        {
            if (idEmpresa <= 0) return; 

            var contratoEmpresa = objContratosService.Listar(idEmpresa); 
            var resultEmpresasContratosVinculados = objVeiculoEmpresaService.Listar(null, null, null, null, null, idEmpresa);

            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao", resultEmpresasContratosVinculados.Select(m => m.EmpresaContratoId).ToArray());
        }

        #endregion
    }
}
