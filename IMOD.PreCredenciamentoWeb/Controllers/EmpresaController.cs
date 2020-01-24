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
using System.IO;
using IMOD.Application.Interfaces;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Empresa
        private readonly IMOD.Application.Interfaces.IEmpresaService objService = new IMOD.Application.Service.EmpresaService();
        private readonly IDadosAuxiliaresFacade objAuxiliaresService = new DadosAuxiliaresFacadeService();

        public string SESS_FOTO_COLABORADOR { get; private set; }

        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            List<EmpresaViewModel> lstEmpresaMapeado = Mapper.Map<List<EmpresaViewModel>>(ObterEmpresas().OrderBy(e => e.Nome));            
            return View(lstEmpresaMapeado);            
        }

        private List<Empresa> ObterEmpresas()
        {
                return objService.Listar(null, null, null, null, null, null).ToList();            
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            // TODO: Add insert logic here
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            PopularEstadosDropDownList();
            ViewBag.Municipio = new List<Municipio>();
            return View();
        }

        // POST: Empresa/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
           

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
                if (!ModelState.IsValid) return View();

                if (ModelState.IsValid)
                {
                    model.EmpresaId = (int)id;
                    
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


        #region Métodos Internos
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
                var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, idEstado).OrderBy(m => m.Nome);
                ViewBag.Municipio = lstMunicipio;
            }
        }


        [Authorize]
        public void OnSelecionaFoto_Click(HttpPostedFileBase file, ColaboradorViewModel model)
        {
            string pic = System.IO.Path.GetFileName(file.FileName);

            // save the image path path to the database or you can send image 
            // directly to database
            // in-case if you want to store byte[] ie. for DB
            using (MemoryStream ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();

                model.Foto = Convert.ToBase64String(array);
            }
        }

        [Authorize]
        public void CarregaFotoColaborador(ColaboradorViewModel model)
        {
            if (model.Foto != null)
            {
                var bytes = Convert.FromBase64String(model.Foto);
                string base64 = Convert.ToBase64String(bytes);

                Session.Add(SESS_FOTO_COLABORADOR, base64);
                ViewBag.FotoColaborador = String.Format("data:image/gif;base64,{0}", base64);
            }
        }


        [Authorize]
        public void BuscarFoto(int colaborador, EmpresaViewModel model)
        {
            try
            {
                if (model.Logo != null) return;
                var listaFoto = objService.BuscarPelaChave(colaborador);

                if (listaFoto != null)
                    model.Logo = listaFoto.Logo;
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

    }
}
