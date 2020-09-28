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
using Correios.Net;
using System.Windows.Forms;
using System.Net.Http;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    [Authorize]
    public class ColaboradorController : Controller
    {
        #region Propriedades
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
        private const string SESS_ANEXOS_SELECIONADOS = "AnexosSelecionados";
        private const string SESS_ANEXOS_REMOVIDOS = "AnexosRemovidos";
        private List<Colaborador> colaboradores = new List<Colaborador>();
        private List<ColaboradorEmpresa> vinculos = new List<ColaboradorEmpresa>();
        private const string SESS_FOTO_COLABORADOR = "Foto";
        #endregion

        [Authorize]
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            List<ColaboradorViewModel> lstColaboradorMapeado = Mapper.Map<List<ColaboradorViewModel>>(ObterColaboradoresEmpresaLogada().OrderBy(e => e.Nome));

            Session[SESS_FOTO_COLABORADOR] = null;
            return View(lstColaboradorMapeado);
        }

        private IList<Colaborador> ObterColaboradoresEmpresaLogada()
        {
            var l1 = objService.Listar(null, null, null, null);
            var l2 = objColaboradorEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId, null);
            var l3 = l2.Select(c => c.ColaboradorId).ToList<int>();
            colaboradores = l1.Where(c => l3.Contains(c.ColaboradorId)).ToList();

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
            ViewBag.Anexos = new List<ColaboradorAnexo>();
            ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();
            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);

            CarregaFotoColaborador(colaboradorMapeado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Popula contratos selecionados
            ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
            var listaVinculosColaborador = Mapper.Map<List<ColaboradorEmpresaViewModel>>(objColaboradorEmpresaService.Listar(colaboradorEditado.ColaboradorId));
            ViewBag.ContratosSelecionados = listaVinculosColaborador;
            colaboradorMapeado.NomeAnexoVinculo = listaVinculosColaborador[0].NomeAnexo;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosColaborador);

            //Propula combo de estado
            PopularEstadosDropDownList();

            //Preenche combo municipio de acordo com o estado
            ViewBag.Municipio = new List<Municipio>();
            var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, colaboradorMapeado.EstadoId).OrderBy(m => m.Nome);
            if (lstMunicipio != null) { ViewBag.Municipio = lstMunicipio; };

            PopularDadosDropDownList();

            //Preenche combo com todos os cursos
            var listCursos = objCursosService.Listar().OrderBy(c => c.Descricao);
            if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; };

            //Popula cursos selecionados do colaborador
            var cursosColaborador = Mapper.Map<List<ColaboradorCurso>>(objColaboradorCursosService.Listar(colaboradorEditado.ColaboradorId));
            var cursosSelecionados = listCursos.Where(c => (cursosColaborador.Where(p => p.CursoId == c.CursoId).Count() > 0)).ToList();
            for (int i = 0; i < cursosSelecionados.Count; i++)
            {
                cursosSelecionados[i].Validade = String.Format("{0:dd/MM/yyyy}", cursosColaborador[i].Validade.ToString());
            }

            if (cursosSelecionados != null && cursosSelecionados.Any())
            {
                colaboradorMapeado.NomeAnexoCurso = cursosColaborador[0].NomeArquivo;
                ViewBag.CursosSelecionados = cursosSelecionados;
                Session.Add(SESS_CURSOS_SELECIONADOS, cursosSelecionados);
            }

            //Popula anexos do colaborador
            var anexosSelecionados = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId);
            if (anexosSelecionados != null)
            {
                ViewBag.AnexosSelecionados = anexosSelecionados;
                Session.Add(SESS_ANEXOS_SELECIONADOS, anexosSelecionados);
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
            Session.Remove(SESS_ANEXOS_SELECIONADOS);
            Session.Remove(SESS_ANEXOS_REMOVIDOS);
            Session.Remove(SESS_FOTO_COLABORADOR);
            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
            ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();

            //carrega os cursos
            var listCursos = objCursosService.Listar().ToList();
            if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; }
            ViewBag.CursosSelecionados = new List<Curso>();

            //carrega os anexos
            var listAnexos = objColaboradorAnexoService.Listar().ToList();
            if (listAnexos != null && listAnexos.Any()) { ViewBag.Anexos = listAnexos; }
            ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();

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

                if (model.FotoColaborador != null)
                {
                    OnSelecionaFoto_Click(model.FotoColaborador, model);
                }

                //if (model.FileUpload != null)
                //{
                //    if (model.FileUpload.ContentLength > 2048000)
                //        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo 2,00 MB");

                //    //if (!Path.GetExtension(model.FileUpload.FileName).Equals(".pdf"))
                //    //    ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                //}


                if (((List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("EmpresaContratoId", "Necessário adicionar pelo menos um contrato!");
                    //TempData["error"] = "É necessário adicionar pelo menos um contrato!";
                }

                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    colaboradorMapeado.Precadastro = true;
                    colaboradorMapeado.StatusCadastro = 0;

                    if (colaboradorMapeado.Estrangeiro)
                    {
                        colaboradorMapeado.Cpf = "000.000.000-00";
                    }

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
                                vinculo.Motorista = colaboradorMapeado.Motorista;
                                vinculo.Ativo = false;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);
                                objColaboradorEmpresaService.Criar(colaboradorEmpresa);
                                objColaboradorEmpresaService.CriarNumeroMatricula(colaboradorEmpresa);
                            }
                        }
                    }


                    // excluir os cursos removidos da lista
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
                                colaboradorCurso.Validade = Convert.ToDateTime(curso.Validade);
                                objColaboradorCursosService.Criar(colaboradorCurso);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objColaboradorAnexoService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            var colaboradorAnexo = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (colaboradorAnexo == null)
                            {
                                colaboradorAnexo = new ColaboradorAnexo();
                                colaboradorAnexo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorAnexo.ColaboradorAnexoId = anexo.ColaboradorAnexoId;
                                colaboradorAnexo.Descricao = anexo.Descricao;
                                colaboradorAnexo.NomeArquivo = anexo.NomeArquivo;
                                colaboradorAnexo.Arquivo = anexo.Arquivo;
                                objColaboradorAnexoService.Criar(colaboradorAnexo);
                            }
                        }
                    }


                    CriarAnexo(model, colaboradorMapeado.ColaboradorId);
                    CriarColaboradorAceite(model, colaboradorMapeado.ColaboradorId);

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

                ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();

                ShowListaErros();

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
            Session.Remove(SESS_ANEXOS_SELECIONADOS);
            Session.Remove(SESS_ANEXOS_REMOVIDOS);
            ViewBag.Contratos = new List<EmpresaContrato>();
            ViewBag.Cursos = new List<Curso>();
            ViewBag.CursosSelecionados = new List<Curso>();
            ViewBag.Anexos = new List<ColaboradorAnexo>();
            ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();
            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);

            colaboradorMapeado.chkAceite = true;

            CarregaFotoColaborador(colaboradorMapeado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Popula contratos selecionados
            ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
            var listaVinculosColaborador = Mapper.Map<List<ColaboradorEmpresaViewModel>>(objColaboradorEmpresaService.Listar(colaboradorEditado.ColaboradorId));
            ViewBag.ContratosSelecionados = listaVinculosColaborador;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosColaborador);

            //Popula combo de estado
            PopularEstadosDropDownList();

            //Preenche combo municipio de acordo com o estado
            ViewBag.Municipio = new List<Municipio>();
            var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, colaboradorMapeado.EstadoId).OrderBy(m => m.Nome);
            if (lstMunicipio != null) { ViewBag.Municipio = lstMunicipio; };

            PopularDadosDropDownList();

            //Preenche combo com todos os cursos
            var listCursos = objCursosService.Listar().OrderBy(c => c.Descricao);
            if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; };

            //Popula cursos selecionados do colaborador
            var cursosColaborador = objColaboradorCursosService.Listar(colaboradorEditado.ColaboradorId).ToList();
            var cursosSelecionados = listCursos.Where(c => (cursosColaborador.Where(p => p.CursoId == c.CursoId).Count() > 0)).ToList();
            for (int i = 0; i < cursosSelecionados.Count; i++)
            {
                cursosSelecionados[i].Validade = String.Format("{0:dd/MM/yyyy}", cursosColaborador[i].Validade.ToString());
            }
            if (cursosSelecionados != null && cursosSelecionados.Any())
            {
                ViewBag.CursosSelecionados = cursosSelecionados;
                Session.Add(SESS_CURSOS_SELECIONADOS, cursosSelecionados);
            }

            //Popula anexos do colaborador
            var anexosSelecionados = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId);
            if (anexosSelecionados != null)
            {
                ViewBag.AnexosSelecionados = anexosSelecionados;
                Session.Add(SESS_ANEXOS_SELECIONADOS, anexosSelecionados);
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

                if (model.FotoColaborador != null)
                {
                    OnSelecionaFoto_Click(model.FotoColaborador, model);
                }
                if (String.IsNullOrEmpty(model.Foto))
                {
                    model.Foto = (string)Session[SESS_FOTO_COLABORADOR];
                }

                //if (model.FileUpload != null)
                //{
                //    if (model.FileUpload.ContentLength > 2048000)
                //        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo 2,00 MB");

                //    if (!Path.GetExtension(model.FileUpload.FileName).Equals(".pdf"))
                //        ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                //}

                model.chkAceite = true;
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    colaboradorMapeado.Precadastro = true;
                    colaboradorMapeado.Observacao = null;

                    //Aguardando Revisão
                    colaboradorMapeado.StatusCadastro = 1;

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

                    // excluir os cursos removidos da lista
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
                                colaboradorCurso.Validade = Convert.ToDateTime(curso.Validade);
                                objColaboradorCursosService.Criar(colaboradorCurso);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objColaboradorAnexoService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            var colaboradorAnexo = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (colaboradorAnexo != null)
                            {
                                objColaboradorAnexoService.Alterar(colaboradorAnexo);
                            }
                            else
                            {
                                colaboradorAnexo = new ColaboradorAnexo();
                                colaboradorAnexo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorAnexo.ColaboradorAnexoId = anexo.ColaboradorAnexoId;
                                colaboradorAnexo.Descricao = anexo.Descricao;
                                colaboradorAnexo.NomeArquivo = anexo.NomeArquivo;
                                colaboradorAnexo.Arquivo = anexo.Arquivo;
                                objColaboradorAnexoService.Criar(colaboradorAnexo);
                            }
                        }
                    }

                    //Insere anexos do vínculo e dos cursos
                    CriarAnexo(model, colaboradorMapeado.ColaboradorId);
                    //if (model.FileUpload != null)
                    //{
                    //    ExcluirColaboradorAnexoAnterior(model);
                    //    CriarColaboradorAnexo(model, colaboradorMapeado.ColaboradorId);
                    //}

                    return RedirectToAction("Index");
                }
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

                ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();

                ShowListaErros();

                //PopularEstadosDropDownList();
                PopularDadosDropDownList();
                return View(model);
                //throw new Exception("Campos obrigatórios não foram preenchidos");
            }
            catch (Exception ex)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, Colaborador model)
        {

            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

            try
            {
                //if (id == null)
                //    return HttpNotFound();
                var idColaborador = id;

                // Initializes the variables to pass to the MessageBox.Show method.

                var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                colaboradorMapeado.ColaboradorId = Convert.ToInt32(idColaborador);

                //Pega os anexos do colaborador
                var anexosColaborador = objColaboradorAnexoService.Listar(idColaborador);
                //exclui os anexos
                foreach (var anexo in anexosColaborador)
                {
                    objColaboradorAnexoService.Remover(anexo);
                }

                objService.Remover(colaboradorMapeado);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Colaborador/Delete/5
        //[Authorize]
        //public ActionResult Delete(int id)
        //{
        //    // TODO: Add delete logic here
        //    Colaborador colaborador = new Colaborador();
        //    colaborador.ColaboradorId = id;
        //    //var colaboradorMapeado = Mapper.Map<Colaborador>(colaborador);
        //    objService.Remover(colaborador);
        //    return View();
        //}

        //POST: Colaborador/Delete/5

        public ActionResult Delete(int id, System.Web.Mvc.FormCollection collection)
        {
            try
            {
                var colaborador = objService.Listar(id).FirstOrDefault();

                //obtém vinculos do colaborador
                ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaborador);

                CarregaFotoColaborador(colaboradorMapeado);

                // TODO: Add delete logic here
                //var colaboradorMapeado = Mapper.Map<Colaborador>(collection);

                //return RedirectToAction("Index");
                return View(colaboradorMapeado);
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
        public ActionResult AdicionarCurso()
        {
            if (Session[SESS_CURSOS_SELECIONADOS] == null) Session[SESS_CURSOS_SELECIONADOS] = new List<Curso>();
            var item = objCursosService.Listar(Convert.ToInt32(Request.Form["id"])).FirstOrDefault();
            item.Validade = Request.Form["validade"];
            ((List<Curso>)Session[SESS_CURSOS_SELECIONADOS]).Add(item);
            return Json((List<Curso>)Session[SESS_CURSOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverCurso(int id)
        {
            var listCurso = (List<Curso>)Session[SESS_CURSOS_SELECIONADOS];
            listCurso.Remove(new Curso(id));
            Session[SESS_CURSOS_SELECIONADOS] = listCurso;
            if (Session[SESS_CURSOS_REMOVIDOS] == null) Session[SESS_CURSOS_REMOVIDOS] = new List<int>();
            ((List<int>)Session[SESS_CURSOS_REMOVIDOS]).Add(id);
            return Json(listCurso, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AdicionarAnexo()
        {
            if (Session[SESS_ANEXOS_SELECIONADOS] == null) Session[SESS_ANEXOS_SELECIONADOS] = new List<ColaboradorAnexo>();

            byte[] bufferArquivo;
            string NomeArquivo;
            string ExtensaoArquivo;

            HttpFileCollectionBase files = Request.Files;

            if (files.Count != 0)
            {
                HttpPostedFileBase file = files[0];
                NomeArquivo = Path.GetFileNameWithoutExtension(file.FileName);
                ExtensaoArquivo = Path.GetExtension(file.FileName);

                var arquivoStream = file.InputStream;
                using (MemoryStream ms = new MemoryStream())
                {
                    arquivoStream.CopyTo(ms);
                    bufferArquivo = ms.ToArray();
                }

                var arquivoBase64 = Convert.ToBase64String(bufferArquivo);

                ColaboradorAnexo item = new ColaboradorAnexo();

                item.Arquivo = arquivoBase64;
                item.NomeArquivo = NomeArquivo + ExtensaoArquivo;
                item.Descricao = Request.Form["descricao"];
                ((List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS]).Add(item);
            }
            return Json((List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RemoverAnexo(int id)
        {
            var listAnexo = (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS];

            int indice = listAnexo.FindIndex(c => c.ColaboradorAnexoId.Equals(id));
            listAnexo.RemoveAt(indice);

            //listAnexo.Remove(new ColaboradorAnexo(id));
            Session[SESS_ANEXOS_SELECIONADOS] = listAnexo;
            if (Session[SESS_ANEXOS_REMOVIDOS] == null) Session[SESS_ANEXOS_REMOVIDOS] = new List<int>();
            ((List<int>)Session[SESS_ANEXOS_REMOVIDOS]).Add(id);
            return Json(listAnexo, JsonRequestBehavior.AllowGet);
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
            Colaborador colaborador = new Colaborador();

            if (!cpf.Equals("___.___.___-__"))
            {
                colaborador = objService.ObterPorCpf(cpf);
                return Json(colaborador, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }


        }


        [Authorize]
        public JsonResult LocalizarCEP(string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {

                    string cep = id.Replace("-", "");

                    Address endereco = SearchZip.GetAddress(cep);
                    if (endereco.Zip != null)
                    {

                        return Json(endereco, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
            if (model.FotoColaborador != null) return;
            var listaFoto = objService.BuscarPelaChave(model.ColaboradorId);

            if (listaFoto != null)
                model.Foto = listaFoto.Foto;
            if (model.Foto != null)
            {
                var bytes = Convert.FromBase64String(model.Foto);
                string base64 = Convert.ToBase64String(bytes);

                Session.Add(SESS_FOTO_COLABORADOR, base64);
                ViewBag.FotoColaborador = String.Format("data:image/gif;base64,{0}", base64);
            }
        }


        [Authorize]
        public void BuscarFoto(int colaborador, ColaboradorViewModel model)
        {
            try
            {
                if (model.FotoColaborador != null) return;
                var listaFoto = objService.BuscarPelaChave(colaborador);

                if (listaFoto != null)
                    model.Foto = listaFoto.Foto;
            }
            catch (Exception ex)
            {
            }
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

        private void CriarColaboradorAceite(ColaboradorViewModel colaborador, int colaboradorId = 0)
        {
            byte[] bufferArquivo;
            string NomeArquivo;
            string ExtensaoArquivo;

            if (colaborador.Aceite == null) return;
            if (!colaborador.chkAceite) return;
            if (colaborador == null || colaboradorId == 0) return;
            if (colaborador.Aceite.ContentLength <= 0 || colaborador.Aceite.ContentLength > 2048000) return;

            NomeArquivo = Path.GetFileNameWithoutExtension(colaborador.Aceite.FileName);
            ExtensaoArquivo = Path.GetExtension(colaborador.Aceite.FileName);

            if (!ExtensaoArquivo.Equals(".pdf")) return;

            var arquivoStream = colaborador.Aceite.InputStream;
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
        #region Colaborador Anexo

        private void CriarAnexo(ColaboradorViewModel colaborador, int colaboradorId)
        {
            byte[] bufferArquivo;
            string NomeArquivo;
            string ExtensaoArquivo;
            Stream arquivoStream;
            var arquivoBase64 = "";

            if (colaborador == null || colaboradorId == 0) return;
            if (colaborador.AnexoCurso == null && colaborador.AnexoVinculo == null)
            {
                return;
            }
            else
            {
                if (colaborador.AnexoCurso != null)
                {
                    NomeArquivo = Path.GetFileNameWithoutExtension(colaborador.AnexoCurso.FileName);
                    ExtensaoArquivo = Path.GetExtension(colaborador.AnexoCurso.FileName);
                    arquivoStream = colaborador.AnexoCurso.InputStream;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        arquivoStream.CopyTo(ms);
                        bufferArquivo = ms.ToArray();
                    }

                    arquivoBase64 = Convert.ToBase64String(bufferArquivo);

                    var cursosColaborador = objColaboradorCursosService.Listar(colaboradorId);

                    foreach (var curso in cursosColaborador)
                    {
                        curso.Arquivo = arquivoBase64;
                        curso.NomeArquivo = NomeArquivo + ExtensaoArquivo;
                        objColaboradorCursosService.Alterar(curso);
                    }
                }

                if (colaborador.AnexoVinculo != null)
                {
                    NomeArquivo = Path.GetFileNameWithoutExtension(colaborador.AnexoVinculo.FileName);
                    ExtensaoArquivo = Path.GetExtension(colaborador.AnexoVinculo.FileName);
                    arquivoStream = colaborador.AnexoVinculo.InputStream;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        arquivoStream.CopyTo(ms);
                        bufferArquivo = ms.ToArray();
                    }

                    arquivoBase64 = Convert.ToBase64String(bufferArquivo);

                    var vinculosColaborador = objColaboradorEmpresaService.Listar(colaboradorId);

                    foreach (var vinculo in vinculosColaborador)
                    {
                        vinculo.Anexo = arquivoBase64;
                        vinculo.NomeAnexo = NomeArquivo + ExtensaoArquivo;
                        objColaboradorEmpresaService.Alterar(vinculo);
                    }
                }
            }
            //if (colaborador.FileUpload.ContentLength <= 0 || colaborador.FileUpload.ContentLength > 2048000) return;

        }

        //private void ExcluirColaboradorAnexoAnterior(ColaboradorViewModel colaborador)
        //{
        //    if (!colaborador.Precadastro) return;
        //    if (colaborador.FileUpload == null) return;
        //    if (colaborador == null || colaborador.ColaboradorId == 0) return;
        //    if (colaborador.FileUpload.ContentLength <= 0 || colaborador.FileUpload.ContentLength > 2048000) return;

        //    var objColaboradoAnexo = objColaboradorAnexoService.ListarComAnexo(colaborador.ColaboradorId).FirstOrDefault();
        //    if (objColaboradoAnexo == null) return;

        //    objColaboradorAnexoService.Remover(objColaboradoAnexo);
        //}

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


        [HttpPost]
        public ActionResult Capturar()
        {
            if (Request.InputStream.Length > 0)
            {
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    string hexString = Server.UrlEncode(reader.ReadToEnd());
                    string nomeImagem = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
                    string caminhoImagem = string.Format("~/Imagens/{0}.png", nomeImagem);
                    System.IO.File.WriteAllBytes(Server.MapPath(caminhoImagem), ConvertHexToBytes(hexString));
                    Session["ImagemCapturada"] = VirtualPathUtility.ToAbsolute(caminhoImagem);
                }
            }
            return View();
        }
        [HttpPost]
        public ContentResult GetCapturar()
        {
            string url = Session["ImagemCapturada"].ToString();
            Session["ImagemCapturada"] = null;
            return Content(url);
        }
        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        #endregion

        #endregion

        public void ShowListaErros()
        {
            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            var lista = "";

            for (int i = 0; i < errorList.Count; i++)
            {
                lista = lista + "<li>" + errorList[i].ToString() + "</li>";
            }

            TempData["error"] = "<ul>" + lista + "</ul>";
        }
    }
}