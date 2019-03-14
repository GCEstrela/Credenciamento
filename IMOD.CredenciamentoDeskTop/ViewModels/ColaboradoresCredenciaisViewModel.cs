﻿// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.Modulo;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Infra.Servicos;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;
using IMOD.CredenciamentoDeskTop.Enums;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresCredenciaisViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorEmpresaService _colaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly ConfiguraSistema _configuraSistema;
        private readonly IEmpresaContratosService _contratosService = new EmpresaContratoService();
        private readonly IColaboradorCredencialService _service = new ColaboradorCredencialService();

        /// <summary>
        ///     Lista de todos os contratos disponíveis
        /// </summary>
        private readonly List<ColaboradorEmpresa> _todosContratosEmpresas = new List<ColaboradorEmpresa>();

        private ColaboradorView _colaboradorView;

        private int _count;
        private List<CredencialMotivo> _credencialMotivo;

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        private ColaboradorViewModel _viewModelParent;

        #region  Propriedades

        /// <summary>
        ///     Habilitar Controles
        /// </summary>
        public bool Habilitar { get; set; }

        public CredencialStatus StatusCredencial { get; set; }
        public List<CredencialStatus> CredencialStatus { get; set; }

        public List<CredencialMotivo> CredenciaisMotivo
        {
            get
            {
                if (StatusCredencial == null) return _credencialMotivo;
                var lst = _credencialMotivo.Where (n => n.CodigoStatus == StatusCredencial.Codigo);
                return lst.ToList();
            }
            set { _credencialMotivo = value; }
        }

        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public ObservableCollection<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }
        public ColaboradorEmpresa ColaboradorEmpresa { get; set; }
        public List<AreaAcesso> ColaboradorPrivilegio { get; set; }
        public ColaboradoresCredenciaisView Entity { get; set; }

        public bool IsCheckDevolucao { get; set; } = false;
        public Visibility VisibilityCheckDevolucao { get; set; } = Visibility.Hidden;
        public string TextCheckDevolucao { get; set; } = String.Empty;
        private DevoluçãoCredencial devolucaoCredencial;

        /// <summary>
        ///     Mensagem de alerta
        /// </summary>
        public string MensagemAlerta { get; private set; }

        /// <summary>
        ///     Habilitar impressao de credencial com base no status da credencial
        ///     e condição de pendencia impeditiva
        /// </summary>
        public bool HabilitaImpressao { get; set; }

        public ObservableCollection<ColaboradoresCredenciaisView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        #endregion

        public ColaboradoresCredenciaisViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico (false, true, false, false, false);
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
            SelectListViewIndex = -1;
            _configuraSistema = ObterConfiguracao();
        }

        #region  Metodos

        private void AtualizarMensagem(ColaboradoresCredenciaisView entity)
        {
            MensagemAlerta = string.Empty;
            if (entity == null) return;
            if (entity.ColaboradorCredencialId <= 0) return;

            #region Habilitar botão de impressao e mensagem ao usuario

            //Verifica se a data de validade da credencial é maior que a data atural
            HabilitaImpressao = entity.Ativa && !entity.PendenciaImpeditiva && !entity.Impressa && (entity.ColaboradorCredencialId > 0) && entity.Validade >= DateTime.Now.Date;
            //Verificar se a empresa esta impedida
            var n1 = _service.BuscarCredencialPelaChave(entity.ColaboradorCredencialId);
            var mensagem1 = !n1.Ativa ? "Credencial Inativa" : string.Empty;
            var mensagem2 = n1.PendenciaImpeditiva ? "Pendência Impeditiva (consultar dados da empresa na aba Geral)" : string.Empty;
            var mensagem3 = n1.Impressa ? "Credencial já foi emitida" : string.Empty;
            var mensagem4 = (entity.Validade < DateTime.Now.Date) ? "Credencial vencida." : string.Empty;
            //Exibir mensagem de impressao de credencial, esta tem prioridade sobre as demais regras            
            //if (n1.Impressa) return;

            if (!string.IsNullOrEmpty(mensagem1 + mensagem2 + mensagem3 + mensagem4))
            {
                MensagemAlerta = $"A credencial não poderá ser impressa pelo seguinte motivo: ";
                if (!string.IsNullOrEmpty(mensagem1))
                {
                    MensagemAlerta += mensagem1;
                }
                else if (!string.IsNullOrEmpty(mensagem2))
                {
                    MensagemAlerta += mensagem2;
                }
                else if (!string.IsNullOrEmpty(mensagem3))
                {
                    MensagemAlerta += mensagem3;
                }
                else if (!string.IsNullOrEmpty(mensagem4))
                {
                    MensagemAlerta += mensagem4;
                }
            }
            //================================================================================
            #endregion
        }

        #region Regras de Negócio

        private bool ExisteNumeroCredencial()
        {
            if (Entity == null) return false;
            var numCredencial = Entity.NumeroCredencial.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
                if (_service.ExisteNumeroCredencial (numCredencial))
                    return true;
            //Verificar dados antes de salvar uma alteraçao
            if (!_prepareAlterarCommandAcionado) return false;
            var n1 = _service.BuscarPelaChave (Entity.ColaboradorId);
            if (n1 == null) return false;
            //Comparar o CNPJ antes e o depois
            //Verificar se há cnpj exisitente
            return string.Compare (n1.NumeroCredencial.RetirarCaracteresEspeciais(),
                       numCredencial, StringComparison.Ordinal) != 0 && _service.ExisteNumeroCredencial (numCredencial);
        }

        #endregion

        private void ListarDadosAuxiliares()
        {
            var lst0 = _auxiliaresService.CredencialStatusService.Listar();
            CredencialStatus = new List<CredencialStatus>();
            CredencialStatus.AddRange (lst0);

            var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
            FormatoCredencial = new List<FormatoCredencial>();
            FormatoCredencial.AddRange (lst2);

            var lst3 = _auxiliaresService.TipoCredencialService.Listar();
            TipoCredencial = new List<TipoCredencial>();
            TipoCredencial.AddRange (lst3);

            var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
            TecnologiasCredenciais = new List<TecnologiaCredencial>();
            TecnologiasCredenciais.AddRange (lst5);

            ColaboradoresEmpresas = new ObservableCollection<ColaboradorEmpresa>();

            var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            ColaboradorPrivilegio = new List<AreaAcesso>();
            ColaboradorPrivilegio.AddRange (lst7);

            _credencialMotivo = new List<CredencialMotivo>();
            var lst8 = _auxiliaresService.CredencialMotivoService.Listar();
            _credencialMotivo.AddRange (lst8);
        }

        /// <summary>
        /// </summary>
        /// <param name="empresaId"></param>
        public void ListarCracha(int empresaId)
        {
            try
            {
                EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView (empresaId);
                var list2 = Mapper.Map<List<EmpresaLayoutCracha>> (list1);
                EmpresaLayoutCracha = list2;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;
                AtualizarMensagem (Entity); 
                ExibirCheckDevolucao(Entity); 
            }
        }

        public void ObterValidade()
        {
            if (!_prepareCriarCommandAcionado) return;
            if (Entity == null) return;
            var empContratoId = ColaboradorEmpresa.EmpresaContratoId;
            var contrato = _contratosService.BuscarPelaChave (empContratoId);
            var data = _service.ObterDataValidadeCredencial (Entity.TipoCredencialId,
                _colaboradorView.ColaboradorId, contrato.NumeroContrato, _service.TipoCredencial);

            Entity.Validade = data;
            OnPropertyChanged ("Entity");
        }

        public void AtualizarDados(ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            if (entity == null) throw new ArgumentNullException (nameof(entity));
            _colaboradorView = entity;
            _viewModelParent = viewModelParent;
            //Obter dados
            var list1 = _service.ListarView (null, null, null, null, entity.ColaboradorId).ToList();
            var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>> (list1.OrderByDescending (n => n.ColaboradorCredencialId));
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
            list2.ForEach (n => { EntityObserver.Add (n); });
            //Listar dados de contratos
            if (_count == 0) ObterContratos();
            _count++;
            ListarTodosContratos();
            MensagemAlerta = "";
        }

        /// <summary>
        ///     Listar dados de empresa e contratos
        /// </summary>
        private void ObterContratos()
        {
            try
            {
                var l2 = _colaboradorEmpresaService.Listar().ToList();
                _todosContratosEmpresas.Clear();
                _todosContratosEmpresas.AddRange (l2);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void ListarTodosContratos()
        {
            ColaboradoresEmpresas.Clear();
            _todosContratosEmpresas.ForEach (n => { ColaboradoresEmpresas.Add (n); });
        }

        /// <summary>
        ///     Obter novos dados de contratos ativos
        /// </summary>
        private void OnAtualizarDadosContratosAtivos()
        {
            //Obter todos os contratos vinculados ao colaborador...
            ObterContratos();
            ListarTodosContratoPorColaboradorAtivos (_colaboradorView.ColaboradorId);
        }

        /// <summary>
        ///     Listar contratos ativos
        /// </summary>
        /// <param name="colaboradorId"></param>
        private void ListarTodosContratoPorColaboradorAtivos(int colaboradorId)
        {
            ColaboradoresEmpresas.Clear();
            var lst2 = _todosContratosEmpresas.Where (n => (n.ColaboradorId == colaboradorId) & n.Ativo).ToList();
            lst2.ForEach (n => { ColaboradoresEmpresas.Add (n); });
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add (new KeyValuePair<int, string> (1, "Nome"));
            ListaPesquisa.Add (new KeyValuePair<int, string> (2, "CPF"));
            PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        }

        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = false;
            Comportamento.PrepareRemover();
        }

        /// <summary>
        ///     Criar Dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                Entity.DevolucaoEntregaBoId = IsCheckDevolucao ? (int)devolucaoCredencial : 0;

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);
                
                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;
                n1.DevolucaoEntregaBoId = IsCheckDevolucao ? Entity.DevolucaoEntregaBoId : 0;

                //Criar registro no banco de dados e setar uma data de validade
                _prepareCriarCommandAcionado = false;
                _service.Criar (n1);
                IsEnableLstView = true;
                SelectListViewIndex = 0;

                var list1 = _service.ListarView (null, null, null, null, _colaboradorView.ColaboradorId).ToList();
                var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>> (list1.OrderByDescending (n => n.ColaboradorCredencialId));
                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();

                list2.ForEach (n => { EntityObserver.Add (n); });
                MensagemAlerta = "";
                Entity = null;
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true, true);
                ExibirCheckDevolucao(Entity);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException ("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            try
            {
                Entity = new ColaboradoresCredenciaisView();

                var statusCred = CredencialStatus.FirstOrDefault (n => n.Codigo == "1"); //Status ativa
                if (statusCred == null) throw new InvalidOperationException ("O status da credencial é requerida.");
                StatusCredencial = statusCred;

                var tipoCredencial = TipoCredencial.FirstOrDefault (n => n.CredPermanente);
                if (tipoCredencial != null) Entity.TipoCredencialId = tipoCredencial.TipoCredencialId;

                Entity.Ativa = true;
                Comportamento.PrepareCriar();
                _prepareCriarCommandAcionado = true;
                _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
                IsEnableLstView = false;
                Habilitar = true;
                MensagemAlerta = "";
                //Listar Colaboradores Ativos
                OnAtualizarDadosContratosAtivos();
                _viewModelParent.HabilitaControleTabControls (false, false, false, false, false, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Editar dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                Entity.DevolucaoEntregaBoId = IsCheckDevolucao ? (int)devolucaoCredencial : 0; 

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity); 

                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;
                n1.DevolucaoEntregaBoId = IsCheckDevolucao ? Entity.DevolucaoEntregaBoId : 0;

                //Alterar o status do titular do cartão
                _service.AlterarStatusTitularCartao (new CredencialGenetecService (Main.Engine), Entity, n1);

                //===================================================
                //Atualizar dados a serem exibidas na tela de empresa
                if (Entity == null) return;
                _service.CriarPendenciaImpeditiva (Entity);
                var view = new ViewSingleton().EmpresaView;
                var dados = view.DataContext as IAtualizarDados;
                dados.AtualizarDadosPendencias();
                //===================================================

                //Atualizar observer
                CollectionViewSource.GetDefaultView (EntityObserver).Refresh();
                IsEnableLstView = true;
                AtualizarMensagem (Entity);
                //===================================================
                Entity = null;
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true, true);
                ExibirCheckDevolucao(Entity); 
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Cancelar operação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                IsEnableLstView = true;
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;
                //Listar todas contratos 
                ListarTodosContratos();
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Remover dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);
                _service.Remover (n1);
                //Retirar empresa da coleção
                EntityObserver.Remove (Entity);
                _viewModelParent.HabilitaControleTabControls (true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        ///     Imprimir Credencial
        /// </summary>
        private void OnImprimirCredencial()
        {
            try
            {
                if (Entity == null) return;
                if (!Entity.Ativa) throw new InvalidOperationException ("Não é possível imprimir uma credencial não ativa.");
                if (Entity.Validade == null) throw new InvalidOperationException ("Não é possível imprimir uma credencial sem data de validade.");

                var layoutCracha = _auxiliaresService.LayoutCrachaService.BuscarPelaChave (Entity.LayoutCrachaId);
                if (layoutCracha == null) throw new InvalidOperationException ("Não é possível imprimir uma credencial sem ter sido definida um layout do crachá.");
                if (string.IsNullOrWhiteSpace (layoutCracha.LayoutRpt)) throw new InvalidOperationException ("Não é possível imprimir uma credencial sem ter sido definida um layout do crachá.");

                Cursor.Current = Cursors.WaitCursor;

                var arrayBytes = WpfHelp.ConverterBase64 (layoutCracha.LayoutRpt, "Layout Cracha");
                var relatorio = WpfHelp.ShowRelatorioCrystalReport (arrayBytes, layoutCracha.Nome);
                //Changed by
                //Author:Valnei Filho
                //Date: 28/02/2019
                var lst = new List<CredencialViewCracha>();
                var credencialView = _service.ObterCredencialView (Entity.ColaboradorCredencialId);
                var c1 = Mapper.Map<CredencialViewCracha> (credencialView);

                lst.Add (c1);
                relatorio.SetDataSource (lst);
                var popupCredencial = new PopupCredencial (relatorio, _service, Entity, layoutCracha);
                popupCredencial.ShowDialog();

                //Atualizar observer
                OnPropertyChanged ("Entity");
                CollectionViewSource.GetDefaultView (EntityObserver).Refresh(); //Atualizar observer
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false;
            //Habilitar controles somente se a credencial não estiver sido impressa
            Habilitar = !Entity.Impressa;
            _viewModelParent.HabilitaControleTabControls (false, false, false, false, false, true);
        }

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
        }

        /// <summary>
        ///     Carregar Caracteres Colete
        /// </summary>
        public void CarregarCaracteresColete(ColaboradorEmpresa colaboradorEmpresa)
        {
            if (Entity == null || colaboradorEmpresa == null || colaboradorEmpresa.EmpresaSigla == null) return;
            Entity.Colete = colaboradorEmpresa.EmpresaSigla.Trim() + Convert.ToString (_colaboradorView.ColaboradorId);
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            if (ExisteNumeroCredencial())
            {
                Entity.SetMessageErro ("NumeroCredencial", "Número de credencial já existente.");
                return true;
            }

            return Entity.HasErrors;
        }

        public void HabilitaCheckDevolucao(int credencialStatus = 0, int credencialMotivoId = 0)
        {
            if (credencialStatus == 2 && credencialMotivoId > 0) 
            {
                switch (credencialMotivoId)
                {
                    case 6:
                    case 8:
                    case 15:
                        TextCheckDevolucao = DevoluçãoCredencial.Devolucao.Descricao();
                        devolucaoCredencial = DevoluçãoCredencial.Devolucao;
                        VisibilityCheckDevolucao = Visibility.Visible;
                        break;
                    case 9:
                    case 10:
                        TextCheckDevolucao = DevoluçãoCredencial.EntregaBO.Descricao();
                        devolucaoCredencial = DevoluçãoCredencial.EntregaBO;
                        VisibilityCheckDevolucao = Visibility.Visible;
                        break;
                    default:
                        TextCheckDevolucao = String.Empty;
                        VisibilityCheckDevolucao = Visibility.Hidden;
                        break;
                }
            }
            else
            {
                IsCheckDevolucao = false;
                TextCheckDevolucao = String.Empty;
                VisibilityCheckDevolucao = Visibility.Hidden;
            }
        }

        private void ExibirCheckDevolucao(ColaboradoresCredenciaisView entity)
        {
            if (entity != null)
            {
                IsCheckDevolucao = entity.DevolucaoEntregaBoId == 0 ? false : (entity.DevolucaoEntregaBoId > 0 ? true : false);

                VisibilityCheckDevolucao = entity.DevolucaoEntregaBoId == 0 ?
                    Visibility.Hidden : (entity.DevolucaoEntregaBoId > 0 ? Visibility.Visible : Visibility.Hidden);

                TextCheckDevolucao = entity.DevolucaoEntregaBoId == 0 ? String.Empty :
                        (entity.DevolucaoEntregaBoId == 1 ? DevoluçãoCredencial.Devolucao.Descricao() : DevoluçãoCredencial.EntregaBO.Descricao());
            }
            else
            {
                IsCheckDevolucao = false;
                TextCheckDevolucao = String.Empty;
                VisibilityCheckDevolucao = Visibility.Hidden;
            }
        }


        #endregion

        #region Propriedade de Pesquisa

        /// <summary>
        ///     String contendo o nome a pesquisa;
        /// </summary>
        public string NomePesquisa { get; set; }

        /// <summary>
        ///     Opções de pesquisa
        /// </summary>
        public List<KeyValuePair<int, string>> ListaPesquisa { get; private set; }

        /// <summary>
        ///     Pesquisar por
        /// </summary>
        public KeyValuePair<int, string> PesquisarPor { get; set; }

        #endregion

        #region Propriedade Commands

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase (PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase (Comportamento.PrepareCancelar, true);

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase (PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase (PrepareRemover, true);

        /// <summary>
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase (Pesquisar, true);

        public ICommand ImprimirCommand => new CommandBase (OnImprimirCredencial, true);

        #endregion
    }
}