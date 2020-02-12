// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
//using IMOD.CredenciamentoDeskTop.Views;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Infra.Servicos;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresObservacoesViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaContratosService _empresaContratoService = new EmpresaContratoService();
        private readonly IColaboradorObservacaoService _service = new ColaboradorObservacaoService();
        private readonly IColaboradorCredencialService _serviceCredencial = new ColaboradorCredencialService();
        private readonly IColaboradorService _serviceColaborador = new ColaboradorService();

        private readonly IColaboradorCredencialService _serviceColaboradorCredencial = new ColaboradorCredencialService();

        //private readonly object _auxiliaresService;
        private ColaboradorView _colaboradorView;

        private ColaboradorViewModel _viewModelParent;

        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades
        public string ExcluirVisivel { get; set; }
        private int _colaboradorid;
        public List<EmpresaContrato> Contratos { get; private set; }
        public List<Empresa> Empresas { get; private set; }
        public Empresa Empresa { get; set; }
        public ColaboradorObservacaoView Entity { get; set; }
        public string VisibleGruposRegras { get; set; }
        public string VisibleGrupo { get; set; }
        public int WidthGrupo { get; set; } = 80;
        public string VisibleRegra { get; set; }
        public int WidthRegra { get; set; } = 80;
        public string Alignment { get; set; }
        public ObservableCollection<ColaboradorObservacaoView> EntityObserver { get; set; }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        /// <summary>
        ///     Habilita Combo de Contratos
        /// </summary>
        public bool IsEnableComboContrato
        {
            get
            {
                return !_configuraSistema.Contrato;
            }
        }
        /// <summary>
        ///     Habilita Combo de Contratos
        /// </summary>
        public bool IsEnableColete { get; private set; } = true;
        /// <summary>
        ///     Tamanho do Arquivo
        /// </summary>
        public int IsTamanhoArquivo
        {
            get
            {
                return _configuraSistema.arquivoTamanho;
            }
        }
        #endregion

        public ColaboradoresObservacoesViewModel()
        {
            try
            {
                if (!Domain.EntitiesCustom.UsuarioLogado.Adm)
                {
                    ExcluirVisivel = "Collapsed";
                }
                else
                {
                    ExcluirVisivel = "Visible";
                }

                Comportamento = new ComportamentoBasico(false, true, true, false, false);
                EntityObserver = new ObservableCollection<ColaboradorObservacaoView>();
                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                base.PropertyChanged += OnEntityChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  Metodos
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            // if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
            //Comportamento.IsEnableEditar = true;
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;

            }
        }
        
        public void AtualizarConfiguracoes()
        {
            try
            {
                _configuraSistema = ObterConfiguracao();
                if (!_configuraSistema.Colete) //Se Cole não for automático false
                {
                    IsEnableColete = false;
                }
                _configuraSistema.VisibleGruposRegras = false;
                if (_configuraSistema.AssociarGrupos == true || _configuraSistema.AssociarRegras == true)
                {
                    _configuraSistema.VisibleGruposRegras = true;
                }

                VisibleGruposRegras = Helper.ExibirCampo(_configuraSistema.VisibleGruposRegras);
                VisibleGrupo = Helper.CollapsedCampo(_configuraSistema.AssociarGrupos);
                VisibleRegra = Helper.CollapsedCampo(_configuraSistema.AssociarRegras);
                if (_configuraSistema.AssociarGrupos == true && _configuraSistema.AssociarRegras == true)
                {
                    WidthGrupo = 85;
                    WidthRegra = 85;
                }
                else if (_configuraSistema.AssociarGrupos == true)
                {
                    WidthGrupo = 180;
                    Alignment = "Stretch";
                }
                else if (_configuraSistema.AssociarRegras == true)
                {

                    WidthRegra = 180;
                    Alignment = "Stretch";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ListarContratos(Empresa empresa)
        {
            if (empresa == null) return;

            Contratos.Clear();

            //if (!_configuraSistema.Contrato)
            //{
            var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId).OrderBy(n => n.Descricao).ToList();
            lstContratos.ForEach(n =>
            {
                n.Descricao = $"{n.Descricao} - {n.NumeroContrato}";
                Contratos.Add(n);

            });
            //}
            //else
            //{
            //    var lstContratos = _empresaContratoService.Listar(empresa.EmpresaId).OrderBy(n => n.Descricao).ToList().FirstOrDefault();
            //    if(lstContratos!=null)
            //        Contratos.Add(lstContratos);

            //    Entity.Validade = lstContratos.Validade;
            //}


            //Contratos.AddRange(lstContratos);
            //Manipular concatenaçção de conrato


        }

        /// <summary>
        /// Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresServiceConfiguraSistema.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
            if (Entity == null) return;

            IsEnableLstView = true;
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

                if (Validar()) return;

                var n1 = Mapper.Map<ColaboradorObservacao>(Entity);
                n1.ColaboradorId = _colaboradorView.ColaboradorId;
                
                _service.Criar(n1);

                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorObservacaoView>(n1);
                //Adicionar o nome da empresa e o contrato
                IsEnableLstView = true;
                _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {

            Entity = new ColaboradorObservacaoView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, false, false, false, false, true);
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
                if (Validar()) return;

                var n1 = Mapper.Map<ColaboradorObservacao>(Entity);

                _service.Alterar(n1);
                IsEnableLstView = true;

                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
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
                IsEnableLstView = true;
                if (Entity != null) Entity.ClearMessageErro();
                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
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
            {/*
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                    return;

                var verificarCredencialExistente = _serviceColaboradorCredencial.Listar(null, null, null, null, Entity.ColaboradorId, null, null, null, null, null, null, Entity.EmpresaContratoId);
                if (verificarCredencialExistente.Count == 0)
                {
                    var n1 = Mapper.Map<ColaboradorEmpresa>(Entity);
                    _service.Remover(n1);

                    
                    _serviceCredencial.RemoverCardHolder(new CredencialGenetecService(Main.Engine), new ColaboradorService(), n1);

                    //Retirar empresa da coleção
                    EntityObserver.Remove(Entity);
                }
                else
                {
                    if (verificarCredencialExistente.Count == 1)
                    {
                        WpfHelp.PopupBox("Este vínculo não pode ser DELETADO, pois existe " + verificarCredencialExistente.Count + " credencial vinculada." + "\n" + "Remova a credencial associada na aba Credenciais" + "\n" + "Ação cancelada pelo sistema.", 1);
                    }
                    else
                    {
                        WpfHelp.PopupBox("Este vínculo não pode ser DELETADO, pois existem " + verificarCredencialExistente.Count + " credenciais vinculadas." + "\n" + "Remova todas as credenciais associadas na aba Credenciais" + "\n" + "Ação cancelada pelo sistema.", 1);
                    }
                    
                }
                 
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);*/
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void PrepareSalvar()
        {
            if (Validar()) return;

            Comportamento.PrepareSalvar();
        }

        /// <summary>
        ///     Acionado antes de alterar
        /// </summary>
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }
            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, true, false, false, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="viewModel"></param>
        public void AtualizarDados(ColaboradorView entity, ColaboradorViewModel viewModel)
        {
            _viewModelParent = viewModel;
            AtualizarDados(entity);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void AtualizarDados(ColaboradorView entity)
        {
            EntityObserver.Clear();
            if (entity == null) return;
            //throw new ArgumentNullException(nameof(entity));
            _colaboradorid = entity.ColaboradorId;

            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.Listar(entity.ColaboradorId);
            var list2 = Mapper.Map<List<ColaboradorObservacaoView>>(list1.OrderByDescending(n => n.ColaboradorId));

            EntityObserver = new ObservableCollection<ColaboradorObservacaoView>();
            list2.ForEach(n =>
            {
                EntityObserver.Add(n);
                n.Observacao = _service.BuscarPelaChave(n.ColaboradorObservacaoId).Observacao;
                n.Impeditivo = _service.BuscarPelaChave(n.ColaboradorObservacaoId).Impeditivo;
            });
            //ListarDadosEmpresaContratos();
        }
        #endregion

        #region Propriedade Commands

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Editar
        /// </summary>
        public ICommand PrepareAlterarCommand => new CommandBase(PrepareAlterar, true);

        /// <summary>
        ///     Cancelar
        /// </summary>
        public ICommand PrepareCancelarCommand => new CommandBase(Comportamento.PrepareCancelar, true);

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareSalvarCommand => new CommandBase(PrepareSalvar, true);

        /// <summary>
        ///     Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase(PrepareRemover, true);

        /// <summary>
        ///  Validar Regras de Negócio 
        /// </summary>
        public bool Validar()
        {

            if (Entity == null) return true;
            Entity.Validate();


            if (string.IsNullOrEmpty(Entity.Observacao))
            {
                Entity.SetMessageErro("Observacao", "Favor preencher a Observação.");
                return true;
            }

            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            return Entity.HasErrors;
        }

        #endregion

    }
}