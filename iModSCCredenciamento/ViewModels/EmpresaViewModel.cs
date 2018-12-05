using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using Genetec.Sdk.Entities;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresaViewModel : ViewModelBase
    {
        #region Inicializacao
        public EmpresaViewModel()
        {
            //if (Main.engine==null)
            //{
            //    Global.AbreConfig();
            //}
            //// 
            // Thread CarregaUI_thr = new Thread(() => CarregaUI());
            // CarregaUI_thr.Start();
            CarregaColecoesIniciais();
            //CarregaUI();
        }



        #endregion

        #region Variaveis privadas
        private SynchronizationContext MainThread;

        private EmpresasSegurosViewModel _SegurosEmpresaViewModel;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;

        private ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro> _Seguros;

        private List<ClasseEmpresas.Empresa> _EmpresasTemp = new List<ClasseEmpresas.Empresa>();

        private ClasseEmpresas.Empresa _empresaTemp = new ClasseEmpresas.Empresa();

        private ObservableCollection<ClasseEstados.Estado> _Estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        private ObservableCollection<ClasseTiposEmpresas.TipoEmpresa> _TiposEmpresa;

        private ObservableCollection<ClasseTiposAtividades.TipoAtividade> _TiposAtividades;

        private ObservableCollection<ClasseEmpresasTiposAtividades.EmpresaTipoAtividade> _EmpresasTiposAtividades;

        private ClasseEmpresasTiposAtividades.EmpresaTipoAtividade _EmpresaTipoAtividadeSelecionada;

        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;

        private ObservableCollection<ClasseEmpresasAreasAcessos.EmpresaAreaAcesso> _EmpresasAreasAcessos;

        private ClasseEmpresasAreasAcessos.EmpresaAreaAcesso _EmpresaAreaAcessoSelecionada;

        private ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> _LayoutsCrachas;

        private ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> _EmpresasLayoutsCrachas;

        private ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha _EmpresaLayoutCrachaSelecionada;

        private ClasseEmpresas.Empresa _EmpresaSelecionada;

        private ImageSource _LogoImageSource;

        private int _selectedIndex;

        private bool _atualizandoLogo = false;

        private BitmapImage _Waiting;

        private bool _HabilitaEdicao = false;

        PopUpPesquisaEmpresa popupPesquisaEmpresa;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private readonly IEmpresaService _service = new EmpresaService();

        private readonly IDadosAuxiliaresFacade _auxiliaresService = new DadosAuxiliaresFacadeService();

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresas.Empresa> Empresas

        {
            get
            {
                return _Empresas;
            }

            set
            {
                if (_Empresas != value)
                {
                    _Empresas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro> Seguros
        {
            get
            {
                return _Seguros;
            }

            set
            {
                if (_Seguros != value)
                {
                    _Seguros = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEstados.Estado> Estados
        {
            get
            {
                return _Estados;
            }

            set
            {
                if (_Estados != value)
                {
                    _Estados = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseMunicipios.Municipio> Municipios
        {
            get
            {
                return _Municipios;
            }

            set
            {
                if (_Municipios != value)
                {
                    _Municipios = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseTiposAtividades.TipoAtividade> TiposAtividades
        {
            get
            {
                return _TiposAtividades;
            }

            set
            {
                if (_TiposAtividades != value)
                {
                    _TiposAtividades = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEmpresasTiposAtividades.EmpresaTipoAtividade> EmpresasTiposAtividades
        {
            get
            {
                return _EmpresasTiposAtividades;
            }

            set
            {
                if (_EmpresasTiposAtividades != value)
                {
                    _EmpresasTiposAtividades = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseEmpresasTiposAtividades.EmpresaTipoAtividade EmpresaTipoAtividadeSelecionada
        {
            get
            {
                return this._EmpresaTipoAtividadeSelecionada;
            }
            set
            {
                this._EmpresaTipoAtividadeSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (EmpresaTipoAtividadeSelecionada != null)
                {

                }

            }
        }

        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
        {
            get
            {
                return _AreasAcessos;
            }

            set
            {
                if (_AreasAcessos != value)
                {
                    _AreasAcessos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEmpresasAreasAcessos.EmpresaAreaAcesso> EmpresasAreasAcessos
        {
            get
            {
                return _EmpresasAreasAcessos;
            }

            set
            {
                if (_EmpresasAreasAcessos != value)
                {
                    _EmpresasAreasAcessos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseEmpresasAreasAcessos.EmpresaAreaAcesso EmpresaAreaAcessoSelecionada
        {
            get
            {
                return this._EmpresaAreaAcessoSelecionada;
            }
            set
            {
                this._EmpresaAreaAcessoSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (EmpresaAreaAcessoSelecionada != null)
                {

                }

            }
        }

        public ObservableCollection<ClasseLayoutsCrachas.LayoutCracha> LayoutsCrachas
        {
            get
            {
                return _LayoutsCrachas;
            }

            set
            {
                if (_LayoutsCrachas != value)
                {
                    _LayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> EmpresasLayoutsCrachas
        {
            get
            {
                return _EmpresasLayoutsCrachas;
            }

            set
            {
                if (_EmpresasLayoutsCrachas != value)
                {
                    _EmpresasLayoutsCrachas = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha EmpresaLayoutCrachaSelecionada
        {
            get
            {
                return this._EmpresaLayoutCrachaSelecionada;
            }
            set
            {
                this._EmpresaLayoutCrachaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (EmpresaLayoutCrachaSelecionada != null)
                {

                }

            }
        }

        public ObservableCollection<ClasseTiposEmpresas.TipoEmpresa> TiposEmpresa
        {
            get
            {
                return _TiposEmpresa;
            }

            set
            {
                if (_TiposEmpresa != value)
                {
                    _TiposEmpresa = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseEmpresas.Empresa EmpresaSelecionada
        {
            get
            {
                return this._EmpresaSelecionada;
            }
            set
            {
                this._EmpresaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (EmpresaSelecionada != null)
                {
                    if (!_atualizandoLogo)
                    {
                        Thread OnEmpresaSelecionada_thr = new Thread(() => OnEmpresaSelecionada());
                        OnEmpresaSelecionada_thr.Start();
                        Thread CarregaLogo_thr = new Thread(() => CarregaLogo(EmpresaSelecionada.EmpresaID));
                        CarregaLogo_thr.Start();
                    }
                }

            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public bool HabilitaEdicao
        {
            get
            {
                return this._HabilitaEdicao;
            }
            set
            {
                this._HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }

        public ImageSource LogoImageSource
        {
            get
            {
                return this._LogoImageSource;
            }
            set
            {
                this._LogoImageSource = value;
                base.OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get
            {
                return this._Criterios;
            }
            set
            {
                this._Criterios = value;
                base.OnPropertyChanged();
            }
        }

        public BitmapImage Waiting
        {
            get
            {
                return this._Waiting;
            }
            set
            {
                this._Waiting = value;
                base.OnPropertyChanged();
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

                _empresaTemp = EmpresaSelecionada.CriaCopia(EmpresaSelecionada);
                Global._cnpjEdicao = _empresaTemp.CNPJ;
                _selectedIndexTemp = SelectedIndex;
                //HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnEditarCommand ex: " + ex.Message);
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
                Global.Log("Erro void OnCancelarEdicaoCommand ex: " + ex.Message);
            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresas));

                ObservableCollection<ClasseEmpresas.Empresa> _EmpresasTemp = new ObservableCollection<ClasseEmpresas.Empresa>();
                ClasseEmpresas _ClasseEmpresasTemp = new ClasseEmpresas();
                _EmpresasTemp.Add(EmpresaSelecionada);
                _ClasseEmpresasTemp.Empresas = _EmpresasTemp;

                var entity = EmpresaSelecionada;
                var entityConv = Mapper.Map<Empresa>(entity);

                _service.Alterar(entityConv);

                /////////////////////////////////////////

                string xmlString;

                serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasTiposAtividades));

                ClasseEmpresasTiposAtividades _ClasseEmpresasTiposAtividadesTemp = new ClasseEmpresasTiposAtividades();
                _ClasseEmpresasTiposAtividadesTemp.EmpresasTiposAtividades = EmpresasTiposAtividades;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {
                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasTiposAtividadesTemp);
                        xmlString = sw.ToString();
                    }

                }
                AtualizaEmpresasTiposAtividadesBD(xmlString);
                _ClasseEmpresasTiposAtividadesTemp = null;

                /////////////////////////////////////////

                serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasAreasAcessos));

                ClasseEmpresasAreasAcessos _ClasseEmpresasAreasAcessosTemp = new ClasseEmpresasAreasAcessos();
                _ClasseEmpresasAreasAcessosTemp.EmpresasAreasAcessos = EmpresasAreasAcessos;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {


                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasAreasAcessosTemp);
                        xmlString = sw.ToString();
                    }

                }
                AtualizaEmpresasAreasAcessosBD(xmlString);
                _ClasseEmpresasAreasAcessosTemp = null;

                /////////////////////////////////////////

                serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasLayoutsCrachas));

                ClasseEmpresasLayoutsCrachas _ClasseEmpresasLayoutsCrachasTemp = new ClasseEmpresasLayoutsCrachas();
                _ClasseEmpresasLayoutsCrachasTemp.EmpresasLayoutsCrachas = EmpresasLayoutsCrachas;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {


                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasLayoutsCrachasTemp);
                        xmlString = sw.ToString();
                    }

                }
                AtualizaEmpresasLayoutsCrachasBD(xmlString);
                _ClasseEmpresasLayoutsCrachasTemp = null;

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarEdicaoCommand ex: " + ex.Message);
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";

                //ObservableCollection<ClasseEmpresas.Empresa> _EmpresasPro = new ObservableCollection<ClasseEmpresas.Empresa>();
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
                var entityConv = Mapper.Map<Empresa>(entity);

                _service.Criar(entityConv);

                int _novoEmpresaID = entityConv.EmpresaId;
                AtualizaPendencias(_novoEmpresaID);
                EmpresaSelecionada.EmpresaID = _novoEmpresaID;

                Thread CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresas());
                CarregaColecaoEmpresasSignatarios_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnSalvarAdicaoCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Empresas)
                {
                    _EmpresasTemp.Add(x);
                }
                Global._cnpjEdicao = "00.000.000/0000-00";
                _selectedIndexTemp = SelectedIndex;
                Empresas.Clear();
                ClasseEmpresas.Empresa _empresa = new ClasseEmpresas.Empresa();
                Empresas.Add(_empresa);

                SelectedIndex = 0;
                //HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnAdicionarCommand ex: " + ex.Message);
            }

        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";
                Empresas = null;
                Empresas = new ObservableCollection<ClasseEmpresas.Empresa>(_EmpresasTemp);
                SelectedIndex = _selectedIndexTemp;
                _EmpresasTemp.Clear();
                //HabilitaEdicao = false;
                _atualizandoLogo = false;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnCancelarAdicaoCommand ex: " + ex.Message);
            }
        }

        public void OnExcluirCommand()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        if (Global.PopupBox("Você realmente tem certeza desta operação?", 2))
                        {
                            var emp = _service.BuscarPelaChave(EmpresaSelecionada.EmpresaID);
                            _service.Remover(emp);

                            Empresas.Remove(EmpresaSelecionada);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnExcluirCommand ex: " + ex.Message);
            }

        }

        public void OnPesquisarCommand()
        {
            try
            {
                popupPesquisaEmpresa = new PopUpPesquisaEmpresa();
                popupPesquisaEmpresa.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaEmpresa.ShowDialog();
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnPesquisarCommand ex: " + ex.Message);
            }
        }

        #endregion

        #region Tipo Atividade
        public void OnInserirAtividadeCommand(string _TipoAtividadeIDstr, string _Descricao)
        {
            try
            {
                int _TipoAtividadeID = Convert.ToInt32(_TipoAtividadeIDstr);
                ClasseEmpresasTiposAtividades.EmpresaTipoAtividade _EmpresaTipoAtividade = new ClasseEmpresasTiposAtividades.EmpresaTipoAtividade();
                _EmpresaTipoAtividade.EmpresaID = EmpresaSelecionada.EmpresaID;
                _EmpresaTipoAtividade.TipoAtividadeID = _TipoAtividadeID;
                _EmpresaTipoAtividade.Descricao = _Descricao;

                var entity = _EmpresaTipoAtividade;
                var entityConv = Mapper.Map<EmpresaTipoAtividade>(entity);

                _auxiliaresService.EmpresaTipoAtividadeService.Criar(entityConv);

                CarregaColecaoEmpresasTiposAtividades(_EmpresaTipoAtividade.EmpresaID);

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnInserirAtividadeCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnExcluirAtividadeCommand()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = EmpresaTipoAtividadeSelecionada;
                    var entityConv = Mapper.Map<EmpresaTipoAtividade>(entity);
                    _auxiliaresService.EmpresaTipoAtividadeService.Remover(entityConv);

                    EmpresasTiposAtividades.Remove(EmpresaTipoAtividadeSelecionada);

                    CarregaColecaoEmpresasTiposAtividades();
                }
            }

            catch (Exception ex)
            {
                Global.Log("Erro void OnExcluirAtividadeCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }
        #endregion


        #region Area Acesso
        public void OnInserirAcessoCommand(object areaAcesso)
        {
            try
            {
                ClasseAreasAcessos.AreaAcesso _areaAcesso = (ClasseAreasAcessos.AreaAcesso)areaAcesso;
                ClasseEmpresasAreasAcessos.EmpresaAreaAcesso _EmpresaAreaAcesso = new ClasseEmpresasAreasAcessos.EmpresaAreaAcesso();
                _EmpresaAreaAcesso.EmpresaID = EmpresaSelecionada.EmpresaID;
                _EmpresaAreaAcesso.AreaAcessoID = _areaAcesso.AreaAcessoID;
                _EmpresaAreaAcesso.Descricao = _areaAcesso.Descricao;
                _EmpresaAreaAcesso.Identificacao = _areaAcesso.Identificacao;
                EmpresasAreasAcessos.Add(_EmpresaAreaAcesso);
                //int _AreaAcessoID = Convert.ToInt32(_AreaAcessoIDstr);
                //ClasseEmpresasAreasAcessos.EmpresaAreaAcesso _EmpresaAreaAcesso = new ClasseEmpresasAreasAcessos.EmpresaAreaAcesso();
                //_EmpresaAreaAcesso.EmpresaID = EmpresaSelecionada.EmpresaID;
                //_EmpresaAreaAcesso.AreaAcessoID = _AreaAcessoID;
                //_EmpresaAreaAcesso.Descricao = _Descricao;
                //_EmpresaAreaAcesso.Identificacao = "xx";
                //EmpresasAreasAcessos.Add(_EmpresaAreaAcesso);

                //EmpresasAreasAcessos.Add();
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnInserirAcessoCommand ex: " + ex.Message);
            }
        }

        public void OnExcluirAcessoCommand()
        {
            try
            {
                EmpresasAreasAcessos.Remove(EmpresaAreaAcessoSelecionada);
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnExcluirAcessoCommand ex: " + ex.Message);
            }
        }
        #endregion

        #region Empresa Layout Crachas
        public void OnInserirCrachaCommand(int _LayoutCrachaID, string _Nome)
        {
            try
            {
                ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha _EmpresaLayoutCracha = new ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha();
                _EmpresaLayoutCracha.EmpresaID = EmpresaSelecionada.EmpresaID;
                _EmpresaLayoutCracha.LayoutCrachaID = _LayoutCrachaID;

                var entity = _EmpresaLayoutCracha;
                var entityConv = Mapper.Map<EmpresaLayoutCracha>(entity);

                _auxiliaresService.EmpresaLayoutCrachaService.Criar(entityConv);

                CarregaColecaoEmpresasLayoutsCrachas(_EmpresaLayoutCracha.EmpresaID);
            }

            catch (Exception ex)
            {
                Global.Log("Erro void OnInserirCrachaCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnExcluirCrachaCommand()
        {
            try
            {
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    var entity = EmpresaLayoutCrachaSelecionada;
                    var entityConv = Mapper.Map<EmpresaLayoutCracha>(entity);
                    _auxiliaresService.EmpresaLayoutCrachaService.Remover(entityConv);

                    EmpresasLayoutsCrachas.Remove(EmpresaLayoutCrachaSelecionada);

                    CarregaColecaoEmpresasTiposAtividades();
                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnExcluirCrachaCommand ex: " + ex.Message);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        #endregion

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaEmpresa.Criterio.Split((char)(20));
                int _codigo;
                if ((((string[])vetor)[0] == null) || (((string[])vetor)[0] == ""))
                {
                    _codigo = 0;
                }
                else
                {
                    _codigo = Convert.ToInt32(((string[])vetor)[0]);
                }

                string _razaosocial = ((string[])vetor)[1];
                string _nomefantasia = ((string[])vetor)[2];
                string _cnpj = ((string[])vetor)[3];
                CarregaColecaoEmpresas(_codigo, _razaosocial, _nomefantasia, _cnpj);
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Global.Log("Erro void On_EfetuarProcura ex: " + ex.Message);
            }

        }

        public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupPendencias popupPendencias = new PopupPendencias(1, ((System.Windows.FrameworkElement)e.OriginalSource).Tag, EmpresaSelecionada.EmpresaID, EmpresaSelecionada.Nome);
                popupPendencias.ShowDialog();
                popupPendencias = null;
                CarregaColecaoEmpresas(EmpresaSelecionada.EmpresaID, "", "", "");


            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnAbrirPendencias ex: " + ex.Message);
            }
        }

        #endregion

        #region Metodos Publicos
        //public bool ConsultaCNPJ(string _cnpj)
        //{
        //    try
        //    {
        //        SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
        //        SqlDataReader SQDR_CNPJ;
        //        SqlCommand SQCMDCNPJ = new SqlCommand("Select * From Empresas Where CNPJ = '" + _cnpj + "'", _Con);
        //        SQDR_CNPJ = SQCMDCNPJ.ExecuteReader(CommandBehavior.Default);
        //        if (SQDR_CNPJ.Read())
        //        {
        //            _Con.Close();
        //            return true;
        //        }
        //        _Con.Close();
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        Global g = new Global();

        public void ValidarAdicao(ClasseEmpresas.Empresa entity)
        {
            //if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            //if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            ValidarEdicao(entity);
            if (ConsultaCNPJ(entity.CNPJ)) throw new InvalidOperationException("CNPJ já cadastrado");


        }

        public void ValidarEdicao(ClasseEmpresas.Empresa entity)
        {
            if (string.IsNullOrWhiteSpace(entity.CNPJ)) throw new InvalidOperationException("Informe CNPJ");
            if (!entity.CNPJ.IsValidCnpj()) throw new InvalidOperationException("CNPJ inválido");
            //if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");
            if (string.IsNullOrWhiteSpace(entity.Nome)) throw new InvalidOperationException("Informe a Razão Social");

        }


        public bool ConsultaCNPJ(string doc)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(doc)) throw new ArgumentNullException("Informe um CNPJ para pesquisar");
                doc = doc.RetirarCaracteresEspeciais().Replace(" ", "");

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                var cmd = new SqlCommand("Select * From Empresas Where CNPJ = '" + doc + "'", _Con);
                var reader = cmd.ExecuteReader(CommandBehavior.Default);
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
        #endregion

        #region Metodos privados
        private void OnEmpresaSelecionada()
        {
            try
            {

                CarregaColecaoEmpresasTiposAtividades(EmpresaSelecionada.EmpresaID);
                CarregaColecaoEmpresasAreasAcessos(EmpresaSelecionada.EmpresaID);
                CarregaColecaoEmpresasLayoutsCrachas(EmpresaSelecionada.EmpresaID);
                CarregaColecaoLayoutsCrachas();

            }
            catch (Exception ex)
            {
                Global.Log("Erro void OnEmpresaSelecionada ex: " + ex.Message);
            }

        }

        #endregion

        #region Carregamento das Colecoes

        public void CarregaColecoesIniciais()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }

        private void CarregaUI()
        {
            CarregaColecaoEmpresas();
            CarregaColecaoEstados();
            CarregaColecaoTiposAtividades();
            CarregaColecaoAreasAcessos();
            CarregaColecaoLayoutsCrachas();
        }


        private void CarregaColecaoEmpresas(int? idEmpresa = null, string nome = null, string apelido = null, string cnpj = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(cnpj)) cnpj = $"%{cnpj}%";

                var list1 = _service.Listar(idEmpresa, nome, apelido, cnpj);
                var list2 = Mapper.Map<List<ClasseEmpresas.Empresa>>(list1.OrderByDescending(a => a.EmpresaId));

                var observer = new ObservableCollection<ClasseEmpresas.Empresa>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Empresas = observer;
                SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoEstados()
        {
            try
            {
                var service = new IMOD.Application.Service.EstadoService();

                var list1 = service.Listar();
                var list2 = Mapper.Map<List<ClasseEstados.Estado>>(list1);
                var observer = new ObservableCollection<ClasseEstados.Estado>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Estados = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoEstados ex: " + ex);
            }
        }

        public void CarregaColecaoMunicipios(string _EstadoUF = null)
        {
            try
            {
                var service = new IMOD.Application.Service.MunicipioService();
                if (!string.IsNullOrWhiteSpace(_EstadoUF)) _EstadoUF = $"%{_EstadoUF}%";
                var list1 = service.Listar(_EstadoUF);

                var list2 = Mapper.Map<List<ClasseMunicipios.Municipio>>(list1);
                var observer = new ObservableCollection<ClasseMunicipios.Municipio>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Municipios = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        private void CarregaColecaoTiposAtividades()
        {
            try
            {
                var service = new IMOD.Application.Service.TipoAtividadeService();

                var list1 = service.Listar();
                var list2 = Mapper.Map<List<ClasseTiposAtividades.TipoAtividade>>(list1);

                var observer = new ObservableCollection<ClasseTiposAtividades.TipoAtividade>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.TiposAtividades = observer;


            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoTiposAtividades ex: " + ex.Message);
            }
        }

        public void CarregaColecaoEmpresasTiposAtividades(int _empresaID = 0)
        {
            try
            {
                var service = new IMOD.Application.Service.EmpresaTipoAtividadeService();
                var list1 = service.ListarEmpresaTipoAtividadeView(null, _empresaID, null, null);

                var list2 = Mapper.Map<List<ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>>(list1);
                var observer = new ObservableCollection<ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });
                this.EmpresasTiposAtividades = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoEmpresasTiposAtividades ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                var service = new IMOD.Application.Service.AreaAcessoService();

                var list1 = service.Listar();
                var list2 = Mapper.Map<List<ClasseAreasAcessos.AreaAcesso>>(list1);

                var observer = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.AreasAcessos = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoAreasAcessos ex: " + ex.Message);
            }
        }

        public void CarregaColecaoEmpresasAreasAcessos(int _empresaID = 0)
        {
            try
            {
                var service = new IMOD.Application.Service.EmpresaAreaAcessoService();

                var list1 = service.Listar(null, _empresaID, null);
                var list2 = Mapper.Map<List<ClasseEmpresasAreasAcessos.EmpresaAreaAcesso>>(list1);

                var observer = new ObservableCollection<ClasseEmpresasAreasAcessos.EmpresaAreaAcesso>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.EmpresasAreasAcessos = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoEmpresasAreasAcessos ex: " + ex);
            }
        }

        private void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                var service = new IMOD.Application.Service.LayoutCrachaService();
                var list1 = service.Listar();

                var list2 = Mapper.Map<List<ClasseLayoutsCrachas.LayoutCracha>>(list1);

                var observer = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.LayoutsCrachas = observer;

            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoLayoutsCrachas ex: " + ex.Message);
            }
        }

        public void CarregaColecaoEmpresasLayoutsCrachas(int _empresaID = 0)
        {
            try
            {
                var service = new IMOD.Application.Service.EmpresaLayoutCrachaService();
                var list1 = service.ListarLayoutCrachaPorEmpresaView(_empresaID);

                var list2 = Mapper.Map<List<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>>(list1);
                var observer = new ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.EmpresasLayoutsCrachas = observer;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColecaoEmpresasLayoutsCrachas ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        private void CarregaLogo(int _EmpresaID)
        {
            try
            {
                _atualizandoLogo = true;

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

                    Waiting.Freeze();
                }));

                string _xmlstring = BuscaLogo(_EmpresaID);

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { Waiting = null; }));

                XmlDocument xmldocument = new XmlDocument();

                xmldocument.LoadXml(_xmlstring);

                XmlNode node = (XmlNode)xmldocument.DocumentElement;

                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                if (arquivoNode.HasChildNodes)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        _empresaTemp = EmpresaSelecionada.CriaCopia(EmpresaSelecionada);

                        _selectedIndexTemp = SelectedIndex;

                        _empresaTemp.Logo = arquivoNode.FirstChild.Value;

                        Empresas[_selectedIndexTemp] = _empresaTemp;

                        SelectedIndex = _selectedIndexTemp;

                    }));
                }
                _atualizandoLogo = false;

            }
            catch (Exception ex)
            {
                _atualizandoLogo = true;
            }
        }
        #endregion


        #region Data Access

        private void AtualizaEmpresasTiposAtividadesBD(string xmlString)
        {
            try
            {

                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);

                ClasseEmpresasTiposAtividades.EmpresaTipoAtividade _EmpresaTipoAtividade = new ClasseEmpresasTiposAtividades.EmpresaTipoAtividade();

                if (_xmlDoc.GetElementsByTagName("EmpresaID").Count == 0)
                {
                    return;
                }

                SqlConnection _Con = new SqlConnection(Global._connectionString);
                _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasTiposAtividades where EmpresaID=" + Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[0].InnerText), _Con);

                _sqlCmd.ExecuteNonQuery();

                for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                {
                    _EmpresaTipoAtividade.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);

                    _EmpresaTipoAtividade.TipoAtividadeID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoAtividadeID")[i].InnerText);

                    _sqlCmd = new SqlCommand("Insert into EmpresasTiposAtividades(EmpresaID,TipoAtividadeID) values (" + _EmpresaTipoAtividade.EmpresaID + "," + _EmpresaTipoAtividade.TipoAtividadeID + ")", _Con);

                    _sqlCmd.ExecuteNonQuery();
                }
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresasTiposAtividadesBD ex: " + ex);


            }

        }

        private void AtualizaEmpresasAreasAcessosBD(string xmlString)
        {
            try
            {

                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);

                ClasseEmpresasAreasAcessos.EmpresaAreaAcesso _EmpresaAreaAcesso = new ClasseEmpresasAreasAcessos.EmpresaAreaAcesso();

                if (_xmlDoc.GetElementsByTagName("EmpresaID").Count == 0)
                {
                    return;
                }

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasAreasAcessos where EmpresaID=" + Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[0].InnerText), _Con);

                _sqlCmd.ExecuteNonQuery();

                for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                {
                    _EmpresaAreaAcesso.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);

                    _EmpresaAreaAcesso.AreaAcessoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("AreaAcessoID")[i].InnerText);

                    _sqlCmd = new SqlCommand("Insert into EmpresasAreasAcessos(EmpresaID,AreaAcessoID) values (" + _EmpresaAreaAcesso.EmpresaID + "," + _EmpresaAreaAcesso.AreaAcessoID + ")", _Con);

                    _sqlCmd.ExecuteNonQuery();
                }
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresasAreasAcessosBD ex: " + ex);


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

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlCmd;
                for (int i = 11; i < 18; i++)
                {
                    _sqlCmd = new SqlCommand("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,EmpresaID) values (" +
                                                      "@v1,@v2, @v3,@v4,@v5)", _Con);

                    _sqlCmd.Parameters.Add("@v1", SqlDbType.Int).Value = i;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
                    _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DateTime.Now;
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.Bit).Value = 1;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.Int).Value = _EmpresaID;
                    _sqlCmd.ExecuteNonQuery();
                }

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void AtualizaPendencias ex: " + ex);


            }

        }

        private void AtualizaEmpresasLayoutsCrachasBD(string xmlString)
        {
            try
            {

                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);

                ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha _EmpresaLayoutCracha = new ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha();

                if (_xmlDoc.GetElementsByTagName("EmpresaID").Count == 0)
                {
                    return;
                }

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlCmd;
                //_sqlCmd = new SqlCommand("Delete from EmpresasLayoutsCrachas where EmpresaID=" + Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[0].InnerText), _Con);

                //_sqlCmd.ExecuteNonQuery();

                for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                {
                    _EmpresaLayoutCracha.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);

                    _EmpresaLayoutCracha.LayoutCrachaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);

                    _sqlCmd = new SqlCommand("select * from EmpresasLayoutsCrachas where LayoutCrachaID = " + _EmpresaLayoutCracha.LayoutCrachaID + " AND EmpresaID=" + _EmpresaLayoutCracha.EmpresaID, _Con);

                    SqlDataReader _sqlreader = _sqlCmd.ExecuteReader(CommandBehavior.Default);

                    if (_sqlreader.HasRows)
                    {
                        _sqlreader.Close();
                    }
                    else
                    {
                        _sqlreader.Close();

                        _sqlCmd = new SqlCommand("Insert into EmpresasLayoutsCrachas(EmpresaID,LayoutCrachaID) values (" + _EmpresaLayoutCracha.EmpresaID + ",'" + _EmpresaLayoutCracha.LayoutCrachaID + "')", _Con);

                        _sqlCmd.ExecuteNonQuery();
                    }

                }
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresasLayoutsCrachasBD ex: " + ex);


            }

        }

        private string BuscaLogo(int empresaID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement("ClasseArquivosImagens");
                _xmlDocument.AppendChild(_ClasseArquivosImagens);

                XmlNode _ArquivosImagens = _xmlDocument.CreateElement("ArquivosImagens");
                _ClasseArquivosImagens.AppendChild(_ArquivosImagens);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand SQCMDXML = new SqlCommand("Select * From Empresas Where EmpresaID = " + empresaID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Logo"].ToString())));
                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void BuscaLogo ex: " + ex);
                return null;
            }
        }
        #endregion


    }
}
