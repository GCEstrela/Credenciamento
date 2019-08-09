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
    public class EquipamentosEmpresasViewModel : ViewModelBase, IComportamento
    {
        private readonly IEmpresaContratosService _empresaContratoService = new EmpresaContratoService(); 
        private readonly IEmpresaService _empresaService = new EmpresaService(); 
        private readonly IVeiculoEmpresaService _service = new VeiculoEmpresaService(); 
        private VeiculoView _veiculoView; 
        private EquipamentosViewModel _viewModelParent; 

        private readonly IDadosAuxiliaresFacade _auxiliaresServiceConfiguraSistema = new DadosAuxiliaresFacadeService();
        private ConfiguraSistema _configuraSistema;

        #region  Propriedades
        private int _equipamentoid;
        public List<EmpresaContrato> Contratos { get; private set; }
        public List<Empresa> Empresas { get; private set; }
        public Empresa Empresa { get; set; }
        public VeiculoEmpresaView Entity { get; set; }
        public ObservableCollection<VeiculoEmpresaView> EntityObserver { get; set; }

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        public bool IsEnableComboContrato
        {
            get
            {
                return !_configuraSistema.Contrato;
            }
        }

        #endregion

        public EquipamentosEmpresasViewModel()
        {
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico (false, true, true, false, false);
            EntityObserver = new ObservableCollection<VeiculoEmpresaView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
        }

        #region  Metodos

        

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

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        private void ListarDadosAuxiliares()
        {
             
            Empresas = new List<Empresa>();
            Contratos = new List<EmpresaContrato>();
            ListarDadosEmpresaContratos();
            _configuraSistema = ObterConfiguracao();
        }

        /// <summary>
        ///  Listar dados de empresa e contratos
        /// </summary>
        private void ListarDadosEmpresaContratos()
        {
            try
            {
                Empresas.Clear();
                var l2 = _empresaService.Listar().ToList().OrderBy(m=>m.Nome);
                Empresas.AddRange(l2);
                var l3 = _empresaContratoService.Listar().ToList();
                Contratos = l3;
                base.OnPropertyChanged("Entity");
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
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

        public void ListarContratos(Empresa empresa)
        {
            if (empresa == null) return;
            var lstContratos = _empresaContratoService.Listar (empresa.EmpresaId).ToList();
            Contratos.Clear();
            //Manipular concatenaçção de contrato
            lstContratos.ForEach (n =>
            {
                n.Descricao = $"{n.Descricao} - {n.NumeroContrato}";
                Contratos.Add (n);
            });
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
                if (ExisteColaboradoContratoAtivo(_equipamentoid)) return;

                var n1 = Mapper.Map<VeiculoEmpresa> (Entity);
                n1.VeiculoId = _veiculoView.EquipamentoVeiculoId;
                if (_configuraSistema.Contrato)
                {
                    n1.EmpresaContratoId = Contratos[0].EmpresaContratoId;
                }

                _service.Criar (n1);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<VeiculoEmpresaView> (n1);
                //Adicionar o nome da empresa e o contrato
                SetDadosEmpresaContrato (n2);
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

        private void SetDadosEmpresaContrato(VeiculoEmpresaView entity)
        {
            var empresa = Empresas.FirstOrDefault (n => n.EmpresaId == entity.EmpresaId);
            var contrato = Contratos.FirstOrDefault (n => n.EmpresaContratoId == entity.EmpresaContratoId);
            if (empresa != null)
                entity.EmpresaNome = empresa.Nome; //Setar o nome da empresa para ser exibida na list view
            if (contrato != null)
                entity.Descricao = contrato.Descricao; //Setar o nome do contrato para ser exibida na list view
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            Entity = new VeiculoEmpresaView(); 
            Entity.Ativo = true; 
            Comportamento.PrepareCriar();
            IsEnableLstView = false;
            ListarDadosEmpresaContratos(); 
            _viewModelParent.HabilitaControleTabControls(false, false, true, false, false, false);
            ListarDadosEmpresaContratos();
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

                var n1 = Mapper.Map<VeiculoEmpresa> (Entity);
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
                IsEnableLstView = true;
                if (Entity != null) Entity.ClearMessageErro();
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

                var n1 = Mapper.Map<VeiculoEmpresa> (Entity);
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

        public void AtualizarDados(VeiculoView entity, EquipamentosViewModel viewModelParent)
        {
            EntityObserver.Clear();
            if (entity == null) return; // throw new ArgumentNullException (nameof (entity));

            _equipamentoid = entity.EquipamentoVeiculoId;
            _veiculoView = entity;
            _viewModelParent = viewModelParent;
            //Obter dados
            var list1 = _service.ListarContratoView (entity.EquipamentoVeiculoId);
            var list2 = Mapper.Map<List<VeiculoEmpresaView>> (list1.OrderByDescending (n => n.VeiculoEmpresaId).ToList());
            EntityObserver = new ObservableCollection<VeiculoEmpresaView>();
            list2.ForEach (n => { EntityObserver.Add (n); });
             
        }
        public bool ExisteColaboradoContratoAtivo(int _veiculo)
        {
            try
            {
                //Verificar se existe numero de contrato
                var n1 = _service.Listar(_equipamentoid, Entity.Ativo, null, null, null, null, Entity.EmpresaContratoId);
                if (n1 != null && n1.Count() > 0)
                {
                    WpfHelp.Mbox("Equipamento já esta vinculado à este contrato. Operação cancelada.");
                    //System.Windows.MessageBox.Show("Colaborador já esta vinculado à este contrato. Operação cancelada.");
                    Comportamento.PrepareCancelar();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
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

        private void PrepareSalvar()
        {
            if (Validar()) return;
            Comportamento.PrepareSalvar();
        }

        private void PrepareAlterar()
        {
            if (Entity == null)
            {
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            IsEnableLstView = false;
            _viewModelParent.HabilitaControleTabControls(false, false, true, false, false, false);
        }

        /// <summary>
        ///     Validar Regras de Negócio
        /// </summary>
        /// <returns></returns>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;

            return Entity.HasErrors;
        }

        #endregion
    }
}