﻿using AutoMapper;
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
        // GET: Colaborador
        private readonly IMOD.Application.Interfaces.IVeiculoService objService = new IMOD.Application.Service.VeiculoService();
        private readonly IMOD.Application.Interfaces.IDadosAuxiliaresFacade objAuxiliaresService = new IMOD.Application.Service.DadosAuxiliaresFacadeService();
        private readonly IMOD.Application.Interfaces.IEmpresaContratosService objContratosService = new IMOD.Application.Service.EmpresaContratoService();
        private readonly IMOD.Application.Interfaces.IVeiculoEmpresaService objVeiculoEmpresaService = new IMOD.Application.Service.VeiculoEmpresaService();
        private readonly IMOD.Application.Interfaces.IVeiculoAnexoService objVeiculoAnexoService = new IMOD.Application.Service.VeiculoAnexoService();
        private readonly IMOD.Application.Interfaces.ITipoServicoService objServicosService = new IMOD.Application.Service.TipoServicoService();
        private const string SESS_CONTRATOS_SELECIONADOS = "ContratosSelecionados";
        private const string SESS_CONTRATOS_REMOVIDOS = "ContratosRemovidos";
        private const string SESS_SERVICOS_SELECIONADOS = "ServicosSelecionados";
        private const string SESS_SERVICOS_REMOVIDOS = "ServicosRemovidos";

        private List<Veiculo> veiculos = new List<Veiculo>();
        private List<VeiculoEmpresa> vinculos = new List<VeiculoEmpresa>();

        // GET: Veiculo
        public ActionResult Index()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            //var lstVeiculo = objService.Listar(null, null, string.Empty);
            //List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(lstVeiculo);
            List<VeiculoViewModel> lstVeiculoMapeado = Mapper.Map<List<VeiculoViewModel>>(ObterVeiculossEmpresaLogada());
            ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            return View(lstVeiculoMapeado); 

        }
        private IList<Veiculo> ObterVeiculossEmpresaLogada()
        {
            vinculos = objVeiculoEmpresaService.Listar(null, null, null, null, null, SessionUsuario.EmpresaLogada.EmpresaId).ToList();
            vinculos.ForEach(v => { veiculos.AddRange(objService.Listar(null,null,null,null,null,v.VeiculoId)); });

            return veiculos.OrderBy(c => c.Descricao).ToList();
        }
        // GET: Veiculo/Details/5
        public ActionResult Details(int id)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            //Remove itens da sessão
            Session.Remove(SESS_CONTRATOS_SELECIONADOS);
            Session.Remove(SESS_CONTRATOS_REMOVIDOS);
            ViewBag.Contratos = new List<EmpresaContrato>();

            //Obtem o veículo pelo ID
            var veiculoEditado = objService.Listar(id).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            //obtém vinculos do colaborador
            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado);

            // carrega os contratos da empresa
            if (SessionUsuario.EmpresaLogada.Contratos != null)
            {
                ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos;
            }

            //Popula contratos selecionados
            ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
            var listaVinculosVeiculo = Mapper.Map<List<VeiculoEmpresaViewModel>>(objVeiculoEmpresaService.Listar(veiculoEditado.EquipamentoVeiculoId));
            ViewBag.ContratosSelecionados = listaVinculosVeiculo;
            Session.Add(SESS_CONTRATOS_SELECIONADOS, listaVinculosVeiculo);

            //Propula combo de estado
            PopularEstadosDropDownList();

            //Preenchie combo municipio de acordo com o estado
            ViewBag.Municipio = new List<Municipio>();
            var lstMunicipio = objAuxiliaresService.MunicipioService.Listar(null, null, veiculoMapeado.EstadoId).OrderBy(m => m.Nome);
            if (lstMunicipio != null) { ViewBag.Municipio = lstMunicipio; };

            PopularDadosDropDownList();

            //var objColaboradorAnexo = objColaboradorAnexoService.Listar(colaboradorMapeado.ColaboradorId).FirstOrDefault();
            //if (objColaboradorAnexo != null)
            //{
            //    colaboradorMapeado.NomeArquivoAnexo = objColaboradorAnexo.NomeArquivo;
            //}

            return View(veiculoMapeado);
        }

        // GET: Veiculo/Create
        public ActionResult Create()
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            Session.Remove(SESS_CONTRATOS_SELECIONADOS);
            Session.Remove(SESS_CONTRATOS_REMOVIDOS);


            //carrega os serviços
            var listServicos = objServicosService.Listar().ToList();
            if (listServicos != null && listServicos.Any()) { ViewBag.Servicos = listServicos; }
            ViewBag.ServicosSelecionados = new List<TipoServico>();

            //carrega contratos
            if (SessionUsuario.EmpresaLogada.Contratos != null) { ViewBag.Contratos = SessionUsuario.EmpresaLogada.Contratos; }
            ViewBag.ContratosSelecionados = new List<VeiculoEmpresaViewModel>();
            

            PopularEstadosDropDownList();
            PopularDadosDropDownList();

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

                if (model.FileUpload != null)
                {
                    if (model.FileUpload.ContentLength > 2048000)
                        ModelState.AddModelError("FileUpload", "Tamanho permitido de arquivo 2,00 MB");

                    if (!Path.GetExtension(model.FileUpload.FileName).Equals(".pdf"))
                        ModelState.AddModelError("FileUpload", "Permitida Somente Extensão  .pdf");
                }


                if (ModelState.IsValid)
                {
                    var veiculoMapeado = Mapper.Map<Veiculo>(model); 
                    objService.Criar(veiculoMapeado);

                    var veiculoEmpresa = Mapper.Map<VeiculoEmpresa>(model); 
                     objVeiculoEmpresaService.Criar(veiculoEmpresa);

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
                        foreach (var vinculo in (List<ColaboradorEmpresaViewModel>)Session[SESS_CONTRATOS_SELECIONADOS])
                        {
                            // Inclusão do vinculo                       
                            var item = objVeiculoEmpresaService.Listar(veiculoMapeado.EquipamentoVeiculoId, null, null, null, null, null, vinculo.EmpresaContratoId).FirstOrDefault();
                            if (item == null)
                            {
                                vinculo.ColaboradorId = veiculoMapeado.EquipamentoVeiculoId;
                                vinculo.Ativo = true;
                                vinculo.EmpresaId = SessionUsuario.EmpresaLogada.EmpresaId;
                                var colaboradorEmpresa = Mapper.Map<ColaboradorEmpresa>(vinculo);
                                objVeiculoEmpresaService.Criar(veiculoEmpresa);
                                objVeiculoEmpresaService.CriarNumeroMatricula(veiculoEmpresa);
                            }
                        }
                    }

                    CriarVeiculoAnexo(model, veiculoMapeado.EquipamentoVeiculoId); 

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
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

            var veiculoEditado = objService.Listar(null,null,null,null,null,id).FirstOrDefault();
            if (veiculoEditado == null)
                return HttpNotFound();

            VeiculoViewModel veiculoMapeado = Mapper.Map<VeiculoViewModel>(veiculoEditado); 

            PopularEstadosDropDownList(); 
            PopularDadosDropDownList(); 
            PopularContratoEditDropDownList(veiculoMapeado, SessionUsuario.EmpresaLogada.EmpresaId);

            return View(veiculoMapeado); 
        }

        // POST: Veiculo/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, VeiculoViewModel model)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); }

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
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Veiculo/Delete/5
        public ActionResult Delete(int id, Veiculo model) 
        { 

            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); } 
            if (id == null || (id <= 0)) { return RedirectToAction("../Login"); } 

            try 
            {
                //if (id == null)
                //    return HttpNotFound();
                var idVeiculo = id;

                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "Confirma a Deleção desse registro?";
                string caption = "Deleção de Veiculo/Equipamento";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    var veiculoMapeado = Mapper.Map<Veiculo>(model);
                    veiculoMapeado.EquipamentoVeiculoId = Convert.ToInt32(idVeiculo);
                    objService.Remover(veiculoMapeado);

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // POST: Veiculo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, System.Web.Mvc.FormCollection collection)
        {
            if (SessionUsuario.EmpresaLogada == null) { return RedirectToAction("../Login"); }

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
            vinculo.FlagAreaManobra = flagAreaManobra;
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

        [Authorize]
        public JsonResult AdicionarServico(int id)
        {
            if (Session[SESS_SERVICOS_SELECIONADOS] == null) Session[SESS_SERVICOS_SELECIONADOS] = new List<TipoServico>();
            var item = objServicosService.Listar(id).FirstOrDefault();
            ((List<TipoServico>)Session[SESS_SERVICOS_SELECIONADOS]).Add(item);
            return Json((List<TipoServico>)Session[SESS_SERVICOS_SELECIONADOS], JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RemoverServico(int id)
        {
            var listServico = (List<TipoServico>)Session[SESS_SERVICOS_SELECIONADOS];
            listServico.Remove(new TipoServico(id));
            Session[SESS_SERVICOS_SELECIONADOS] = listServico;
            if (Session[SESS_SERVICOS_REMOVIDOS] == null) Session[SESS_SERVICOS_REMOVIDOS] = new List<int>();
            ((List<int>)Session[SESS_SERVICOS_REMOVIDOS]).Add(id);
            return Json(listServico, JsonRequestBehavior.AllowGet);
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
            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao");
        }

        private void PopularContratoEditDropDownList(VeiculoViewModel veiculo, int idEmpresa)
        {
            if (idEmpresa <= 0) return; 

            var contratoEmpresa = objContratosService.Listar(idEmpresa); 
            var resultEmpresasContratosVinculados = objVeiculoEmpresaService.Listar(null, null, null, null, null, idEmpresa);

            ViewBag.ContratoEmpresa = new MultiSelectList(contratoEmpresa, "EmpresaContratoId", "Descricao", resultEmpresasContratosVinculados.Select(m => m.EmpresaContratoId).ToArray());
        }
        
        private void CriarVeiculoAnexo(VeiculoViewModel veiculo, int veiculoId = 0)
        {
            byte[] bufferArquivo;
            string NomeArquivo;
            string ExtensaoArquivo;

            if (veiculo == null || veiculoId == 0) return;
            if (veiculo.FileUpload.ContentLength <= 0 || veiculo.FileUpload.ContentLength >2048000) return;

            NomeArquivo = Path.GetFileNameWithoutExtension(veiculo.FileUpload.FileName);
            ExtensaoArquivo = Path.GetExtension(veiculo.FileUpload.FileName);

            if (!ExtensaoArquivo.Equals(".pdf")) return;

            var arquivoStream = veiculo.FileUpload.InputStream;
            using (MemoryStream ms = new MemoryStream())
            {
                arquivoStream.CopyTo(ms);
                bufferArquivo = ms.ToArray();
            }

            var arquivoBase64 = Convert.ToBase64String(bufferArquivo);

            VeiculoAnexo veiculoAnexo = new VeiculoAnexo();
            veiculoAnexo.VeiculoId = veiculoId; 
            veiculoAnexo.Arquivo = arquivoBase64; 
            veiculoAnexo.NomeArquivo = NomeArquivo + ExtensaoArquivo;
            veiculoAnexo.Descricao = NomeArquivo + ExtensaoArquivo;
            objVeiculoAnexoService.Criar(veiculoAnexo);

        }


        #endregion
    }
}
