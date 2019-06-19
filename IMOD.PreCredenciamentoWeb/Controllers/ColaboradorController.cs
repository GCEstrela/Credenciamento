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
using IMOD.Application.Interfaces;
using System.IO;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    [Authorize]
    public class ColaboradorController : Controller
    {
        // GET: Colaborador
        private readonly IColaboradorService objService = new IMOD.Application.Service.ColaboradorService();
        private readonly IDadosAuxiliaresFacade objAuxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService objContratosService = new EmpresaContratoService();
        private readonly IColaboradorEmpresaService objColaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly ICursoService objCursosService = new CursoService();
        private readonly IColaboradorCursoService objColaboradorCursosService = new ColaboradorCursosService();
        private readonly IMunicipioService objMunicipioSevice = new MunicipioService();
        private readonly IMOD.Application.Interfaces.IColaboradorAnexoService objColaboradorAnexoService = new IMOD.Application.Service.ColaboradorAnexoService();
        private const string SESS_CONTRATOS_SELECIONADOS = "ContratosSelecionados";
        private const string SESS_CONTRATOS_REMOVIDOS = "ContratosRemovidos";
        private const string SESS_CURSOS_SELECIONADOS = "CursosSelecionados";
        private const string SESS_CURSOS_REMOVIDOS = "CursosRemovidos";




        private List<Colaborador> colaboradores = new List<Colaborador>();
        private List<ColaboradorEmpresa> vinculos = new List<ColaboradorEmpresa>();

        [Authorize]
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            List<ColaboradorViewModel> lstColaboradorMapeado = Mapper.Map<List<ColaboradorViewModel>>(ObterColaboradoresEmpresaLogada().OrderBy(e => e.Nome));
            return View(lstColaboradorMapeado);
        }

        private IList<Colaborador> ObterColaboradoresEmpresaLogada()
        {
            vinculos = objColaboradorEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId).ToList();
            vinculos.ForEach(v => { colaboradores.AddRange(objService.Listar(v.ColaboradorId)); });

            return colaboradores.OrderBy(c => c.Nome).ToList();
        }

        // GET: Colaborador/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            //Remove itens da sessão
            Session.Remove(SESS_CONTRATOS_SELECIONADOS);
            Session.Remove(SESS_CONTRATOS_REMOVIDOS);
            Session.Remove(SESS_CURSOS_SELECIONADOS);
            Session.Remove(SESS_CURSOS_REMOVIDOS);
            ViewBag.Contratos = new List<EmpresaContrato>();
            ViewBag.Cursos = new List<Curso>();
            ViewBag.CursosSelecionados = new List<Curso>();

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();
            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);

            // carrega os contratosd da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Popula contratos secelionados
            ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
            var listaVinculosColaborador = Mapper.Map<List<ColaboradorEmpresaViewModel>>(objColaboradorEmpresaService.Listar(colaboradorEditado.ColaboradorId));
            ViewBag.ContratosSelecionados = listaVinculosColaborador;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosColaborador);

            //Propula combo de estado
            PopularEstadosDropDownList();

            //Preenchie combo municipio de acordo com o estado
            ViewBag.Municipio = new List<Municipio>();
            var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, colaboradorMapeado.EstadoId).OrderBy(m => m.Nome);
            if (lstMunicipio != null) { ViewBag.Municipio = lstMunicipio; };

            PopularDadosDropDownList();

            //Preenche combo com todos os cursos
            var listCursos = objCursosService.Listar().OrderBy(c => c.Descricao);
            if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; };

            //Popula cussos selecionados do colaborador
            var cursosColaborador = objColaboradorCursosService.Listar(colaboradorEditado.ColaboradorId);
            var cusosSelecionados = listCursos.Where(c => (cursosColaborador.Where(p => p.CursoId == c.CursoId).Count() > 0)).ToList();
            if (cusosSelecionados != null && cusosSelecionados.Any())
            {
                ViewBag.CursosSelecionados = cusosSelecionados;
                Session.Add(SESS_CURSOS_SELECIONADOS, cusosSelecionados);
            }

            var objColaboradorAnexo = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId);
            if (objColaboradorAnexo != null)
            {
                colaboradorMapeado.NomeArquivoAnexo = objColaboradorAnexo.FirstOrDefault().NomeArquivo;
            }

            return View(colaboradorMapeado);

        }

        // GET: Colaborador/Create
        [Authorize]
        public ActionResult Create()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            Session.Remove(SESS_CONTRATOS_SELECIONADOS);
            Session.Remove(SESS_CONTRATOS_REMOVIDOS);
            Session.Remove(SESS_CURSOS_SELECIONADOS);
            Session.Remove(SESS_CURSOS_REMOVIDOS);
            // carrega os contratosd da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
            ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();

            //carrega os cursos
            var listCursos = objCursosService.Listar().ToList();
            if (listCursos != null && listCursos.Any()){ViewBag.Cursos = listCursos;}
            ViewBag.CursosSelecionados = new List<Curso>();

            PopularEstadosDropDownList();
            ViewBag.Municipio = new List<Municipio>();
            PopularDadosDropDownList();                        
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(ColaboradorViewModel model, int[] EmpresaContratoId)
        {
            try
            {
                if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

                if (model.FileUpload != null)
                {
                    if (model.FileUpload.ContentLength > 2048000)
                        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo 2,00 MB");

                    if (!Path.GetExtension(model.FileUpload.FileName).Equals(".pdf"))
                        ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                }

                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    colaboradorMapeado.Precadastro = true;
                    objService.Criar(colaboradorMapeado);

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objColaboradorEmpresaService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objColaboradorEmpresaService.Remover(contratoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objColaboradorEmpresaService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);
                                objColaboradorEmpresaService.Criar(colaboradorEmpresa);
                                objColaboradorEmpresaService.CriarNumeroMatricula(colaboradorEmpresa);
                            }
                        }
                    }


                    // excluir os contratos removidos da lista
                    if (Session[SESS_CURSOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CURSOS_REMOVIDOS])
                        {
                            var cursoExclusao = objColaboradorCursosService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (cursoExclusao != null)
                            {
                                objColaboradorCursosService.Remover(cursoExclusao);
                            }
                        }
                    }

                    //inclui os cursos selecionados
                    if (Session[SESS_CURSOS_SELECIONADOS] != null)
                    {
                        foreach (var curso in (List<Curso>)Session[SESS_CURSOS_SELECIONADOS])
                        {
                            var colaboradorCurso = objColaboradorCursosService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                            if (colaboradorCurso == null)                        
                            {
                                colaboradorCurso = new ColaboradorCurso();
                                colaboradorCurso.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorCurso.CursoId = curso.CursoId;
                                colaboradorCurso.Descricao = curso.Descricao;
                                objColaboradorCursosService.Criar(colaboradorCurso);
                            }
                        }
                    }


                    CriarColaboradorAnexo(model, colaboradorMapeado.ColaboradorId); 

                    return RedirectToAction("Index", "Colaborador");
                }

                // Se ocorrer um erro retorna para pagina
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
                ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();

                //carrega os cursos
                var listCursos = objCursosService.Listar().ToList();
                if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; }
                ViewBag.CursosSelecionados = new List<Curso>();

                PopularEstadosDropDownList();
                ViewBag.Municipio = new List<Municipio>();
                if (model.EstadoId > 0)
                {
                    var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, model.EstadoId).OrderBy(m => m.Nome);
                    ViewBag.Municipio = lstMunicipio;
                }

                //PopularEstadosDropDownList();
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
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            
            //Remove itens da sessão
            Session.Remove(SESS_CONTRATOS_SELECIONADOS);
            Session.Remove(SESS_CONTRATOS_REMOVIDOS);
            Session.Remove(SESS_CURSOS_SELECIONADOS);
            Session.Remove(SESS_CURSOS_REMOVIDOS);
            ViewBag.Contratos = new List<EmpresaContrato>();
            ViewBag.Cursos = new List<Curso>();
            ViewBag.CursosSelecionados = new List<Curso>();

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();
            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);

            // carrega os contratosd da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Popula contratos secelionados
            ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
            var listaVinculosColaborador = Mapper.Map<List<ColaboradorEmpresaViewModel>>(objColaboradorEmpresaService.Listar(colaboradorEditado.ColaboradorId));
            ViewBag.ContratosSelecionados = listaVinculosColaborador;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosColaborador);

            //Propula combo de estado
            PopularEstadosDropDownList();

            //Preenchie combo municipio de acordo com o estado
            ViewBag.Municipio = new List<Municipio>();
            var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, colaboradorMapeado.EstadoId).OrderBy(m => m.Nome);
            if (lstMunicipio != null) { ViewBag.Municipio = lstMunicipio; };

            PopularDadosDropDownList();

            //Preenche combo com todos os cursos
            var listCursos = objCursosService.Listar().OrderBy(c=> c.Descricao);
            if (listCursos != null && listCursos.Any()){ViewBag.Cursos = listCursos;};

            //Popula cussos selecionados do colaborador
            var cursosColaborador = objColaboradorCursosService.Listar(colaboradorEditado.ColaboradorId);
            var cusosSelecionados = listCursos.Where(c => (cursosColaborador.Where(p => p.CursoId == c.CursoId).Count() > 0)).ToList();
            if (cusosSelecionados != null && cusosSelecionados.Any())
            {
                ViewBag.CursosSelecionados = cusosSelecionados;
                Session.Add(SESS_CURSOS_SELECIONADOS, cusosSelecionados);
            }

            var objColaboradorAnexo = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId);
            if (objColaboradorAnexo != null)
            {
                colaboradorMapeado.NomeArquivoAnexo = objColaboradorAnexo.FirstOrDefault().NomeArquivo;
            }

            return View(colaboradorMapeado); 
        }

        // POST: Colaborador/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? id, ColaboradorViewModel model)
        {
            try
            {               
                if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
                if (id == null)
                    return HttpNotFound(); 

                if (model.FileUpload != null)
                {
                    if (model.FileUpload.ContentLength > 2048000)
                        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo 2,00 MB");

                    if (!Path.GetExtension(model.FileUpload.FileName).Equals(".pdf"))
                        ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                }

                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    colaboradorMapeado.Precadastro = true;
                    objService.Alterar(colaboradorMapeado);

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objColaboradorEmpresaService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objColaboradorEmpresaService.Remover(contratoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objColaboradorEmpresaService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);
                                objColaboradorEmpresaService.Criar(colaboradorEmpresa);
                                objColaboradorEmpresaService.CriarNumeroMatricula(colaboradorEmpresa);
                            }
                        }
                    }

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CURSOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CURSOS_REMOVIDOS])
                        {
                            var cursoExclusao = objColaboradorCursosService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (cursoExclusao != null)
                            {
                                objColaboradorCursosService.Remover(cursoExclusao);
                            }
                        }
                    }

                    //inclui os cursos selecionados
                    if (Session[SESS_CURSOS_SELECIONADOS] != null)
                    {
                        foreach (var curso in (List<Curso>)Session[SESS_CURSOS_SELECIONADOS])
                        {
                            var colaboradorCurso = objColaboradorCursosService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                            if (colaboradorCurso != null)
                            {
                                objColaboradorCursosService.Alterar(colaboradorCurso);
                            }
                            else
                            {
                                colaboradorCurso = new ColaboradorCurso();
                                colaboradorCurso.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorCurso.CursoId = curso.CursoId;
                                colaboradorCurso.Descricao = curso.Descricao;
                                objColaboradorCursosService.Criar(colaboradorCurso);
                            }
                        }
                    }

                    if (model.FileUpload != null)
                    {
                        ExcluirColaboradorAnexoAnterior(model); 
                        CriarColaboradorAnexo(model, colaboradorMapeado.ColaboradorId); 
                    }

                    return RedirectToAction("Index"); 
                }

                throw new Exception("Campos obrigatórios não forma preenchidos");
            }
            catch (Exception ex)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Colaborador/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            // TODO: Add delete logic here
            Colaborador colaborador = new Colaborador();
            colaborador.ColaboradorId = id;
            //var colaboradorMapeado = Mapper.Map<Colaborador>(colaborador);
            objService.Remover(colaborador);
            return View();
        }

        // POST: Colaborador/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                // TODO: Add delete logic here
                var colaboradorMapeado = Mapper.Map<Colaborador>(collection);
                objService.Alterar(colaboradorMapeado);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public JsonResult AdicionarContrato(int id, Boolean bagagem, Boolean operadorPonteEmbarque, Boolean flagCcam, string validade, string cargo)
        {
            List<ColaboradorEmpresaViewModel> vinculoList = new List<ColaboradorEmpresaViewModel>();
            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                vinculoList = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];

            var item = SessionUsuario.EmpresaLogada.Contratos.Where(c => c.EmpresaContratoId == id).FirstOrDefault();
            ColaboradorEmpresaViewModel vinculo = new ColaboradorEmpresaViewModel();
            vinculo.EmpresaContratoId = id;
            vinculo.Descricao = item.Descricao;
            vinculo.Cargo = cargo;
            if (!string.IsNullOrEmpty(validade))
                vinculo.Validade = DateTime.Parse(validade);
            vinculo.ManuseioBagagem = bagagem; 
            vinculo.Matricula = " - ";
            vinculo.OperadorPonteEmbarque = operadorPonteEmbarque;  
            vinculo.FlagCcam = flagCcam;  
            vinculoList.Add(vinculo);
            Session.Add(SESS_CONTRATOS_SELECIONADOS, vinculoList);

            return Json(vinculoList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverContrato(int id)
        {
            var listContrato = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            int indice = listContrato.FindIndex(c => c.EmpresaContratoId.Equals(id));
            listContrato.RemoveAt(indice);
            Session[SESS_CONTRATOS_SELECIONADOS] = listContrato;
            if (Session[SESS_CONTRATOS_REMOVIDOS] == null) Session[SESS_CONTRATOS_REMOVIDOS] = new List<int>();
            ((List<int>)Session[SESS_CONTRATOS_REMOVIDOS]).Add(id);
            return Json(listContrato, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult AdicionarCurso(int id)
        {
            if (Session[SESS_CURSOS_SELECIONADOS] == null) Session[SESS_CURSOS_SELECIONADOS] = new List<Curso>();
            var item = objCursosService.Listar(id).FirstOrDefault();
            ((List<Curso>)Session[SESS_CURSOS_SELECIONADOS]).Add(item);             
            return Json((List<Curso>)Session[SESS_CURSOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverCurso(int id)
        {
            var listContrato = (List<Curso>)Session[SESS_CURSOS_SELECIONADOS];
            listContrato.Remove(new Curso(id));
            Session[SESS_CURSOS_SELECIONADOS] = listContrato;
            if (Session[SESS_CURSOS_REMOVIDOS] == null) Session[SESS_CURSOS_REMOVIDOS] = new List<int>();
            ((List<int>)Session[SESS_CURSOS_REMOVIDOS]).Add(id);
            return Json(listContrato, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult BuscarMunicipios(int id)
        {
            var listMunicipio = objMunicipioSevice.Listar(null, null, id);
        
            return Json(listMunicipio, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public JsonResult ConsultarCPF(string cpf)
        {
            var colaborador = objService.ObterPorCpf(cpf);
            return Json(colaborador, JsonRequestBehavior.AllowGet);
        }


        #region Métodos Internos

        #region Popular e Carregar Componentes

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


        #region Colaborador Anexo

            private void CriarColaboradorAnexo(ColaboradorViewModel colaborador, int colaboradorId = 0)
            {
                byte[] bufferArquivo;
                string NomeArquivo;
                string ExtensaoArquivo;

                if (!colaborador.Precadastro) return; 

                if (colaborador.FileUpload == null) return;
                if (colaborador == null || colaboradorId == 0) return;
                if (colaborador.FileUpload.ContentLength <= 0 || colaborador.FileUpload.ContentLength > 2048000) return;

                NomeArquivo = Path.GetFileNameWithoutExtension(colaborador.FileUpload.FileName);
                ExtensaoArquivo = Path.GetExtension(colaborador.FileUpload.FileName);

                if (!ExtensaoArquivo.Equals(".pdf")) return; 

                var arquivoStream = colaborador.FileUpload.InputStream; 
                using (MemoryStream ms = new MemoryStream())
                {
                    arquivoStream.CopyTo(ms);
                    bufferArquivo = ms.ToArray();
                }

                var arquivoBase64 = Convert.ToBase64String(bufferArquivo);

                ColaboradorAnexo colaboradorAnexo = new ColaboradorAnexo();
                colaboradorAnexo.ColaboradorId = colaboradorId;
                colaboradorAnexo.Arquivo = arquivoBase64;
                colaboradorAnexo.NomeArquivo = NomeArquivo + ExtensaoArquivo;
                colaboradorAnexo.Descricao = NomeArquivo + ExtensaoArquivo;
                objColaboradorAnexoService.Criar(colaboradorAnexo); 

            }

            private void ExcluirColaboradorAnexoAnterior(ColaboradorViewModel colaborador)
            {
                if (!colaborador.Precadastro) return;
                if (colaborador.FileUpload == null) return;
                if (colaborador == null || colaborador.ColaboradorId == 0) return; 
                if (colaborador.FileUpload.ContentLength <= 0 || colaborador.FileUpload.ContentLength > 2048000) return;

                var objColaboradoAnexo = objColaboradorAnexoService.ListarComAnexo(colaborador.ColaboradorId).FirstOrDefault();
                if (objColaboradoAnexo == null) return;

                objColaboradorAnexoService.Remover(objColaboradoAnexo);
            }

            public FileResult Download(string id)
            {
                string contentType = "";
                string NomeArquivoAnexo = "";
                string extensao = "";
                string nomeArquivoV = ""; 
                string pastaTemp = Path.GetTempPath();

                //if (string.IsNullOrEmpty(id));

                int colaboradorId = Convert.ToInt32(id);

                var objColaboradorAnexo = objColaboradorAnexoService.ListarComAnexo(colaboradorId).FirstOrDefault();
                NomeArquivoAnexo = objColaboradorAnexo.NomeArquivo; 
                extensao = Path.GetExtension(NomeArquivoAnexo); 
                nomeArquivoV = Path.GetFileNameWithoutExtension(NomeArquivoAnexo); 

                var arrayArquivo = Convert.FromBase64String(objColaboradorAnexo.Arquivo); 
                System.IO.File.WriteAllBytes(pastaTemp + NomeArquivoAnexo, arrayArquivo); 

                if (extensao.Equals(".pdf"))
                    contentType = "application/pdf"; 

                return File(pastaTemp + NomeArquivoAnexo, contentType, nomeArquivoV + extensao);
            }

        #endregion

        #endregion
    }
}