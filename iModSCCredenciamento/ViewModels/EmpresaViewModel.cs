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

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresaViewModel : ViewModelBase
    {

        public string ConteudoPesquisa { get; set; }

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

                    //OnEmpresaSelecionada();
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
            catch (Exception)
            {
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

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasTemp);
                        xmlString = sw.ToString();
                    }

                }

                AtualizaEmpresaBD(xmlString);

                if (EmpresaSelecionada.EmpresaID == 0)
                {
                    //string _xml = RequisitaEmpresas("", Nome_tb.Text, CNPJ_tb.Text, "1");
                    //XmlDocument _xmldocument = new XmlDocument();
                    //_xmldocument.LoadXml(_xml);
                    //Codigo_tb.Text = _xmldocument.GetElementsByTagName("EmpresaID")[0].InnerText;
                    //ListaEmpresas_lv.IsEnabled = true;
                    //_Novo = false;
                }
                _ClasseEmpresasTemp = null;

                /////////////////////////////////////////

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
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                Global._cnpjEdicao = "";
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresas));

                ObservableCollection<ClasseEmpresas.Empresa> _EmpresasPro = new ObservableCollection<ClasseEmpresas.Empresa>();
                ClasseEmpresas _ClasseEmpresasTemp = new ClasseEmpresas();
                EmpresaSelecionada.Pendente = true;
                EmpresaSelecionada.Pendente11 = true;
                EmpresaSelecionada.Pendente12 = true;
                EmpresaSelecionada.Pendente13 = false;
                EmpresaSelecionada.Pendente14 = true;
                EmpresaSelecionada.Pendente15 = false;
                EmpresaSelecionada.Pendente16 = false;
                EmpresaSelecionada.Pendente17 = true;

                _EmpresasPro.Add(EmpresaSelecionada);
                _ClasseEmpresasTemp.Empresas = _EmpresasPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasTemp);
                        xmlString = sw.ToString();
                    }

                }

                int _novoEmpresaID =InsereEmpresaBD(xmlString);

                AtualizaPendencias(_novoEmpresaID);

                EmpresaSelecionada.EmpresaID = _novoEmpresaID;

                _EmpresasTemp.Clear();

                _EmpresasTemp.Add(EmpresaSelecionada);
                Empresas = null;
                Empresas = new ObservableCollection<ClasseEmpresas.Empresa>(_EmpresasTemp);
                SelectedIndex = 0;
                _EmpresasTemp.Clear();

                _EmpresasPro.Clear();
                //_EmpresasPro = null;

                //_EmpresasTemp = null;
                _ClasseEmpresasTemp = null;


            }
            catch (Exception ex)
            {
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
            }
        }

        public void OnExcluirCommand()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir esta empresa?", "Excluir Empresa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados desta empresa, inclusive histórico. Confirma exclusão?", "Excluir Empresa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {

                //        Empresas.Remove(EmpresaSelecionada);
                //        // inserir ação no banco de dados

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        if (Global.PopupBox("Você realmente tem certeza desta operação?", 2))
                        {
                            ExcluiEmpresaBD(EmpresaSelecionada.EmpresaID);
                            Empresas.Remove(EmpresaSelecionada);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
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
            }
        }

        public void OnExcluirAtividadeCommand()
        {
            try
            {
                EmpresasTiposAtividades.Remove(EmpresaTipoAtividadeSelecionada);
            }
            catch (Exception ex)
            {
            }
        }

        public void OnInserirAtividadeCommand(string _TipoAtividadeIDstr, string _Descricao)
        {
            try
            {
                int _TipoAtividadeID = Convert.ToInt32(_TipoAtividadeIDstr);
                ClasseEmpresasTiposAtividades.EmpresaTipoAtividade _EmpresaTipoAtividade = new ClasseEmpresasTiposAtividades.EmpresaTipoAtividade();
                _EmpresaTipoAtividade.EmpresaID = EmpresaSelecionada.EmpresaID;
                _EmpresaTipoAtividade.TipoAtividadeID = _TipoAtividadeID;
                _EmpresaTipoAtividade.Descricao = _Descricao;
                EmpresasTiposAtividades.Add(_EmpresaTipoAtividade);

                //EmpresasTiposAtividades.Add();
            }
            catch (Exception ex)
            {
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
            }
        }

        //public void OnInserirAcessoCommand(string _AreaAcessoIDstr, string _Descricao)
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
            }
        }

        public void OnExcluirCrachaCommand()
        {
            try
            {
                EmpresasLayoutsCrachas.Remove(EmpresaLayoutCrachaSelecionada);
            }
            catch (Exception ex)
            {
            }
        }

        public void OnInserirCrachaCommand(int _LayoutCrachaID, string _Nome)
        {
            try
            {
                ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha _EmpresaLayoutCracha = new ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha();
                _EmpresaLayoutCracha.EmpresaID = EmpresaSelecionada.EmpresaID;
                _EmpresaLayoutCracha.LayoutCrachaID = _LayoutCrachaID;
                _EmpresaLayoutCracha.Nome = _Nome;
                EmpresasLayoutsCrachas.Add(_EmpresaLayoutCracha);

                //EmpresasLayoutsCrachas.Add();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
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

        public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupPendencias popupPendencias = new PopupPendencias(1,((System.Windows.FrameworkElement)e.OriginalSource).Tag,EmpresaSelecionada.EmpresaID, EmpresaSelecionada.Nome);
                popupPendencias.ShowDialog();
                popupPendencias = null;
                CarregaColecaoEmpresas(EmpresaSelecionada.EmpresaID);


            }
            catch (Exception ex)
            {
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
                
                CarregaColeçãoEmpresasTiposAtividades(EmpresaSelecionada.EmpresaID);
                CarregaColeçãoEmpresasAreasAcessos(EmpresaSelecionada.EmpresaID);
                CarregaColeçãoEmpresasLayoutsCrachas(EmpresaSelecionada.EmpresaID);
                CarregaColecaoLayoutsCrachas();

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void OnEmpresaSelecionada ex: " + ex.Message);
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
            CarregaColeçãoEstados();
            CarregaColecaoTiposAtividades();
            CarregaColecaoAreasAcessos();
            CarregaColecaoLayoutsCrachas();
        }

        private void CarregaColecaoEmpresas(int _empresaID = 0 , string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        {
            try
            {
                string _xml = RequisitaEmpresas(_empresaID, _nome, _apelido, _cNPJ);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresas classeEmpresas = new ClasseEmpresas();
                classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
                Empresas = new ObservableCollection<ClasseEmpresas.Empresa>();
                Empresas = classeEmpresas.Empresas;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColeçãoEstados()
        {
            try
            {

                string _xml = RequisitaEstados();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEstados));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseEstados classeEstados = new ClasseEstados();
                classeEstados = (ClasseEstados)deserializer.Deserialize(reader);
                Estados = new ObservableCollection<ClasseEstados.Estado>();
                Estados = classeEstados.Estados;

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoEstados ex: " + ex);
            }
        }

        public void CarregaColeçãoMunicipios(string _EstadoUF = "%")
        {

            try
            {

                string _xml = RequisitaMunicipios(_EstadoUF);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseMunicipios));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseMunicipios classeMunicipios = new ClasseMunicipios();
                classeMunicipios = (ClasseMunicipios)deserializer.Deserialize(reader);
                Municipios = new ObservableCollection<ClasseMunicipios.Municipio>();
                Municipios = classeMunicipios.Municipios;

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
                string _xml = RequisitaTiposAtividades();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposAtividades));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseTiposAtividades classeTiposAtividades = new ClasseTiposAtividades();
                classeTiposAtividades = (ClasseTiposAtividades)deserializer.Deserialize(reader);
                TiposAtividades = new ObservableCollection<ClasseTiposAtividades.TipoAtividade>();
                TiposAtividades = classeTiposAtividades.TiposAtividades;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void CarregaColeçãoEmpresasTiposAtividades(int _empresaID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaEmpresasTiposAtividades(_empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasTiposAtividades));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseEmpresasTiposAtividades classeEmpresasTiposAtividades = new ClasseEmpresasTiposAtividades();
                classeEmpresasTiposAtividades = (ClasseEmpresasTiposAtividades)deserializer.Deserialize(reader);
                EmpresasTiposAtividades = new ObservableCollection<ClasseEmpresasTiposAtividades.EmpresaTipoAtividade>();
                EmpresasTiposAtividades = classeEmpresasTiposAtividades.EmpresasTiposAtividades;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                string _xml = RequisitaAreasAcessos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseAreasAcessos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseAreasAcessos classeAreasAcessos = new ClasseAreasAcessos();
                classeAreasAcessos = (ClasseAreasAcessos)deserializer.Deserialize(reader);
                AreasAcessos = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                AreasAcessos = classeAreasAcessos.AreasAcessos;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void CarregaColeçãoEmpresasAreasAcessos(int _empresaID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaEmpresasAreasAcessos(_empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasAreasAcessos));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseEmpresasAreasAcessos classeEmpresasAreasAcessos = new ClasseEmpresasAreasAcessos();
                classeEmpresasAreasAcessos = (ClasseEmpresasAreasAcessos)deserializer.Deserialize(reader);
                EmpresasAreasAcessos = new ObservableCollection<ClasseEmpresasAreasAcessos.EmpresaAreaAcesso>();
                EmpresasAreasAcessos = classeEmpresasAreasAcessos.EmpresasAreasAcessos;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        private void CarregaColecaoLayoutsCrachas()
        {
            try
            {
                string _xml = RequisitaLayoutsCrachas();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseLayoutsCrachas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseLayoutsCrachas classeLayoutsCrachas = new ClasseLayoutsCrachas();
                classeLayoutsCrachas = (ClasseLayoutsCrachas)deserializer.Deserialize(reader);
                LayoutsCrachas = new ObservableCollection<ClasseLayoutsCrachas.LayoutCracha>();
                LayoutsCrachas = classeLayoutsCrachas.LayoutsCrachas;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void CarregaColeçãoEmpresasLayoutsCrachas(int _empresaID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaEmpresasLayoutsCrachas(_empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasLayoutsCrachas));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseEmpresasLayoutsCrachas classeEmpresasLayoutsCrachas = new ClasseEmpresasLayoutsCrachas();
                classeEmpresasLayoutsCrachas = (ClasseEmpresasLayoutsCrachas)deserializer.Deserialize(reader);
                EmpresasLayoutsCrachas = new ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha>();
                EmpresasLayoutsCrachas = classeEmpresasLayoutsCrachas.EmpresasLayoutsCrachas;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        private void CarregaColeçãoTiposEmpresa()
        {
            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaTiposEmpresa();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposEmpresas));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseTiposEmpresas classeTiposEmpresa = new ClasseTiposEmpresas();
                classeTiposEmpresa = (ClasseTiposEmpresas)deserializer.Deserialize(reader);
                TiposEmpresa = new ObservableCollection<ClasseTiposEmpresas.TipoEmpresa>();
                TiposEmpresa = classeTiposEmpresa.TiposEmpresas;


                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoEstados ex: " + ex);
            }
        }

        private void CarregaColecaoSeguros(int empresaID)
        {
            try
            {
                string _xml = RequisitaSeguros(empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasSeguros));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasSeguros classeSegurosEmpresa = new ClasseEmpresasSeguros();
                classeSegurosEmpresa = (ClasseEmpresasSeguros)deserializer.Deserialize(reader);
                Seguros = new ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro>();
                Seguros = classeSegurosEmpresa.EmpresasSeguros;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
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
        private string RequisitaEmpresas(int _empresaID = 0 , string _nome = "", string _apelido = "", string _cNPJ = "", int _excluida = 0, string _quantidaderegistro = "500")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresas = _xmlDocument.CreateElement("ClasseEmpresas");
                _xmlDocument.AppendChild(_ClasseEmpresas);

                XmlNode _Empresas = _xmlDocument.CreateElement("Empresas");
                _ClasseEmpresas.AppendChild(_Empresas);

                string _strSql = " [EmpresaID],[Nome],[Apelido],[Sigla],[CNPJ],[CEP],[Endereco]," +
                    "[Numero],[Complemento],[Bairro],[MunicipioID],[EstadoID]," +
                    "[Email1],[Contato1],[Telefone1],[Celular1],[Email2],[Contato2],[Telefone2],[Celular2]," +
                    "[Obs],[Responsavel],[InsEst],[InsMun],[Excluida],[Pendente11],[Pendente12],[Pendente13],[Pendente14]" +
                    ",[Pendente15],[Pendente16],[Pendente17]";



                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _empresaIDSTR = "";

                _empresaIDSTR = _empresaID == 0 ? "" : " AND EmpresaID = " + _empresaID;
                _nome = _nome == "" ? "" : " AND Nome like '%" + _nome + "%' ";
                _apelido = _apelido == "" ? "" : "AND Apelido like '%" + _apelido + "%' ";
                _cNPJ = _cNPJ == "" ? "" : " AND CNPJ like '%" + _cNPJ.RetirarCaracteresEspeciais() + "%'";

                if (_quantidaderegistro == "0")
                {
                    _strSql = "select " + _strSql + " from Empresas where Excluida = " + _excluida + _empresaIDSTR +
                       _nome + _apelido + _cNPJ + " order by EmpresaID desc";
                }
                else
                {
                    _strSql = "select Top " + _quantidaderegistro + _strSql + " from Empresas where Excluida = " + _excluida + _empresaIDSTR +
                       _nome + _apelido + _cNPJ + " order by EmpresaID desc";
                }



                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {
                    XmlNode _Empresa = _xmlDocument.CreateElement("Empresa");
                    _Empresas.AppendChild(_Empresa);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString().Trim())));
                    _Empresa.AppendChild(_EmpresaID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Empresa.AppendChild(_Nome);

                    XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
                    _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
                    _Empresa.AppendChild(_Apelido);

                    XmlNode _Sigla = _xmlDocument.CreateElement("Sigla");
                    _Sigla.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Sigla"].ToString())));
                    _Empresa.AppendChild(_Sigla);

                    XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
                    _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString().FormatarCnpj())));
                    _Empresa.AppendChild(_CNPJ);

                    XmlNode _InsEst = _xmlDocument.CreateElement("InsEst");
                    _InsEst.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsEst"].ToString())));
                    _Empresa.AppendChild(_InsEst);

                    XmlNode _InsMun = _xmlDocument.CreateElement("InsMun");
                    _InsMun.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsMun"].ToString())));
                    _Empresa.AppendChild(_InsMun);

                    XmlNode _Responsavel = _xmlDocument.CreateElement("Responsavel");
                    _Responsavel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Responsavel"].ToString())));
                    _Empresa.AppendChild(_Responsavel);

                    XmlNode _CEP = _xmlDocument.CreateElement("CEP");
                    _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
                    _Empresa.AppendChild(_CEP);

                    XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    _Empresa.AppendChild(_Endereco);

                    XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    _Empresa.AppendChild(_Numero);

                    XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    _Empresa.AppendChild(_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    _Empresa.AppendChild(_Bairro);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _Empresa.AppendChild(_EstadoID);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _Empresa.AppendChild(_MunicipioID);

                    XmlNode _Email1 = _xmlDocument.CreateElement("Email1");
                    _Email1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email1"].ToString())));
                    _Empresa.AppendChild(_Email1);

                    XmlNode _Contato1 = _xmlDocument.CreateElement("Contato1");
                    _Contato1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato1"].ToString())));
                    _Empresa.AppendChild(_Contato1);

                    XmlNode _Tel1 = _xmlDocument.CreateElement("Telefone1");
                    _Tel1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone1"].ToString())));
                    _Empresa.AppendChild(_Tel1);

                    XmlNode _Cel1 = _xmlDocument.CreateElement("Celular1");
                    _Cel1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular1"].ToString())));
                    _Empresa.AppendChild(_Cel1);

                    XmlNode Email2 = _xmlDocument.CreateElement("Email2");
                    Email2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email2"].ToString())));
                    _Empresa.AppendChild(Email2);

                    XmlNode _Contato2 = _xmlDocument.CreateElement("Contato2");
                    _Contato2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato2"].ToString())));
                    _Empresa.AppendChild(_Contato2);

                    XmlNode _Tel2 = _xmlDocument.CreateElement("Telefone2");
                    _Tel2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone2"].ToString())));
                    _Empresa.AppendChild(_Tel2);

                    XmlNode _Cel2 = _xmlDocument.CreateElement("Celular2");
                    _Cel2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular2"].ToString())));
                    _Empresa.AppendChild(_Cel2);
                    //////////////////////////////////////////////
                    XmlNode _Obs = _xmlDocument.CreateElement("Obs");
                    _Obs.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Obs"].ToString())));
                    _Empresa.AppendChild(_Obs);

                    XmlNode _Logo = _xmlDocument.CreateElement("Logo");
                    //_Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
                    _Empresa.AppendChild(_Logo);

                    XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
                    _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    _Empresa.AppendChild(_Excluida);

                    XmlNode _Pendente1 = _xmlDocument.CreateElement("Pendente11");
                    _Pendente1.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente11"])).ToString()));
                    _Empresa.AppendChild(_Pendente1);

                    XmlNode _Pendente2 = _xmlDocument.CreateElement("Pendente12");
                    _Pendente2.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente12"])).ToString()));
                    _Empresa.AppendChild(_Pendente2);

                    XmlNode _Pendente3 = _xmlDocument.CreateElement("Pendente13");
                    _Pendente3.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente13"])).ToString()));
                    _Empresa.AppendChild(_Pendente3);

                    XmlNode _Pendente4 = _xmlDocument.CreateElement("Pendente14");
                    _Pendente4.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente14"])).ToString()));
                    _Empresa.AppendChild(_Pendente4);

                    XmlNode _Pendente5 = _xmlDocument.CreateElement("Pendente15");
                    _Pendente5.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente15"])).ToString()));
                    _Empresa.AppendChild(_Pendente5);

                    XmlNode _Pendente6 = _xmlDocument.CreateElement("Pendente16");
                    _Pendente6.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente16"])).ToString()));
                    _Empresa.AppendChild(_Pendente6);

                    XmlNode _Pendente7 = _xmlDocument.CreateElement("Pendente17");
                    _Pendente7.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente17"])).ToString()));
                    _Empresa.AppendChild(_Pendente7);

                    bool _pend = false;
                    _pend = (bool)_sqlreader["Pendente11"] ||
                        (bool)_sqlreader["Pendente12"] ||
                        (bool)_sqlreader["Pendente13"] ||
                        (bool)_sqlreader["Pendente14"] ||
                        (bool)_sqlreader["Pendente15"] ||
                        (bool)_sqlreader["Pendente16"] ||
                        (bool)_sqlreader["Pendente17"];

                    XmlNode _Pendente = _xmlDocument.CreateElement("Pendente");
                    _Pendente.AppendChild(_xmlDocument.CreateTextNode(Convert.ToInt32(_pend).ToString()));
                    _Empresa.AppendChild(_Pendente);

                    _strSql = " SELECT COUNT(dbo.ColaboradoresCredenciais.ColaboradorCredencialID) AS TotalPermanente " +
                        "FROM dbo.ColaboradoresCredenciais INNER JOIN dbo.ColaboradoresEmpresas ON" +
                        " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID " +
                        " WHERE(dbo.ColaboradoresEmpresas.EmpresaID =" + (_sqlreader["EmpresaID"].ToString().Trim()) + ") AND(dbo.ColaboradoresCredenciais.Ativa = 1) AND " +
                        "(dbo.ColaboradoresCredenciais.TipoCredencialID = 1)";

                    SqlCommand _sqlcmd1 = new SqlCommand(_strSql, _Con);
                    SqlDataReader _sqlreader1 = _sqlcmd1.ExecuteReader(CommandBehavior.Default);

                    if (_sqlreader1.Read())
                    {
                        XmlNode _TotalPermanente = _xmlDocument.CreateElement("TotalPermanente");
                        _TotalPermanente.AppendChild(_xmlDocument.CreateTextNode((_sqlreader1["TotalPermanente"].ToString())));
                        _Empresa.AppendChild(_TotalPermanente);
                    }

                    _strSql = " SELECT COUNT(dbo.ColaboradoresCredenciais.ColaboradorCredencialID) AS TotalTemporaria " +
                        "FROM dbo.ColaboradoresCredenciais INNER JOIN dbo.ColaboradoresEmpresas ON" +
                        " dbo.ColaboradoresCredenciais.ColaboradorEmpresaID = dbo.ColaboradoresEmpresas.ColaboradorEmpresaID " +
                        " WHERE(dbo.ColaboradoresEmpresas.EmpresaID = " + (_sqlreader["EmpresaID"].ToString().Trim()) + ") AND(dbo.ColaboradoresCredenciais.Ativa = 1) AND " +
                        "(dbo.ColaboradoresCredenciais.TipoCredencialID = 2)";

                    SqlCommand _sqlcmd2 = new SqlCommand(_strSql, _Con);
                    SqlDataReader _sqlreader2 = _sqlcmd2.ExecuteReader(CommandBehavior.Default);

                    if (_sqlreader2.Read())
                    {
                        XmlNode _TotalTemporaria = _xmlDocument.CreateElement("TotalTemporaria");
                        _TotalTemporaria.AppendChild(_xmlDocument.CreateTextNode((_sqlreader2["TotalTemporaria"].ToString())));
                        _Empresa.AppendChild(_TotalTemporaria);
                    }

                }



                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception ex)
            {

                return null;
            }
            return null;
        }

        //private string RequisitaEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", int _excluida = 0, string _quantidaderegistro = "500")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseEmpresas = _xmlDocument.CreateElement("ClasseEmpresas");
        //        _xmlDocument.AppendChild(_ClasseEmpresas);

        //        XmlNode _Empresas = _xmlDocument.CreateElement("Empresas");
        //        _ClasseEmpresas.AppendChild(_Empresas);

        //        string _strSql = " [EmpresaID],[Nome],[Apelido],[CNPJ],[CEP],[Endereco]," +
        //            "[Numero],[Complemento],[Bairro],[MunicipioID],[EstadoID],[Telefone],[Contato]," +
        //            "[Celular],[Email],[Obs],[Responsavel],[InsEst],[InsMun],[Excluida]";


        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

        //        _empresaID = "%" + _empresaID + "%";
        //        _nome = "%" + _nome + "%";
        //        _apelido = "%" + _apelido + "%";
        //        _cNPJ = "%" + _cNPJ + "%";

        //        if (_quantidaderegistro == "0")
        //        {
        //            _strSql = "select " + _strSql  + " from Empresas where EmpresaID like '" + _empresaID + "' and EmpresaNome like '" +
        //                _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + " order by EmpresaID desc";
        //        }
        //        else
        //        {
        //            _strSql = "select Top " + _quantidaderegistro +  _strSql  + "  from Empresas where EmpresaID like '" +
        //                _empresaID + "' and Nome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ +
        //                "' and Excluida  = " + _excluida + " order by EmpresaID desc";
        //        }


        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _Empresa = _xmlDocument.CreateElement("Empresa");
        //            _Empresas.AppendChild(_Empresa);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
        //            _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
        //            _Empresa.AppendChild(_EmpresaID);

        //            XmlNode _Nome = _xmlDocument.CreateElement("Nome");
        //            _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
        //            _Empresa.AppendChild(_Nome);

        //            XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
        //            _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
        //            _Empresa.AppendChild(_Apelido);

        //            XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
        //            _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString())));
        //            _Empresa.AppendChild(_CNPJ);

        //            XmlNode _InsEst = _xmlDocument.CreateElement("InsEst");
        //            _InsEst.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsEst"].ToString())));
        //            _Empresa.AppendChild(_InsEst);

        //            XmlNode _InsMun = _xmlDocument.CreateElement("InsMun");
        //            _InsMun.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsMun"].ToString())));
        //            _Empresa.AppendChild(_InsMun);

        //            XmlNode _Responsavel = _xmlDocument.CreateElement("Responsavel");
        //            _Responsavel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Responsavel"].ToString())));
        //            _Empresa.AppendChild(_Responsavel);

        //            XmlNode _CEP = _xmlDocument.CreateElement("CEP");
        //            _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
        //            _Empresa.AppendChild(_CEP);

        //            XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
        //            _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
        //            _Empresa.AppendChild(_Endereco);

        //            XmlNode _Numero = _xmlDocument.CreateElement("Numero");
        //            _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
        //            _Empresa.AppendChild(_Numero);

        //            XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
        //            _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
        //            _Empresa.AppendChild(_Complemento);

        //            XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
        //            _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
        //            _Empresa.AppendChild(_Bairro);

        //            XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
        //            _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
        //            _Empresa.AppendChild(_EstadoID);

        //            XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
        //            _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
        //            _Empresa.AppendChild(_MunicipioID);

        //            XmlNode _Tel = _xmlDocument.CreateElement("Telefone");
        //            _Tel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone"].ToString())));
        //            _Empresa.AppendChild(_Tel);

        //            XmlNode _Cel = _xmlDocument.CreateElement("Celular");
        //            _Cel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular"].ToString())));
        //            _Empresa.AppendChild(_Cel);

        //            XmlNode _Contato = _xmlDocument.CreateElement("Contato");
        //            _Contato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato"].ToString())));
        //            _Empresa.AppendChild(_Contato);

        //            XmlNode _Email = _xmlDocument.CreateElement("Email");
        //            _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
        //            _Empresa.AppendChild(_Email);

        //            XmlNode _Obs = _xmlDocument.CreateElement("Obs");
        //            _Obs.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Obs"].ToString())));
        //            _Empresa.AppendChild(_Obs);

        //            XmlNode _Logo = _xmlDocument.CreateElement("Logo");
        //            //_Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
        //            _Empresa.AppendChild(_Logo);

        //            XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
        //            _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
        //            _Empresa.AppendChild(_Excluida);


        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        return _xml;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //    return null;
        //}

        private string RequisitaEstados()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEstados = _xmlDocument.CreateElement("ClasseEstados");
                _xmlDocument.AppendChild(_ClasseEstados);

                XmlNode _Estados = _xmlDocument.CreateElement("Estados");
                _ClasseEstados.AppendChild(_Estados);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from Estados", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Estado = _xmlDocument.CreateElement("Estado");
                    _Estados.AppendChild(_Estado);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EstadoID"].ToString())));
                    _Estado.AppendChild(_EstadoID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
                    _Estado.AppendChild(_Nome);

                    XmlNode _UF = _xmlDocument.CreateElement("UF");
                    _UF.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["UF"].ToString())));
                    _Estado.AppendChild(_UF);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaEstados ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaMunicipios(string _EstadoUF = "%")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseMunicipios = _xmlDocument.CreateElement("ClasseMunicipios");
                _xmlDocument.AppendChild(_ClasseMunicipios);

                XmlNode _Municipios = _xmlDocument.CreateElement("Municipios");
                _ClasseMunicipios.AppendChild(_Municipios);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from Municipios where UF Like '" + _EstadoUF + "'", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Municipio = _xmlDocument.CreateElement("Municipio");
                    _Municipios.AppendChild(_Municipio);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["MunicipioID"].ToString())));
                    _Municipio.AppendChild(_MunicipioID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
                    _Municipio.AppendChild(_Nome);

                    XmlNode _UF = _xmlDocument.CreateElement("UF");
                    _UF.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["UF"].ToString())));
                    _Municipio.AppendChild(_UF);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaMunicipios ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaTiposAtividades()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposAtividades = _xmlDocument.CreateElement("ClasseTiposAtividades");
                _xmlDocument.AppendChild(_ClasseTiposAtividades);

                XmlNode _TiposAtividades = _xmlDocument.CreateElement("TiposAtividades");
                _ClasseTiposAtividades.AppendChild(_TiposAtividades);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from TiposAtividades order by TipoAtividadeID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoAtividade = _xmlDocument.CreateElement("TipoAtividade");
                    _TiposAtividades.AppendChild(_TipoAtividade);

                    XmlNode _TipoAtividadeID = _xmlDocument.CreateElement("TipoAtividadeID");
                    _TipoAtividadeID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoAtividadeID"].ToString())));
                    _TipoAtividade.AppendChild(_TipoAtividadeID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoAtividade.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTiposAtividades ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaEmpresasTiposAtividades(int _empresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasTiposAtividades = _xmlDocument.CreateElement("ClasseEmpresasTiposAtividades");
                _xmlDocument.AppendChild(_ClasseEmpresasTiposAtividades);

                XmlNode _EmpresasTiposAtividades = _xmlDocument.CreateElement("EmpresasTiposAtividades");
                _ClasseEmpresasTiposAtividades.AppendChild(_EmpresasTiposAtividades);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                string _SQL = "SELECT dbo.EmpresasTiposAtividades.EmpresaID,dbo.EmpresasTiposAtividades.TipoAtividadeID,dbo.EmpresasTiposAtividades.EmpresaTipoAtividadeID," +
                    " dbo.TiposAtividades.Descricao FROM dbo.EmpresasTiposAtividades INNER JOIN dbo.TiposAtividades ON " +
                    "dbo.EmpresasTiposAtividades.TipoAtividadeID = dbo.TiposAtividades.TipoAtividadeID WHERE(dbo.EmpresasTiposAtividades.EmpresaID=" + _empresaID + ")";
                //SqlCommand _sqlcmd = new SqlCommand("select * from EmpresasTiposAtividades where EmpresaID = " + _empresaID, _Con);
                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _EmpresaTipoAtividade = _xmlDocument.CreateElement("EmpresaTipoAtividade");
                    _EmpresasTiposAtividades.AppendChild(_EmpresaTipoAtividade);

                    XmlNode _EmpresaTipoAtividadeID = _xmlDocument.CreateElement("EmpresaTipoAtividadeID");
                    _EmpresaTipoAtividadeID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaTipoAtividadeID"].ToString())));
                    _EmpresaTipoAtividade.AppendChild(_EmpresaTipoAtividadeID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaID"].ToString())));
                    _EmpresaTipoAtividade.AppendChild(_EmpresaID);

                    XmlNode _TipoAtividadeID = _xmlDocument.CreateElement("TipoAtividadeID");
                    _TipoAtividadeID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoAtividadeID"].ToString())));
                    _EmpresaTipoAtividade.AppendChild(_TipoAtividadeID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _EmpresaTipoAtividade.AppendChild(_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaEmpresasTiposAtividades ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaAreasAcessos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseAreasAcessos = _xmlDocument.CreateElement("ClasseAreasAcessos");
                _xmlDocument.AppendChild(_ClasseAreasAcessos);

                XmlNode _AreasAcessos = _xmlDocument.CreateElement("AreasAcessos");
                _ClasseAreasAcessos.AppendChild(_AreasAcessos);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from AreasAcessos order by AreaAcessoID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _AreaAcesso = _xmlDocument.CreateElement("AreaAcesso");
                    _AreasAcessos.AppendChild(_AreaAcesso);

                    XmlNode _AreaAcessoID = _xmlDocument.CreateElement("AreaAcessoID");
                    _AreaAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["AreaAcessoID"].ToString())));
                    _AreaAcesso.AppendChild(_AreaAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _AreaAcesso.AppendChild(_Descricao);

                    XmlNode _Identificacao = _xmlDocument.CreateElement("Identificacao");
                    _Identificacao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Identificacao"].ToString())));
                    _AreaAcesso.AppendChild(_Identificacao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaAreasAcessos ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaEmpresasAreasAcessos(int _empresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasAreasAcessos = _xmlDocument.CreateElement("ClasseEmpresasAreasAcessos");
                _xmlDocument.AppendChild(_ClasseEmpresasAreasAcessos);

                XmlNode _EmpresasAreasAcessos = _xmlDocument.CreateElement("EmpresasAreasAcessos");
                _ClasseEmpresasAreasAcessos.AppendChild(_EmpresasAreasAcessos);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                string _SQL = "SELECT dbo.EmpresasAreasAcessos.EmpresaID,dbo.EmpresasAreasAcessos.AreaAcessoID,dbo.EmpresasAreasAcessos.EmpresaAreaAcessoID," +
                    " dbo.AreasAcessos.Descricao, dbo.AreasAcessos.Identificacao FROM dbo.EmpresasAreasAcessos INNER JOIN dbo.AreasAcessos ON " +
                    "dbo.EmpresasAreasAcessos.AreaAcessoID = dbo.AreasAcessos.AreaAcessoID WHERE(dbo.EmpresasAreasAcessos.EmpresaID=" + _empresaID + ")";
                //SqlCommand _sqlcmd = new SqlCommand("select * from EmpresasAreasAcessos where EmpresaID = " + _empresaID, _Con);
                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _EmpresaAreaAcesso = _xmlDocument.CreateElement("EmpresaAreaAcesso");
                    _EmpresasAreasAcessos.AppendChild(_EmpresaAreaAcesso);

                    XmlNode _EmpresaAreaAcessoID = _xmlDocument.CreateElement("EmpresaAreaAcessoID");
                    _EmpresaAreaAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaAreaAcessoID"].ToString())));
                    _EmpresaAreaAcesso.AppendChild(_EmpresaAreaAcessoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaID"].ToString())));
                    _EmpresaAreaAcesso.AppendChild(_EmpresaID);

                    XmlNode _AreaAcessoID = _xmlDocument.CreateElement("AreaAcessoID");
                    _AreaAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["AreaAcessoID"].ToString())));
                    _EmpresaAreaAcesso.AppendChild(_AreaAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _EmpresaAreaAcesso.AppendChild(_Descricao);

                    XmlNode _Identificacao = _xmlDocument.CreateElement("Identificacao");
                    _Identificacao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Identificacao"].ToString())));
                    _EmpresaAreaAcesso.AppendChild(_Identificacao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaEmpresasAreasAcessos ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaLayoutsCrachas()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseLayoutsCrachas = _xmlDocument.CreateElement("ClasseLayoutsCrachas");
                _xmlDocument.AppendChild(_ClasseLayoutsCrachas);

                XmlNode _LayoutsCrachas = _xmlDocument.CreateElement("LayoutsCrachas");
                _ClasseLayoutsCrachas.AppendChild(_LayoutsCrachas);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from LayoutsCrachas order by Nome", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _LayoutCracha = _xmlDocument.CreateElement("LayoutCracha");
                    _LayoutsCrachas.AppendChild(_LayoutCracha);

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaID"].ToString())));
                    _LayoutCracha.AppendChild(_LayoutCrachaID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
                    _LayoutCracha.AppendChild(_Nome);

                    XmlNode _LayoutCrachaGUID = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _LayoutCrachaGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString())));
                    _LayoutCracha.AppendChild(_LayoutCrachaGUID);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaLayoutsCrachas ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaEmpresasLayoutsCrachas(int _empresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasLayoutsCrachas = _xmlDocument.CreateElement("ClasseEmpresasLayoutsCrachas");
                _xmlDocument.AppendChild(_ClasseEmpresasLayoutsCrachas);

                XmlNode _EmpresasLayoutsCrachas = _xmlDocument.CreateElement("EmpresasLayoutsCrachas");
                _ClasseEmpresasLayoutsCrachas.AppendChild(_EmpresasLayoutsCrachas);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                string _SQL = "SELECT dbo.EmpresasLayoutsCrachas.EmpresaLayoutCrachaID, dbo.EmpresasLayoutsCrachas.EmpresaID, dbo.EmpresasLayoutsCrachas.LayoutCrachaID," +
                    " dbo.LayoutsCrachas.Nome, dbo.LayoutsCrachas.LayoutCrachaGUID FROM dbo.EmpresasLayoutsCrachas INNER JOIN" +
                    " dbo.LayoutsCrachas ON dbo.EmpresasLayoutsCrachas.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID WHERE(dbo.EmpresasLayoutsCrachas.EmpresaID = " + _empresaID + ")";
                //SqlCommand _sqlcmd = new SqlCommand("select * from EmpresasLayoutsCrachas where EmpresaID = " + _empresaID, _Con);
                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _EmpresaLayoutCracha = _xmlDocument.CreateElement("EmpresaLayoutCracha");
                    _EmpresasLayoutsCrachas.AppendChild(_EmpresaLayoutCracha);

                    XmlNode _EmpresaLayoutCrachaID = _xmlDocument.CreateElement("EmpresaLayoutCrachaID");
                    _EmpresaLayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaLayoutCrachaID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_EmpresaLayoutCrachaID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_EmpresaID);

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_LayoutCrachaID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Nome");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaEmpresasLayoutsCrachas ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaTiposEmpresa()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposEmpresa = _xmlDocument.CreateElement("ClasseTiposEmpresa");
                _xmlDocument.AppendChild(_ClasseTiposEmpresa);

                XmlNode _TiposEmpresa = _xmlDocument.CreateElement("TiposEmpresa");
                _ClasseTiposEmpresa.AppendChild(_TiposEmpresa);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlcmd = new SqlCommand("select * from TiposEmpresas", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoEmpresa = _xmlDocument.CreateElement("TipoEmpresa");
                    _TiposEmpresa.AppendChild(_TipoEmpresa);

                    XmlNode _TipoEmpresaID = _xmlDocument.CreateElement("TipoEmpresaID");
                    _TipoEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoEmpresaID"].ToString())));
                    _TipoEmpresa.AppendChild(_TipoEmpresaID);

                    XmlNode _TipoEmpresaDesc = _xmlDocument.CreateElement("TipoEmpresaDesc");
                    _TipoEmpresaDesc.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoEmpresaDesc"].ToString())));
                    _TipoEmpresa.AppendChild(_TipoEmpresaDesc);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTiposEmpresa ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaSeguros(int _empresaID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseSegurosEmpresas = _xmlDocument.CreateElement("ClasseSegurosEmpresa");
                _xmlDocument.AppendChild(_ClasseSegurosEmpresas);

                XmlNode _Seguros = _xmlDocument.CreateElement("Seguros");
                _ClasseSegurosEmpresas.AppendChild(_Seguros);

                string _strSql;


                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _strSql = "select * from SegurosEmpresa where SeguroEmpresaID = " + _empresaID;



                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Seguro = _xmlDocument.CreateElement("Seguro");
                    _Seguros.AppendChild(_Seguro);

                    XmlNode _SeguroID = _xmlDocument.CreateElement("SeguroID");
                    _SeguroID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroID"].ToString())));
                    _Seguro.AppendChild(_SeguroID);

                    XmlNode _SeguroSeguradora = _xmlDocument.CreateElement("SeguroSeguradora");
                    _SeguroSeguradora.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroSeguradora"].ToString())));
                    _Seguro.AppendChild(_SeguroSeguradora);

                    XmlNode _SeguroNumero = _xmlDocument.CreateElement("SeguroNumero");
                    _SeguroNumero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroNumero"].ToString())));
                    _Seguro.AppendChild(_SeguroNumero);

                    XmlNode _SeguroValorCobertura = _xmlDocument.CreateElement("SeguroValorCobertura");
                    _SeguroValorCobertura.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroValorCobertura"].ToString())));
                    _Seguro.AppendChild(_SeguroValorCobertura);

                    XmlNode _SeguroEmpresaID = _xmlDocument.CreateElement("SeguroEmpresaID");
                    _SeguroEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroEmpresaID"].ToString())));
                    _Seguro.AppendChild(_SeguroEmpresaID);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {
                
                return null;
            }
            return null;
        }

        private int InsereEmpresaBD(string xmlString)
        {
            try
            {
                int _novID = 0;

                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseEmpresas.Empresa _empresa = new ClasseEmpresas.Empresa();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresa.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);


                _empresa.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Nome")[i].InnerText);
                _empresa.Apelido = _xmlDoc.GetElementsByTagName("Apelido")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Apelido")[i].InnerText;
                _empresa.Sigla = _xmlDoc.GetElementsByTagName("Sigla")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Sigla")[i].InnerText;
                _empresa.CNPJ = _xmlDoc.GetElementsByTagName("CNPJ")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNPJ")[i].InnerText;
                _empresa.CEP = _xmlDoc.GetElementsByTagName("CEP")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CEP")[i].InnerText;
                _empresa.Endereco = _xmlDoc.GetElementsByTagName("Endereco")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Endereco")[i].InnerText;
                _empresa.Numero = _xmlDoc.GetElementsByTagName("Numero")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Numero")[i].InnerText;
                _empresa.Complemento = _xmlDoc.GetElementsByTagName("Complemento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Complemento")[i].InnerText;
                _empresa.Bairro = _xmlDoc.GetElementsByTagName("Bairro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bairro")[i].InnerText;
                _empresa.EstadoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
                _empresa.MunicipioID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);

                _empresa.Telefone1 = _xmlDoc.GetElementsByTagName("Telefone1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Telefone1")[i].InnerText;
                _empresa.Contato1 = _xmlDoc.GetElementsByTagName("Contato1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Contato1")[i].InnerText;
                _empresa.Celular1 = _xmlDoc.GetElementsByTagName("Celular1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Celular1")[i].InnerText;
                _empresa.Email1 = _xmlDoc.GetElementsByTagName("Email1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email1")[i].InnerText;
                _empresa.Telefone2 = _xmlDoc.GetElementsByTagName("Telefone2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Telefone2")[i].InnerText;
                _empresa.Contato2 = _xmlDoc.GetElementsByTagName("Contato2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Contato2")[i].InnerText;
                _empresa.Celular2 = _xmlDoc.GetElementsByTagName("Celular2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Celular2")[i].InnerText;
                _empresa.Email2 = _xmlDoc.GetElementsByTagName("Email2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email2")[i].InnerText;


                _empresa.Obs = _xmlDoc.GetElementsByTagName("Obs")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Obs")[i].InnerText;
                _empresa.Responsavel = _xmlDoc.GetElementsByTagName("Responsavel")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Responsavel")[i].InnerText;
                _empresa.Logo = _xmlDoc.GetElementsByTagName("Logo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Logo")[i].InnerText;
                _empresa.InsEst = _xmlDoc.GetElementsByTagName("InsEst")[i] == null ? "" : _xmlDoc.GetElementsByTagName("InsEst")[i].InnerText;
                _empresa.InsMun = _xmlDoc.GetElementsByTagName("InsMun")[i] == null ? "" : _xmlDoc.GetElementsByTagName("InsMun")[i].InnerText;
                _empresa.Excluida = Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
                bool _controlado1, _controlado2, _controlado3, _controlado4, _controlado5, _controlado6, _controlado7;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente11")[i].InnerText, out _controlado1);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente12")[i].InnerText, out _controlado2);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente13")[i].InnerText, out _controlado3);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente14")[i].InnerText, out _controlado4);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente15")[i].InnerText, out _controlado5);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente16")[i].InnerText, out _controlado6);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente17")[i].InnerText, out _controlado7);

                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_empresa.EmpresaID == 0)
                {
                    _sqlCmd = new SqlCommand("Insert into Empresas(Nome,Apelido,Sigla,CNPJ,CEP,Endereco,Numero,Complemento,Bairro" +
                        ",MunicipioID,EstadoID,Telefone1,Contato1,Celular1,Email1,Telefone2,Contato2,Celular2" +
                        ",Email2,Obs,Responsavel,Logo,InsEst,InsMun,Excluida,Pendente11,Pendente12,Pendente13,Pendente14" +
                        ",Pendente15,Pendente16,Pendente17) values ('" +
                                                          _empresa.Nome + "','" + _empresa.Apelido + "','" + _empresa.Sigla + "','" + _empresa.CNPJ.RetirarCaracteresEspeciais() + "','" + _empresa.CEP + "','" + _empresa.Endereco + "','" + _empresa.Numero +
                                                          "','" + _empresa.Complemento + "','" + _empresa.Bairro + "'," + _empresa.MunicipioID + "," + _empresa.EstadoID + ",'" +
                                                          _empresa.Telefone1 + "','" + _empresa.Contato1 + "','" + _empresa.Celular1 + "','" + _empresa.Email1 + "','" +
                                                          _empresa.Telefone2 + "','" + _empresa.Contato2 + "','" + _empresa.Celular2 + "','" + _empresa.Email2 + "','" +
                                                          _empresa.Obs + "','" + _empresa.Responsavel + "','" + _empresa.Logo + "','" + _empresa.InsEst + "','" + _empresa.InsMun +
                                                          "'," + _empresa.Excluida + ",'" + _controlado1 + "','" + _controlado2 + "','" + _controlado3 + "','"
                                                          + _controlado4 + "','" + _controlado5 + "','" + _controlado6 + "','" + _controlado7  + "');SELECT SCOPE_IDENTITY();", _Con);

                     _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
                    _Con.Close();
                 

                }

                return _novID;
 
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresaBD ex: " + ex);
                return 0;

            }

        }

        private void AtualizaEmpresaBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseEmpresas.Empresa _empresa = new ClasseEmpresas.Empresa();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresa.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);


                _empresa.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Nome")[i].InnerText);
                _empresa.Apelido = _xmlDoc.GetElementsByTagName("Apelido")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Apelido")[i].InnerText;
                _empresa.Sigla = _xmlDoc.GetElementsByTagName("Sigla")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Sigla")[i].InnerText;
                _empresa.CNPJ = _xmlDoc.GetElementsByTagName("CNPJ")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNPJ")[i].InnerText;
                _empresa.CEP = _xmlDoc.GetElementsByTagName("CEP")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CEP")[i].InnerText;
                _empresa.Endereco = _xmlDoc.GetElementsByTagName("Endereco")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Endereco")[i].InnerText;
                _empresa.Numero = _xmlDoc.GetElementsByTagName("Numero")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Numero")[i].InnerText;
                _empresa.Complemento = _xmlDoc.GetElementsByTagName("Complemento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Complemento")[i].InnerText;
                _empresa.Bairro = _xmlDoc.GetElementsByTagName("Bairro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bairro")[i].InnerText;
                _empresa.EstadoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
                _empresa.MunicipioID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);

                _empresa.Telefone1 = _xmlDoc.GetElementsByTagName("Telefone1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Telefone1")[i].InnerText;
                _empresa.Contato1 = _xmlDoc.GetElementsByTagName("Contato1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Contato1")[i].InnerText;
                _empresa.Celular1 = _xmlDoc.GetElementsByTagName("Celular1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Celular1")[i].InnerText;
                _empresa.Email1 = _xmlDoc.GetElementsByTagName("Email1")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email1")[i].InnerText;
                _empresa.Telefone2 = _xmlDoc.GetElementsByTagName("Telefone2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Telefone2")[i].InnerText;
                _empresa.Contato2 = _xmlDoc.GetElementsByTagName("Contato2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Contato2")[i].InnerText;
                _empresa.Celular2 = _xmlDoc.GetElementsByTagName("Celular2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Celular2")[i].InnerText;
                _empresa.Email2 = _xmlDoc.GetElementsByTagName("Email2")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email2")[i].InnerText;


                _empresa.Obs = _xmlDoc.GetElementsByTagName("Obs")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Obs")[i].InnerText;
                _empresa.Responsavel = _xmlDoc.GetElementsByTagName("Responsavel")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Responsavel")[i].InnerText;
                _empresa.Logo = _xmlDoc.GetElementsByTagName("Logo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Logo")[i].InnerText;
                _empresa.InsEst = _xmlDoc.GetElementsByTagName("InsEst")[i] == null ? "" : _xmlDoc.GetElementsByTagName("InsEst")[i].InnerText;
                _empresa.InsMun = _xmlDoc.GetElementsByTagName("InsMun")[i] == null ? "" : _xmlDoc.GetElementsByTagName("InsMun")[i].InnerText;
                _empresa.Excluida = Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
                bool _controlado1, _controlado2, _controlado3, _controlado4, _controlado5, _controlado6, _controlado7;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente11")[i].InnerText, out _controlado1);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente12")[i].InnerText, out _controlado2);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente13")[i].InnerText, out _controlado3);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente14")[i].InnerText, out _controlado4);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente15")[i].InnerText, out _controlado5);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente16")[i].InnerText, out _controlado6);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente17")[i].InnerText, out _controlado7);

                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_empresa.EmpresaID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Empresas Set" +
                        " Nome= '" + _empresa.Nome + "'" +
                        ",Apelido= '" + _empresa.Apelido + "'" +
                        ",Sigla= '" + _empresa.Sigla + "'" +
                        ",CNPJ= '" + _empresa.CNPJ.RetirarCaracteresEspeciais() + "'" +
                        ",CEP= '" + _empresa.CEP + "'" +
                        ",Complemento= '" + _empresa.Complemento + "'" +
                        ",Endereco= '" + _empresa.Endereco + "'" +
                        ",Numero= '" + _empresa.Numero + "'" +
                        ",Bairro= '" + _empresa.Bairro + "'" +
                        ",MunicipioID=" + _empresa.MunicipioID + "" +
                        ",EstadoID =" + _empresa.EstadoID + "" +
                        ",Telefone1= '" + _empresa.Telefone1 + "'" +
                        ",Contato1= '" + _empresa.Contato1 + "'" +
                        ",Celular1= '" + _empresa.Celular1 + "'" +
                        ",Email1= '" + _empresa.Email1 + "'" +
                        ",Telefone2= '" + _empresa.Telefone2 + "'" +
                        ",Contato2= '" + _empresa.Contato2 + "'" +
                        ",Celular2= '" + _empresa.Celular2 + "'" +
                        ",Email2= '" + _empresa.Email2 + "'" +
                        ",Obs= '" + _empresa.Obs + "'" +
                        ",Responsavel= '" + _empresa.Responsavel + "'" +
                        ",Logo= '" + _empresa.Logo + "'" +
                        ",InsEst= '" + _empresa.InsEst + "'" +
                        ",InsMun= '" + _empresa.InsMun + "'" +
                        ",Excluida=" + _empresa.Excluida + "" +
                        " Where EmpresaID = " + _empresa.EmpresaID + "", _Con);
                    _sqlCmd.ExecuteNonQuery();
                    _Con.Close();
                }
                
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void AtualizaEmpresaBD ex: " + ex);


            }

        }

        private void ExcluiEmpresaBD(int _EmpresaID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Empresas where EmpresaID=" + _EmpresaID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiEmpresaBD ex: " + ex);


            }
        }


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
                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
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
                for (int i = 11; i <18; i++)
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
                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
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

        #region Testes



        //protected bool CanExecute
        //{
        //    get
        //    {
        //        return this.EmpresaSelecionada != null;
        //    }
        //}

        //private void Teste()
        //{
        //    MessageBox.Show(this.EmpresaSelecionada.CNPJ);
        //    HabilitaEdicao = false;
        //}
        //private List<BadgeTemplate> _badges = new List<BadgeTemplate>();
        //public void BuscaBadges()
        //{

        //    try
        //    {
        //        EntityConfigurationQuery query = iModSCCredenciamento.engine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
        //        query.EntityTypeFilter.Add(EntityType.Badge);
        //        //query.Name = "a";
        //        //query.NameSearchMode = StringSearchMode.StartsWith;
        //        QueryCompletedEventArgs result = query.Query();

        //        if (result.Success)
        //        {
        //            foreach (DataRow dr in result.Data.Rows)
        //            {
        //                BadgeTemplate badge = iModSCCredenciamento.engine.GetEntity((Guid)dr[0]) as BadgeTemplate;
        //                _badges.Add(badge);
        //                //BadgePrinter instance = new BadgePrinter;
        //                //Guid credential = new Guid("");
        //                //instance.Print(credential, badge.Guid);
        //                //bool value;

        //                //value = instance.Print(credential, badge.Guid);

        //            }
        //        }
        //        else
        //        {

        //        }
        //        var credential = iModSCCredenciamento.engine.GetEntity(new Guid("d7a7061b-01ca-4d0f-8f3f-d93eb2ab9af7")) as Credential;
        //        var credRequest = new CredentialBadgeRequest();
        //        credRequest.BadgeTemplate = _badges[0].Guid;

        //        if (credential != null)
        //        {
        //            iModSCCredenciamento.engine.TransactionManager.CreateTransaction();
        //            CredencialManager credencialManager = new CredencialManager();
        //            //GetCredentialBuilder()
        //            credencialManager.Build();
        //            credencialManager.SetCredentialRequest(credRequest);

        //            ////Genetec.Sdk.Engine EntityManager = new;
        //            //iModSCCredenciamento.engine.
        //            //Genetec.Sdk.Workflows.EntityManager.IEntityManager GetCredentialBuilder;
        //            //Genetec.Sdk.Entities.Builders.ICredentialBuilder SetCardholder;
        //            //Genetec.Sdk.Entities.Builders.ICredentialBuilder SetCredentialRequest;
        //            //Genetec.Sdk.Entities.Builders.ICredentialBuilder SetFormat;
        //            //Genetec.Sdk.Credentials CardRequestCredentialFormat;



        //            //credRequest= iModSCCredenciamento.engine.
        //            //credential.RequestCredential(credRequest); // This sets the credential request to the credential 
        //            iModSCCredenciamento.engine.TransactionManager.CommitTransaction();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }


        //}
        #endregion

    }
}
