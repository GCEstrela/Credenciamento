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
using System.Text;
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
using IMOD.CrossCutting.Entities;
using IMOD.Domain.Entities;
using EmpresaLayoutCrachaView = IMOD.Domain.EntitiesCustom.EmpresaLayoutCrachaView;

#endregion

namespace IMOD.CredenciamentoDeskTop.ViewModels
{
    public class EmpresaViewModel : ViewModelBase, IComportamento, IAtualizarDados
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();
        private readonly IEmpresaService _service = new EmpresaService();
        private List<EmpresaView> _entityObserverCloned = new List<EmpresaView>();

        private readonly IEmpresaContratosService _serviceContratos = new EmpresaContratoService();
        private ConfiguraSistema _configuraSistema;


        #region Propriedades da Classe

        public Boolean empresaFake = false;
        /// <summary>
        ///     True, Comando de alteração acionado
        /// </summary>
        private bool _prepareAlterarCommandAcionado;

        /// <summary>
        ///     True, Comando de criação acionado
        /// </summary>
        private bool _prepareCriarCommandAcionado;

        #region  Propriedades

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

        /// <summary>
        ///     True, empresa possui pendência de codigo 21
        /// </summary>
        public bool Pendencia21 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 12
        /// </summary>
        public bool Pendencia12 { get; set; }

        /// <summary>
        ///     True, empresa possui pendência de codigo 14
        /// </summary>
        public bool Pendencia14 { get; set; }

        /// <summary>
        /// </summary>
        public bool Pendencia24 { get; set; }

        /// <summary>
        ///     Indice da tabela de controle selecionada
        /// </summary>
        public bool IsEnableTabGeral{ get; set; } 
        public bool IsEnableTabRepresentantes { get; set; } 
        public bool IsEnableTabContratos { get; set; }
        public bool IsEnableTabSeguros { get; set; }
        public bool IsEnableTabAnexo { get; set; } 

        public bool HabilitaCommandPincipal { get; set; } = true;

        /// <summary>
        ///     Seleciona indice da listview
        /// </summary>
        public short SelectListViewIndex { get; set; }

        /// <summary>
        ///     Seleciona o indice da tabcontrol desejada
        /// </summary>
        public short SelectedTabIndex { get; set; }

        /// <summary>
        ///     Habilita listView
        /// </summary>
        public bool IsEnableLstView { get; private set; } = true;

        public EmpresaView Entity { get; set; }
        public ObservableCollection<EmpresaView> EntityObserver { get; set; }
        public ObservableCollection<EmpresaLayoutCrachaView> TiposLayoutCracha { get; set; }
        public LayoutCrachaView TipoCracha { get; set; }
        public ObservableCollection<EmpresaTipoAtividadeView> TiposAtividades { get; set; }
        public TipoAtividadeView TipoAtividade { get; set; }

        /// <summary>
        ///     LayouCrachas
        /// </summary>
        public List<LayoutCrachaView> ListaCrachas { get; set; }

        /// <summary>
        ///     Tipos de atividade
        /// </summary>
        public List<TipoAtividadeView> ListaAtividades { get; set; }

        /// <summary>
        ///     Estados
        /// </summary>
        public List<Estados> Estados { get; set; }

        public Estados Estado { get; set; }

        /// <summary>
        ///     Municipios
        /// </summary>
        public List<Municipio> Municipios { get; set; }

        /// <summary>
        ///     Dados de municipio armazendas em memoria
        /// </summary>
        public List<Municipio> _municipios { get; set; }

        public int QuantidadeTipoCredencialTemporario { get; set; }

        public int QuantidadeTipoCredencialPermanente { get; set; }
        /// <summary>
        ///     Tamanho da Imagem
        /// </summary>
        public int IsTamanhoImagem
        {
            get
            {
                return _configuraSistema.imagemTamanho;
            }
        }
        #endregion
        #endregion

        #region Construtor da Classe
        public EmpresaViewModel()
        {
            try
            {
                ItensDePesquisaConfigura();
                ListarDadosAuxiliares();
                Comportamento = new ComportamentoBasico(false, true, true, false, false);
                EntityObserver = new ObservableCollection<EmpresaView>();
                TiposAtividades = new ObservableCollection<EmpresaTipoAtividadeView>();
                TiposLayoutCracha = new ObservableCollection<EmpresaLayoutCrachaView>();
                EntityObserver = new ObservableCollection<EmpresaView>();

                Comportamento.SalvarAdicao += OnSalvarAdicao;
                Comportamento.SalvarEdicao += OnSalvarEdicao;
                Comportamento.Remover += OnRemover;
                Comportamento.Cancelar += OnCancelar;
                PropertyChanged += OnEntityChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Metodos

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntityChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Entity")

                {
                    var enableControls = Entity != null;
                    Comportamento.IsEnableEditar = Entity != null;
                    HabilitaControleTabControls(true, enableControls, enableControls, enableControls, enableControls);
                }

                if (e.PropertyName == "SelectedTabIndex")
                    //Habilita/Desabilita botoes principais...
                    HabilitaCommandPincipal = SelectedTabIndex == 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void bucarLogo(int empresa)
        {
            try
            {
                if (Entity.Logo != null) return;
                var listaFoto = _service.BuscarPelaChave(empresa);
                if (listaFoto != null)
                    Entity.Logo = listaFoto.Logo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTiposAtividades()
        {
            try
            {
                if (Entity == null) return;

                TiposAtividades.Clear();
                var id = Entity.EmpresaId;
                var list = _service.Atividade.ListarEmpresaTipoAtividadeView(null, id, null, null).ToList();
                var list2 = Mapper.Map<List<EmpresaTipoAtividadeView>>(list);
                list2.ForEach(n => TiposAtividades.Add(n));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Atualizar dados de atividade
        /// </summary>
        public void AtualizarDadosTipoCrachas()
        {
            try
            {
                if (Entity == null) return;

                TiposLayoutCracha.Clear();
                var id = Entity.EmpresaId;
                var list = _service.CrachaService.ListarLayoutCrachaPorEmpresaView(id, 0).ToList();
                var list2 = Mapper.Map<List<EmpresaLayoutCrachaView>>(list);
                list2.ForEach(n => TiposLayoutCracha.Add(n));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisaConfigura()
        {
            try
            {
                ListaPesquisa = new List<KeyValuePair<int, string>>();
                ListaPesquisa.Add(new KeyValuePair<int, string>(1, "CNPJ"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Razão Social"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(3, "Código"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Pendências"));
                ListaPesquisa.Add(new KeyValuePair<int, string>(5, "Todos"));
                PesquisarPor = ListaPesquisa[1]; //Pesquisa Default
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Busca e preenche o quantitativo de tipo de credenciais (permanentes e temporária)
        /// </summary>
        public void CarregarQuantidadeTipoCredencial()
        {
            try
            {
                if (Entity == null) return;

                var id = Entity.EmpresaId;
                var objTipocredenciaisEmpresa = _service.ListarTipoCredenciaisEmpresa(id).ToList();
                QuantidadeTipoCredencialPermanente = objTipocredenciaisEmpresa.Count(p => p.TipoCredencialId == 1 && p.Impressa == true);
                QuantidadeTipoCredencialTemporario = objTipocredenciaisEmpresa.Count(p => p.TipoCredencialId == 2 && p.Impressa == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Atualizar dados de pendências
        /// </summary>
        /// ValidarCnpj
        public void AtualizarDadosPendencias()
        {
            try
            {
                if (Entity == null) return;

                var pendencia = _service.Pendencia.ListarPorEmpresa(Entity.EmpresaId).ToList();
                //Set valores
                SetPendenciaFalse();
                //Buscar pendências referente aos códigos: 21; 12;14;24
                Pendencia21 = pendencia.Any(n => n.CodPendencia == 21 & n.Ativo);
                Pendencia12 = pendencia.Any(n => n.CodPendencia == 12 & n.Ativo);
                Pendencia14 = pendencia.Any(n => n.CodPendencia == 14 & n.Ativo);
                Pendencia24 = pendencia.Any(n => n.CodPendencia == 24 & n.Ativo);
            }
            catch (Exception ex)
            {
                throw ex;
                //Utils.TraceException(ex);
                //WpfHelp.PopupBox(ex);
            }
        }

        private void SetPendenciaFalse()
        {
            Pendencia21 = false;
            Pendencia12 = false;
            Pendencia14 = false;
            Pendencia24 = false;
        }

        /// <summary>
        ///     Listar dados auxilizares
        /// </summary>
        public void ListarDadosAuxiliares()
        {
            try
            {
                var lst1 = _auxiliaresService.LayoutCrachaService.Listar();
                var lst2 = _auxiliaresService.TipoAtividadeService.Listar();
                var lst3 = _auxiliaresService.EstadoService.Listar();

                ListaCrachas = Mapper.Map<List<LayoutCrachaView>>(lst1);
                ListaAtividades = Mapper.Map<List<TipoAtividadeView>>(lst2);
                Estados = Mapper.Map<List<Estados>>(lst3);

                //Obter configuracoes de sistema
                _configuraSistema = ObterConfiguracao();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion
        /// <summary>
        /// Obtem configuração de sistema
        /// </summary>
        /// <returns></returns>
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
                throw ex;
            }
        }

        #region Regras de Negócio

        public bool ExisteCnpj()
        {
            try
            {
                if (Entity == null) return false;
                var cnpj = Entity.Cnpj.RetirarCaracteresEspeciais();

                //Verificar dados antes de salvar uma criação
                if (_prepareCriarCommandAcionado)
                    if (_service.ExisteCnpj(cnpj)) return true;
                //Verificar dados antes de salvar uma alteraçao
                if (!_prepareAlterarCommandAcionado) return false;
                var n1 = _service.BuscarPelaChave(Entity.EmpresaId);
                if (n1 == null) return false;
                //Comparar o CNPJ antes e o depois
                //Verificar se há cnpj exisitente
                return string.Compare(n1.Cnpj.RetirarCaracteresEspeciais(),
                    cnpj, StringComparison.Ordinal) != 0 && _service.ExisteCnpj(cnpj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ExisteSigla()
        {
            try
            {
                if (Entity == null) return false;
                var sigla = Entity.Sigla?.Trim();

                //Verificar dados antes de salvar uma criação
                if (_prepareCriarCommandAcionado)
                    if (_service.ExisteSigla(sigla)) return true;
                //Verificar dados antes de salvar uma alteraçao
                if (!_prepareAlterarCommandAcionado) return false;
                var n1 = _service.BuscarPelaChave(Entity.EmpresaId);
                if (n1 == null) return false;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///     Verificar se dados válidos
        ///     <para>True, inválido</para>
        /// </summary>
        /// <returns></returns>
        private bool EInValidoCnpj()
        {
            try
            {
                if (Entity == null) return false;
                var cnpj = Entity.Cnpj.RetirarCaracteresEspeciais();
                if (!Utils.IsValidCnpj(cnpj)) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Validar Regras de Negócio
        ///     <para>True, dados válidos</para>
        /// </summary>
        public bool Validar()
        {
            if (Entity == null) return true;
            Entity.Validate();
            var hasErros = Entity.HasErrors;
            if (hasErros) return true;
            if (!empresaFake)
            {
                //Verificar valiade de cnpj
                if (EInValidoCnpj())
                {
                    Entity.SetMessageErro("Cnpj", "CNPJ inválido");
                    return true;
                }

                //Verificar existência de CNPJ
                if (ExisteCnpj())
                {
                    Entity.SetMessageErro("Cnpj", "CNPJ já existe");
                    return true;
                }
                if (ExisteSigla())
                {
                    Entity.SetMessageErro("Sigla", "Sigla já existe");
                    return true;
                }

                if (TiposLayoutCracha.Count() <= 0)
                {
                    System.Windows.MessageBox.Show("Tipo de Crachá é obrigatório!","Credenciamnto");
                    return true;
                }

                return Entity.HasErrors;
            }
            return Entity.HasErrors;
        }

        #endregion

        #region Commands

        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Listar Municipios
        /// </summary>
        /// <param name="uf"></param>
        public void ListarMunicipios(string uf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (uf)) return;

                if (Municipios == null) Municipios = new List<Municipio>();

                if (_municipios == null) _municipios = new List<Municipio>();

                if (Estado == null) return;

                //Verificar se há municipios já carregados...
                var l1 = _municipios.Where (n => n.Uf == uf);
                Municipios.Clear();
                //Nao havendo municipios... obter do repositorio
                if (!l1.Any())
                {
                    var l2 = _auxiliaresService.MunicipioService.Listar (null, uf);
                    _municipios.AddRange (Mapper.Map<List<Municipio>> (l2));
                }

                var municipios = _municipios.Where (n => n.Uf == uf).ToList();
                Municipios = municipios;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase (PrepareCriar, true);

        private void PrepareCriar()
        {
            try
            {
                this.empresaFake = false;
                Entity = new EmpresaView();
                ListarDadosAuxiliares();
                _prepareCriarCommandAcionado = true;
                Comportamento.PrepareCriar();
                _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
                TiposLayoutCracha.Clear();
                TiposAtividades.Clear();
                QuantidadeTipoCredencialTemporario = 0;
                QuantidadeTipoCredencialPermanente = 0;
                HabilitaControle(false, false);
                CloneObservable();
                SetPendenciaFalse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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


        public ICommand PrepareSenhaWebCommand => new CommandBase(GerarSenhaWeb, true);


        #endregion

        #region Salva Dados

        public void Pesquisar()
        {
            try
            { 
                var pesquisa = NomePesquisa;
                var num = PesquisarPor;

                //Por CNPJ
                if (num.Key == 1)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;
                    var l1 = _service.Listar (null, null, pesquisa);
                    PopularObserver (l1);
                }

                //Por Razão Social
                if (num.Key == 2)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;
                    var l1 = _service.Listar ($"%{pesquisa}%", null, null);
                    PopularObserver (l1);
                }

                //Por código
                if (num.Key == 3)
                {
                    if (string.IsNullOrWhiteSpace (pesquisa)) return;

                    var cod = 0;
                    int.TryParse (pesquisa, out cod);
                    var n1 = _service.BuscarPelaChave (cod);
                    if (n1 == null) return;

                    EntityObserver.Clear();
                    var n2 = Mapper.Map<EmpresaView> (n1);
                   
                    var observer = new ObservableCollection<EmpresaView>();
                    observer.Add (n2);
                    EntityObserver = observer;
                }

                //com pendências
                if (num.Key == 4)
                {
                    //if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    var l1 = _service.ListarEmpresasPendentes($"%{pesquisa}%", null, null);
                    PopularObserver(l1);
                }

                //Todos
                if (num.Key == 5)
                {
                    var l1 = _service.Listar();
                    PopularObserver (l1);
                }

                IsEnableLstView = true;
            }
            catch (Exception ex)
            {
                WpfHelp.PopupBox(ex.Message, 1);
                Utils.TraceException(ex);
                EntityObserver.Clear();
            }
        }

        private void PopularObserver(ICollection<Empresa> list)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                //var contrato = _serviceContratos.Listar().OrderByDescending(ec => ec.PraVencer).GroupBy(ec => ec.EmpresaId).ToList();

                var list2 = Mapper.Map<List<EmpresaView>> (list.OrderBy(n => n.Nome));
                
                EntityObserver = new ObservableCollection<EmpresaView>();               
                list2.ForEach (n => { EntityObserver.Add (n); });
                //Havendo registros, selecione o primeiro
                //if (EntityObserver.Any()) SelectListViewIndex = 0;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
            }

            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.IBeam;
                Utils.TraceException (ex);
            }
        }

        private void PrepareSalvar()
        {
            try
            {
                //if(!empresaFake)
                if (Validar()) return;

                Comportamento.PrepareSalvar();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }

        }

        private void PrepareAlterar()
        {
            try
            {
                if (Entity == null)
                {
                    WpfHelp.PopupBox("Selecione um item da lista", 1);
                    return;
                }
                //ListarDadosAuxiliares();
                Comportamento.PrepareAlterar();
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = !_prepareCriarCommandAcionado;
                AtualizarDadosTiposAtividades();
                AtualizarDadosTipoCrachas();
                HabilitaControle(false, false);
                CloneObservable();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.PopupBox(ex);
            }
        }

        private void PrepareRemover()
        {
            try
            {
                if (Entity == null) return;

                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                Comportamento.PrepareRemover();
                HabilitaControle(true, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<Empresa> (Entity);
                var status = _auxiliaresService.StatusService.Listar().FirstOrDefault (n => n.CodigoStatus);

                _service.CriarContrato(n1, DateTime.Now.Date, "0", status, _configuraSistema);

                //Salvar Tipo de Atividades
                SalvarTipoAtividades (n1.EmpresaId);
                //Salvar Tipo Cracha
                SalvarTipoCracha (n1.EmpresaId);
                //Adicionar no inicio da lista um item a coleção
                var n2 = Mapper.Map<EmpresaView> (n1);
                EntityObserver.Insert (0, n2);
                HabilitaControle (true, true);
                SelectListViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        private void SalvarTipoAtividades(int empresaId)
        {
            //Remover
            _service.Atividade.RemoverPorEmpresa (empresaId);
            //Adicionar
            var lst = TiposAtividades.ToList();
            lst.ForEach (n =>
            {
                var n1 = Mapper.Map<EmpresaTipoAtividade> (n);
                n1.EmpresaId = empresaId;
                _service.Atividade.Criar (n1);
            });

        }

        private void SalvarTipoCracha(int empresaId)
        {
            try
            {
                //Remover
                _service.CrachaService.RemoverPorEmpresa(empresaId);
                //Adicionar
                var lst = TiposLayoutCracha.ToList();
                lst.ForEach(n =>
                {
                    var n1 = Mapper.Map<EmpresaLayoutCracha>(n);
                    n1.EmpresaId = empresaId;
                    _service.CrachaService.Criar(n1);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (Entity == null) return;
                if (Validar()) return;

                var n1 = Mapper.Map<Empresa> (Entity);
                _service.Alterar (n1);
                //Salvar Tipo de Atividades
                SalvarTipoAtividades (n1.EmpresaId);
                //Salvar Tipo Cracha
                SalvarTipoCracha (n1.EmpresaId);
                HabilitaControle (true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.PopupBox (ex);
            }
        }

        /// <summary>
        ///     Habilita controles
        /// </summary>
        /// <param name="isEnableTabItem"></param>
        /// <param name="isEnableLstView"></param>
        private void HabilitaControle(bool isEnableTabItem, bool isEnableLstView)
        {
            HabilitaControleTabControls(isEnableLstView, isEnableTabItem, isEnableTabItem, isEnableTabItem, isEnableTabItem);
            IsEnableLstView = isEnableLstView;
        }

        public void HabilitaControleTabControls(bool lstViewSuperior = true, bool isItemGeral = true,
        bool isItemRpresentantes = false, bool isItemContratos = false, bool isItemSeguros = false, bool isItemAnexo = false)
        {
            IsEnableLstView = lstViewSuperior;

            IsEnableTabGeral = isItemGeral;
            IsEnableTabRepresentantes = isItemRpresentantes;
            IsEnableTabContratos = isItemContratos;
            IsEnableTabSeguros = isItemSeguros;
            IsEnableTabAnexo = isItemAnexo;
            Comportamento.IsEnableCriar = lstViewSuperior;
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
                _prepareCriarCommandAcionado = false;
                _prepareAlterarCommandAcionado = false;
                if (Entity != null) Entity.ClearMessageErro();

                AtualizarDadosTipoCrachas();
                AtualizarDadosTiposAtividades();

               
                TiposAtividades.Clear();
                TiposLayoutCracha.Clear();
                HabilitaControle (true, true);
                Entity = null;

                EntityObserver.Clear();
                EntityObserver = new ObservableCollection<EmpresaView>(_entityObserverCloned);
            }
            catch (Exception ex)
            {
                //throw ex;
                Utils.TraceException(ex);
               // WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        /// <summary>
        /// Clone Observable
        /// </summary>
        private void CloneObservable()
        {
            try
            {
                _entityObserverCloned.Clear();
                EntityObserver.ToList().ForEach(n => { _entityObserverCloned.Add((EmpresaView)n.Clone()); });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Entity == null) return;

                var result = WpfHelp.MboxDialogRemove();
                if (result != DialogResult.Yes) return;

                var n1 = Mapper.Map<Empresa> (Entity);
                _service.Remover (n1);
                //Retirar empresa da coleção
                EntityObserver.Remove (Entity);
                HabilitaControle (true, true);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                WpfHelp.MboxError ("Não foi realizar a operação solicitada", ex);
            }
        }

        private void GerarSenhaWeb()
        {
            if (Entity == null) return;
            Entity.Senha = RandomString(6, Entity);
            var n1 = Mapper.Map<Empresa>(Entity);
            _service.Alterar(n1);

            EnviarEmailSenha();
        }

        private void EnviarEmailSenha()
        {
            try
            {
                ConfiguraSistema configSistema = ObterConfiguracao();
                Email email = new Email();
                email.Assunto = string.Format("{0}  - Acesso ao pré-cadastro do credenciamento web".ToUpper(), configSistema.NomeAeroporto);
                email.Usuario = configSistema.EmailUsuario.Trim();
                email.Senha = configSistema.EmailSenha.Trim();
                email.ServidorEmail = configSistema.SMTP.Trim();
                email.Porta = "587";       // porta para SSL                                   
                email.UsarSsl = false; // GMail requer SSL         
                                      //email.UsarTls = true;
                email.UsarAutenticacao = false;
                email.EmailDestinatario = Entity.Email1.Split(';').ToList();
                email.EmailRemetente = configSistema.EmailUsuario;
                email.NomeRemetente = configSistema.NomeAeroporto;

                // seu usuário e senha para autenticação

                email.NomeRemetente = configSistema.NomeAeroporto;

                StringBuilder emailFraport = new StringBuilder();
                emailFraport.AppendLine("Prezado usuário,");
                emailFraport.AppendLine(string.Empty);
                emailFraport.AppendLine(string.Format("Sua senha para acesso ao portal de pré-cadastro web do credenciamento é: <b>{0}</b>", Entity.Senha));
                emailFraport.AppendLine("O acesso deve ser realizado através do link:" + configSistema.UrlSistemaPreCadastro);
                emailFraport.AppendLine("");
                emailFraport.AppendLine("Att:");
                emailFraport.AppendLine("");
                emailFraport.AppendLine("Sistema de Credenciamento.");
                emailFraport.AppendLine("Setor de Credenciamento - Fraport-Brasil");

                email.Mensagem = emailFraport.ToString();

                Utils.EnviarEmail(email, configSistema.NomeAeroporto);

                WpfHelp.Mbox("Senha enviada para o email: " + Entity.Email1);
            }
            catch (Exception ex)
            {

                WpfHelp.MboxError(ex);
            }
            
        }


        private static Random random = new Random();
        public static string RandomString(int length, EmpresaView entity)
        {
            string chars = string.Format("@!&*#_%{0}{1}", "AbCdEfGhIjKlMnOpQrStUvWxYz", DateTime.Now.Ticks);
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}