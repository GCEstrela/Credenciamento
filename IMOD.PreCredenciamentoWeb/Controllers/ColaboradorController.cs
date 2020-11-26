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
using IMOD.Domain.Enums;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    [Authorize]
    public class ColaboradorController : Controller
    {
        #region Propriedades
        private readonly IColaboradorService objService = new IMOD.Application.Service.ColaboradorService();
        private readonly IColaboradorWebService objServiceWeb = new IMOD.Application.Service.ColaboradorWebService();
        private readonly IDadosAuxiliaresFacade objAuxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService objContratosService = new EmpresaContratoService();
        private readonly IColaboradorEmpresaService objColaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly IColaboradorEmpresaWebService objColaboradorEmpresaWebService = new ColaboradorEmpresaWebService();
        private readonly ICursoService objCursosService = new CursoService();
        private readonly IColaboradorCursoService objColaboradorCursosService = new ColaboradorCursosService();
        private readonly IColaboradorCursoWebService objColaboradorCursosWebService = new ColaboradorCursosWebService();
        private readonly IMunicipioService objMunicipioSevice = new MunicipioService();
        private readonly IMOD.Application.Interfaces.IColaboradorAnexoService objColaboradorAnexoService = new IMOD.Application.Service.ColaboradorAnexoService();
        private readonly IMOD.Application.Interfaces.IColaboradorAnexoWebService objColaboradorAnexoWebService = new IMOD.Application.Service.ColaboradorAnexoWebService();
        private readonly IColaboradorObservacaoService objColaboradorObservacaoService = new ColaboradorObservacaoService();
        private const string SESS_CONTRATOS_SELECIONADOS = "ContratosSelecionados";
        private const string SESS_CONTRATOS_REMOVIDOS = "ContratosRemovidos";
        private const string SESS_CURSOS_SELECIONADOS = "CursosSelecionados";
        private const string SESS_CURSOS_REMOVIDOS = "CursosRemovidos";
        private const string SESS_ANEXOS_SELECIONADOS = "AnexosSelecionados";
        private const string SESS_ANEXOS_REMOVIDOS = "AnexosRemovidos";
        private const string SESS_MUNICIPIO_SELECIONADO = "MunicipioSelecionado";
        private List<Colaborador> colaboradores = new List<Colaborador>();
        private List<Colaborador> colaboradoresWeb = new List<Colaborador>();
        private List<ColaboradorEmpresa> vinculos = new List<ColaboradorEmpresa>();
        private const string SESS_FOTO_COLABORADOR = "Foto";
        #endregion

        [Authorize]
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            List<ColaboradorViewModel> lstColaboradorMapeado = Mapper.Map<List<ColaboradorViewModel>>(ObterColaboradoresEmpresaLogada().OrderBy(e => e.Nome));

            Session[SESS_FOTO_COLABORADOR] = null;
            Session[SESS_CONTRATOS_SELECIONADOS] = null;
            Session[SESS_CONTRATOS_REMOVIDOS] = null;
            Session[SESS_CURSOS_SELECIONADOS] = null;
            Session[SESS_CURSOS_REMOVIDOS] = null;
            Session[SESS_ANEXOS_SELECIONADOS] = null;
            Session[SESS_ANEXOS_REMOVIDOS] = null;
            Session[SESS_MUNICIPIO_SELECIONADO] = null;

            return View(lstColaboradorMapeado);
        }

        private IList<Colaborador> ObterColaboradoresEmpresaLogada()
        {
            var listaColaboradores = objService.Listar(null, null, null, null);
            var listaColaboradoresVinculos = objColaboradorEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId, null);
            var listaIdsVinculos = listaColaboradoresVinculos.Select(c => c.ColaboradorId).ToList<int>();
            colaboradores = listaColaboradores.Where(c => listaIdsVinculos.Contains(c.ColaboradorId)).ToList();

            var listaColaboradoresWeb = objServiceWeb.Listar(null, null, null, null);
            var listaColaboradoresVinculosWeb = objColaboradorEmpresaWebService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId, null);
            var listaIdsVinculosWeb = listaColaboradoresVinculosWeb.Select(c => c.ColaboradorId).ToList<int>();
            colaboradoresWeb = listaColaboradoresWeb.Where(c => listaIdsVinculosWeb.Contains(c.ColaboradorId)).ToList();

            colaboradores = colaboradores.Except(colaboradoresWeb).ToList();
            colaboradores.AddRange(colaboradoresWeb);

            return colaboradores.OrderBy(c => c.Nome).ToList();
        }

        // GET: Colaborador/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();

            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = ExibirInfoColaboradorGET(colaboradorEditado);

            return View(colaboradorMapeado);
        }

        // GET: Colaborador/Details/5
        [Authorize]
        public ActionResult DetailsPreCadastro(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objServiceWeb.Listar(id).FirstOrDefault();

            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = ExibirInfoColaboradorGET(colaboradorEditado);

            return View(colaboradorMapeado);
        }

        // GET: Colaborador/Create
        [Authorize]
        public ActionResult Create()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
            //carrega os cursos
            var listCursos = objCursosService.Listar().ToList();
            if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; }
            //carrega os anexos
            var listAnexos = objColaboradorAnexoWebService.Listar().ToList();
            if (listAnexos != null && listAnexos.Any()) { ViewBag.Anexos = listAnexos; }

            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
            {
                ViewBag.ContratosSelecionados = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            }
            else
            {
                ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
            }

            if (Session[SESS_CURSOS_SELECIONADOS] != null)
            {
                ViewBag.CursosSelecionados = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];
            }
            else
            {
                ViewBag.CursosSelecionados = new List<ColaboradorCurso>();
            }

            if (Session[SESS_ANEXOS_SELECIONADOS] != null)
            {
                ViewBag.AnexosSelecionados = (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();
            }

            if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
            {
                ViewBag.Municipio = (List<Municipio>)Session[SESS_MUNICIPIO_SELECIONADO];
            }
            else
            {
                ViewBag.Municipio = new List<Municipio>();
            }

            PopularEstadosDropDownList();
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

                if (((List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("EmpresaContratoId", "Necessário adicionar pelo menos um contrato!");
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

                    objServiceWeb.Criar(colaboradorMapeado);

                    // Atualiza codigo do colaborador de acordo com o novo registro criado de forma a criar uma associacao entre eles para os registros recem criados.
                    colaboradorMapeado.ColaboradorId = colaboradorMapeado.ColaboradorWebId;
                    objServiceWeb.Alterar(colaboradorMapeado);

                    // exclui os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objColaboradorEmpresaWebService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objColaboradorEmpresaWebService.Remover(contratoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objColaboradorEmpresaWebService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                vinculo.Ativo = false;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);
                                objColaboradorEmpresaWebService.Criar(colaboradorEmpresa);
                                colaboradorEmpresa.ColaboradorEmpresaId = colaboradorEmpresa.ColaboradorEmpresaWebId;
                                objColaboradorEmpresaWebService.CriarNumeroMatricula(colaboradorEmpresa);

                                // Atualiza codigo do colaborador de acordo com o novo registro criado de forma a criar uma associacao entre eles para os registros recem criados.
                                objColaboradorEmpresaWebService.Alterar(colaboradorEmpresa);
                            }
                        }
                    }


                    // excluir os cursos removidos da lista
                    if (Session[SESS_CURSOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CURSOS_REMOVIDOS])
                        {
                            var cursoExclusao = objColaboradorCursosWebService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (cursoExclusao != null)
                            {
                                objColaboradorCursosWebService.Remover(cursoExclusao);
                            }
                        }
                    }

                    //inclui os cursos selecionados
                    if (Session[SESS_CURSOS_SELECIONADOS] != null)
                    {
                        foreach (var curso in (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS])
                        {
                            var colaboradorCurso = objColaboradorCursosWebService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                            if (colaboradorCurso == null)
                            {
                                colaboradorCurso = new ColaboradorCurso();
                                curso.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorCurso = Mapper.Map<ColaboradorCurso>(curso);
                                objColaboradorCursosWebService.Criar(colaboradorCurso);

                                // Atualiza codigo do colaborador de acordo com o novo registro criado de forma a criar uma associacao entre eles para os registros recem criados.
                                colaboradorCurso.ColaboradorCursoId = colaboradorCurso.ColaboradorCursoWebId;
                                objColaboradorCursosWebService.Alterar(colaboradorCurso);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objColaboradorAnexoWebService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objColaboradorAnexoWebService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            var colaboradorAnexo = objColaboradorAnexoWebService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (colaboradorAnexo == null)
                            {
                                colaboradorAnexo = new ColaboradorAnexo();
                                anexo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorAnexo = Mapper.Map<ColaboradorAnexo>(anexo);
                                objColaboradorAnexoWebService.Criar(colaboradorAnexo);

                                // Atualiza codigo do colaborador de acordo com o novo registro criado de forma a criar uma associacao entre eles para os registros recem criados.
                                colaboradorAnexo.ColaboradorAnexoId = colaboradorAnexo.ColaboradorAnexoWebId;
                                objColaboradorAnexoWebService.Alterar(colaboradorAnexo);
                            }
                        }
                    }

                    //Limpa sessão
                    Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                    Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                    Session.Remove(SESS_CURSOS_SELECIONADOS);
                    Session.Remove(SESS_CURSOS_REMOVIDOS);
                    Session.Remove(SESS_ANEXOS_SELECIONADOS);
                    Session.Remove(SESS_ANEXOS_REMOVIDOS);
                    Session.Remove(SESS_FOTO_COLABORADOR);

                    CriarColaboradorAceite(model, colaboradorMapeado.ColaboradorId);

                    return RedirectToAction("Index", "Colaborador");
                }

                // Se ocorrer um erro retorna para pagina
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }

                //carrega os cursos
                var listCursos = objCursosService.Listar().ToList();
                if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; }

                PopularEstadosDropDownList();
                PopularDadosDropDownList();

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                {
                    ViewBag.ContratosSelecionados = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
                }

                if (Session[SESS_CURSOS_SELECIONADOS] != null)
                {
                    ViewBag.CursosSelecionados = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.CursosSelecionados = new List<ColaboradorCurso>();
                }

                if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                {
                    ViewBag.AnexosSelecionados = (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();
                }

                if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
                {
                    ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
                }
                else
                {
                    ViewBag.Municipio = new List<Municipio>();
                }

                GetFotoOnErro(model);
                ShowListaErros();

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar registro");
                return View();
            }

        }

        // GET: Colaborador/EditRevisao/5
        public ActionResult EditRevisao(int? id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o colaborador pelo ID
            var colaboradorEditado = objService.Listar(id).FirstOrDefault();

            if (colaboradorEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            ColaboradorViewModel colaboradorMapeado = ExibirInfoColaboradorGET(colaboradorEditado);

            return View(colaboradorMapeado);
        }

        // GET: Colaborador/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o colaboradorWeb pelo ID
            var colaboradorEditado = objServiceWeb.Listar(id).FirstOrDefault();
            if (colaboradorEditado == null)
                return HttpNotFound();

            ColaboradorViewModel colaboradorMapeado = ExibirInfoColaboradorGET(colaboradorEditado);

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

                if (((List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("EmpresaContratoId", "Necessário adicionar pelo menos um contrato!");
                }

                if (model.FotoColaborador != null)
                {
                    OnSelecionaFoto_Click(model.FotoColaborador, model);
                }
                if (String.IsNullOrEmpty(model.Foto))
                {
                    model.Foto = (string)Session[SESS_FOTO_COLABORADOR];
                }

                ModelState.Remove("chkAceite");

                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    colaboradorMapeado.Precadastro = true;
                    colaboradorMapeado.Observacao = null;
                    colaboradorMapeado.ColaboradorWebId = id.Value;

                    //Aguardando Aprovação
                    colaboradorMapeado.StatusCadastro = (int)StatusCadastro.AGUARDANDO_APROVACAO;

                    objServiceWeb.Alterar(colaboradorMapeado);

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objColaboradorEmpresaWebService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objColaboradorEmpresaWebService.Remover(contratoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objColaboradorEmpresaWebService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);
                                objColaboradorEmpresaWebService.Criar(colaboradorEmpresa);
                                colaboradorEmpresa.ColaboradorEmpresaId = colaboradorEmpresa.ColaboradorEmpresaWebId;
                                objColaboradorEmpresaWebService.CriarNumeroMatricula(colaboradorEmpresa);

                                objColaboradorEmpresaWebService.Alterar(colaboradorEmpresa);
                            }
                        }
                    }

                    // excluir os cursos removidos da lista
                    if (Session[SESS_CURSOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CURSOS_REMOVIDOS])
                        {
                            var cursoExclusao = objColaboradorCursosWebService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (cursoExclusao != null)
                            {
                                objColaboradorCursosWebService.Remover(cursoExclusao);
                            }
                        }
                    }

                    //inclui os cursos selecionados
                    if (Session[SESS_CURSOS_SELECIONADOS] != null)
                    {
                        foreach (var curso in (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS])
                        {
                            var colaboradorCurso = objColaboradorCursosWebService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                            if (colaboradorCurso == null)                            
                            {
                                colaboradorCurso = new ColaboradorCurso();
                                curso.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorCurso = Mapper.Map<ColaboradorCurso>(curso);
                                objColaboradorCursosWebService.Criar(colaboradorCurso);

                                colaboradorCurso.ColaboradorCursoId = colaboradorCurso.ColaboradorCursoWebId;
                                objColaboradorCursosWebService.Alterar(colaboradorCurso);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objColaboradorAnexoWebService.Listar(colaboradorMapeado.ColaboradorId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objColaboradorAnexoWebService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            if (anexo.ColaboradorAnexoId.Equals(0))
                                anexo.ColaboradorAnexoId = -1;

                            var colaboradorAnexo = objColaboradorAnexoWebService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (colaboradorAnexo == null)
                            {
                                colaboradorAnexo = new ColaboradorAnexo();
                                anexo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                colaboradorAnexo = Mapper.Map<ColaboradorAnexo>(anexo);
                                objColaboradorAnexoWebService.Criar(colaboradorAnexo);

                                colaboradorAnexo.ColaboradorAnexoId = colaboradorAnexo.ColaboradorAnexoWebId;
                                objColaboradorAnexoWebService.Alterar(colaboradorAnexo);
                            }
                        }
                    }

                    //Remove itens da sessão
                    Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                    Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                    Session.Remove(SESS_CURSOS_SELECIONADOS);
                    Session.Remove(SESS_CURSOS_REMOVIDOS);
                    Session.Remove(SESS_ANEXOS_SELECIONADOS);
                    Session.Remove(SESS_ANEXOS_REMOVIDOS);

                    return RedirectToAction("Index");
                }
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }

                PopularObservacoesColaborador(model);

                //carrega os cursos
                var listCursos = objCursosService.Listar().ToList();
                if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; }

                PopularEstadosDropDownList();

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                PopularDadosDropDownList();

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                {
                    ViewBag.ContratosSelecionados = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
                }

                if (Session[SESS_CURSOS_SELECIONADOS] != null)
                {
                    ViewBag.CursosSelecionados = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.CursosSelecionados = new List<ColaboradorCurso>();
                }

                if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                {
                    ViewBag.AnexosSelecionados = (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();
                }

                if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
                {
                    ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
                }
                else
                {
                    ViewBag.Municipio = new List<Municipio>();
                }

                GetFotoOnErro(model);

                ShowListaErros();

                return View(model);

            }
            catch (Exception ex)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // POST: Colaborador/EditRevisao/5
        [HttpPost]
        [Authorize]
        public ActionResult EditRevisao(int? id, ColaboradorViewModel model)
        {
            try
            {
                if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
                if (id == null)
                    return HttpNotFound();

                if (((List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("EmpresaContratoId", "Necessário adicionar pelo menos um contrato!");
                }

                if (model.FotoColaborador != null)
                {
                    OnSelecionaFoto_Click(model.FotoColaborador, model);
                }
                if (String.IsNullOrEmpty(model.Foto))
                {
                    model.Foto = (string)Session[SESS_FOTO_COLABORADOR];
                }

                ModelState.Remove("chkAceite");

                if (ModelState.IsValid)
                {
                    var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                    colaboradorMapeado.Precadastro = false;

                    //Aguardando Revisao
                    colaboradorMapeado.StatusCadastro = (int)StatusCadastro.AGUARDANDO_REVISAO;

                    var colaborador = objService.Listar(colaboradorMapeado.ColaboradorId).FirstOrDefault();
                    colaborador.StatusCadastro = (int)StatusCadastro.AGUARDANDO_REVISAO;
                    colaborador.Foto = (string)Session[SESS_FOTO_COLABORADOR];
                    objService.Alterar(colaborador);

                    // Inclusao de colaboradorweb 
                    var colaboradorWebListar = objServiceWeb.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, (int)StatusCadastro.APROVADO).FirstOrDefault();
                    if (colaboradorWebListar == null)
                    {
                        objServiceWeb.Criar(colaboradorMapeado);
                    }
                    else
                    {
                        colaboradorMapeado.ColaboradorWebId = colaboradorWebListar.ColaboradorWebId;
                        objServiceWeb.Alterar(colaboradorMapeado);
                    }

                    colaboradorMapeado.ColaboradorId = id.Value;

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objColaboradorEmpresaService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objColaboradorEmpresaService.Remover(contratoExclusao);
                                objColaboradorEmpresaWebService.Remover(contratoExclusao);
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
                                var ItemWeb = objColaboradorEmpresaWebService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                                if (ItemWeb == null)
                                {
                                    vinculo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                    vinculo.Ativo = true;
                                    vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                    var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);

                                    // Inserindo registro de colaboradorempresa em coloboradorempresaweb
                                    objColaboradorEmpresaWebService.Criar(colaboradorEmpresa);
                                    objColaboradorEmpresaWebService.CriarNumeroMatricula(colaboradorEmpresa);

                                    colaboradorEmpresa.ColaboradorEmpresaId = colaboradorEmpresa.ColaboradorEmpresaWebId;
                                    objColaboradorEmpresaWebService.Alterar(colaboradorEmpresa);
                                }
                            }
                            else
                            {
                                var ItemWeb = objColaboradorEmpresaWebService.Listar(colaboradorMapeado.ColaboradorId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                                if (ItemWeb == null)
                                {
                                    // Inserindo registro de colaboradorempresa em coloboradorempresaweb quando já exista um contrato que ainda não foi inserido em coloboradorempresaweb
                                    objColaboradorEmpresaWebService.Criar(item);
                                }
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
                                objColaboradorCursosWebService.Remover(cursoExclusao);
                            }
                        }
                    }

                    //inclui os cursos selecionados
                    if (Session[SESS_CURSOS_SELECIONADOS] != null)
                    {
                        foreach (var curso in (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS])
                        {
                            var colaboradorCurso = objColaboradorCursosService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                            if (colaboradorCurso == null)
                            {
                                var colaboradorCursoWeb = objColaboradorCursosWebService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                                if (colaboradorCursoWeb == null)
                                {
                                    colaboradorCurso = new ColaboradorCurso();
                                    curso.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                    colaboradorCurso = Mapper.Map<ColaboradorCurso>(curso);

                                    // Inserindo registro de colaboradorcurso em coloboradorcursoweb
                                    objColaboradorCursosWebService.Criar(colaboradorCurso);

                                    colaboradorCurso.ColaboradorCursoId = colaboradorCurso.ColaboradorCursoWebId;
                                    objColaboradorCursosWebService.Alterar(colaboradorCurso);
                                }
                            } 
                            else
                            {
                                var colaboradorCursoWeb = objColaboradorCursosWebService.Listar(colaboradorMapeado.ColaboradorId, curso.CursoId, null, null, null, null, null).FirstOrDefault();
                                if(colaboradorCursoWeb == null)
                                {
                                    objColaboradorCursosWebService.Criar(colaboradorCurso);
                                }
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
                                objColaboradorAnexoWebService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            if (anexo.ColaboradorAnexoId.Equals(0))
                                anexo.ColaboradorAnexoId = -1;

                            var colaboradorAnexo = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (colaboradorAnexo == null)
                            {
                                var colaboradorAnexoWeb = objColaboradorAnexoWebService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo).FirstOrDefault();
                                if (colaboradorAnexoWeb == null)
                                {
                                    colaboradorAnexo = new ColaboradorAnexo();
                                    anexo.ColaboradorId = colaboradorMapeado.ColaboradorId;
                                    colaboradorAnexo = Mapper.Map<ColaboradorAnexo>(anexo);

                                    // Inserindo registro de colaboradoranexo em coloboradoranexoweb
                                    objColaboradorAnexoWebService.Criar(colaboradorAnexo);

                                    colaboradorAnexo.ColaboradorAnexoId = colaboradorAnexo.ColaboradorAnexoWebId;
                                    objColaboradorAnexoWebService.Alterar(colaboradorAnexo);
                                }
                            }
                            else
                            {
                                var colaboradorAnexoWeb = objColaboradorAnexoWebService.Listar(colaboradorMapeado.ColaboradorId, anexo.ColaboradorAnexoId, anexo.NomeArquivo).FirstOrDefault();
                                if (colaboradorAnexoWeb == null)
                                {
                                    objColaboradorAnexoWebService.Criar(colaboradorAnexo);
                                }
                            }
                        }
                    }

                    //Remove itens da sessão
                    Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                    Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                    Session.Remove(SESS_CURSOS_SELECIONADOS);
                    Session.Remove(SESS_CURSOS_REMOVIDOS);
                    Session.Remove(SESS_ANEXOS_SELECIONADOS);
                    Session.Remove(SESS_ANEXOS_REMOVIDOS);

                    return RedirectToAction("Index");
                }
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }

                PopularObservacoesColaborador(model);

                //carrega os cursos
                var listCursos = objCursosService.Listar().ToList();
                if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; }

                PopularEstadosDropDownList();

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                PopularDadosDropDownList();

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                {
                    ViewBag.ContratosSelecionados = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
                }

                if (Session[SESS_CURSOS_SELECIONADOS] != null)
                {
                    ViewBag.CursosSelecionados = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.CursosSelecionados = new List<ColaboradorCurso>();
                }

                if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                {
                    ViewBag.AnexosSelecionados = (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();
                }

                if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
                {
                    ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
                }
                else
                {
                    ViewBag.Municipio = new List<Municipio>();
                }

                GetFotoOnErro(model);

                ShowListaErros();

                return View(model);

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

            if (id.Equals(null))
                return HttpNotFound();

            try
            {
                var idColaborador = id;

                var colaboradorMapeado = Mapper.Map<Colaborador>(model);
                colaboradorMapeado.ColaboradorId = idColaborador;

                objServiceWeb.Remover(colaboradorMapeado);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id, System.Web.Mvc.FormCollection collection)
        {
            try
            {
                var colaborador = objServiceWeb.Listar(id).FirstOrDefault();

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

        [HttpPost]
        [Authorize]
        public JsonResult AdicionarContrato()
        {
            try
            {               
                List<ColaboradorEmpresaViewModel> vinculoList = new List<ColaboradorEmpresaViewModel>();
                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    vinculoList = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];

                foreach (var listContratoCurso in vinculoList)
                {
                    if (listContratoCurso.EmpresaContratoId == Convert.ToInt32(Request.Form["id"]))
                        throw new Exception("contrato_ja_associado_ao_colaborador");
                }

                var item = SessionUsuario.EmpresaLogada.Contratos.Where(c => c.EmpresaContratoId == Convert.ToInt32(Request.Form["id"])).FirstOrDefault();
                ColaboradorEmpresaViewModel vinculo = new ColaboradorEmpresaViewModel();
                vinculo.EmpresaContratoId = Convert.ToInt32(Request.Form["id"]);
                vinculo.Descricao = item.Descricao;
                vinculo.Cargo = Request.Form["cargo"];
                if (!string.IsNullOrEmpty(Request.Form["validade"]))
                    vinculo.Validade = DateTime.Parse(Request.Form["validade"]);
                vinculo.ManuseioBagagem = Convert.ToBoolean(Request.Form["bagagem"]);
                vinculo.Motorista = Convert.ToBoolean(Request.Form["motorista"]);
                vinculo.Matricula = " - ";
                vinculo.OperadorPonteEmbarque = Convert.ToBoolean(Request.Form["ponteEmbarque"]);
                vinculo.FlagCcam = Convert.ToBoolean(Request.Form["ccam"]);
                HttpPostedFileBase file = Request.Files["anexo"];
                vinculo.NomeAnexo = file.FileName;
                vinculo.Anexo = ConverterArquivoBase64(file);
                vinculoList.Add(vinculo);
                Session.Add(SESS_CONTRATOS_SELECIONADOS, vinculoList);

                return Json(vinculoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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
            try
            {
                List<ColaboradorCurso> cursoList = new List<ColaboradorCurso>();
                if (Session[SESS_CURSOS_SELECIONADOS] != null)
                    cursoList = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];

                foreach (var listColabCurso in cursoList)
                {
                    if (listColabCurso.CursoId == Convert.ToInt32(Request.Form["id"]))
                        throw new Exception("curso_ja_associado_ao_colaborador");
                }

                var item = objCursosService.Listar(Convert.ToInt32(Request.Form["id"])).FirstOrDefault();
                ColaboradorCurso colabCurso = new ColaboradorCurso();
                colabCurso.Validade = Convert.ToDateTime(Request.Form["validade"]);
                colabCurso.Descricao = item.Descricao;
                colabCurso.CursoId = Convert.ToInt32(Request.Form["id"]);
                HttpPostedFileBase file = Request.Files["anexoCurso"];
                colabCurso.Arquivo = ConverterArquivoBase64(file);
                colabCurso.NomeArquivo = file.FileName;
                cursoList.Add(colabCurso);
                Session.Add(SESS_CURSOS_SELECIONADOS, cursoList);

                return Json(cursoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public JsonResult RemoverCurso(int id)
        {
            var listCurso = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];
            
            int indice = listCurso.FindIndex(c => c.CursoId.Equals(id));
            listCurso.RemoveAt(indice);

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
        public ActionResult AdicionarObservacao()
        {
            var Observacoes = new List<ColaboradorObservacao>();
            var ColaboradorId = Convert.ToInt32(Request.Form["ColaboradorId"]);
            var item = new ColaboradorObservacao();
            item.Observacao = Request.Form["ObservacaoAprovacao"];
            item.DataRevisao = DateTime.Now;
            item.TipoSituacao = Convert.ToInt32(Request.Form["TipoSituacao"]);
            item.UsuarioRevisao = (int)UsuarioRevisao.CADASTRO_WEB;
            item.ColaboradorId = ColaboradorId;

            try
            {
                objColaboradorObservacaoService.Criar(item);
                Observacoes = objColaboradorObservacaoService.Listar(ColaboradorId, item.ColaboradorObservacaoId).ToList();
            }
            catch (Exception ex)
            {
            }

            return Json(Observacoes, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RemoverObservacao(int id, int colaboradorId)
        {
            var status = "";
            var observacaoExclusao = objColaboradorObservacaoService.Listar(colaboradorId, id).FirstOrDefault();
            if (observacaoExclusao != null)
            {
                var observacaoRespostaId = id;
                var observacaoRespExclusao = objColaboradorObservacaoService.Listar(colaboradorId, null, null, observacaoRespostaId);

                try
                {
                    foreach (var itemResp in observacaoRespExclusao)
                    {
                        objColaboradorObservacaoService.Remover(itemResp);
                    }

                    objColaboradorObservacaoService.Remover(observacaoExclusao);
                    status = id.ToString();
                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }

            return Json(new { resultItemRemovido = status }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AdicionarObservacaoResposta()
        {
            var repostasObservacao = new List<ColaboradorObservacao>();

            var colaboradorObservacaoId = Convert.ToInt32(Request.Form["ColaboradorObservacaoId"]);
            var ColaboradorId = Convert.ToInt32(Request.Form["ColaboradorId"]);

            var item = new ColaboradorObservacao();
            item = objColaboradorObservacaoService.Listar(ColaboradorId, colaboradorObservacaoId).Where(co => co.ColaboradorObservacaoRespostaID == null).FirstOrDefault();
            item.ObservacaoResposta = Request.Form["ObservacaoResposta"];
            item.ColaboradorObservacaoRespostaID = colaboradorObservacaoId;
            item.DataRevisao = DateTime.Now;
            item.UsuarioRevisao = (int)UsuarioRevisao.CADASTRO_WEB;

            try
            {
                objColaboradorObservacaoService.Criar(item);
                repostasObservacao = objColaboradorObservacaoService.Listar(ColaboradorId, null, null, colaboradorObservacaoId).ToList();
            }
            catch (Exception ex)
            {
            }

            return Json(repostasObservacao, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult BuscarEstado(string uf)
        {
            var EstadoUF = objAuxiliaresService.EstadoService.BuscarEstadoPorUf(uf);

            return Json(EstadoUF, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult BuscarMunicipios(int id)
        {
            var listMunicipio = objMunicipioSevice.Listar(null, null, id);

            return Json(listMunicipio, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ConsultarCPF(string cpf)
        {
            try
            {
                objService.ExisteCpf(cpf);
                var colaborador = Mapper.Map<ColaboradorViewModel>(objService.ObterPorCpf(cpf));

                return Json(new { response = colaborador?.ColaboradorId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { response = ex.Message }, JsonRequestBehavior.AllowGet); ;
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
                Session.Add(SESS_FOTO_COLABORADOR, model.Foto);

            }
        }

        [Authorize]
        public void GetFotoOnErro(ColaboradorViewModel model)
        {
            var bytes = Convert.FromBase64String(model.Foto);
            string base64 = Convert.ToBase64String(bytes);

            Session.Add(SESS_FOTO_COLABORADOR, base64);
            ViewBag.FotoColaborador = String.Format("data:image/gif;base64,{0}", base64);
        }

        [Authorize]
        public void CarregaFotoColaborador(ColaboradorViewModel model)
        {
            if (model.FotoColaborador != null) return;

            var listaFoto = objService.BuscarPelaChave(model.ColaboradorId);
            if (!model.StatusCadastro.Equals((int)StatusCadastro.APROVADO))
                listaFoto = objServiceWeb.BuscarPelaChave(model.ColaboradorId);

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
                Session[SESS_MUNICIPIO_SELECIONADO] = lstMunicipio;
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

        private void PopularObservacoesColaborador(ColaboradorViewModel colaboradorMapeado)
        {
            var observacoesAuxSelecionadas = objColaboradorObservacaoService.Listar(colaboradorMapeado.ColaboradorId);
            var observacoesSelecionadas = observacoesAuxSelecionadas.Where(co => co.ColaboradorObservacaoRespostaID == null).OrderBy(co => co.DataRevisao).ToList();
            var observacoesRespostaSelecionadas = observacoesAuxSelecionadas.Where(cor => cor.ColaboradorObservacaoRespostaID != null).OrderBy(cor => cor.DataRevisao).ToList();

            if (observacoesSelecionadas != null)
            {
                ViewBag.ObservacoesSelecionadas = observacoesSelecionadas;
            }

            if (observacoesRespostaSelecionadas != null)
            {
                ViewBag.ObservacoesRespostaSelecionadas = observacoesRespostaSelecionadas;
            }
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
            objColaboradorAnexoWebService.Criar(colaboradorAnexo);

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

            var objColaboradorAnexo = objColaboradorAnexoWebService.ListarComAnexo(colaboradorId).FirstOrDefault();
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

        public string ConverterArquivoBase64(HttpPostedFileBase file)
        {
            byte[] bufferArquivo;

            var arquivoStream = file.InputStream;
                using (MemoryStream ms = new MemoryStream())
                {
                    arquivoStream.CopyTo(ms);
                    bufferArquivo = ms.ToArray();
                }

                var arquivoBase64 = Convert.ToBase64String(bufferArquivo);

            return arquivoBase64;
        }

        private ColaboradorViewModel ExibirInfoColaboradorGET(Colaborador colaboradorEditado)
        {
            //Popula vinculos do colaborador
            ICollection<ColaboradorEmpresa> objColaboradorEmpresa = null;

            //Popula cursos do colaborador
            ICollection<ColaboradorCurso> cursosColaborador = null;

            //Popula anexos do colaborador
            ICollection<ColaboradorAnexo> anexosSelecionados = null;            

            if (colaboradorEditado.StatusCadastro != (int)StatusCadastro.APROVADO)
            {
                if(colaboradorEditado.ColaboradorWebId != colaboradorEditado.ColaboradorId)
                    colaboradorEditado = objServiceWeb.Listar(colaboradorEditado.ColaboradorId).FirstOrDefault();

                objColaboradorEmpresa = objColaboradorEmpresaWebService.Listar(colaboradorEditado.ColaboradorId);
                cursosColaborador = objColaboradorCursosWebService.Listar(colaboradorEditado.ColaboradorId);
                anexosSelecionados = objColaboradorAnexoWebService.Listar(colaboradorEditado.ColaboradorId);

                // A listagem de colaboradorxempresa sem ser web ja possui uma view realizando o join, portanto esta expressao so e aplicada na colaboradorxempresa web
                objColaboradorEmpresa = objColaboradorEmpresa.Join(SessionUsuario.EmpresaLogada.Contratos, c => c.EmpresaContratoId, contrato => contrato.EmpresaContratoId, (empresaContrato, contrato) => { empresaContrato.Descricao = contrato.Descricao; return empresaContrato; }).ToList();
            }
            else
            {
                objColaboradorEmpresa = objColaboradorEmpresaService.Listar(colaboradorEditado.ColaboradorId);
                cursosColaborador = objColaboradorCursosService.Listar(colaboradorEditado.ColaboradorId);
                anexosSelecionados = objColaboradorAnexoService.Listar(colaboradorEditado.ColaboradorId);
            }

            ColaboradorViewModel colaboradorMapeado = Mapper.Map<ColaboradorViewModel>(colaboradorEditado);

            CarregaFotoColaborador(colaboradorMapeado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Popula combo de estado
            PopularEstadosDropDownList();

            PopularMunicipiosDropDownList(colaboradorMapeado.EstadoId.ToString());

            PopularDadosDropDownList();

            //Preenche combo com todos os cursos
            var listCursos = objCursosService.Listar().OrderBy(c => c.Descricao);
            if (listCursos != null && listCursos.Any()) { ViewBag.Cursos = listCursos; };

            var listaVinculosColaborador = Mapper.Map<List<ColaboradorEmpresaViewModel>>(objColaboradorEmpresa);
            ViewBag.ContratosSelecionados = listaVinculosColaborador;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosColaborador);
            
            cursosColaborador = cursosColaborador.Join(listCursos, cc => cc.CursoId, c => c.CursoId, (colaboradorCurso, curso) => { colaboradorCurso.Descricao = curso.Descricao; return colaboradorCurso; }).ToList();

            if (cursosColaborador != null && cursosColaborador.Any())
            {
                ViewBag.CursosSelecionados = cursosColaborador;
                Session.Add(SESS_CURSOS_SELECIONADOS, cursosColaborador);
            }
            
            if (anexosSelecionados != null)
            {
                ViewBag.AnexosSelecionados = anexosSelecionados;
                Session.Add(SESS_ANEXOS_SELECIONADOS, anexosSelecionados);
            }

            //Popula observações para aprovação do colaborador
            PopularObservacoesColaborador(colaboradorMapeado);

            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
            {
                ViewBag.ContratosSelecionados = (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            }
            else
            {
                ViewBag.ContratosSelecionados = new List<ColaboradorEmpresaViewModel>();
            }

            if (Session[SESS_CURSOS_SELECIONADOS] != null)
            {
                ViewBag.CursosSelecionados = (List<ColaboradorCurso>)Session[SESS_CURSOS_SELECIONADOS];
            }
            else
            {
                ViewBag.CursosSelecionados = new List<ColaboradorCurso>();
            }

            if (Session[SESS_ANEXOS_SELECIONADOS] != null)
            {
                ViewBag.AnexosSelecionados = (List<ColaboradorAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<ColaboradorAnexo>();
            }

            if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
            {
                ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
            }
            else
            {
                ViewBag.Municipio = new List<Municipio>();
            }

            return colaboradorMapeado;
        }
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