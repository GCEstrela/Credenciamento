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
        private readonly IMOD.Application.Interfaces.IEmpresaService empresaService = new IMOD.Application.Service.EmpresaService();
        private readonly IDadosAuxiliaresFacade auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IMunicipioService municipioSevice = new MunicipioService();
        private readonly IMOD.Application.Interfaces.ITipoAtividadeService tipoAtividadeService = new IMOD.Application.Service.TipoAtividadeService();
        private readonly IMOD.Application.Interfaces.ILayoutCrachaService layoutCrachaService = new IMOD.Application.Service.LayoutCrachaService();
        private readonly IMOD.Application.Interfaces.IEmpresaLayoutCrachaService empresaLayoutCrachaService = new IMOD.Application.Service.EmpresaLayoutCrachaService();
        private readonly IMOD.Application.Interfaces.IEmpresaTipoAtividadeService empresaTipoAtividadeService = new IMOD.Application.Service.EmpresaTipoAtividadeService();
        private const string SESS_ATIVIDADE_SELECIONADOS = "AtividadesSelecionadas";
        private const string SESS_CRACHA_SELECIONADOS = "CrachasSelecionados";

        public string SESS_FOTO_COLABORADOR { get; private set; }

        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            List<EmpresaViewModel> lstEmpresaMapeado = Mapper.Map<List<EmpresaViewModel>>(ObterEmpresas().OrderBy(e => e.Nome));
            return View(lstEmpresaMapeado);
        }

        private List<Empresa> ObterEmpresas()
        {
            return empresaService.Listar(null, null, null, null, null, null).ToList();
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
            Session.Remove(SESS_ATIVIDADE_SELECIONADOS);            
            Session.Remove(SESS_CRACHA_SELECIONADOS);            
            Session.Remove(SESS_FOTO_COLABORADOR);

            PopularEstadosDropDownList();
            PopularAtividades();
            PopularCrachas();
            ViewBag.Municipio = new List<Municipio>();
            return View();
        }

        // POST: Empresa/Create
        [HttpPost]
        public ActionResult Create(EmpresaViewModel model)
        {
            try
            {
                Empresa empresa = Mapper.Map<Empresa>(model);
                empresaService.Criar(empresa);
                CriarAtividadesSelecionadas(empresa.EmpresaId);
                CriarCrachasSelecionados(empresa.EmpresaId);
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
            var empresaEditado = empresaService.Listar(null, null, null, null, null, null, id).FirstOrDefault();
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
                    empresaService.Alterar(empresaMapeado);

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
            var lstEstado = auxiliaresService.EstadoService.Listar();

            ViewBag.Estados = lstEstado;
            ViewBag.UfRg = lstEstado;
            ViewBag.UfCnh = lstEstado;
        }

        private void PopularMunicipiosDropDownList(String idEstado)
        {
            if (!string.IsNullOrEmpty(idEstado))
            {
                var lstMunicipio = auxiliaresService.MunicipioService.Listar(null, null, idEstado).OrderBy(m => m.Nome);
                ViewBag.Municipio = lstMunicipio;
            }
        }

        [Authorize]
        public JsonResult BuscarMunicipios(int id)
        {
            var listMunicipio = municipioSevice.Listar(null, null, id);
            return Json(listMunicipio, JsonRequestBehavior.AllowGet);
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
                var listaFoto = empresaService.BuscarPelaChave(colaborador);

                if (listaFoto != null)
                    model.Logo = listaFoto.Logo;
            }
            catch (Exception ex)
            {
            }
        }

        [Authorize]
        private void PopularAtividades()
        {
            ViewBag.Atividades = tipoAtividadeService.Listar();
            ViewBag.AtividadesSelecionadas = new List<TipoAtividade>();
        }

        [Authorize]
        private void PopularCrachas()
        {
            ViewBag.Crachas = layoutCrachaService.Listar();
            ViewBag.CrachasSelecionadas = new List<LayoutCracha>();
        }

        [Authorize]
        public JsonResult AdicionarAtividade(int id)
        {
            if (Session[SESS_ATIVIDADE_SELECIONADOS] == null) Session[SESS_ATIVIDADE_SELECIONADOS] = new List<TipoAtividade>();
            var item = tipoAtividadeService.BuscarPelaChave(id);
            ((List<TipoAtividade>)Session[SESS_ATIVIDADE_SELECIONADOS]).Add(item);
            return Json((List<TipoAtividade>)Session[SESS_ATIVIDADE_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverAtividade(int id)
        {
            var listContrato = (List<TipoAtividade>)Session[SESS_ATIVIDADE_SELECIONADOS];
            TipoAtividade atividade = new TipoAtividade();
            atividade.TipoAtividadeId = id;
            listContrato.Remove(atividade);
            Session[SESS_ATIVIDADE_SELECIONADOS] = listContrato;
            return Json(listContrato, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AdicionarCracha(int id)
        {
            if (Session[SESS_CRACHA_SELECIONADOS] == null) Session[SESS_CRACHA_SELECIONADOS] = new List<LayoutCracha>();
            var item = layoutCrachaService.BuscarPelaChave(id);
            ((List<LayoutCracha>)Session[SESS_CRACHA_SELECIONADOS]).Add(item);
            return Json((List<LayoutCracha>)Session[SESS_CRACHA_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverCracha(int id)
        {
            var listContrato = (List<LayoutCracha>)Session[SESS_CRACHA_SELECIONADOS];
            LayoutCracha layout = new LayoutCracha();
            layout.LayoutCrachaId = id;
            listContrato.Remove(layout);
            Session[SESS_CRACHA_SELECIONADOS] = listContrato;
            return Json(listContrato, JsonRequestBehavior.AllowGet);
        }


        private void CriarAtividadesSelecionadas(int idEmpresa)
        {
            try
            {                
                ((List<TipoAtividade>)Session[SESS_ATIVIDADE_SELECIONADOS]).ForEach(a =>
                {
                    var atividade = new EmpresaTipoAtividade();
                    atividade.TipoAtividadeId = a.TipoAtividadeId;
                    atividade.Descricao = a.Descricao;
                    atividade.EmpresaId = idEmpresa;
                    empresaTipoAtividadeService.Criar(atividade);
                });
            }
            catch { }

        }


        private void CriarCrachasSelecionados(int idEmpresa)
        {
            try
            {
                ((List<LayoutCracha>)Session[SESS_CRACHA_SELECIONADOS]).ForEach(a =>
                {
                    var cracha = new EmpresaLayoutCracha();
                    cracha.LayoutCrachaId = a.LayoutCrachaId;
                    cracha.Modelo = a.Modelo;
                    cracha.Nome = a.Nome;
                    cracha.EmpresaId = idEmpresa;
                    empresaLayoutCrachaService.Criar(cracha);
                });

            }
            catch { }
        }



        #endregion

    }
}
