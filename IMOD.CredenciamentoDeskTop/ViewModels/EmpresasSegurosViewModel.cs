// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  13 - 11 - 2019
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
    class EmpresasSegurosViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaService _empresaService = new EmpresaService();

        private readonly IEmpresaSeguroService _service = new EmpresaSeguroService();
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _serviceContratos = new EmpresaContratoService();
        private ConfiguraSistema _configuraSistema;

        private EmpresaView _empresaView;
        private EmpresaViewModel _viewModelParent;

        #region  Propriedades
        //public List<Empresa> Empresas { get; private set; }
        public List<EmpresaContrato> Contratos { get; private set; }
        public Empresa Empresa { get; set; }
        public EmpresaSeguroView Entity { get; set; }
        public ObservableCollection<EmpresaSeguroView> EntityObserver { get; set; }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }
        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        /// <summary>
        ///     Tamanho da Imagem
        /// </summary>
        public int IsTamanhoArquivo
        {
            get
            {
                return _configuraSistema.arquivoTamanho;
            }
        }
        #endregion

        #region  Metodos
        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            //if (Entity == null) return true;
            //Entity.Validate();
            //var hasErros = Entity.HasErrors;
            //if (hasErros) return true;

            return false;// Entity.HasErrors;
        }
        #endregion

        #region Inicializacao
        public void AtualizarDados(EmpresaView entity, EmpresaViewModel viewModelParent)
        {
            try
            {
                EntityObserver.Clear();
                if (entity == null) return; // throw new ArgumentNullException (nameof (entity));
                _empresaView = entity;
                _viewModelParent = viewModelParent;

                //Obter dados
                var list1 = _service.Listar(null, null, null,null, entity.EmpresaId);

                var list2 = Mapper.Map<List<EmpresaSeguroView>>(list1.OrderByDescending(n => n.EmpresaSeguroId));
                EntityObserver = new ObservableCollection<EmpresaSeguroView>();
                list2.ForEach(n => { EntityObserver.Add(n); });

                ListarContratos(entity.EmpresaId);
                //Obter configuracoes de sistema
                _configuraSistema = ObterConfiguracao();
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
            }
        }
        /// <summary>
        /// Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
        private ConfiguraSistema ObterConfiguracao()
        {
            //Obter configuracoes de sistema
            var config = _auxiliaresService.ConfiguraSistemaService.Listar();
            //Obtem o primeiro registro de configuracao
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }
        public EmpresasSegurosViewModel()
        {
            Comportamento = new ComportamentoBasico(false, true, true, false, false);
            EntityObserver = new ObservableCollection<EmpresaSeguroView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
        }
        #endregion
        #region Metódos
        // <summary>
        ///  Listar dados de empresa e contratos
        /// </summary>
        public void ListarContratos(int empresa)
        {
            try
            {
                if (empresa == null) return;
                Contratos = _serviceContratos.Listar(empresa).OrderBy(n => n.Descricao).ToList();
                
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
            }
        }
        public void BuscarAnexo(int EmpresaSeguroID)
        {
            try
            {
                var anexo = _service.BuscarPelaChave(EmpresaSeguroID);
                Entity.Arquivo = anexo.Arquivo;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
            //Comportamento.IsEnableEditar = true;
            if (e.PropertyName == "Entity")
            {
                Comportamento.IsEnableEditar = Entity != null;
                Comportamento.isEnableRemover = Entity != null;

            }
        }
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

                var n1 = Mapper.Map<EmpresaSeguro>(Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaSeguroView>(n1);
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

            Entity = new EmpresaSeguroView();
            //ListarDadosEmpresaContratos();
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, false, true, false, false);
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
                var n1 = Mapper.Map<EmpresaSeguro>(Entity);
                _service.Alterar(n1);
                IsEnableLstView = true;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
                Entity = null;
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

                if (Entity != null) //Entity.ClearMessageErro();
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
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<EmpresaSeguro>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
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
        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, false, true, false, false);
        }
        /// <summary>
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
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
        ///     Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar, true);

        #endregion
    }
}
