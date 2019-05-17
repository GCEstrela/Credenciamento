using IMOD.PreCredenciamentoWeb.Models;
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
    public class EmpresaController : Controller
    {
        // GET: Empresa
        private readonly IMOD.Application.Interfaces.IEmpresaService objService = new IMOD.Application.Service.EmpresaService();

        public ActionResult Index()
        {
            return View();
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresa/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Empresa/Edit/5
        public ActionResult Edit(int? id)
        {
            var empresaEditado = objService.Listar(null,null,null,null,null,null,id).FirstOrDefault();
            if (empresaEditado == null)
                return HttpNotFound();

            EmpresaViewModel empresaMapeado = Mapper.Map<EmpresaViewModel>(empresaEditado);

            return View(empresaMapeado);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, EmpresaViewModel model)
        {
            try
            {
                if (id == null)
                    return HttpNotFound();
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    model.EmpresaID = (int)id;
                    var empresaMapeado = Mapper.Map<Empresa>(model);
                    objService.Alterar(empresaMapeado);

                    return RedirectToAction("../Home/Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Empresa/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Empresa/Delete/5
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
    }
}
