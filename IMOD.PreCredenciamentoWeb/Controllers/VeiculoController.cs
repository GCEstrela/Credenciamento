using AutoMapper;
using IMOD.Domain.Entities;
using IMOD.PreCredenciamentoWeb.Models;
using IMOD.PreCredenciamentoWeb.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class VeiculoController : Controller
    {
        // GET: Veículo
        private readonly IMOD.Application.Interfaces.IVeiculoService objService = new IMOD.Application.Service.VeiculoService();
        private readonly IMOD.Application.Interfaces.IDadosAuxiliaresFacade objAuxiliaresService = new IMOD.Application.Service.DadosAuxiliaresFacadeService();
        private readonly IMOD.Application.Interfaces.IEmpresaContratosService objContratosService = new IMOD.Application.Service.EmpresaContratoService();
        private readonly IMOD.Application.Interfaces.IVeiculoEmpresaService objVeiculoEmpresaService = new IMOD.Application.Service.VeiculoEmpresaService();
        private readonly IMOD.Application.Interfaces.IVeiculoAnexoService objVeiculoAnexoService = new IMOD.Application.Service.VeiculoAnexoService();
        private readonly IMOD.Application.Interfaces.IVeiculoSeguroService objVeiculoSeguroService = new IMOD.Application.Service.VeiculoSeguroService();
        private readonly IMOD.Application.Interfaces.ITipoServicoService objServicosService = new IMOD.Application.Service.TipoServicoService();
        private readonly IMOD.Application.Interfaces.IMunicipioService objMunicipioSevice = new IMOD.Application.Service.MunicipioService();
        //private readonly IMOD.Application.Interfaces.IEmpresaSeguroService objSeguroSevice = new IMOD.Application.Service.EmpresaSeguroService();
        private const string SESS_CONTRATOS_SELECIONADOS = "ContratosSelecionados";
        private const string SESS_CONTRATOS_REMOVIDOS = "ContratosRemovidos";
        private const string SESS_ANEXOS_SELECIONADOS = "AnexosSelecionados";
        private const string SESS_ANEXOS_REMOVIDOS = "AnexosRemovidos";
        //private const string SESS_ARQUIVO_SEGURO = "ArquivoSeguro";
        //private const string SESS_NOME_ARQUIVO_SEGURO = "NomeArquivoSeguro";
        private const string SESS_MUNICIPIO_SELECIONADO = "MunicipioSelecionado";

        private List<Veiculo> veiculos = new List<Veiculo>();
        private List<VeiculoEmpresa> vinculos = new List<VeiculoEmpresa>();

        // GET: Veiculo
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            //var lstVeiculo = objService.Listar(null, null, string.Empty);
            //List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(lstVeiculo);            

            List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(ObterVeiculosEmpresaLogada()).Distinct().ToList();
            ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;

            Session[SESS_CONTRATOS_SELECIONADOS] = null;
            Session[SESS_CONTRATOS_REMOVIDOS] = null;
            Session[SESS_ANEXOS_SELECIONADOS] = null;
            Session[SESS_ANEXOS_REMOVIDOS] = null;
            Session[SESS_MUNICIPIO_SELECIONADO] = null;
            //Session[SESS_ARQUIVO_SEGURO] = null;
            //Session[SESS_NOME_ARQUIVO_SEGURO] = null;

            return View(lstVeiculoMapeado.Distinct());

        }
        private IList<Veiculo> ObterVeiculosEmpresaLogada()
        {
            vinculos = objVeiculoEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId).ToList();
            vinculos.ForEach(v => { veiculos.AddRange(objService.Listar(v.VeiculoId, null, null, null, null, null)); });
            return veiculos.OrderBy(c => c.Descricao).ToList();
        }
        // GET: Veiculo/Details/5
        public ActionResult Details(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o veículo pelo ID
            var veiculoEditado = objService.Listar(id).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            //Propula combo de estado
            PopularEstadosDropDownList();

            //obtém vinculos do veículo
            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Carrega dados Seguro
            //VeiculoSeguro veiculoSeguro = new VeiculoSeguro();
            var veiculoSeguro = objVeiculoSeguroService.Listar(id);

            foreach (var item in veiculoSeguro)
            {
                veiculoMapeado.NomeSeguradora = item.NomeSeguradora;
                veiculoMapeado.NumeroApolice = item.NumeroApolice;
                veiculoMapeado.Emissao = item.Emissao;
                veiculoMapeado.ValorCobertura = Convert.ToDouble(item.ValorCobertura);
                veiculoMapeado.Validade = item.Validade;
                //veiculoMapeado.NomeAnexoApolice = item.NomeArquivo;
            }

            //Popula contratos selecionados
            ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
            var listaVinculosVeiculo = Mapper.Map<List<VeiculoEmpresaViewModel>>(objVeiculoEmpresaService.Listar(veiculoEditado.EquipamentoVeiculoId));
            ViewBag.ContratosSelecionados = listaVinculosVeiculo;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosVeiculo);

            //Preenchie combo municipio de acordo com o estado
            ViewBag.Municipio = new List<Municipio>();
            var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, veiculoMapeado.EstadoId).OrderBy(m => m.Nome);
            if (lstMunicipio != null) { ViewBag.Municipio = lstMunicipio; };

            PopularDadosDropDownList();

            var anexosSelecionados = objVeiculoAnexoService.Listar(veiculoMapeado.EquipamentoVeiculoId);
            if (anexosSelecionados != null)
            {
                //veiculoMapeado.NomeArquivoAnexo = anexosSelecionados.NomeArquivo;
                ViewBag.AnexosSelecionados = anexosSelecionados;
                Session.Add(SESS_ANEXOS_SELECIONADOS, anexosSelecionados);
            }

            return View(veiculoMapeado);
        }

        // GET: Veiculo/Create
        public ActionResult Create()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            PopularDadosDropDownList();

            //carrega os serviços
            var listServicos = objServicosService.Listar().ToList();
            if (listServicos != null && listServicos.Any()) { ViewBag.Servicos = listServicos; }
            //carrega contratos
            if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }

            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
            {
                ViewBag.ContratosSelecionados = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            }
            else
            {
                ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
            }

            if (Session[SESS_ANEXOS_SELECIONADOS] != null)
            {
                ViewBag.AnexosSelecionados = (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<VeiculoAnexo>();
            }

            if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
            {
                ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
            }
            else
            {
                ViewBag.Municipio = new List<Municipio>();
            }

            PopularEstadosDropDownList();

            PopularContratoCreateDropDownList(SessionUsuario.EmpresaLogada.EmpresaId);

            return View();
        }

        // POST: Veiculo/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(VeiculoViewModel model)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            try
            {
                if (model.FileAnexo != null)
                {
                    if (model.FileAnexo.ContentLength > 2048000)
                        ModelState.AddModelError("FileUploadAnexo", "Tamanho permitido de arquivo anexo 2,00 MB");

                }
                if (model.AnexoApolice != null)
                {
                    if (model.AnexoApolice.ContentLength > 2048000)
                        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo (Apólice) 2,00 MB");

                    if (!Path.GetExtension(model.AnexoApolice.FileName).Equals(".pdf"))
                        ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                }

                if (((List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("EmpresaVeiculoId", "Necessário adicionar pelo menos um contrato!");
                }

                if (ModelState.IsValid)
                {
                    var veiculoMapeado = Mapper.Map<Veiculo>(model);
                    veiculoMapeado.Precadastro = true;
                    veiculoMapeado.StatusCadastro = 0;
                    veiculoMapeado.Tipo = "VEÍCULO";

                    objService.Criar(veiculoMapeado);

                    CriarVeiculoSeguro(model, false, veiculoMapeado.EquipamentoVeiculoId);

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objVeiculoEmpresaService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objVeiculoEmpresaService.Remover(contratoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objVeiculoEmpresaService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.VeiculoId = veiculoMapeado.EquipamentoVeiculoId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var veiculoEmpresa = Mapper.Map<VeiculoEmpresa>(vinculo);
                                objVeiculoEmpresaService.Criar(veiculoEmpresa);
                                objVeiculoEmpresaService.CriarNumeroMatricula(veiculoEmpresa);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objVeiculoAnexoService.Listar(veiculoMapeado.EquipamentoVeiculoId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objVeiculoAnexoService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            var veiculoAnexo = objVeiculoAnexoService.Listar(veiculoMapeado.EquipamentoVeiculoId, anexo.VeiculoAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (veiculoAnexo == null)
                            {
                                veiculoAnexo = new VeiculoAnexo();
                                veiculoAnexo.VeiculoId = veiculoMapeado.EquipamentoVeiculoId;
                                veiculoAnexo.VeiculoAnexoId = anexo.VeiculoAnexoId;
                                veiculoAnexo.Descricao = anexo.Descricao;
                                veiculoAnexo.NomeArquivo = anexo.NomeArquivo;
                                veiculoAnexo.Arquivo = anexo.Arquivo;
                                objVeiculoAnexoService.Criar(veiculoAnexo);
                            }
                        }
                    }

                    //CriarVeiculoAnexo(model, veiculoMapeado.EquipamentoVeiculoId);

                    Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                    Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                    Session.Remove(SESS_ANEXOS_SELECIONADOS);
                    Session.Remove(SESS_ANEXOS_REMOVIDOS);
                    //Session.Remove(SESS_ARQUIVO_SEGURO);
                    //Session.Remove(SESS_NOME_ARQUIVO_SEGURO);


                    return RedirectToAction("Index", "Veiculo");
                }
                PopularDadosDropDownList();

                // Se ocorrer um erro retorna para pagina
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
                //carrega os serviços
                var listServicos = objServicosService.Listar().ToList();
                if (listServicos != null && listServicos.Any()) { ViewBag.Servicos = listServicos; }

                //var listEmpresaSeguros = objSeguroSevice.Listar(null, null, null, null, null, null, model.EmpresaSeguroId).FirstOrDefault();
                //model.NomeAnexoApolice = listEmpresaSeguros.NomeArquivo;

                PopularEstadosDropDownList();
                

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                {
                    ViewBag.ContratosSelecionados = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
                }

                if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                {
                    ViewBag.AnexosSelecionados = (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.AnexosSelecionados = new List<VeiculoAnexo>();
                }

                if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
                {
                    ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
                }
                else
                {
                    ViewBag.Municipio = new List<Municipio>();
                }

                ShowListaErros();

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar registro");
                return View();
            }
        }

        // GET: Veiculo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

            var veiculoEditado = objService.Listar(id, null, null, null, null, null).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            PopularDadosDropDownList();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Carrega dados Seguro
            var veiculoSeguro = objVeiculoSeguroService.Listar(id);
            foreach (var item in veiculoSeguro)
            {
                veiculoMapeado.EmpresaSeguroId = item.EmpresaSeguroid;
                veiculoMapeado.VeiculoSeguroId = item.VeiculoSeguroId;
                veiculoMapeado.NomeSeguradora = item.NomeSeguradora;
                veiculoMapeado.NumeroApolice = item.NumeroApolice;
                veiculoMapeado.Emissao = item.Emissao;
                veiculoMapeado.ValorCobertura = Convert.ToDouble(item.ValorCobertura);
                veiculoMapeado.Validade = item.Validade;
                //veiculoMapeado.NomeAnexoApolice = item.NomeArquivo;
            }

            //Popula combo de estado
            PopularEstadosDropDownList();

            PopularMunicipiosDropDownList(veiculoMapeado.EstadoId.ToString());

            //Popula contratos selecionados
            ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
            var listaVinculosVeiculo = Mapper.Map<List<VeiculoEmpresaViewModel>>(objVeiculoEmpresaService.Listar(veiculoEditado.EquipamentoVeiculoId));
            ViewBag.ContratosSelecionados = listaVinculosVeiculo;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosVeiculo);
            veiculoMapeado.chkAreaManobra = listaVinculosVeiculo[0].AreaManobra;

            PopularContratoEditDropDownList(veiculoMapeado, SessionUsuario.EmpresaLogada.EmpresaId);

            //Popula anexos do veiculo
            var anexosSelecionados = objVeiculoAnexoService.Listar(veiculoMapeado.EquipamentoVeiculoId);
            if (anexosSelecionados != null)
            {
                ViewBag.AnexosSelecionados = anexosSelecionados;
                Session.Add(SESS_ANEXOS_SELECIONADOS, anexosSelecionados);
            }

            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
            {
                ViewBag.ContratosSelecionados = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            }
            else
            {
                ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
            }

            if (Session[SESS_ANEXOS_SELECIONADOS] != null)
            {
                ViewBag.AnexosSelecionados = (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<VeiculoAnexo>();
            }

            if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
            {
                ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
            }
            else
            {
                ViewBag.Municipio = new List<Municipio>();
            }

            return View(veiculoMapeado);
        }

        // POST: Veiculo/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, VeiculoViewModel model)
        {
            try
            {
                if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
                if (id == null)
                    return HttpNotFound();

                if (((List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("ContratoEmpresaId", "Necessário adicionar pelo menos um contrato!");
                }

                if (model.FileAnexo != null)
                {
                    if (model.FileAnexo.ContentLength > 2048000)
                        ModelState.AddModelError("FileAnexo", "Tamanho permitido de arquivo 2,00 MB");

                    if (!Path.GetExtension(model.FileAnexo.FileName).Equals(".pdf"))
                        ModelState.AddModelError("FileAnexo", "Permitida Somente Extensão  .pdf");
                }

                var idVeiculo = id;

                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var veiculoMapeado = Mapper.Map<Veiculo>(model);
                    veiculoMapeado.Precadastro = true;
                    veiculoMapeado.Tipo = "VEÍCULO";
                    veiculoMapeado.Observacao = null;

                    //Aguardando Aprovação
                    veiculoMapeado.StatusCadastro = 1;

                    veiculoMapeado.EquipamentoVeiculoId = Convert.ToInt32(idVeiculo);
                    objService.Alterar(veiculoMapeado);

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var veiculoExclusao = objVeiculoEmpresaService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, item).FirstOrDefault();
                            if (veiculoExclusao != null)
                            {
                                objVeiculoEmpresaService.Remover(veiculoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objVeiculoEmpresaService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.VeiculoEmpresaId = veiculoMapeado.EquipamentoVeiculoId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var veiculoEmpresa = Mapper.Map<VeiculoEmpresa>(vinculo);
                                objVeiculoEmpresaService.Criar(veiculoEmpresa);
                                objVeiculoEmpresaService.CriarNumeroMatricula(veiculoEmpresa);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objVeiculoAnexoService.ListarComAnexo(veiculoMapeado.EquipamentoVeiculoId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objVeiculoAnexoService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            var veiculoAnexo = objVeiculoAnexoService.ListarComAnexo(veiculoMapeado.EquipamentoVeiculoId, anexo.VeiculoAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (veiculoAnexo == null)
                            {
                                veiculoAnexo = new VeiculoAnexo();
                                veiculoAnexo.VeiculoId = veiculoMapeado.EquipamentoVeiculoId;
                                veiculoAnexo.VeiculoAnexoId = anexo.VeiculoAnexoId;
                                veiculoAnexo.Descricao = anexo.Descricao;
                                veiculoAnexo.NomeArquivo = anexo.NomeArquivo;
                                veiculoAnexo.Arquivo = anexo.Arquivo;
                                objVeiculoAnexoService.Criar(veiculoAnexo);
                            }
                        }
                    }

                    CriarVeiculoSeguro(model, true, veiculoMapeado.EquipamentoVeiculoId);

                    //Remove itens da sessão
                    Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                    Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                    Session.Remove(SESS_ANEXOS_SELECIONADOS);
                    Session.Remove(SESS_ANEXOS_REMOVIDOS);
                    //Session.Remove(SESS_ARQUIVO_SEGURO);
                    //Session.Remove(SESS_NOME_ARQUIVO_SEGURO);

                    //if (model.FileAnexo != null)
                    //{
                    //    ExcluirVeiculoAnexoAnterior(model);
                    //}

                    return RedirectToAction("Index");
                }

                // Se ocorrer um erro retorna para pagina
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
                //carrega os serviços
                var listServicos = objServicosService.Listar().ToList();
                if (listServicos != null && listServicos.Any()) { ViewBag.Servicos = listServicos; }

                PopularEstadosDropDownList();
                PopularDadosDropDownList();

                //var listEmpresaSeguros = objSeguroSevice.Listar(null, null, null, null, null, null, model.EmpresaSeguroId).FirstOrDefault();
                //model.NomeAnexoApolice = listEmpresaSeguros.NomeArquivo;

                if (model.EstadoId > 0)
                {
                    PopularMunicipiosDropDownList(model.EstadoId.ToString());
                }

                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                {
                    ViewBag.ContratosSelecionados = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
                }

                if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                {
                    ViewBag.AnexosSelecionados = (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.AnexosSelecionados = new List<VeiculoAnexo>();
                }

                if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
                {
                    ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
                }
                else
                {
                    ViewBag.Municipio = new List<Municipio>();
                }
                


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
        public ActionResult Delete(int id, Veiculo model)
        {

            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

            try
            {
                var idVeiculo = id;

                var veiculoMapeado = Mapper.Map<Veiculo>(model);
                veiculoMapeado.EquipamentoVeiculoId = Convert.ToInt32(idVeiculo);

                //Pega os anexos do colaborador
                var anexosVeiculo = objVeiculoAnexoService.Listar(idVeiculo);
                //exclui os anexos
                foreach (var anexo in anexosVeiculo)
                {
                    objVeiculoAnexoService.Remover(anexo);
                }

                objService.Remover(veiculoMapeado);

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
                var veiculo = objService.Listar(id).FirstOrDefault();

                VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculo);

                return View(veiculoMapeado);
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public JsonResult AdicionarContrato(int id, Boolean flagAreaManobra)
        {
            List<VeiculoEmpresaViewModel> vinculoList = new List<VeiculoEmpresaViewModel>();
            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                vinculoList = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];

            var item = SessionUsuario.EmpresaLogada.Contratos.Where(c => c.EmpresaContratoId == id).FirstOrDefault();
            VeiculoEmpresaViewModel vinculo = new VeiculoEmpresaViewModel();
            vinculo.EmpresaContratoId = id;
            vinculo.Descricao = item.Descricao;
            vinculo.Matricula = " - ";
            vinculo.AreaManobra = flagAreaManobra;
            vinculoList.Add(vinculo);
            Session.Add(SESS_CONTRATOS_SELECIONADOS, vinculoList);

            return Json(vinculoList, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverContrato(int id)
        {
            var listContrato = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            int indice = listContrato.FindIndex(c => c.EmpresaContratoId.Equals(id));
            listContrato.RemoveAt(indice);
            Session[SESS_CONTRATOS_SELECIONADOS] = listContrato;
            if (Session[SESS_CONTRATOS_REMOVIDOS] == null) Session[SESS_CONTRATOS_REMOVIDOS] = new List<int>();
            ((List<int>)Session[SESS_CONTRATOS_REMOVIDOS]).Add(id);
            return Json(listContrato, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //public JsonResult AdicionarServico(int id)
        //{
        //    if (Session[SESS_SERVICOS_SELECIONADOS] == null) Session[SESS_SERVICOS_SELECIONADOS] = new List<TipoServico>();
        //    var item = objServicosService.Listar(id).FirstOrDefault();
        //    ((List<TipoServico>)Session[SESS_SERVICOS_SELECIONADOS]).Add(item);
        //    return Json((List<TipoServico>)Session[SESS_SERVICOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        //}

        //[Authorize]
        //public JsonResult RemoverServico(int id)
        //{
        //    var listServico = (List<TipoServico>)Session[SESS_SERVICOS_SELECIONADOS];
        //    listServico.Remove(new TipoServico(id));
        //    Session[SESS_SERVICOS_SELECIONADOS] = listServico;
        //    if (Session[SESS_SERVICOS_REMOVIDOS] == null) Session[SESS_SERVICOS_REMOVIDOS] = new List<int>();
        //    ((List<int>)Session[SESS_SERVICOS_REMOVIDOS]).Add(id);
        //    return Json(listServico, JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        public ActionResult AdicionarAnexo()
        {
            if (Session[SESS_ANEXOS_SELECIONADOS] == null) Session[SESS_ANEXOS_SELECIONADOS] = new List<VeiculoAnexo>();

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

                VeiculoAnexo item = new VeiculoAnexo();

                item.Arquivo = arquivoBase64;
                item.NomeArquivo = NomeArquivo + ExtensaoArquivo;
                item.Descricao = Request.Form["descricao"];
                ((List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS]).Add(item);
            }
            return Json((List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RemoverAnexo(int id)
        {
            var listAnexo = (List<VeiculoAnexo>)Session[SESS_ANEXOS_SELECIONADOS];

            int indice = listAnexo.FindIndex(c => c.VeiculoAnexoId.Equals(id));
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

        /*[Authorize]
        public ActionResult PopulaSeguro(int id)
        {
            var listEmpresaSeguros = objSeguroSevice.Listar(null, null, null, null, null, null, id).FirstOrDefault();
            Session[SESS_ARQUIVO_SEGURO] = listEmpresaSeguros.Arquivo;
            Session[SESS_NOME_ARQUIVO_SEGURO] = listEmpresaSeguros.NomeArquivo;
            JsonResult jsonResult = Json(listEmpresaSeguros, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }*/

        #region Métodos Internos Carregamento de Componentes

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

            //var lstEmpresaSeguros = objSeguroSevice.Listar(null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId, null, null);
            //ViewBag.EmpresaSeguros = lstEmpresaSeguros;
        }

        private void PopularContratoCreateDropDownList(int idEmpresa)
        {
            if (idEmpresa <= 0) return;

            var contratoEmpresa = objContratosService.Listar(idEmpresa);
            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao");
        }

        private void PopularContratoEditDropDownList(VeiculoViewModel veiculo, int idEmpresa)
        {
            if (idEmpresa <= 0) return;

            var contratoEmpresa = objContratosService.Listar(idEmpresa);
            var resultEmpresasContratosVinculados = objVeiculoEmpresaService.Listar(null, null, null, null, null, idEmpresa);

            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao", resultEmpresasContratosVinculados.Select(m => m.EmpresaContratoId).ToArray());
        }

        //private void CriarVeiculoAnexo(VeiculoViewModel veiculo, int veiculoId = 0)
        //{
        //    byte[] bufferArquivo;
        //    string NomeArquivo;
        //    string ExtensaoArquivo;

        //    if (veiculo == null || veiculoId == 0) return;
        //    if (veiculo.ArquivoAnexo == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        NomeArquivo = Path.GetFileNameWithoutExtension(veiculo.FileAnexo.FileName);
        //        ExtensaoArquivo = Path.GetExtension(veiculo.FileAnexo.FileName);

        //        if (!ExtensaoArquivo.Equals(".pdf")) return;

        //        var arquivoStream = veiculo.FileAnexo.InputStream;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            arquivoStream.CopyTo(ms);
        //            bufferArquivo = ms.ToArray();
        //        }

        //        var arquivoBase64 = Convert.ToBase64String(bufferArquivo);

        //        VeiculoAnexo veiculoAnexo = new VeiculoAnexo();
        //        veiculoAnexo.VeiculoId = veiculoId;
        //        veiculoAnexo.Arquivo = arquivoBase64;
        //        veiculoAnexo.NomeArquivo = NomeArquivo + ExtensaoArquivo;
        //        veiculoAnexo.Descricao = NomeArquivo + ExtensaoArquivo;
        //        objVeiculoAnexoService.Criar(veiculoAnexo);
        //    }
        //}

        private void ExcluirVeiculoAnexoAnterior(VeiculoViewModel veiculo)
        {
            if (!veiculo.Precadastro) return;
            if (veiculo.FileAnexo == null) return;
            if (veiculo == null || veiculo.EquipamentoVeiculoId == 0) return;
            if (veiculo.FileAnexo.ContentLength <= 0 || veiculo.FileAnexo.ContentLength > 2048000) return;

            var objVeiculoAnexo = objVeiculoAnexoService.ListarComAnexo(veiculo.EquipamentoVeiculoId).FirstOrDefault();
            if (objVeiculoAnexo == null) return;

            objVeiculoAnexoService.Remover(objVeiculoAnexo);
        }

        private void CriarVeiculoSeguro(VeiculoViewModel veiculo, bool isEdicao, int veiculoId = 0)
        {
            byte[] bufferArquivo;
            string NomeArquivo = "";
            string ExtensaoArquivo = "";
            var arquivoBase64 = "";
            if (veiculo == null || veiculoId == 0) return;

            if (veiculo.AnexoApolice != null)
            {
                NomeArquivo = Path.GetFileNameWithoutExtension(veiculo.AnexoApolice.FileName);
                ExtensaoArquivo = Path.GetExtension(veiculo.AnexoApolice.FileName);

                var arquivoStream = veiculo.AnexoApolice.InputStream;
                using (MemoryStream ms = new MemoryStream())
                {
                    arquivoStream.CopyTo(ms);
                    bufferArquivo = ms.ToArray();
                }

                arquivoBase64 = Convert.ToBase64String(bufferArquivo);
            }

            /*if (Session[SESS_ARQUIVO_SEGURO] != null)
            {
                //System.Text.Encoding.UTF8.GetBytes(plainText);
                arquivoBase64 = Session[SESS_ARQUIVO_SEGURO].ToString();
                NomeArquivo = Session[SESS_NOME_ARQUIVO_SEGURO].ToString();
            }*/

            if (isEdicao)
            {
                var veiculoSeguros = objVeiculoSeguroService.Listar(veiculoId);

                foreach (var vSeguros in veiculoSeguros)
                {
                    vSeguros.VeiculoSeguroId = veiculo.EmpresaSeguroId;
                    vSeguros.NomeSeguradora = veiculo.NomeSeguradora;
                    vSeguros.NumeroApolice = veiculo.NumeroApolice.ToString();
                    vSeguros.ValorCobertura = Convert.ToDecimal(veiculo.ValorCobertura);
                    vSeguros.Emissao = veiculo.Emissao;
                    vSeguros.Validade = veiculo.Validade;

                    vSeguros.VeiculoId = veiculoId;
                    vSeguros.Arquivo = arquivoBase64;
                    vSeguros.NomeArquivo = NomeArquivo + ExtensaoArquivo;

                    objVeiculoSeguroService.Alterar(vSeguros);
                }
            }
            else
            {
                VeiculoSeguro veiculoSeguro = new VeiculoSeguro();
                veiculoSeguro.VeiculoSeguroId = veiculo.EmpresaSeguroId;
                veiculoSeguro.NomeSeguradora = veiculo.NomeSeguradora;
                veiculoSeguro.NumeroApolice = veiculo.NumeroApolice.ToString();
                veiculoSeguro.ValorCobertura = Convert.ToDecimal(veiculo.ValorCobertura);
                veiculoSeguro.Emissao = veiculo.Emissao;
                veiculoSeguro.Validade = veiculo.Validade;

                veiculoSeguro.VeiculoId = veiculoId;
                veiculoSeguro.Arquivo = arquivoBase64;
                veiculoSeguro.NomeArquivo = NomeArquivo + ExtensaoArquivo;

                objVeiculoSeguroService.Criar(veiculoSeguro);
            }
        }


        public FileResult Download(string id)
        {
            string contentType = "";
            string NomeArquivoAnexo = "";
            string extensao = "";
            string nomeArquivoV = "";
            string pastaTemp = Path.GetTempPath();

            //if (string.IsNullOrEmpty(id));

            int veiculoId = Convert.ToInt32(id);

            var objVeiculoAnexo = objVeiculoAnexoService.ListarComAnexo(veiculoId).FirstOrDefault();
            NomeArquivoAnexo = objVeiculoAnexo.NomeArquivo;
            extensao = Path.GetExtension(NomeArquivoAnexo);
            nomeArquivoV = Path.GetFileNameWithoutExtension(NomeArquivoAnexo);

            var arrayArquivo = Convert.FromBase64String(objVeiculoAnexo.Arquivo);
            System.IO.File.WriteAllBytes(pastaTemp + NomeArquivoAnexo, arrayArquivo);

            if (extensao.Equals(".pdf"))
                contentType = "application/pdf";

            return File(pastaTemp + NomeArquivoAnexo, contentType, nomeArquivoV + extensao);
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
