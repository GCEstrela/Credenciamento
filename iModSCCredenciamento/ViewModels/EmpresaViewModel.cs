// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using iModSCCredenciamento.Enums;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Views.Model;
using iModSCCredenciamento.Windows;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.CrossCutting;
using IMOD.Domain.Entities; 
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.ViewModels.Commands;
using iModSCCredenciamento.ViewModels.Comportamento;
using Application = System.Windows.Application;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresaViewModel : ViewModelBase, IComportamento
    {
        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        private readonly IEmpresaService _service = new EmpresaService();

        #region  Propriedades

        /// <summary>
        /// String contendo o nome a pesquisa;
        /// </summary>
        public  string NomePesquisa { get; set; }
        /// <summary>
        /// Opções de pesquisa
        /// </summary>
        public List<KeyValuePair<int, string>> ListaPesquisa { get; private set; }

        /// <summary>
        /// Pesquisar por
        /// </summary>
        public KeyValuePair<int,string> PesquisarPor { get; set; }

        //**********************************
        /// <summary>
        ///     True, empresa possui pendências
        /// </summary>
        public bool Pendencias { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Geral
        /// </summary>
        public bool PendenciaGeral { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Represenante
        /// </summary>
        public bool PendenciaRepresentante { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Contrato
        /// </summary>
        public bool PendenciaContrato { get; set; }

        /// <summary>
        ///     True, empresa possui pendência na aba Anexo
        /// </summary>
        public bool PendenciaAnexo { get;  set; }

        ///// <summary>
        ///// Habilita aba Geral
        ///// </summary>
        //public bool IsEnableTabGeral { get; }

        public EmpresaView Empresa { get; set; }
        public ObservableCollection<EmpresaView> Empresas { get; set; }

        #endregion

        #region Inicializacao

        public EmpresaViewModel()
        {
            //CarregaColecoesIniciais();
            ListarTodos();
            ItensDePesquisa();
            Comportamento = new ComportamentoBasico(true, true, true, true, true);
            Comportamento.SalvarAdicao += OnSalvarAdicao;
            Comportamento.SalvarEdicao += OnSalvarEdicao;
            Comportamento.Remover += OnRemover;
            Comportamento.Cancelar += OnCancelar;
        }
 

        #endregion

        #region  Metodos

        public void AtualizarDadosPendencias()
        {
            if (Empresa == null) return;
            var pendencia = _service.Pendencia.ListarPorEmpresa (Empresa.EmpresaId).ToList();
            //Set valores
            PendenciaGeral = false;
            PendenciaRepresentante = false;
            PendenciaContrato = false;
            PendenciaAnexo = false;
            //Buscar pendências referente aos códigos: 21; 12;14;24
            PendenciaGeral = pendencia.Any (n => n.CodPendencia == 21);
            PendenciaRepresentante = pendencia.Any (n => n.CodPendencia == 12);
            PendenciaContrato = pendencia.Any (n => n.CodPendencia == 14);
            PendenciaAnexo = pendencia.Any (n => n.CodPendencia == 24);
            //Indica se a empresa possue pendências
            Pendencias = PendenciaGeral || PendenciaRepresentante || PendenciaContrato || PendenciaAnexo;
        }

        /// <summary>
        /// Relação dos itens de pesauisa
        /// </summary>
        private void ItensDePesquisa()
        {
            ListaPesquisa = new List<KeyValuePair<int, string>>();
            ListaPesquisa.Add(new KeyValuePair<int, string>(1, "Razão Social"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(2, "Código"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(3, "CNPJ"));
            ListaPesquisa.Add(new KeyValuePair<int, string>(4, "Todos"));
            PesquisarPor = ListaPesquisa[0];//Pesquisa Default
        }
        
        public void ListarTodos()
        {
            try
            {
                var list1 = _service.Listar();
                PopularObserver (list1); 
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #region Comportamento
        public ComportamentoBasico Comportamento { get; set; }

        /// <summary>
        ///     Novo
        /// </summary>
        public ICommand PrepareCriarCommand => new CommandBase(PrepareCriar, true);


        private void PrepareCriar()
        {

            Comportamento.PrepareCriar();

        }

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
        public ICommand PrepareSalvarCommand => new CommandBase(Comportamento.PrepareSalvar, true);
        /// <summary>
        /// Remover
        /// </summary>
        public ICommand PrepareRemoverCommand => new CommandBase(PrepareRemover, true);

        /// <summary>
        /// Pesquisar
        /// </summary>
        public ICommand PesquisarCommand => new CommandBase(Pesquisar,true);

        #endregion

        #region Salva Dados

        private void Pesquisar()
        {
            try
            {
                var pesquisa = NomePesquisa;
                
                var num = PesquisarPor;
               
                //Por Razão Social
                if (num.Key == 1)
                {
                    var l1 = _service.Listar($"%{pesquisa}%",null,null);
                    PopularObserver(l1);
                }
                //Por código
                if (num.Key == 2)
                {
                    if (string.IsNullOrWhiteSpace(pesquisa)) return;
                    int cod = 0;
                    int.TryParse(pesquisa, out cod);
                    var n1 = _service.BuscarPelaChave(cod);
                    if (n1 == null) return;
                    Empresas.Clear();
                    var n2 = Mapper.Map<EmpresaView>(n1);
                    var observer = new ObservableCollection<EmpresaView>();
                    observer.Add(n2);
                    Empresas = observer;
                }
                //Por CNPJ
                if (num.Key == 3)
                {
                    var l1 = _service.Listar(null, null, pesquisa);
                    PopularObserver(l1);
                }
                //Todos
                if (num.Key == 4)
                {
                    var l1 = _service.Listar();
                    PopularObserver(l1);
                }


            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
           

        }

        private void PopularObserver(ICollection<Empresa> list )
        {
            try
            {
               
                var list2 = Mapper.Map<List<EmpresaView>>(list.OrderBy(n => n.Nome));
                Empresas = new ObservableCollection<EmpresaView>();
                list2.ForEach(n => { Empresas.Add(n); });
                //Empresas = observer;
            }

            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }

        private void PrepareAlterar()
        {
            if (Empresa == null) return;
            Comportamento.PrepareAlterar();
        }

        private void PrepareRemover()
        {
            if (Empresa == null) return;
            Comportamento.PrepareRemover();
        }
        private void OnSalvarAdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Empresa == null) return;
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnSalvarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Empresa == null) return;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnCancelar(object sender, RoutedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }

        private void OnRemover(object sender, RoutedEventArgs e)
        {
            try
            {
                 
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
                WpfHelp.MboxError("Não foi realizar a operação solicitada", ex);
            }
        }
        #endregion








































































































        public void ValidarAdicao(EmpresaView entity)
        {
            //if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            //if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            ValidarEdicao (entity);
            if (ConsultaCNPJ (entity.Cnpj)) throw new InvalidOperationException ("CNPJ já cadastrado");
        }

        public void ValidarEdicao(EmpresaView entity)
        {
            if (string.IsNullOrWhiteSpace (entity.Cnpj)) throw new InvalidOperationException ("Informe CNPJ");
            if (!Utils.IsValidCnpj (entity.Cnpj)) throw new InvalidOperationException ("CNPJ inválido");
            //if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");
            if (string.IsNullOrWhiteSpace (entity.Nome)) throw new InvalidOperationException ("Informe a Razão Social");
        }

        public bool ConsultaCNPJ(string doc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (doc)) throw new ArgumentNullException ("Informe um CNPJ para pesquisar");
                doc = doc.RetirarCaracteresEspeciais().Replace (" ", "");

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                var cmd = new SqlCommand ("Select * From Empresas Where CNPJ = '" + doc + "'", _Con);
                var reader = cmd.ExecuteReader (CommandBehavior.Default);
                if (reader.Read())
                {
                    _Con.Close();
                    return true;
                }
                _Con.Close();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void OnEmpresaSelecionada()
        {
            try
            {
                CarregaColecaoEmpresasTiposAtividades (EmpresaSelecionada.EmpresaId);
                CarregaColecaoEmpresasLayoutsCrachas (EmpresaSelecionada.EmpresaId);
                CarregaColecaoLayoutsCrachas();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        //**********************************

        #region Variaveis privadas

        private SynchronizationContext MainThread;

        private EmpresasSegurosViewModel _SegurosEmpresaViewModel;

        private ObservableCollection<EmpresaView> _Empresas;

        //private ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro> _Seguros;

        private readonly List<EmpresaView> _EmpresasTemp = new List<EmpresaView>();

        private readonly EmpresaView _empresaTemp = new EmpresaView();

        private ObservableCollection<EstadoView> _Estados;

        private ObservableCollection<MunicipioView> _Municipios;

        private ObservableCollection<TipoEmpresaView> _TiposEmpresa;

        private ObservableCollection<TipoAtividadeView> _TiposAtividades;

        private ObservableCollection<EmpresaTipoAtividadeView> _EmpresasTiposAtividades;

        private EmpresaTipoAtividadeView _EmpresaTipoAtividadeSelecionada;

        //private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;

        //private ObservableCollection<ClasseEmpresasAreasAcessos.EmpresaAreaAcesso> _EmpresasAreasAcessos;

        //private ClasseEmpresasAreasAcessos.EmpresaAreaAcesso _EmpresaAreaAcessoSelecionada;

        private ObservableCollection<LayoutCrachaView> _LayoutsCrachas;

        private ObservableCollection<EmpresaLayoutCrachaView> _EmpresasLayoutsCrachas;

        private EmpresaLayoutCrachaView _EmpresaLayoutCrachaSelecionada;

        private EmpresaView _EmpresaSelecionada;

        private ImageSource _LogoImageSource;

        private int _selectedIndex;

        private bool _atualizandoLogo;

        private BitmapImage _Waiting;

        private bool _HabilitaEdicao;

        private PopUpPesquisaEmpresa popupPesquisaEmpresa;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores

        public ObservableCollection<EstadoView> Estados
        {
            get { return _Estados; }

            set
            {
                if (_Estados != value)
                {
                    _Estados = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<MunicipioView> Municipios
        {
            get { return _Municipios; }

            set
            {
                if (_Municipios != value)
                {
                    _Municipios = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<TipoAtividadeView> TiposAtividades
        {
            get { return _TiposAtividades; }

            set
            {
                if (_TiposAtividades != value)
                {
                    _TiposAtividades = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<EmpresaTipoAtividadeView> EmpresasTiposAtividades
        {
            get { return _EmpresasTiposAtividades; }

            set
            {
                if (_EmpresasTiposAtividades != value)
                {
                    _EmpresasTiposAtividades = value;
                    OnPropertyChanged();
                }
            }
        }

        public EmpresaTipoAtividadeView EmpresaTipoAtividadeSelecionada
        {
            get { return _EmpresaTipoAtividadeSelecionada; }
            set
            {
                _EmpresaTipoAtividadeSelecionada = value;
                OnPropertyChanged ("SelectedItem");
                if (EmpresaTipoAtividadeSelecionada != null)
                {
                }
            }
        }

        public ObservableCollection<EmpresaLayoutCrachaView> EmpresasLayoutsCrachas //EmpresasLayoutsCrachas
        {
            get { return _EmpresasLayoutsCrachas; }

            set
            {
                if (_EmpresasLayoutsCrachas != value)
                {
                    _EmpresasLayoutsCrachas = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<LayoutCrachaView> LayoutsCrachas //
        {
            get { return _LayoutsCrachas; }

            set
            {
                if (_LayoutsCrachas != value)
                {
                    _LayoutsCrachas = value;
                    OnPropertyChanged();
                }
            }
        }

        //public ObservableCollection<EmpresaLayoutCrachaView> EmpresasLayoutsCrachas
        //{
        //    get
        //    {
        //        return _EmpresasLayoutsCrachas;
        //    }

        //    set
        //    {
        //        if (_EmpresasLayoutsCrachas != value)
        //        {
        //            _EmpresasLayoutsCrachas = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public EmpresaLayoutCrachaView EmpresaLayoutCrachaSelecionada
        //{
        //    get
        //    {
        //        return _EmpresaLayoutCrachaSelecionada;
        //    }
        //    set
        //    {
        //        _EmpresaLayoutCrachaSelecionada = value;
        //        base.OnPropertyChanged("SelectedItem");
        //        if (EmpresaLayoutCrachaSelecionada != null)
        //        {

        //        }

        //    }
        //}

        public ObservableCollection<TipoEmpresaView> TiposEmpresa
        {
            get { return _TiposEmpresa; }

            set
            {
                if (_TiposEmpresa != value)
                {
                    _TiposEmpresa = value;
                    OnPropertyChanged();
                }
            }
        }

        public EmpresaView EmpresaSelecionada
        {
            get { return _EmpresaSelecionada; }
            set
            {
                _EmpresaSelecionada = value;
                OnPropertyChanged ("SelectedItem");
                if (EmpresaSelecionada != null)
                {
                    if (!_atualizandoLogo)
                    {
                        var OnEmpresaSelecionada_thr = new Thread (() => OnEmpresaSelecionada());
                        OnEmpresaSelecionada_thr.Start();

                        //Thread CarregaLogo_thr = new Thread(() => CarregaLogo(EmpresaSelecionada.EmpresaId));
                        //CarregaLogo_thr.Start();
                    }
                }
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged ("SelectedIndex");
            }
        }

        public bool HabilitaEdicao
        {
            get { return _HabilitaEdicao; }
            set
            {
                _HabilitaEdicao = value;
                OnPropertyChanged();
            }
        }

        public ImageSource LogoImageSource
        {
            get { return _LogoImageSource; }
            set
            {
                _LogoImageSource = value;
                OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get { return _Criterios; }
            set
            {
                _Criterios = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage Waiting
        {
            get { return _Waiting; }
            set
            {
                _Waiting = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos dos Botoes

        #region Empresa

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();

                // _empresaTemp = EmpresaSelecionada.CriaCopia(EmpresaSelecionada);
                Global._cnpjEdicao = _empresaTemp.Cnpj;
                _selectedIndexTemp = SelectedIndex;
                //HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";
                Empresas[_selectedIndexTemp] = _empresaTemp;
                //SelectedIndex = _selectedIndexTemp; /// isto não est;a funcionando corretamente
                ////HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";
                var serializer = new XmlSerializer (typeof(EmpresaView));

                var _EmpresasTemp = new ObservableCollection<EmpresaView>();
                var _ClasseEmpresasTemp = new EmpresaView();
                _EmpresasTemp.Add (EmpresaSelecionada);
                //_ClasseEmpresasTemp.Empresas = _EmpresasTemp;

                var entity = EmpresaSelecionada;
                var entityConv = Mapper.Map<Empresa> (entity);

                _service.Alterar (entityConv);

                /////////////////////////////////////////
            }
            catch (Exception ex)
            {
                Global.Log ("Erro void OnSalvarEdicaoCommand ex: " + ex.Message);
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";

                //ObservableCollection<EmpresaView> _EmpresasPro = new ObservableCollection<EmpresaView>();
                //ClasseEmpresas _ClasseEmpresasTemp = new ClasseEmpresas();

                EmpresaSelecionada.Pendente = true;
                EmpresaSelecionada.Pendente11 = true;
                EmpresaSelecionada.Pendente12 = true;
                EmpresaSelecionada.Pendente13 = false;
                EmpresaSelecionada.Pendente14 = true;
                EmpresaSelecionada.Pendente15 = false;
                EmpresaSelecionada.Pendente16 = false;
                EmpresaSelecionada.Pendente17 = true;

                var entity = EmpresaSelecionada;
                var entityConv = Mapper.Map<Empresa> (entity);

                _service.Criar (entityConv);

                var _novoEmpresaID = entityConv.EmpresaId;
                AtualizaPendencias (_novoEmpresaID);
                EmpresaSelecionada.EmpresaId = _novoEmpresaID;

                var CarregaColecaoEmpresasSignatarios_thr = new Thread (() => CarregaColecaoEmpresas());
                CarregaColecaoEmpresasSignatarios_thr.Start();
            }
            catch (Exception ex)
            {
                Global.Log ("Erro void OnSalvarAdicaoCommand ex: " + ex.Message);
                Utils.TraceException (ex);
                throw;
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Empresas)
                {
                    _EmpresasTemp.Add (x);
                }
                Global._cnpjEdicao = "00.000.000/0000-00";
                _selectedIndexTemp = SelectedIndex;
                Empresas.Clear();
                var _empresa = new EmpresaView();
                Empresas.Add (_empresa);

                SelectedIndex = 0;
                //HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro void OnAdicionarCommand ex: " + ex.Message);
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";
                Empresas = null;
                Empresas = new ObservableCollection<EmpresaView> (_EmpresasTemp);
                SelectedIndex = _selectedIndexTemp;
                _EmpresasTemp.Clear();
                //HabilitaEdicao = false;
                _atualizandoLogo = false;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro void OnCancelarAdicaoCommand ex: " + ex.Message);
            }
        }

        public void OnExcluirCommand()
        {
            try
            {
                if (Global.PopupBox ("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox ("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        if (Global.PopupBox ("Você realmente tem certeza desta operação?", 2))
                        {
                            var emp = _service.BuscarPelaChave (EmpresaSelecionada.EmpresaId);
                            _service.Remover (emp);

                            Empresas.Remove (EmpresaSelecionada);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Log ("Erro void OnExcluirCommand ex: " + ex.Message);
            }
        }

        //public void OnPesquisarCommand()
        //{
        //    try
        //    {
        //        popupPesquisaEmpresa = new PopUpPesquisaEmpresa();
        //        popupPesquisaEmpresa.EfetuarProcura += On_EfetuarProcura;
        //        popupPesquisaEmpresa.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log ("Erro void OnPesquisarCommand ex: " + ex.Message);
        //    }
        //}

        #endregion

        #region Tipo Atividade

        public void OnInserirAtividadeCommand(string _TipoAtividadeIDstr, string _Descricao)
        {
            try
            {
                var _TipoAtividadeID = Convert.ToInt32 (_TipoAtividadeIDstr);
                var _EmpresaTipoAtividade = new EmpresaTipoAtividadeView();
                _EmpresaTipoAtividade.EmpresaId = EmpresaSelecionada.EmpresaId;
                _EmpresaTipoAtividade.TipoAtividadeId = _TipoAtividadeID;
                _EmpresaTipoAtividade.Descricao = _Descricao;

                var entity = _EmpresaTipoAtividade;
                var entityConv = Mapper.Map<EmpresaTipoAtividade> (entity);
                _service.Atividade.Criar (entityConv);
                CarregaColecaoEmpresasTiposAtividades (_EmpresaTipoAtividade.EmpresaId);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirAtividadeCommand()
        {
            try
            {
                if (Global.PopupBox ("Tem certeza que deseja excluir?", 2))
                {
                    var entity = EmpresaTipoAtividadeSelecionada;
                    var entityConv = Mapper.Map<EmpresaTipoAtividade> (entity);
                    _service.Atividade.Remover (entityConv);

                    EmpresasTiposAtividades.Remove (EmpresaTipoAtividadeSelecionada);

                    CarregaColecaoEmpresasTiposAtividades (EmpresaSelecionada.EmpresaId);
                }
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Area Acesso

        public void OnInserirAcessoCommand(object areaAcesso)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirAcessoCommand()
        {
            try
            {
                //EmpresasAreasAcessos.Remove(EmpresaAreaAcessoSelecionada);
            }
            catch (Exception ex)
            {
                Global.Log ("Erro void OnExcluirAcessoCommand ex: " + ex.Message);
            }
        }

        #endregion

        #region Empresa Layout Crachas

        public void OnInserirCrachaCommand(int _LayoutCrachaID, string _Nome)
        {
            try
            {
                var _EmpresaLayoutCracha = new EmpresaLayoutCrachaView();
                _EmpresaLayoutCracha.EmpresaId = EmpresaSelecionada.EmpresaId;
                _EmpresaLayoutCracha.LayoutCrachaId = _LayoutCrachaID;

                var entity = _EmpresaLayoutCracha;
                var entityConv = Mapper.Map<EmpresaLayoutCracha> (entity);
                _service.CrachaService.Criar (entityConv);

                CarregaColecaoEmpresasLayoutsCrachas (_EmpresaLayoutCracha.EmpresaId);
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnExcluirCrachaCommand()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaEmpresa.Criterio.Split ((char) 20);
                int _codigo;
                if ((((string[]) vetor)[0] == null) || (((string[]) vetor)[0] == ""))
                {
                    _codigo = 0;
                }
                else
                {
                    _codigo = Convert.ToInt32 (((string[]) vetor)[0]);
                }

                var _razaosocial = ((string[]) vetor)[1];
                var _nomefantasia = ((string[]) vetor)[2];
                var _cnpj = ((string[]) vetor)[3];
                CarregaColecaoEmpresas (_codigo, _razaosocial, _nomefantasia, _cnpj);
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirPendenciaGeral(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa (21, EmpresaSelecionada.EmpresaId, PendenciaTipo.Empresa);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirPendenciaContratos(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa (14, EmpresaSelecionada.EmpresaId, PendenciaTipo.Empresa);
                frm.ShowDialog();
                // CarregaColecaoEmpresas(EmpresaSelecionada.EmpresaID, "", "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirPendenciaAnexos(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa (24, EmpresaSelecionada.EmpresaId, PendenciaTipo.Empresa);
                frm.ShowDialog();
                //CarregaColecaoEmpresas(EmpresaSelecionada.EmpresaID, "", "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void OnAbrirPendenciaRepresentante(object sender, RoutedEventArgs e)
        {
            try
            {
                var frm = new PopupPendencias();
                frm.Inicializa (12, EmpresaSelecionada.EmpresaId, PendenciaTipo.Empresa);
                frm.ShowDialog();
                // CarregaColecaoEmpresas(EmpresaSelecionada.EmpresaID, "", "", "");
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Carregamento das Colecoes

        public void CarregaColecoesIniciais()
        {
            var CarregaUI_thr = new Thread (() => CarregaUI());
            CarregaUI_thr.Start();
        }

        private void CarregaUI()
        {
            CarregaColecaoEmpresas();
            CarregaColecaoEstados();
            CarregaColecaoMunicipios();
            CarregaColecaoTiposAtividades();
            CarregaColecaoLayoutsCrachas();
        }

        private void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace (nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace (apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace (cnpj)) cnpj = $"%{cnpj}%";

                var list1 = _service.Listar (idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<EmpresaView>> (list1.OrderByDescending (a => a.EmpresaId));

                var observer = new ObservableCollection<EmpresaView>();
                list2.ForEach (n => { observer.Add (n); });

                Empresas = observer;

                //Hotfix auto-selecionar registro do topo da ListView
                var topList = observer.FirstOrDefault();
                EmpresaSelecionada = topList;

                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void CarregaColecaoEstados()
        {
            try
            {
                var service = new EstadoService();

                var list1 = service.Listar();
                var list2 = Mapper.Map<List<EstadoView>> (list1);
                var observer = new ObservableCollection<EstadoView>();
                list2.ForEach (n => { observer.Add (n); });

                Estados = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoMunicipios(string _EstadoUF = null)
        {
            try
            {
                var service = new MunicipioService();
                if (!string.IsNullOrWhiteSpace (_EstadoUF)) _EstadoUF = $"%{_EstadoUF}%";
                var list1 = service.Listar (null, _EstadoUF);

                var list2 = Mapper.Map<List<MunicipioView>> (list1);
                var observer = new ObservableCollection<MunicipioView>();
                list2.ForEach (n => { observer.Add (n); });

                Municipios = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void CarregaColecaoTiposAtividades()
        {
            try
            {
                var service = new TipoAtividadeService();

                var list1 = service.Listar();
                var list2 = Mapper.Map<List<TipoAtividadeView>> (list1);

                var observer = new ObservableCollection<TipoAtividadeView>();
                list2.ForEach (n => { observer.Add (n); });

                TiposAtividades = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoEmpresasTiposAtividades(int _empresaID = 0)
        {
            try
            {
                var service = new EmpresaTipoAtividadeService();
                var list1 = service.ListarEmpresaTipoAtividadeView (null, _empresaID, null, null);

                var list2 = Mapper.Map<List<EmpresaTipoAtividadeView>> (list1);
                var observer = new ObservableCollection<EmpresaTipoAtividadeView>();
                list2.ForEach (n => { observer.Add (n); });
                EmpresasTiposAtividades = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                var service = new LayoutCrachaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<LayoutCrachaView>> (list1);

                var observer = new ObservableCollection<LayoutCrachaView>();
                list2.ForEach (n => { observer.Add (n); });

                LayoutsCrachas = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        public void CarregaColecaoEmpresasLayoutsCrachas(int _empresaID = 0)
        {
            try
            {
                var service = new EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView (_empresaID);

                var list2 = Mapper.Map<List<EmpresaLayoutCrachaView>> (list1);
                var observer = new ObservableCollection<EmpresaLayoutCrachaView>();
                list2.ForEach (n => { observer.Add (n); });

                EmpresasLayoutsCrachas = observer;
                //LayoutsCrachas = observer;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void CarregaLogo(int _EmpresaID)
        {
            try
            {
                _atualizandoLogo = true;

                Application.Current.Dispatcher.Invoke (() =>
                {
                    Waiting = new BitmapImage (new Uri ("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

                    Waiting.Freeze();
                });

                var _xmlstring = BuscaLogo (_EmpresaID);

                Application.Current.Dispatcher.Invoke (() => { Waiting = null; });

                var xmldocument = new XmlDocument();

                xmldocument.LoadXml (_xmlstring);

                XmlNode node = xmldocument.DocumentElement;

                var arquivoNode = node.SelectSingleNode ("ArquivosImagens/ArquivoImagem/Arquivo");

                if (arquivoNode.HasChildNodes)
                {
                    Application.Current.Dispatcher.Invoke (() =>
                    {
                        //_empresaTemp = EmpresaSelecionada.CriaCopia(EmpresaSelecionada);

                        _selectedIndexTemp = SelectedIndex;

                        _empresaTemp.Logo = arquivoNode.FirstChild.Value;

                        Empresas[_selectedIndexTemp] = _empresaTemp;

                        SelectedIndex = _selectedIndexTemp;
                    });
                }
                _atualizandoLogo = false;
            }
            catch (Exception ex)
            {
                _atualizandoLogo = true;
                Utils.TraceException (ex);
            }
        }

        #endregion

        #region Data Access

        private void AtualizaEmpresasTiposAtividadesBD(string xmlString)
        {
            try
            {
                var _xmlDoc = new XmlDocument();

                _xmlDoc.LoadXml (xmlString);

                var _EmpresaTipoAtividade = new EmpresaTipoAtividadeView();

                if (_xmlDoc.GetElementsByTagName ("EmpresaID").Count == 0)
                {
                    return;
                }

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand ("Delete from EmpresasTiposAtividades where EmpresaID=" + Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EmpresaID")[0].InnerText), _Con);

                _sqlCmd.ExecuteNonQuery();

                for (var i = 0; i <= _xmlDoc.GetElementsByTagName ("EmpresaID").Count - 1; i++)
                {
                    _EmpresaTipoAtividade.EmpresaId = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EmpresaID")[i].InnerText);

                    _EmpresaTipoAtividade.TipoAtividadeId = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("TipoAtividadeID")[i].InnerText);

                    _sqlCmd = new SqlCommand ("Insert into EmpresasTiposAtividades(EmpresaID,TipoAtividadeID) values (" + _EmpresaTipoAtividade.EmpresaId + "," + _EmpresaTipoAtividade.TipoAtividadeId + ")", _Con);

                    _sqlCmd.ExecuteNonQuery();
                }
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void InsereEmpresasTiposAtividadesBD ex: " + ex);
            }
        }

        private void AtualizaEmpresasAreasAcessosBD(string xmlString)
        {
            try
            {
                var _xmlDoc = new XmlDocument();

                _xmlDoc.LoadXml (xmlString);

                var _EmpresaAreaAcesso = new ClasseEmpresasAreasAcessos.EmpresaAreaAcesso();

                if (_xmlDoc.GetElementsByTagName ("EmpresaID").Count == 0)
                {
                    return;
                }

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand ("Delete from EmpresasAreasAcessos where EmpresaID=" + Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EmpresaID")[0].InnerText), _Con);

                _sqlCmd.ExecuteNonQuery();

                for (var i = 0; i <= _xmlDoc.GetElementsByTagName ("EmpresaID").Count - 1; i++)
                {
                    _EmpresaAreaAcesso.EmpresaID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("EmpresaID")[i].InnerText);

                    _EmpresaAreaAcesso.AreaAcessoID = Convert.ToInt32 (_xmlDoc.GetElementsByTagName ("AreaAcessoID")[i].InnerText);

                    _sqlCmd = new SqlCommand ("Insert into EmpresasAreasAcessos(EmpresaID,AreaAcessoID) values (" + _EmpresaAreaAcesso.EmpresaID + "," + _EmpresaAreaAcesso.AreaAcessoID + ")", _Con);

                    _sqlCmd.ExecuteNonQuery();
                }
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void InsereEmpresasAreasAcessosBD ex: " + ex);
            }
        }

        private void AtualizaPendencias(int _EmpresaID)
        {
            try
            {
                if (_EmpresaID == 0)
                {
                    return;
                }

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();
                SqlCommand _sqlCmd;
                for (var i = 11; i < 18; i++)
                {
                    _sqlCmd = new SqlCommand ("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,EmpresaID) values (" + "@v1,@v2, @v3,@v4,@v5)", _Con);

                    _sqlCmd.Parameters.Add ("@v1", SqlDbType.Int).Value = i;
                    _sqlCmd.Parameters.Add ("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
                    _sqlCmd.Parameters.Add ("@v3", SqlDbType.DateTime).Value = DateTime.Now;
                    _sqlCmd.Parameters.Add ("@v4", SqlDbType.Bit).Value = 1;
                    _sqlCmd.Parameters.Add ("@v5", SqlDbType.Int).Value = _EmpresaID;
                    _sqlCmd.ExecuteNonQuery();
                }

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void AtualizaPendencias ex: " + ex);
            }
        }

        //private void AtualizaEmpresasLayoutsCrachasBD(string xmlString)
        //{
        //    try
        //    {

        //        XmlDocument _xmlDoc = new XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);

        //        EmpresaLayoutCrachaView _EmpresaLayoutCracha = new EmpresaLayoutCrachaView();

        //        if (_xmlDoc.GetElementsByTagName("EmpresaID").Count == 0)
        //        {
        //            return;
        //        }

        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlCommand _sqlCmd;
        //        //_sqlCmd = new SqlCommand("Delete from EmpresasLayoutsCrachas where EmpresaID=" + Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[0].InnerText), _Con);

        //        //_sqlCmd.ExecuteNonQuery();

        //        for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        {
        //            _EmpresaLayoutCracha.EmpresaId = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);

        //            _EmpresaLayoutCracha.LayoutCrachaId = Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);

        //            _sqlCmd = new SqlCommand("select * from EmpresasLayoutsCrachas where LayoutCrachaID = " + _EmpresaLayoutCracha.LayoutCrachaId + " AND EmpresaID=" + _EmpresaLayoutCracha.EmpresaId, _Con);

        //            SqlDataReader _sqlreader = _sqlCmd.ExecuteReader(CommandBehavior.Default);

        //            if (_sqlreader.HasRows)
        //            {
        //                _sqlreader.Close();
        //            }
        //            else
        //            {
        //                _sqlreader.Close();

        //                _sqlCmd = new SqlCommand("Insert into EmpresasLayoutsCrachas(EmpresaID,LayoutCrachaID) values (" + _EmpresaLayoutCracha.EmpresaId + ",'" + _EmpresaLayoutCracha.LayoutCrachaId + "')", _Con);

        //                _sqlCmd.ExecuteNonQuery();
        //            }

        //        }
        //        _Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void InsereEmpresasLayoutsCrachasBD ex: " + ex);

        //    }

        //}

        private string BuscaLogo(int empresaID)
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement ("ClasseArquivosImagens");
                _xmlDocument.AppendChild (_ClasseArquivosImagens);

                XmlNode _ArquivosImagens = _xmlDocument.CreateElement ("ArquivosImagens");
                _ClasseArquivosImagens.AppendChild (_ArquivosImagens);

                var _Con = new SqlConnection (Global._connectionString);
                _Con.Open();

                var SQCMDXML = new SqlCommand ("Select * From Empresas Where EmpresaID = " + empresaID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader (CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement ("ArquivoImagem");
                    _ArquivosImagens.AppendChild (_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement ("Arquivo");
                    _Arquivo.AppendChild (_xmlDocument.CreateTextNode (SQDR_XML["Logo"].ToString()));
                    _ArquivoImagem.AppendChild (_Arquivo);
                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log ("Erro na void BuscaLogo ex: " + ex);
                return null;
            }
        }

        #endregion
    }
}