using IMOD.PreCredenciamentoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using IMOD.Domain.Entities;
using IMOD.Application.Service;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class ColaboradorController : Controller
    {
        // GET: Colaborador
        private readonly IMOD.Application.Interfaces.IColaboradorService _service = new IMOD.Application.Service.ColaboradorService();
        private readonly IMOD.Application.Interfaces.IDadosAuxiliaresFacade _auxiliaresService = new IMOD.Application.Service.DadosAuxiliaresFacadeService();


        public ActionResult Index()
        {
            var lstColaborador = _service.Listar(null, null, string.Empty);
            List<ColaboradorViewModel> lstColaboradorMapeado = Mapper.Map<List<ColaboradorViewModel>>(lstColaborador);

            return View(lstColaboradorMapeado);
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ColaboradorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model); 
                    _service.Criar(colaboradorMapeado);
 
                    return RedirectToAction("Index", "Colaborador");
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

            // GET: Colaborador/Edit/5
        public ActionResult Edit(int? id)
        {
            var colaboradorEditado = _service.Listar(id).FirstOrDefault(); 
            if (colaboradorEditado == null)
                return HttpNotFound();

            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);
            PopularEstadosDropDownList();
            PopularDadosDropDownList();

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
                    _service.Alterar(colaboradorMapeado);

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

        #region Métodos carregar componentes

        private void PopularEstadosDropDownList()
        {
            var lstEstado = _auxiliaresService.EstadoService.Listar();

            ViewBag.Estados = lstEstado; 
            ViewBag.UfRg = lstEstado; 
            ViewBag.UfCnh = lstEstado; 
        } 

        private void PopularMunicipiosDropDownList(String idEstado)
        {
            if (!string.IsNullOrEmpty(idEstado))
            {
                var lstMunicipio = _auxiliaresService.MunicipioService.Listar(null, null, idEstado);
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
                    }

        #endregion
    }
}
