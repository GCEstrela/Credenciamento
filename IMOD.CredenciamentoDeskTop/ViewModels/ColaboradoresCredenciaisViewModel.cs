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

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class ColaboradoresCredenciaisViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IColaboradorEmpresaService _colaboradorEmpresaService = new ColaboradorEmpresaService();
        private readonly IColaboradorCredencialService _service = new ColaboradorCredencialService();
        private readonly IEmpresaContratosService _contratosService = new EmpresaContratoService();
        private ColaboradorView _colaboradorView; 
        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;
        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;
        private List<CredencialMotivo> _credencialMotivo;

        #region  Propriedades

        private bool ObterDataValidade {get {return !IsEnableLstView; } }
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
                if (StatusCredencial == null)
                {
                    return _credencialMotivo;
                }
                else
                {
                     var lst = _credencialMotivo.Where (n => n.CodigoStatus == StatusCredencial.Codigo);
                    return lst.ToList();
                }

            }
            set { _credencialMotivo = value; }
        }
        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public List<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }
        public ColaboradorEmpresa ColaboradorEmpresa { get; set; } 
        public List<AreaAcesso> ColaboradorPrivilegio { get; set; }
        public ColaboradoresCredenciaisView Entity { get; set; }
        /// <summary>
        /// Mensagem de alerta
        /// </summary>
        public string MensagemAlerta { get; private set; }

        /// <summary>
        ///     Habilitar impressao de credencial com base no status da credencial
        ///     e condição de pendencia impeditiva
        /// </summary>
        public bool HabilitaImpressao
        {
            get
            {
                var entity = Entity;
                if (entity == null) return false;
                var habilita = entity.Ativa & !entity.PendenciaImpeditiva;

                if (habilita)
                {
                    MensagemAlerta = "";
                }
                else
                {
                    var mensagem1 = !entity.Ativa ? "Credencial Inativa" : string.Empty;
                    var mensagem2 = entity.PendenciaImpeditiva ? "Pendência Impeditiva (consultar dados da empresa na aba Geral)" : string.Empty;
                    MensagemAlerta = $"A empresa está impedida de imprimir credencial pelo seguinte motivo: { mensagem1} { mensagem2}";
                }

                return habilita;
            }


        }

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

        #region  Metodos

        #region Regras de Negócio

        private bool ExisteNumeroCredencial()
        {
            if (Entity == null) return false;
            var numCredencial = Entity.NumeroCredencial.RetirarCaracteresEspeciais();

            //Verificar dados antes de salvar uma criação
            if (_prepareCriarCommandAcionado)
                if (_service.ExisteNumeroCredencial (numCredencial)) return true;
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

            ColaboradoresEmpresas = new List<ColaboradorEmpresa>();
            //_colaboradoresEmpresa = new List<ColaboradorEmpresa>();

            var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            ColaboradorPrivilegio = new List<AreaAcesso>();
            ColaboradorPrivilegio.AddRange (lst7);

           _credencialMotivo = new List<CredencialMotivo>();
           var lst8 = _auxiliaresService.CredencialMotivoService.Listar();
           _credencialMotivo.AddRange(lst8); 
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

        #endregion

        #region Inicializacao

        public ColaboradoresCredenciaisViewModel()
        {
            ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico (false, true, true, false, false);
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
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
            if (e.PropertyName == "Entity") //habilitar botão alterar todas as vezes em que houver entidade diferente de null
                Comportamento.IsEnableEditar = true; 
        }

        public void ObterValidade()
        {
            if (!_prepareCriarCommandAcionado) return;
            if (Entity == null) return;
            var empContratoId = ColaboradorEmpresa.EmpresaContratoId;
            var contrato = _contratosService.BuscarPelaChave (empContratoId);
            var data = _service.ObterDataValidadeCredencial(Entity.TipoCredencialId,
                _colaboradorView.ColaboradorId, contrato.NumeroContrato, _service.TipoCredencial);
            
            Entity.Validade = data;
              base.OnPropertyChanged ("Entity");
        }

        public void AtualizarDados(ColaboradorView entity)
        {
            if (entity == null) throw new ArgumentNullException (nameof (entity));
            _colaboradorView = entity;
            //Obter dados
            var list1 = _service.ListarView (null, null, null, null, entity.ColaboradorId).ToList();
            var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>> (list1.OrderByDescending (n => n.ColaboradorCredencialId));
            EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
            list2.ForEach (n => { EntityObserver.Add (n); });
            //Listar dados de contratos
            ListarDadosEmpresaContratos (_colaboradorView.ColaboradorId);
        }

        /// <summary>
        ///     Listar dados de empresa e contratos
        /// </summary>
        /// <param name="colaboradorId"></param>
        private void ListarDadosEmpresaContratos(int colaboradorId)
        {
            try
            {
               
                if (colaboradorId == 0) return;
                var l2 = _colaboradorEmpresaService.Listar (colaboradorId, null, null, null, null).ToList();
                //_colaboradoresEmpresa = l2;
                ColaboradoresEmpresas = l2;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
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

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);

                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;
                //Criar registro no banco de dados e setar uma data de validade
                _service.Criar (n1);
                IsEnableLstView = true;
                SelectListViewIndex = 0;

                var list1 = _service.ListarView (null, null, null, null, _colaboradorView.ColaboradorId).ToList();
                var list2 = Mapper.Map<List<ColaboradoresCredenciaisView>> (list1.OrderByDescending (n => n.ColaboradorCredencialId));
                EntityObserver = new ObservableCollection<ColaboradoresCredenciaisView>();
                list2.ForEach (n => { EntityObserver.Add (n); });
                 
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            
            Entity = new ColaboradoresCredenciaisView();
           
            
            var statusCred = CredencialStatus.FirstOrDefault (n => n.Codigo == "1");//Status ativa
            if(statusCred == null) throw new InvalidOperationException("O status da credencial é requerida.");
            StatusCredencial = statusCred;
            
            var tipoCredencial = TipoCredencial.FirstOrDefault(n=>n.CredPermanente);
            if (tipoCredencial != null) Entity.TipoCredencialId = tipoCredencial.TipoCredencialId;
            //Exibir somente Vinculos ativos
            //_colaboradoresEmpresa = _colaboradoresEmpresa.Where (n => n.Ativo).ToList();


            Entity.Ativa = true;
            Comportamento.PrepareCriar();
            _prepareCriarCommandAcionado = true;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false;
            Habilitar = true;
            ListarDadosEmpresaContratos (_colaboradorView.ColaboradorId); 
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

                var n1 = Mapper.Map<ColaboradorCredencial> (Entity);
                //Alterar o status do titular do cartão
                _service.AlterarStatusTitularCartao (new CredencialGenetecService (Main.Engine), Entity, n1);
                
                //Atualizar Dados
                if (Entity == null) return;
                _service.CriarPendenciaImpeditiva (Entity);
                var view = new ViewSingleton().EmpresaView;
                var dados = view.DataContext as IAtualizarDados;
                dados.AtualizarDadosPendencias();

                //Atualizar observer
                CollectionViewSource.GetDefaultView(EntityObserver).Refresh();
                IsEnableLstView = true;
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
                var lst = new List<CredencialView>();
                var credencialView = _service.ObterCredencialView (Entity.ColaboradorCredencialId);
                lst.Add (credencialView);
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
            Habilitar = false;
          
            ListarDadosEmpresaContratos (_colaboradorView.ColaboradorId);
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

        #endregion
    }
}