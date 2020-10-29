// ***********************************************************************
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
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CredenciamentoDeskTop.Helpers;
using IMOD.CredenciamentoDeskTop.ViewModels.Commands;
using IMOD.CredenciamentoDeskTop.ViewModels.Comportamento;
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresAnexosViewModel : ViewModelBase,IComportamento
    {
        private readonly IColaboradorAnexoService _service = new ColaboradorAnexoService();
        private readonly IColaboradorAnexoWebService _serviceWeb = new ColaboradorAnexoWebService();
        private ColaboradorView _colaboradorView;
        private ColaboradorViewModel _viewModelParent;

        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _serviceContratos = new EmpresaContratoService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades 
        public ColaboradorAnexoView Entity { get; set; }
        public ObservableCollection<ColaboradorAnexoView> EntityObserver { get; set; }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }
        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
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

        public ColaboradoresAnexosViewModel()
        {
            try
            {
                Comportamento = new ComportamentoBasico(false, true, false, false, false);
                EntityObserver = new ObservableCollection<ColaboradorAnexoView>();
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

        private void Clear(object sender, EventArgs e)
        {
            EntityObserver.Clear();
        }



        #region  Metodos
        public void BuscarAnexo(int ColaboradorAnexoId)
        {
            try
            {
                if (Entity.Arquivo != null) return;
                var anexo = _service.BuscarPelaChave(ColaboradorAnexoId);
                Entity.Arquivo = anexo.Arquivo;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
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

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }
        /// <summary>
        ///     Acionado antes de remover
        /// </summary>
        private void PrepareRemover()
        {
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

                var n1 = Mapper.Map<ColaboradorAnexo> (Entity);
                n1.ColaboradorId = _colaboradorView.ColaboradorId;
                _service.Criar (n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<ColaboradorAnexoView> (n1);
                EntityObserver.Insert (0, n2);
                IsEnableLstView = true;
                _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criarss
        /// </summary>
        private void PrepareCriar()
        {
            
            Entity = new ColaboradorAnexoView();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, false, false, true, false);
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

                BuscarAnexo(Entity.ColaboradorAnexoId);

                var n1 = Mapper.Map<ColaboradorAnexo> (Entity);
                _service.Alterar (n1);
                IsEnableLstView = true;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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
                if (Entity != null) Entity.ClearMessageErro();
                IsEnableLstView = true;
                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
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

                var n1 = Mapper.Map<ColaboradorAnexo> (Entity);
                _service.Remover (n1);
                //Retirar empresa da coleção
                EntityObserver.Remove (Entity);
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
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
            _viewModelParent.HabilitaControleTabControls(false, false, false, false, true, false);
        }
        
        public void AtualizarDadosAnexo(ColaboradorView entity, ColaboradorViewModel viewModelParent)
        {
            EntityObserver.Clear();
            if (entity == null) return;// throw new ArgumentNullException(nameof(entity));
            _colaboradorView = entity;
            _viewModelParent = viewModelParent;

            var list1 = new List<ColaboradorAnexo>();
            if (_viewModelParent.IsEnablePreCadastro)
            {
                list1 = _serviceWeb.Listar(entity.ColaboradorId).ToList();
            }
            else
            {
                list1 = _service.Listar(entity.ColaboradorId).ToList();
            }

            var list2 = Mapper.Map<List<ColaboradorAnexoView>>(list1.OrderByDescending(n => n.ColaboradorAnexoId));
            EntityObserver = new ObservableCollection<ColaboradorAnexoView>();
            list2.ForEach(n => { EntityObserver.Add(n); });
            
            _configuraSistema = ObterConfiguracao();
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
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
        ///    Validar Regras de Negócio 
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErro = Entity.HasErrors;

            return hasErro;
        }

        #endregion
    }
}