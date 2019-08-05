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
using IMOD.CredenciamentoDeskTop.Enums;
using IMOD.CredenciamentoDeskTop.Funcoes;
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
using AutorizacaoView = IMOD.Domain.EntitiesCustom.AutorizacaoView;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class VeiculosCredenciaisViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IVeiculoCredencialService _service = new VeiculoCredencialService();
        private readonly IVeiculoEmpresaService _veiculoEmpresaService = new VeiculoEmpresaService();
        private readonly IEmpresaContratosService _contratosService = new EmpresaContratoService();
        private VeiculoViewModel _viewModelParent;
        private readonly ConfiguraSistema _configuraSistema;
        private readonly IEmpresaService serviceEmpresa = new EmpresaService();

        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;
        private List<CredencialMotivo> _credencialMotivo;

        private VeiculoView _veiculoView;
        /// <summary>
        ///     Lista de todos os contratos disponíveis
        /// </summary>
        private readonly List<VeiculoEmpresa> _todosContratosEmpresas = new List<VeiculoEmpresa>();

        #region  Propriedades
        /// <summary>
        ///     Habilitar Controles
        /// </summary>
        public bool Habilitar { get; set; }
        /// <summary>
        ///     Mensagem de alerta
        /// </summary>
        public string MensagemAlerta { get; private set; }
        /// <summary>
        ///     Habilitar impressao de credencial com base no status da credencial
        ///     e condição de pendencia impeditiva
        /// </summary>
        public bool HabilitaImpressao { get; set; }
        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }
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
                    var lst = _credencialMotivo.Where(n => n.CodigoStatus == StatusCredencial.Codigo);
                    return lst.ToList();
                }

            }
            set { _credencialMotivo = value; }
        }
        public List<FormatoCredencial> FormatoCredencial { get; set; }
        public List<TipoCredencial> TipoCredencial { get; set; }
        public List<EmpresaLayoutCracha> EmpresaLayoutCracha { get; set; }
        public List<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public ObservableCollection<VeiculoEmpresa> VeiculosEmpresas { get; set; }
        public VeiculoEmpresa VeiculoEmpresa { get; set; }
        public List<AreaAcesso> VeiculoPrivilegio { get; set; }
        public VeiculosCredenciaisView Entity { get; set; }
        public ObservableCollection<VeiculosCredenciaisView> EntityObserver { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; set; } = true;

        #endregion

        public VeiculosCredenciaisViewModel()
        {
            //ItensDePesquisaConfigura();
            ListarDadosAuxiliares();
            Comportamento = new ComportamentoBasico(false, true, false, false, false);
            EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
            PropertyChanged += OnEntityChanged;
            SelectListViewIndex = -1;
            _configuraSistema = ObterConfiguracao();
        }

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
            var n1 = _service.BuscarPelaChave (Entity.VeiculoId);
            if (n1 == null) return false;
            //Comparar o CNPJ antes e o depois
            //Verificar se há cnpj exisitente
            return string.Compare (n1.NumeroCredencial.RetirarCaracteresEspeciais(),
                numCredencial, StringComparison.Ordinal) != 0 && _service.ExisteNumeroCredencial (numCredencial);
        }

        #endregion

        public void ObterValidade()
        {
            if (!_prepareCriarCommandAcionado) return;
            if (Entity == null) return;
            var empContratoId = VeiculoEmpresa.EmpresaContratoId;
            var contrato = _contratosService.BuscarPelaChave(empContratoId);
            var data = _service.ObterDataValidadeCredencial(Entity.TipoCredencialId,
                _veiculoView.EquipamentoVeiculoId, contrato.NumeroContrato, _service.TipoCredencial);

            TimeSpan diferenca = Convert.ToDateTime(data) - DateTime.Now.Date;
            int credencialDias = int.Parse(diferenca.Days.ToString());

            Entity.Validade = credencialDias > Constantes.Constantes.diasPorAno ? DateTime.Now.Date.AddDays(Constantes.Constantes.diasPorAno) : data;
            OnPropertyChanged("Entity");

        }

        private void ListarDadosAuxiliares()
        {
            var lst0 = _auxiliaresService.CredencialStatusService.Listar();
            CredencialStatus = new List<CredencialStatus>();
            CredencialStatus.AddRange (lst0.OrderBy(n => n.Descricao));

            var lst2 = _auxiliaresService.FormatoCredencialService.Listar();
            FormatoCredencial = new List<FormatoCredencial>();
            FormatoCredencial.AddRange (lst2.OrderBy(n => n.Descricao));

            var lst3 = _auxiliaresService.TipoCredencialService.Listar();
            TipoCredencial = new List<TipoCredencial>();
            TipoCredencial.AddRange (lst3.OrderBy(n => n.Descricao));

            var lst5 = _auxiliaresService.TecnologiaCredencialService.Listar();
            TecnologiasCredenciais = new List<TecnologiaCredencial>();
            TecnologiasCredenciais.AddRange (lst5.OrderBy(n => n.Descricao));

            VeiculosEmpresas = new ObservableCollection<VeiculoEmpresa>();

             var lst7 = _auxiliaresService.AreaAcessoService.Listar();
            VeiculoPrivilegio = new List<AreaAcesso>();
            VeiculoPrivilegio.AddRange (lst7.OrderBy(n => n.Descricao));

            _credencialMotivo = new List<CredencialMotivo>();
            var lst8 = _auxiliaresService.CredencialMotivoService.Listar();
            _credencialMotivo.AddRange(lst8.OrderBy(n => n.Descricao));
        }

        public void CarregaColecaoLayoutsCrachas(int empresaId,int tipoCracha)
        {
            try
            {
                EmpresaLayoutCracha = new List<EmpresaLayoutCracha>();
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView (empresaId, tipoCracha);
                var list2 = Mapper.Map<List<EmpresaLayoutCracha>> (list1);
                EmpresaLayoutCracha = list2;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
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
                WpfHelp.PopupBox ("Selecione um item da lista", 1);
                return;
            }

            Comportamento.PrepareAlterar();
            _prepareCriarCommandAcionado = false;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false; 
            //Habilitar controles somente se a credencial não estiver sido impressa
            Habilitar = !Entity.Impressa;
            _viewModelParent.HabilitaControleTabControls(false, false, false, false, false, true);
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
                AtualizarMensagem(Entity);
            }
        }

        private int _count;

        public void AtualizarDados(VeiculoView entity, VeiculoViewModel viewModelParent) 
        {
            EntityObserver.Clear();
            if (entity == null) return; // throw new ArgumentNullException (nameof (entity)); 
            _veiculoView = entity;
            _viewModelParent = viewModelParent;
            //Obter dados
            var list1 = _service.ListarView (entity.EquipamentoVeiculoId, null, null, null, null).ToList();
            var list2 = Mapper.Map<List<VeiculosCredenciaisView>> (list1.OrderByDescending (n => n.VeiculoCredencialId));
            EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
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
                var l2 = _veiculoEmpresaService.Listar().ToList();
                _todosContratosEmpresas.Clear();
                _todosContratosEmpresas.AddRange(l2);
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        private void ListarTodosContratos()
        {
            VeiculosEmpresas.Clear();
            _todosContratosEmpresas.ForEach(n => { VeiculosEmpresas.Add(n); });
        }

        /// <summary>
        /// Obter novos dados de contratos ativos
        /// </summary>
        private void OnAtualizarDadosContratosAtivos()
        {
            //Obter todos os contratos vinculados ao colaborador...
            ObterContratos();
            ListarTodosContratoPorColaboradorAtivos(_veiculoView.EquipamentoVeiculoId);
        }
        /// <summary>
        ///     Listar contratos ativos
        /// </summary>
        /// <param name="veiculoId"></param>
        private void ListarTodosContratoPorColaboradorAtivos(int veiculoId)
        {
            VeiculosEmpresas.Clear();
            var lst2 = _todosContratosEmpresas.Where(n => n.VeiculoId == veiculoId & n.Ativo).ToList();
            lst2.ForEach(n => { VeiculosEmpresas.Add(n); });
        }

        ///// <summary>
        /////     Relação dos itens de pesauisa
        ///// </summary>
        //private void ItensDePesquisaConfigura()
        //{
        //    ListaPesquisa = new List<KeyValuePair<int, string>>();
        //    ListaPesquisa.Add (new KeyValuePair<int, string> (1, "Nome"));
        //    ListaPesquisa.Add (new KeyValuePair<int, string> (2, "CPF"));
        //    PesquisarPor = ListaPesquisa[0]; //Pesquisa Default
        //}

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
                if (Validar()) return;

                var n1 = Mapper.Map<VeiculoCredencial> (Entity);

                n1.CredencialMotivoId = Entity.CredencialMotivoId;
                n1.CredencialStatusId = Entity.CredencialStatusId;
                n1.FormatoCredencialId = Entity.FormatoCredencialId;
                n1.LayoutCrachaId = Entity.LayoutCrachaId;
                n1.TecnologiaCredencialId = Entity.TecnologiaCredencialId;
                n1.TipoCredencialId = Entity.TipoCredencialId;
                n1.Validade = Entity.Validade.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59); //Sempre Add 23:59:59 horas à credencial nova.
                if (n1.Validade <= DateTime.Now)
                {
                    WpfHelp.Mbox("Data de Validade da Autorização é inferior à data atual.", MessageBoxIcon.Information);
                    MensagemAlerta = "";

                    Comportamento.PrepareCriarSegundaTentativa();
                    return;
                }
                //Criar registro no banco de dados e setar uma data de validade
                _prepareCriarCommandAcionado = false;
                _service.Criar (n1);
                IsEnableLstView = true;
                SelectListViewIndex = 0;

                var list1 = _service.ListarView (_veiculoView.EquipamentoVeiculoId, null, null, null, null).ToList();
                var list2 = Mapper.Map<List<VeiculosCredenciaisView>> (list1.OrderByDescending (n => n.VeiculoCredencialId));
                EntityObserver = new ObservableCollection<VeiculosCredenciaisView>();
                list2.ForEach (n => { EntityObserver.Add (n); });
                MensagemAlerta = "";
                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true); 
                
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
            if (config == null) throw new InvalidOperationException("Não foi possivel obter dados de configuração do sistema.");
            return config.FirstOrDefault();
        }

        /// <summary>
        ///     Acionado antes de criar
        /// </summary>
        private void PrepareCriar()
        {
            Entity = new VeiculosCredenciaisView();
            Entity.Ativa = true;
            var statusCred = CredencialStatus.FirstOrDefault(n => n.Codigo == "1");//Status ativa
            if (statusCred == null) throw new InvalidOperationException("O status da credencial é requerida.");
            StatusCredencial = statusCred;

            Comportamento.PrepareCriar();
            _prepareCriarCommandAcionado = true;
            _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
            IsEnableLstView = false;
            Habilitar = true;
            //Listar Colaboradores Ativos
            OnAtualizarDadosContratosAtivos();
            _viewModelParent.HabilitaControleTabControls(false, false, false, false, false, true);
        }
        private void AtualizarMensagem(VeiculosCredenciaisView entity)
        {
            try
            {
                MensagemAlerta = string.Empty;
                if (entity == null) return;
                if (entity.VeiculoCredencialId <= 0) return;

                #region Habilitar botão de impressao e mensagem ao usuario

                var pendenciaImpeditivaEmpresa = serviceEmpresa.Pendencia.ListarPorEmpresa(entity.EmpresaId).Where(n => n.Impeditivo == true && n.Ativo == true).ToList();
                var pendenciaImpeditivaVeiculo = serviceEmpresa.Pendencia.ListarPorVeiculo(entity.VeiculoId).Where(n => n.Impeditivo == true && n.Ativo == true).ToList();

                HabilitaImpressao = entity.Ativa & !entity.PendenciaImpeditiva & !entity.Impressa && entity.Validade >= DateTime.Now.Date && (pendenciaImpeditivaEmpresa.Count <= 0) && (pendenciaImpeditivaVeiculo.Count <= 0);
                
                //Verificar se a empresa esta impedida
                var mensagem1 = "";
                var mensagem2 = "";
                var mensagem3 = "";
                var mensagem4 = "";
                var n1 = _service.BuscarCredencialPelaChave(entity.VeiculoCredencialId);
                if (n1 != null)
                {
                    HabilitaImpressao = n1.Ativa & !n1.PendenciaImpeditiva & !n1.Impressa && entity.Validade >= DateTime.Now.Date && (pendenciaImpeditivaEmpresa.Count <= 0) && (pendenciaImpeditivaVeiculo.Count <= 0);
                    mensagem1 = !n1.Ativa ? "Autorização Inativa" : string.Empty;
                    mensagem2 = n1.PendenciaImpeditiva ? "Pendência Impeditiva (consultar dados da empresa na aba Geral)" : string.Empty;
                    mensagem3 = n1.Impressa ? "Autorização já foi impressa" : string.Empty;
                    mensagem4 = (entity.Validade < DateTime.Now.Date) ? "Autorização vencida." : string.Empty;
                    //Exibir mensagem de impressao de credencial, esta tem prioridade sobre as demais regras
                    //if (n1.Impressa) return;
                    Entity.DevolucaoEntregaBo = n1.DevolucaoEntregaBo;
                    Entity.DataStatus = n1.DataStatus;
                    Entity.Impressa = n1.Impressa;
                    Entity.Emissao = n1.Emissao;
                }


                if (!string.IsNullOrEmpty(mensagem1 + mensagem2 + mensagem3 + mensagem4))
                {
                    MensagemAlerta = $"A autorização não poderá ser impressa pelo seguinte motivo: ";
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
                CollectionViewSource.GetDefaultView(EntityObserver).Refresh();
            }
            catch (Exception)
            {

                //throw;
            }
            
            //================================================================================
            #endregion

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

                var n1 = Mapper.Map<VeiculoCredencial> (Entity);
                
                //Alterar o status do titular do cartão
                _service.AlterarStatusTitularCartao (new CredencialGenetecService (Main.Engine), Entity, n1);
                //===================================================
                //Atualizar dados a serem exibidas na tela de empresa
                if (Entity == null) return;
                _service.CriarPendenciaImpeditiva(Entity);
                var view = new ViewSingleton().EmpresaView;
                var dados = view.DataContext as IAtualizarDados;
                dados.AtualizarDadosPendencias();
                //===================================================
                //Atualizar observer
                CollectionViewSource.GetDefaultView (EntityObserver).Refresh();
                IsEnableLstView = true;
                AtualizarMensagem(Entity);
                //===================================================
                Entity = null;
                _viewModelParent.HabilitaControleTabControls(true, true, true, true, true, true);
               

            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        /// Criar CardHolder e Credencial do usuario
        /// <para>Criar um card holder caso o usuario nao o possua</para>
        /// </summary>
        /// <param name="veiculoCredencialId">Identificador</param>
        private void GerarCardHolder(int veiculoCredencialId, VeiculosCredenciaisView entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var n1 = _service.BuscarCredencialPelaChave(veiculoCredencialId);

            var tecCredencial = _auxiliaresService.TecnologiaCredencialService.BuscarPelaChave(entity.TecnologiaCredencialId);
            if (tecCredencial.PodeGerarCardHolder)
                _service.CriarTitularCartao(new CredencialGenetecService(Main.Engine), new VeiculoService(), n1);
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

                var n1 = Mapper.Map<VeiculoCredencial> (Entity);
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
        ///     Pesquisar
        /// </summary>
        private void Pesquisar()
        {
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

                var tipoEquipamentoService = _auxiliaresService.EquipamentoVeiculoTipoServicoService.ListarEquipamentoVeiculoTipoServicoView(Entity.VeiculoId).ToList();

                string _servicoAutorizacao = ""; 

                foreach (IMOD.Domain.EntitiesCustom.EquipamentoVeiculoTipoServicoView element in tipoEquipamentoService)
                {
                    if (_servicoAutorizacao == "")
                    {
                        _servicoAutorizacao = !String.IsNullOrEmpty(element.Descricao) ? " - " + element.Descricao?.ToString() : "";
                    }
                    else
                    {
                        _servicoAutorizacao = _servicoAutorizacao + Environment.NewLine + " - " + element.Descricao.ToString();
                    }
                }

                var arrayBytes = WpfHelp.ConverterBase64 (layoutCracha.LayoutRpt, "Layout de Autorização");
                var relatorio = WpfHelp.ShowRelatorioCrystalReport (arrayBytes, layoutCracha.Nome);
                var lst = new List<Views.Model.AutorizacaoView>();
                var credencialView = _service.ObterCredencialView (Entity.VeiculoCredencialId);

                credencialView.TipoServico = _servicoAutorizacao;
                var AutorizacaoMapeada = Mapper.Map<Views.Model.AutorizacaoView>(credencialView);
                AutorizacaoMapeada.Emissao = DateTime.Now;
                lst.Add (AutorizacaoMapeada); 

                relatorio.SetDataSource (lst);

                var objCode = new QrCode();
                string querySistema = _configuraSistema.UrlSistema?.Trim().ToString() + "/Veiculo/Credential/"
                            + Helpers.Helper.Encriptar(AutorizacaoMapeada.VeiculoCredencialId.ToString());

                var pathImagem = objCode.GerarQrCode(querySistema, "QrCodeAutorizacao" + AutorizacaoMapeada.VeiculoCredencialId.ToString() + ".png");
                relatorio.SetParameterValue("PathImgQrCode", pathImagem);

                var popupCredencial = new PopupAutorizacao (relatorio, _service, Entity, layoutCracha);
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
        ///     Validar Regras de Negócio
        /// </summary>
        /// <returns></returns>
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
    }
}