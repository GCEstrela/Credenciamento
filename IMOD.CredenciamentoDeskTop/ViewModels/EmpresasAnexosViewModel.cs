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
using IMOD.CredenciamentoDeskTop.Views.Model;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class EmpresasAnexosViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaAnexoService _service = new EmpresaAnexoService();
        private EmpresaView _empresaView;
        private EmpresaViewModel _viewModelParent;

        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaContratosService _serviceContratos = new EmpresaContratoService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades

        public EmpresaAnexoView Entity { get; set; }
        public ObservableCollection<EmpresaAnexoView> EntityObserver { get; set; } 

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }
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

        public EmpresasAnexosViewModel()
        {
            try
            {
                Comportamento = new ComportamentoBasico(false, true, false, false, false);
                EntityObserver = new ObservableCollection<EmpresaAnexoView>();
                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                base.PropertyChanged += OnEntityChanged;
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
            }
        }

        #region  Metodos
        public void BuscarAnexo(int EmpresaAnexoID)
        {
            try
            {
                if (Entity.Anexo != null) return;
                var anexo = _service.BuscarPelaChave(EmpresaAnexoID);
                Entity.Anexo = anexo.Anexo;
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
            //if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
            // Comportamento.IsEnableEditar = true;
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
                var n1 = Mapper.Map<EmpresaAnexo>(Entity);
                n1.EmpresaId = _empresaView.EmpresaId;
                _service.Criar(n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaAnexoView>(n1);
                EntityObserver.Insert(0, n2);
                IsEnableLstView = true;
                _viewModelParent.AtualizarDadosPendencias();
                SelectListViewIndex = 0;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true);

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
            try
            {
                Entity = new EmpresaAnexoView();
                Comportamento.PrepareCriar();
                IsEnableLstView = false;
                _viewModelParent.HabilitaControleTabControls(false, false, false, false, true);
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
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

                if (Validar()) return;
                var n1 = Mapper.Map<EmpresaAnexo>(Entity);
                _service.Alterar(n1);
                IsEnableLstView = true;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true);
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
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true);
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

                var n1 = Mapper.Map<EmpresaAnexo>(Entity);
                _service.Remover(n1);
                //Retirar empresa da coleção
                EntityObserver.Remove(Entity);
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true);
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
            try
            {
                if (Entity == null)
                {
                    WpfHelp.PopupBox("Selecione um item da lista", 1);
                    return;
                }

                Comportamento.PrepareAlterar();
                IsEnableLstView = false;
                _viewModelParent.HabilitaControleTabControls(false, false, false, false, true);
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
            }
        }

        public void AtualizarDadosAnexo(EmpresaView entity, EmpresaViewModel viewModelParent)
        {
            try
            {
                EntityObserver.Clear();
                if (entity == null) return; // throw new ArgumentNullException(nameof(entity));
                _empresaView = entity;
                _viewModelParent = viewModelParent;
                //Obter dados
                var list1 = _service.Listar(entity.EmpresaId);
                var list2 = Mapper.Map<List<EmpresaAnexoView>>(list1.OrderByDescending(n => n.EmpresaAnexoId));
                EntityObserver = new ObservableCollection<EmpresaAnexoView>();
                list2.ForEach(n => { EntityObserver.Add(n); });

                _configuraSistema = ObterConfiguracao();
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
            }
        }
        private ConfiguraSistema ObterConfiguracao()
        {
            try
            {
                //Obter configuracoes de sistema
                var config = _auxiliaresService.ConfiguraSistemaService.Listar();
                //Obtem o primeiro registro de configuracao
                if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
                return config.FirstOrDefault();
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
                return null;
            }
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
            try
            {
                if (Entity == null) return true;
                //Verificar valiade de Descricao do Anexo
                if (EInValidandoDescricao())
                {
                    Entity.SetMessageErro("Descricao", "Descrição inválida");
                    return true;
                }

                //Verificar valiade do Anexo
                if (EInValidandoAnexo())
                {
                    Entity.SetMessageErro("NomeAnexo", "Nome do Anexo é inválido");
                    return true;
                }
                var hasErros = Entity.HasErrors;
                return hasErros;
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex);
                return false;
            }
        }
        #endregion

        #region Regras de Negócio
        private bool EInValidandoDescricao()
        {
            if (Entity == null) return false;
            var descricao = Entity.Descricao;
            if (descricao == null) return true;
            return false;
        }
        private bool EInValidandoAnexo()
        {
            if (Entity == null) return false;
            var nomeAnexo = Entity.NomeAnexo;
            if (nomeAnexo == null) return true;
            return false;
        }

        #endregion
    }
}