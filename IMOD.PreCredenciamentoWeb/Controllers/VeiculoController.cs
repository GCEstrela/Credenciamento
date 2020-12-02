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
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Enums;

namespace IMOD.PreCredenciamentoWeb.Controllers
{
    public class VeiculoController : Controller
    {
        // GET: Veículo
        private readonly IVeiculoService objService = new VeiculoService();
        private readonly IDadosAuxiliaresFacade objAuxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService objContratosService = new EmpresaContratoService();
        private readonly IVeiculoEmpresaService objVeiculoEmpresaService = new VeiculoEmpresaService();
        private readonly IVeiculoAnexoService objVeiculoAnexoService = new VeiculoAnexoService();
        private readonly IVeiculoSeguroService objVeiculoSeguroService = new VeiculoSeguroService();
        private readonly ITipoServicoService objServicosService = new TipoServicoService();
        private readonly IMunicipioService objMunicipioSevice = new MunicipioService();
        private readonly IVeiculoObservacaoService objObservacaoService = new VeiculoObservacaoService();
        //Web
        private readonly IVeiculoWebService objWebService = new VeiculoWebService();
        private readonly IVeiculoempresaWebService objVeiculoEmpresaWebService = new VeiculoEmpresaWebService();
        private readonly IVeiculoAnexoWebService objVeiculoAnexoWebService = new VeiculoAnexoWebService();
        private readonly IVeiculoSeguroWebService objVeiculoSeguroWebService = new VeiculoSeguroWebService();
        //private readonly IMOD.Application.Interfaces.IEmpresaSeguroService objSeguroSevice = new IMOD.Application.Service.EmpresaSeguroService();
        private const string SESS_CONTRATOS_SELECIONADOS = "ContratosSelecionados";
        private const string SESS_CONTRATOS_REMOVIDOS = "ContratosRemovidos";
        private const string SESS_ANEXOS_SELECIONADOS = "AnexosSelecionados";
        private const string SESS_ANEXOS_REMOVIDOS = "AnexosRemovidos";
        //private const string SESS_ARQUIVO_SEGURO = "ArquivoSeguro";
        //private const string SESS_NOME_ARQUIVO_SEGURO = "NomeArquivoSeguro";
        private const string SESS_MUNICIPIO_SELECIONADO = "MunicipioSelecionado";
        private const string SESS_FOTO_VEICULO = "Foto";

        private List<Veiculo> veiculos = new List<Veiculo>();
        private List<VeiculoWeb> veiculosWeb = new List<VeiculoWeb>();
        private List<VeiculoEmpresa> vinculos = new List<VeiculoEmpresa>();

        // GET: Veiculo
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            //var lstVeiculo = objService.Listar(null, null, string.Empty);
            //List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(lstVeiculo);            

            List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(ObterVeiculosEmpresaLogada());
            //ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;

            Session[SESS_CONTRATOS_SELECIONADOS] = null;
            Session[SESS_CONTRATOS_REMOVIDOS] = null;
            Session[SESS_ANEXOS_SELECIONADOS] = null;
            Session[SESS_ANEXOS_REMOVIDOS] = null;
            Session[SESS_MUNICIPIO_SELECIONADO] = null;
            //Session[SESS_ARQUIVO_SEGURO] = null;
            //Session[SESS_NOME_ARQUIVO_SEGURO] = null;

            return View(lstVeiculoMapeado);

        }

        private IList<Veiculo> ObterVeiculosEmpresaLogada()
        {
            var listaVeiculos = objService.Listar(null, null, null, null);
            var listaVeiculosVinculos = objVeiculoEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId, null);
            var listaIdsVinculos = listaVeiculosVinculos.Select(v => v.VeiculoId).ToList<int>();
            veiculos = listaVeiculos.Where(v => listaIdsVinculos.Contains(v.EquipamentoVeiculoId)).ToList();

            var listaVeiculosWeb = objWebService.Listar(null, null, null, null);
            var listaVeiculosVinculosWeb = objVeiculoEmpresaWebService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId, null);
            var listaIdsVinculosWeb = listaVeiculosVinculosWeb.Select(v => v.VeiculoId).ToList<int>();
            veiculosWeb = listaVeiculosWeb.Where(v => listaIdsVinculosWeb.Contains(v.EquipamentoVeiculoId)).ToList();

            veiculos = veiculos.Except(veiculosWeb).ToList();
            veiculos.AddRange(veiculosWeb);

            return veiculos.OrderBy(v => v.SerieChassi).ToList();
        }
        
        // GET: Veiculo/Details/5
        public ActionResult Details(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o veículo pelo ID
            var veiculoEditado = objWebService.Listar(id).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);
            veiculoMapeado = ExibirInfoVeiculoGET(veiculoMapeado);

            return View(veiculoMapeado);
        }

        // GET: Veiculo/Details/5
        public ActionResult DetailsRevisao(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            //Obtem o veículo pelo ID
            var veiculoEditado = objService.Listar(id).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);
            veiculoMapeado = ExibirInfoVeiculoGET(veiculoMapeado);

            return View(veiculoMapeado);
        }

        // GET: Veiculo/Create
        public ActionResult Create()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            
            PopularEstadosDropDownList();
            PopularDadosDropDownList();

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
                ViewBag.AnexosSelecionados = (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<VeiculoAnexoViewModel>();
            }

            if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
            {
                ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
            }
            else
            {
                ViewBag.Municipio = new List<Municipio>();
            }

            return View();
        }

        // POST: Veiculo/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(VeiculoViewModel model)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            if (model.FotoVeiculo != null)
            {
                OnSelecionaFoto_Click(model.FotoVeiculo, model);
            }

            try
            {
                if (model.FileAnexo != null)
                {
                    if (model.FileAnexo.ContentLength > 2048000)
                        ModelState.AddModelError("FileUploadAnexo", "Tamanho permitido de arquivo anexo 2,00 MB");
                }

                if (model.VeiculoSeguro.SeguroArquivo != null)
                {
                    if (model.VeiculoSeguro.SeguroArquivo.ContentLength > 2048000)
                        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo (Apólice) 2,00 MB");

                    if (!Path.GetExtension(model.VeiculoSeguro.SeguroArquivo.FileName).Equals(".pdf"))
                        ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                }

                if (((List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
                {
                    ModelState.AddModelError("EmpresaVeiculoId", "Necessário adicionar pelo menos um contrato!");
                }

                if (ModelState.IsValid)
                {
                    var veiculoMapeado = Mapper.Map<VeiculoWeb>(model);

                    objWebService.Criar(veiculoMapeado);
                    veiculoMapeado.EquipamentoVeiculoId = veiculoMapeado.EquipamentoVeiculoWebId;
                    objWebService.Alterar(veiculoMapeado);

                    CriarVeiculoSeguro(model, false, veiculoMapeado.EquipamentoVeiculoId);

                    // excluir os contratos removidos da lista
                    if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                        {
                            var contratoExclusao = objVeiculoEmpresaWebService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, item).FirstOrDefault();
                            if (contratoExclusao != null)
                            {
                                objVeiculoEmpresaWebService.Remover(contratoExclusao);
                            }
                        }
                    }

                    //inclui os contratos selecionados
                    if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                    {
                        foreach (var vinculo in (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objVeiculoEmpresaWebService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.VeiculoId = veiculoMapeado.EquipamentoVeiculoId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var veiculoEmpresa = Mapper.Map<VeiculoEmpresaWeb>(vinculo);
                                objVeiculoEmpresaWebService.Criar(veiculoEmpresa);

                                veiculoEmpresa.VeiculoEmpresaId = veiculoEmpresa.VeiculoEmpresaWebId;
                                objVeiculoEmpresaWebService.Alterar(veiculoEmpresa);
                            }
                        }
                    }

                    // excluir os anexos removidos da lista
                    if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                    {
                        foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                        {
                            var anexoExclusao = objVeiculoAnexoWebService.Listar(veiculoMapeado.EquipamentoVeiculoId, item, null, null, null, null, null).FirstOrDefault();
                            if (anexoExclusao != null)
                            {
                                objVeiculoAnexoWebService.Remover(anexoExclusao);
                            }
                        }
                    }

                    //inclui os anexos selecionados
                    if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                    {
                        foreach (var anexo in (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS])
                        {
                            var veiculoAnexo = objVeiculoAnexoWebService.Listar(veiculoMapeado.EquipamentoVeiculoId, anexo.VeiculoAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (veiculoAnexo == null)
                            {
                                anexo.VeiculoId = veiculoMapeado.EquipamentoVeiculoId;
                                veiculoAnexo = Mapper.Map<VeiculoAnexoWeb>(anexo);
                                objVeiculoAnexoWebService.Criar(veiculoAnexo);

                                veiculoAnexo.VeiculoAnexoId = veiculoAnexo.VeiculoAnexoWebId;
                                objVeiculoAnexoWebService.Alterar(veiculoAnexo);
                            }
                        }
                    }

                    Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                    Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                    Session.Remove(SESS_ANEXOS_SELECIONADOS);
                    Session.Remove(SESS_ANEXOS_REMOVIDOS);
                    Session.Remove(SESS_FOTO_VEICULO);
                    //Session.Remove(SESS_ARQUIVO_SEGURO);
                    //Session.Remove(SESS_NOME_ARQUIVO_SEGURO);


                    return RedirectToAction("Index", "Veiculo");
                }

                PopularObservacoesColaborador(model);

                PopularDadosDropDownList();

                // Se ocorrer um erro retorna para pagina
                if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
                //carrega os serviços
                var listServicos = objServicosService.Listar().ToList();
                if (listServicos != null && listServicos.Any()) { ViewBag.Servicos = listServicos; }

                //var listEmpresaSeguros = objSeguroSevice.Listar(null, null, null, null, null, null, model.EmpresaSeguroId).FirstOrDefault();
                //model.NomeAnexoApolice = listEmpresaSeguros.NomeArquivo;

                PopularEstadosDropDownList();
                GetFotoOnErro(model);

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
                    ViewBag.AnexosSelecionados = (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS];
                }
                else
                {
                    ViewBag.AnexosSelecionados = new List<VeiculoAnexoViewModel>();
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

            var veiculoEditado = objWebService.Listar(id).FirstOrDefault();

            if (veiculoEditado == null)
                return HttpNotFound();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);
            veiculoMapeado = ExibirInfoVeiculoGET(veiculoMapeado);

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
                
                return EditarVeiculo(model);  
            }
            catch (Exception ex)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Veiculo/EditRevisao/{id}
        public ActionResult EditRevisao(int? id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

            var veiculoEditado = objService.Listar(id).FirstOrDefault();

            if (veiculoEditado == null)
                return HttpNotFound();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);
            veiculoMapeado = ExibirInfoVeiculoGET(veiculoMapeado);

            return View(veiculoMapeado);
        }

        // POST: Veiculo/Edit/5
        [HttpPost]
        public ActionResult EditRevisao(int? id, VeiculoViewModel model)
        {
            try
            {
                if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
                if (id == null)
                    return HttpNotFound();
                
                return EditarVeiculo(model);  
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
        public ActionResult Delete(int id, VeiculoViewModel model)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

            try
            {
                var idVeiculo = id;

                var veiculoMapeado = Mapper.Map<VeiculoWeb>(model);
                veiculoMapeado.EquipamentoVeiculoId = Convert.ToInt32(idVeiculo);

                objWebService.Remover(veiculoMapeado);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public ActionResult Delete(int? id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

            if (id == null)
                return HttpNotFound();

            var veiculo = objWebService.Listar(id).FirstOrDefault();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculo);

            return View(veiculoMapeado);
        }

        [Authorize]
        public JsonResult AdicionarContrato(int id, Boolean flagAreaManobra)
        {
            List<VeiculoEmpresaViewModel> vinculoList = new List<VeiculoEmpresaViewModel>();
            if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                vinculoList = (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS];
            try
            {


                foreach (var listContrato in vinculoList)
                {
                    if (listContrato.EmpresaContratoId == Convert.ToInt32(Request.Form["id"]))
                        throw new Exception("contrato_ja_associado_ao_veiculo");
                }

                var item = SessionUsuario.EmpresaLogada.Contratos.Where(c => c.EmpresaContratoId == id).FirstOrDefault();
                VeiculoEmpresaViewModel vinculo = new VeiculoEmpresaViewModel();
                vinculo.EmpresaContratoId = id;
                vinculo.Descricao = item.Descricao;
                vinculo.Matricula = " - ";
                vinculo.AreaManobra = flagAreaManobra;
                vinculoList.Add(vinculo);
                Session.Add(SESS_CONTRATOS_SELECIONADOS, vinculoList);

                return Json(vinculoList, JsonRequestBehavior.AllowGet);
            } catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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
            if (Session[SESS_ANEXOS_SELECIONADOS] == null) Session[SESS_ANEXOS_SELECIONADOS] = new List<VeiculoAnexoViewModel>();

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

                VeiculoAnexoViewModel item = new VeiculoAnexoViewModel();

                item.Arquivo = arquivoBase64;
                item.NomeArquivo = NomeArquivo + ExtensaoArquivo;
                item.Descricao = Request.Form["descricao"];
                ((List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS]).Add(item);
            }
            return Json((List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RemoverAnexo(int id)
        {
            var listAnexo = (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS];

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

        [Authorize]
        public ActionResult AdicionarObservacao()
        {
            var Observacoes = new List<VeiculoObservacaoViewModel>();
            var VeiculoId = Convert.ToInt32(Request.Form["EquipamentoVeiculoID"]);
            
            VeiculoObservacaoViewModel obs = new VeiculoObservacaoViewModel()
            {
                Observacao = Request.Form["ObservacaoAprovacao"],
                DataRevisao = DateTime.Now,
                UsuarioRevisao = (int)UsuarioRevisao.CADASTRO_WEB,
                VeiculoId = VeiculoId
            }; 

            try
            {
                var veicObs = Mapper.Map<VeiculoObservacao>(obs);
                objObservacaoService.Criar(veicObs);
                Observacoes = Mapper.Map<List<VeiculoObservacaoViewModel>>(objObservacaoService.Listar(VeiculoId, veicObs.VeiculoObservacaoId));
            }
            catch (Exception ex)
            {
            }

            return Json(Observacoes, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AdicionarObservacaoResposta()
        {
            var repostasObservacao = new List<VeiculoObservacaoViewModel>();

            var VeiculoObservacaoId = Convert.ToInt32(Request.Form["VeiculoObservacaoId"]);
            var VeiculoId = Convert.ToInt32(Request.Form["EquipamentoVeiculoID"]);

            var repostaObservacao = objObservacaoService.Listar(VeiculoId, VeiculoObservacaoId).Where(ver => ver.VeiculoObservacaoRespostaId == null).FirstOrDefault();

            VeiculoObservacaoViewModel obs = new VeiculoObservacaoViewModel();

            obs = Mapper.Map<VeiculoObservacaoViewModel>(repostaObservacao);
            obs.ObservacaoResposta = Request.Form["ObservacaoResposta"];
            obs.DataRevisao = DateTime.Now;
            obs.VeiculoObservacaoRespostaId = VeiculoObservacaoId;
            obs.UsuarioRevisao = (int)UsuarioRevisao.CADASTRO_WEB;

            try
            {
                objObservacaoService.Criar(Mapper.Map<VeiculoObservacao>(obs));
                repostasObservacao = Mapper.Map<List<VeiculoObservacaoViewModel>>(objObservacaoService.Listar(VeiculoId, null, null, VeiculoObservacaoId).ToList());
            }
            catch (Exception ex)
            {
            }

            return Json(repostasObservacao, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RemoverObservacao(int id, int veiculoId)
        {
            var status = "";
            var observacaoExclusao = objObservacaoService.Listar(veiculoId, id).FirstOrDefault();
            if (observacaoExclusao != null)
            {
                var observacaoRespostaId = id;

                try
                {
                    objObservacaoService.Remover(observacaoExclusao);
                    status = id.ToString();
                }
                catch (Exception ex)
                {
                    status = ex.ToString();
                }
            }

            return Json(new { resultItemRemovido = status }, JsonRequestBehavior.AllowGet);
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
            //ViewBag.TipoVeiculo = new SelectList(new object[]
            //    {
            //        new {Name = "VEICULO", Value = "1"},
            //        new {Name = "EQUIPAMENTO", Value = "2"}
            //    }, "Value", "Name");

            //var lstTipoServico = objAuxiliaresService.TipoServico.Listar();
            //ViewBag.TipoServicos = lstTipoServico;

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

            ICollection<EmpresaContrato> contratoEmpresa = objContratosService.Listar(idEmpresa);
            var resultEmpresasContratosVinculados = Mapper.Map<List<VeiculoEmpresaViewModel>>(objVeiculoEmpresaService.Listar(null, null, null, null, null, idEmpresa));

            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao", resultEmpresasContratosVinculados.Select(m => m.EmpresaContratoId).ToArray());
        }

        private void PopularObservacoesColaborador(VeiculoViewModel veiculoMapeado)
        {
            var observacoesAuxSelecionadas = objObservacaoService.Listar(veiculoMapeado.EquipamentoVeiculoId);
            var observacoesAuxSelecionadasView = Mapper.Map<List<VeiculoObservacaoViewModel>>(observacoesAuxSelecionadas);
            var observacoesSelecionadas = observacoesAuxSelecionadasView.Where(ve => ve.VeiculoObservacaoRespostaId == null).OrderBy(co => co.DataRevisao).ToList();
            var observacoesRespostaSelecionadas = observacoesAuxSelecionadasView.Where(ver => ver.VeiculoObservacaoRespostaId != null).OrderBy(cor => cor.DataRevisao).ToList();

            if (observacoesSelecionadas != null)
            {
                ViewBag.ObservacoesSelecionadas = observacoesSelecionadas;
            }

            if (observacoesRespostaSelecionadas != null)
            {
                ViewBag.ObservacoesRespostaSelecionadas = observacoesRespostaSelecionadas;
            }
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
            string NomeArquivo = "";
            var arquivoBase64 = "";

            if (veiculo == null || veiculoId == 0) return;

            if (veiculo.VeiculoSeguro.SeguroArquivo != null)
            {
                NomeArquivo = veiculo.VeiculoSeguro.SeguroArquivo.FileName;
                arquivoBase64 = ConverterArquivo.Base64(veiculo.VeiculoSeguro.SeguroArquivo);
            }

            if (isEdicao)
            {
                if (veiculo.Precadastro)
                {
                    var veiculoSeguro = objVeiculoSeguroWebService.Listar(veiculoId).FirstOrDefault();

                    if(veiculoSeguro != null)
                    {
                        var veiculoSeguroView = Mapper.Map<VeiculoSeguroViewModel>(veiculoSeguro);
                        var veiculoSeguroWeb = AssociarDadosVeiculoSeguro(veiculo, veiculoSeguroView, arquivoBase64, NomeArquivo);

                        objVeiculoSeguroWebService.Alterar(veiculoSeguroWeb);
                    }
                } else
                {
                    var veiculoSeguro = objVeiculoSeguroService.Listar(veiculoId).FirstOrDefault();
                    var veicSeguroWeb = objVeiculoSeguroWebService.Listar(veiculoId).FirstOrDefault();

                    if(veicSeguroWeb == null)
                    {
                        var veiculoSeguroView = Mapper.Map<VeiculoSeguroViewModel>(veiculoSeguro);
                        var veiculoSeguroWeb = AssociarDadosVeiculoSeguro(veiculo, veiculoSeguroView, arquivoBase64, NomeArquivo);

                        objVeiculoSeguroWebService.Criar(veiculoSeguroWeb);
                    }
                    else
                    {
                        var veiculoSeguroView = Mapper.Map<VeiculoSeguroViewModel>(veicSeguroWeb);
                        var veiculoSeguroWeb = AssociarDadosVeiculoSeguro(veiculo, veiculoSeguroView, arquivoBase64, NomeArquivo);

                        objVeiculoSeguroWebService.Alterar(veiculoSeguroWeb);
                    }
                }
            }
            else
            {
                veiculo.VeiculoSeguro.Arquivo = arquivoBase64;
                veiculo.VeiculoSeguro.NomeArquivo = NomeArquivo;
                veiculo.VeiculoSeguro.VeiculoId = veiculoId;

                VeiculoSeguroWeb veicSeguro = Mapper.Map<VeiculoSeguroWeb>(veiculo.VeiculoSeguro);
                objVeiculoSeguroWebService.Criar(veicSeguro);
                veicSeguro.VeiculoSeguroId = veicSeguro.VeiculoSeguroWebId;
                objVeiculoSeguroWebService.Alterar(veicSeguro);
            }
        }

        private VeiculoSeguroWeb AssociarDadosVeiculoSeguro(VeiculoViewModel veiculo, VeiculoSeguroViewModel veiculoSeguro,string arquivoBase64, string NomeArquivo)
        {
            if (veiculo.VeiculoSeguro.SeguroArquivo == null)
            {
                arquivoBase64 = veiculoSeguro.Arquivo;
                NomeArquivo = veiculoSeguro.NomeArquivo;
            }
            veiculo.VeiculoSeguro.NomeArquivo = NomeArquivo;
            veiculo.VeiculoSeguro.Arquivo = arquivoBase64;

            var veiculoSeguroWeb = Mapper.Map<VeiculoSeguroWeb>(veiculo.VeiculoSeguro);

            return veiculoSeguroWeb;
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

        [NonAction]
        public ActionResult EditarVeiculo(VeiculoViewModel veiculo)
        {
            if (((List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS]) == null)
            {
                ModelState.AddModelError("ContratoEmpresaId", "Necessário adicionar pelo menos um contrato!");
            }

            if (veiculo.FileAnexo != null)
            {
                if (veiculo.FileAnexo.ContentLength > 2048000)
                    ModelState.AddModelError("FileAnexo", "Tamanho permitido de arquivo 2,00 MB");

                if (!Path.GetExtension(veiculo.FileAnexo.FileName).Equals(".pdf"))
                    ModelState.AddModelError("FileAnexo", "Permitida Somente Extensão  .pdf");
            }

            if (veiculo.VeiculoSeguro.SeguroArquivo != null)
            {
                if (veiculo.VeiculoSeguro.SeguroArquivo.ContentLength > 2048000)
                    ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo (Apólice) 2,00 MB");

                if (!Path.GetExtension(veiculo.VeiculoSeguro.SeguroArquivo.FileName).Equals(".pdf"))
                    ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
            }

            if (veiculo.FotoVeiculo != null)
            {
                OnSelecionaFoto_Click(veiculo.FotoVeiculo, veiculo);
            }
            if (String.IsNullOrEmpty(veiculo.Foto))
            {
                veiculo.Foto = (string)Session[SESS_FOTO_VEICULO];
            }

            // TODO: Add update logic here
            if (ModelState.IsValid)
            {
                if (veiculo.Precadastro)
                {
                    veiculo.EquipamentoVeiculoWebId = veiculo.EquipamentoVeiculoId;
                    var veiculoMapeado = Mapper.Map<VeiculoWeb>(veiculo);
                    objWebService.Alterar(veiculoMapeado);
                }
                else
                {
                    var veiculoMapeado = objService.Listar(veiculo.EquipamentoVeiculoId).FirstOrDefault();
                    veiculoMapeado.StatusCadastro = (int)StatusCadastro.AGUARDANDO_REVISAO;
                    objService.Alterar(veiculoMapeado);

                    var veiculoWebListar = objWebService.Listar(veiculo.EquipamentoVeiculoId).FirstOrDefault();
                    veiculo.StatusCadastro = (int)StatusCadastro.AGUARDANDO_REVISAO;

                    if (veiculoWebListar == null)
                    {
                        var veiculoMapeadoWeb = Mapper.Map<VeiculoWeb>(veiculo);
                        objWebService.Criar(veiculoMapeadoWeb);
                    }
                    else
                    {
                        var veiculoMapeadoWeb = Mapper.Map<VeiculoWeb>(veiculo);
                        veiculoMapeadoWeb.EquipamentoVeiculoWebId = veiculoWebListar.EquipamentoVeiculoWebId;
                        objWebService.Alterar(veiculoMapeadoWeb);
                    }
                }

                CriarVeiculoSeguro(veiculo, true, veiculo.EquipamentoVeiculoId);

                //inclui os contratos selecionados
                if (Session[SESS_CONTRATOS_SELECIONADOS] != null)
                {
                    foreach (var vinculo in (List<VeiculoEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                    {
                        vinculo.VeiculoId = veiculo.EquipamentoVeiculoId;
                        vinculo.Ativo = true;
                        if(vinculo.EmpresaId == 0)
                            vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;

                        if (veiculo.Precadastro)
                        {
                            // Inclusão do vinculo Caso seja Pre Cadastro
                            var item = objVeiculoEmpresaWebService.Listar(veiculo.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                var veiculoEmpresa = Mapper.Map<VeiculoEmpresaWeb>(vinculo);
                                objVeiculoEmpresaWebService.Criar(veiculoEmpresa);
                                veiculoEmpresa.VeiculoEmpresaId = veiculoEmpresa.VeiculoEmpresaWebId;
                                objVeiculoEmpresaWebService.Alterar(veiculoEmpresa);
                            }
                        }
                        else
                        {
                            // Inclusao de Vinculo
                            var item = objVeiculoEmpresaService.Listar(veiculo.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            var itemWeb = objVeiculoEmpresaWebService.Listar(veiculo.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();

                            if (item == null)
                            {
                                if (itemWeb == null)
                                {
                                    var veiculoEmpresa = Mapper.Map<VeiculoEmpresaWeb>(vinculo);
                                    objVeiculoEmpresaWebService.Criar(veiculoEmpresa);
                                    veiculoEmpresa.VeiculoEmpresaId = veiculoEmpresa.VeiculoEmpresaWebId;
                                    objVeiculoEmpresaWebService.Alterar(veiculoEmpresa);
                                }
                            }
                            else
                            {
                                // Inseri os já existentes e que não form inseridos ainda na tabela web
                                if (itemWeb == null)
                                {
                                    var veiculoEmpresa = Mapper.Map<VeiculoEmpresaViewModel>(item);
                                    var veiculoEmpresaWeb = Mapper.Map<VeiculoEmpresaWeb>(veiculoEmpresa);
                                    objVeiculoEmpresaWebService.Criar(veiculoEmpresaWeb);
                                }
                            }
                        }
                    }
                }
                
                // excluir os contratos removidos da lista
                if (Session[SESS_CONTRATOS_REMOVIDOS] != null)
                {
                    foreach (var item in (List<int>)Session[SESS_CONTRATOS_REMOVIDOS])
                    {
                        var veiculoExclusao = objVeiculoEmpresaService.Listar(veiculo.EquipamentoVeiculoId, null, null, null, null, null, item).FirstOrDefault();
                        var veiculoExclusaoWeb = objVeiculoEmpresaWebService.Listar(veiculo.EquipamentoVeiculoId, null, null, null, null, null, item).FirstOrDefault();

                        if (veiculoExclusao != null)
                        {
                            objVeiculoEmpresaService.Remover(veiculoExclusao);
                        }
                        if (veiculoExclusaoWeb != null)
                        {
                            objVeiculoEmpresaWebService.Remover(veiculoExclusaoWeb);
                        }
                    }
                }

                //inclui os anexos selecionados
                if (Session[SESS_ANEXOS_SELECIONADOS] != null)
                {
                    foreach (var anexo in (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS])
                    {
                        // Parametro com valor 0 não é utilizado como item de condicao.
                        if (anexo.VeiculoAnexoId == 0)
                            anexo.VeiculoAnexoId = -1;

                        if (veiculo.Precadastro)
                        {
                            var item = objVeiculoAnexoWebService.Listar(veiculo.EquipamentoVeiculoId, anexo.VeiculoAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            if (item == null)
                            {
                                anexo.VeiculoId = veiculo.EquipamentoVeiculoId;
                                var veiculoAnexo = Mapper.Map<VeiculoAnexoWeb>(anexo);
                                objVeiculoAnexoWebService.Criar(veiculoAnexo);
                                veiculoAnexo.VeiculoAnexoId = veiculoAnexo.VeiculoAnexoWebId;
                                objVeiculoAnexoWebService.Alterar(veiculoAnexo);
                            }
                        }
                        else
                        {
                            var item = objVeiculoAnexoService.ListarComAnexo(veiculo.EquipamentoVeiculoId, anexo.VeiculoAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();
                            var itemWeb = objVeiculoAnexoWebService.Listar(veiculo.EquipamentoVeiculoId, anexo.VeiculoAnexoId, anexo.NomeArquivo, null, null, null, null).FirstOrDefault();

                            if (item == null)
                            {
                                if (itemWeb == null)
                                {
                                    var veiculoAnexo = Mapper.Map<VeiculoAnexoWeb>(anexo);
                                    objVeiculoAnexoWebService.Criar(veiculoAnexo);
                                }
                            }
                            else
                            {
                                if (itemWeb == null)
                                {
                                    var veiculoAnexo = Mapper.Map<VeiculoAnexoViewModel>(item);
                                    var veiculoAnexoWeb = Mapper.Map<VeiculoAnexoWeb>(veiculoAnexo);
                                    objVeiculoAnexoWebService.Criar(veiculoAnexoWeb);
                                }
                            }
                        }
                    }
                }

                // excluir os anexos removidos da lista
                if (Session[SESS_ANEXOS_REMOVIDOS] != null)
                {
                    foreach (var item in (List<int>)Session[SESS_ANEXOS_REMOVIDOS])
                    {
                        var anexoExclusaoWeb = objVeiculoAnexoWebService.Listar(veiculo.EquipamentoVeiculoId, item, null, null, null, null, null).FirstOrDefault();
                        var anexoExclusao = objVeiculoAnexoService.ListarComAnexo(veiculo.EquipamentoVeiculoId, item, null, null, null, null, null).FirstOrDefault();
                        
                        if (anexoExclusaoWeb != null)
                        {
                            objVeiculoAnexoWebService.Remover(anexoExclusaoWeb);
                        }

                        if (anexoExclusao != null)
                        {
                            objVeiculoAnexoService.Remover(anexoExclusao);
                        }
                    }
                }

                //Remove itens da sessão
                Session.Remove(SESS_CONTRATOS_SELECIONADOS);
                Session.Remove(SESS_CONTRATOS_REMOVIDOS);
                Session.Remove(SESS_ANEXOS_SELECIONADOS);
                Session.Remove(SESS_ANEXOS_REMOVIDOS);
                Session.Remove(SESS_FOTO_VEICULO);

                return RedirectToAction("Index");
            }

            // Se ocorrer um erro retorna para pagina
            if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
            //carrega os serviços
            var listServicos = objServicosService.Listar().ToList();
            if (listServicos != null && listServicos.Any()) { ViewBag.Servicos = listServicos; }

            PopularObservacoesColaborador(veiculo);
            PopularEstadosDropDownList();
            PopularDadosDropDownList();
            GetFotoOnErro(veiculo);
            //var listEmpresaSeguros = objSeguroSevice.Listar(null, null, null, null, null, null, model.EmpresaSeguroId).FirstOrDefault();
            //model.NomeAnexoApolice = listEmpresaSeguros.NomeArquivo;

            if (veiculo.EstadoId > 0)
            {
                PopularMunicipiosDropDownList(veiculo.EstadoId.ToString());
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
                ViewBag.AnexosSelecionados = (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<VeiculoAnexoViewModel>();
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

            return View(veiculo);
        }

        private VeiculoViewModel ExibirInfoVeiculoGET(VeiculoViewModel veiculoMapeado)
        {
            ICollection<VeiculoAnexoViewModel> objVeiculoAnexo;
            ICollection<VeiculoEmpresaViewModel> objVeiculoEmpresa;
            ICollection<VeiculoSeguroViewModel> objVeiculoSeguro;
            

            if (veiculoMapeado.StatusCadastro != (int)StatusCadastro.APROVADO && veiculoMapeado.StatusCadastro != null)
            {
                if(veiculoMapeado.EquipamentoVeiculoWebId != veiculoMapeado.EquipamentoVeiculoId)
                {
                    var veiculoWeb = objWebService.Listar(veiculoMapeado.EquipamentoVeiculoId).FirstOrDefault();
                    veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoWeb);
                }

                objVeiculoAnexo = Mapper.Map<List<VeiculoAnexoViewModel>>(objVeiculoAnexoWebService.Listar(veiculoMapeado.EquipamentoVeiculoId));
                objVeiculoSeguro = Mapper.Map<List<VeiculoSeguroViewModel>>(objVeiculoSeguroWebService.Listar(veiculoMapeado.EquipamentoVeiculoId));
                objVeiculoEmpresa = Mapper.Map<List<VeiculoEmpresaViewModel>>(objVeiculoEmpresaWebService.Listar(veiculoMapeado.EquipamentoVeiculoId));

                var empresaContratos = objContratosService.Listar();
                objVeiculoEmpresa = objVeiculoEmpresa.Join(empresaContratos, ve => ve.EmpresaContratoId, contrato => contrato.EmpresaContratoId, (empresaContrato, contrato) => { empresaContrato.Descricao = contrato.Descricao; return empresaContrato; }).ToList();
            }
            else
            {
                objVeiculoAnexo = Mapper.Map<List<VeiculoAnexoViewModel>>(objVeiculoAnexoService.Listar(veiculoMapeado.EquipamentoVeiculoId));
                objVeiculoSeguro = Mapper.Map<List<VeiculoSeguroViewModel>>(objVeiculoSeguroService.Listar(veiculoMapeado.EquipamentoVeiculoId));
                objVeiculoEmpresa = Mapper.Map<List<VeiculoEmpresaViewModel>>(objVeiculoEmpresaService.Listar(veiculoMapeado.EquipamentoVeiculoId));
            }

            PopularDadosDropDownList();
            PopularEstadosDropDownList();
            PopularMunicipiosDropDownList(veiculoMapeado.EstadoId.ToString());
            PopularContratoEditDropDownList(veiculoMapeado, SessionUsuario.EmpresaLogada.EmpresaId);
            PopularObservacoesColaborador(veiculoMapeado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Carrega dados Seguro
            var veiculoSeguro = objVeiculoSeguro.FirstOrDefault();
            if (veiculoSeguro != null)
            {
                veiculoSeguro = Mapper.Map<VeiculoSeguroViewModel>(veiculoSeguro);
                veiculoMapeado.VeiculoSeguro = veiculoSeguro;
            }

            //Popula contratos selecionados
            var listaVinculosVeiculo = objVeiculoEmpresa;

            if (listaVinculosVeiculo != null)
            {
                ViewBag.ContratosSelecionados = listaVinculosVeiculo;
                Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosVeiculo);
            }

            //Popula anexos do veiculo
            var anexosSelecionados = Mapper.Map<List<VeiculoAnexoViewModel>>(objVeiculoAnexo);
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
                ViewBag.AnexosSelecionados = (List<VeiculoAnexoViewModel>)Session[SESS_ANEXOS_SELECIONADOS];
            }
            else
            {
                ViewBag.AnexosSelecionados = new List<VeiculoAnexoViewModel>();
            }

            if (Session[SESS_MUNICIPIO_SELECIONADO] != null)
            {
                ViewBag.Municipio = Session[SESS_MUNICIPIO_SELECIONADO];
            }
            else
            {
                ViewBag.Municipio = new List<Municipio>();
            }

            return veiculoMapeado;
        }
        
        [Authorize]
        public void OnSelecionaFoto_Click(HttpPostedFileBase file, VeiculoViewModel model)
        {
            model.Foto = ConverterArquivo.Base64(file);
            Session.Add(SESS_FOTO_VEICULO, model.Foto);
        }

        [Authorize]
        public void GetFotoOnErro(VeiculoViewModel model)
        {
            if (model.Foto != null)
            {
                var bytes = Convert.FromBase64String(model.Foto);
                string base64 = Convert.ToBase64String(bytes);

                Session.Add(SESS_FOTO_VEICULO, base64);
                ViewBag.FotoVeiculo = String.Format("data:image/gif;base64,{0}", base64);
            }
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
