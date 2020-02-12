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

        private ColaboradorView _colaboradorView;
        private ColaboradorViewModel _viewModelParent;

        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;
        private ObservableCollection<ColaboradorObservacaoView> _entityObserver;

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
        //public ObservableCollection<ColaboradorObservacaoView> EntityObserver { get; set; }

        public ObservableCollection<ColaboradorObservacaoView> EntityObserver
        {
            get { return _entityObserver; }

            set
            {
                if (_entityObserver != value)
                {
                    _entityObserver = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        
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
          
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;

            }
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
                var observer = new ObservableCollection<ColaboradorObservacaoView>();
                
                EntityObserver.Insert(0, n2);
                
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
            {
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes)
                    return;

                    var n1 = Mapper.Map<ColaboradorObservacao>(Entity);
                    _service.Remover(n1);

                    EntityObserver.Remove(Entity);

                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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

            var observer = new ObservableCollection<ColaboradorObservacaoView>();
            list2.ForEach(n =>
            {
                observer.Add(n);
                n.Observacao = _service.BuscarPelaChave(n.ColaboradorObservacaoId).Observacao;
                //n.Impeditivo = _service.BuscarPelaChave(n.ColaboradorObservacaoId).Impeditivo;
            });

            EntityObserver = observer;
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