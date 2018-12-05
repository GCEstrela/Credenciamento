using iModSCCredenciamento.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using System.Windows.Forms;
using iModSCCredenciamento.Windows;
using System.Threading;
using System.Linq;
using System.Reflection;
using iModSCCredenciamento.Views;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;
using IMOD.Domain.Entities;
using IMOD.Infra.Repositorios;
using MessageBox = System.Windows.MessageBox;

//using IMOD.Application.Service;

//using IMOD.Application.Service;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradorViewModel : ViewModelBase
    {
        #region Inicializacao
        public ColaboradorViewModel()
        {


            //CarregaUI();
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
            //CarregaColecaoColaboradores();
        }
        private void CarregaUI()
        {
            CarregaColecaoColaboradores();
            CarregaColeçãoEstados();
            //CarregaColecaoTiposAtividades();
            //CarregaColecaoAreasAcessos();
            //CarregaColecaoLayoutsCrachas();
        }
        #endregion



        #region Variaveis Privadas

        private ObservableCollection<ClasseColaboradores.Colaborador> _Colaboradores;

        private ClasseColaboradores.Colaborador _ColaboradorSelecionado;

        private ClasseColaboradores.Colaborador _colaboradorTemp = new ClasseColaboradores.Colaborador();

        private List<ClasseColaboradores.Colaborador> _ColaboradoresTemp = new List<ClasseColaboradores.Colaborador>();

        private ObservableCollection<ClasseEstados.Estado> _Estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _municipios;

        PopupPesquisaColaborador popupPesquisaColaborador;

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;

        //private bool _EditandoUserControl;

        private readonly IColaboradorService _service = new ColaboradorService();

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseColaboradores.Colaborador> Colaboradores
        {
            get
            {
                return _Colaboradores;
            }

            set
            {
                if (_Colaboradores != value)
                {
                    _Colaboradores = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseColaboradores.Colaborador ColaboradorSelecionado
        {
            get
            {

                return this._ColaboradorSelecionado;
            }
            set
            {
                this._ColaboradorSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (ColaboradorSelecionado != null)
                {
                    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                    //string _imgstr = Conversores.IMGtoSTR(_img);
                    //ColaboradorSelecionado.Foto = _imgstr;
                    if (!_atualizandoFoto)
                    {
                        Thread CarregaFoto_thr = new Thread(() => CarregaFoto(ColaboradorSelecionado.ColaboradorID));
                        CarregaFoto_thr.Start();
                    }

                    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
                }

            }
        }

        public int EmpresaSelecionadaID
        {
            get
            {
                return this._EmpresaSelecionadaID;
            }
            set
            {
                this._EmpresaSelecionadaID = value;
                base.OnPropertyChanged();
                if (EmpresaSelecionadaID != null)
                {
                    // OnEmpresaSelecionada();
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

        //public bool EditandoUserControl
        //{
        //    get
        //    {
        //        return _EditandoUserControl;
        //    }
        //    set
        //    {
        //        SetProperty(ref _EditandoUserControl, value);
        //    }
        //}


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
                return _municipios;
            }

            set
            {
                if (_municipios != value)
                {
                    _municipios = value;
                    OnPropertyChanged();

                }
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
        public void OnAtualizaCommand(object colaboradorID)
        {
            //EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            //Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(empresaID)));
            //CarregaColecaoSeguros_thr.Start();
            //CarregaColecaoSeguros(Convert.ToInt32(empresaID));
        }
        public void OnBuscarArquivoCommand()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
                string _sql;
                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoPDF.InitialDirectory = "c:\\\\";
                _arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();
                //if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                //{
                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                //_seguroTemp.NomeArquivo = _nomecompletodoarquivo;
                //_seguroTemp.Arquivo = _arquivoSTR;
                //InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

                //AtualizaListaAnexos(_resp);

                //}
            }
            catch (Exception ex)
            {

            }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
                Global.CpfEdicao = _colaboradorTemp.CPF;
                _selectedIndexTemp = SelectedIndex;
                HabilitaEdicao = true;
            }
            catch (Exception)
            {
            }
        }

        public void OnCancelarEdicaoCommand()
        {
            try
            {
                Global.CpfEdicao = "";
                Colaboradores[_selectedIndexTemp] = _colaboradorTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task OnSalvarEdicaoCommandAsync()
        {
            try
            {
                Global.CpfEdicao = "";
                _PopupSalvando = new PopupMensagem("Aguarde, salvando ...");

                Thread PopupSalvando_thr = new Thread(() => AbrePopupSalvando());

                PopupSalvando_thr.Start();

                await Task.Run(() => SalvarEdicao());

                _PopupSalvando.Close();

                _PopupSalvando = null;

            }
            catch (Exception ex)
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.Close();
                }

            }
        }

        public async Task OnSalvarAdicaoCommandAsync()
        {

            try
            {
                Global.CpfEdicao = "";
                _PopupSalvando = new PopupMensagem("Aguarde, salvando ...");

                Thread PopupSalvando_thr = new Thread(() => AbrePopupSalvando());

                PopupSalvando_thr.Start();

                await Task.Run(() => SalvarAdicao());

                _PopupSalvando.Close();

                _PopupSalvando = null;

            }
            catch (Exception ex)
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.Close();
                }

            }
        }

        public void OnSalvarEdicaoCommand2()
        {
            //var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
            //_colaboradorService.Alterar(colab);
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Colaboradores)
                {
                    _ColaboradoresTemp.Add(x);
                }
                Global.CpfEdicao = "000.000.000-00";
                _selectedIndexTemp = SelectedIndex;
                Colaboradores.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _colaboradorTemp = new ClasseColaboradores.Colaborador();
                ////////////////////////////////////////////////////////
                _colaboradorTemp.ColaboradorID = EmpresaSelecionadaID;  //OBS
                ////////////////////////////////////////////////////////
                Colaboradores.Add(_colaboradorTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {
            }

        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Global.CpfEdicao = "";
                Colaboradores = null;
                Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>(_ColaboradoresTemp);
                SelectedIndex = _selectedIndexTemp;
                _ColaboradoresTemp.Clear();
                HabilitaEdicao = false;
                _atualizandoFoto = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand2()
        {
            try
            {
                //var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                //_colaboradorService.Remover(colab);

            }
            catch (Exception ex)
            {
            }

        }
        public void OnExcluirCommand()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este colaborador?", "Excluir Colaborador", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste colaborador, inclusive histórico. Confirma exclusão?", "Excluir Colaborador", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        ExcluiColaboradorBD(ColaboradorSelecionado.ColaboradorID);
                //        Colaboradores.Remove(ColaboradorSelecionado);

                //    }
                //}

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiColaboradorBD(ColaboradorSelecionado.ColaboradorID);
                        Colaboradores.Remove(ColaboradorSelecionado);
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

                popupPesquisaColaborador = new PopupPesquisaColaborador();
                popupPesquisaColaborador.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaColaborador.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaColaborador.Criterio.Split((char)(20));
            int _codigo;
            if ((((string[])vetor)[0] == null) || (((string[])vetor)[0] == ""))
            {
                _codigo = 0;
            }
            else
            {
                _codigo = Convert.ToInt32(((string[])vetor)[0]);
            }
            string _nome = ((string[])vetor)[1];
            string _apelido = ((string[])vetor)[2];
            string _cpf = ((string[])vetor)[3];
            CarregaColecaoColaboradores(_codigo, _nome, _apelido, _cpf);
            SelectedIndex = 0;
        }

        public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupPendencias popupPendencias = new PopupPendencias(2, ((System.Windows.FrameworkElement)e.OriginalSource).Tag, ColaboradorSelecionado.ColaboradorID, ColaboradorSelecionado.Nome);
                popupPendencias.ShowDialog();
                popupPendencias = null;
                CarregaColecaoColaboradores(ColaboradorSelecionado.ColaboradorID);


            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Carregamento das Colecoes

        private void CarregaColecaoColaboradores(int? _ColaboradorID = 0, string nome = "", string apelido = "", string cpf = "", string _quantidaderegistro = "500")
        {
            try
            {
                //string _xml = RequisitaColaboradores(_ColaboradorID, _nome, _apelido, _cpf);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradores));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseColaboradores classeColaboradores = new ClasseColaboradores();
                //classeColaboradores = (ClasseColaboradores)deserializer.Deserialize(reader);
                //Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>();
                //Colaboradores = classeColaboradores.Colaboradores;
                //SelectedIndex = 0;
                //////////////////////////////////////////////////////////////////


                if (!string.IsNullOrWhiteSpace(nome)) nome = $"%{nome}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{apelido}%";
                if (!string.IsNullOrWhiteSpace(apelido)) apelido = $"%{cpf}%";
                var list1 = _service.Listar(_ColaboradorID, nome, apelido, cpf);

                var list2 = Mapper.Map<List<ClasseColaboradores.Colaborador>>(list1.OrderByDescending(a => a.ColaboradorId));

                var observer = new ObservableCollection<ClasseColaboradores.Colaborador>();
                list2.ForEach(n =>
                {
                    observer.Add(n);
                });

                this.Colaboradores = observer;
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

        private void CarregaFoto(int _ColaboradorID)
        {
            try
            {
                _atualizandoFoto = true; //para que o evento de ColaboradorSelecionado não entre em looping
                ///
                //                BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                //                string _imgstr = Conversores.IMGtoSTR(_img);
                //                ColaboradorSelecionado.Foto = _imgstr;

                //                System.Windows.Application.Current.Dispatcher.Invoke(
                //(Action)(() => {
                //_colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);
                //_selectedIndexTemp = SelectedIndex;

                //_colaboradorTemp.Foto = _imgstr;
                //Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

                //SelectedIndex = _selectedIndexTemp;



                //}));

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

                    Waiting.Freeze();
                }));

                string _xmlstring = BuscaFoto(_ColaboradorID);

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { Waiting = null; }));

                XmlDocument xmldocument = new XmlDocument();

                xmldocument.LoadXml(_xmlstring);

                XmlNode node = (XmlNode)xmldocument.DocumentElement;

                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                if (arquivoNode.HasChildNodes)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        _colaboradorTemp = ColaboradorSelecionado.CriaCopia(ColaboradorSelecionado);

                        _selectedIndexTemp = SelectedIndex;

                        _colaboradorTemp.Foto = arquivoNode.FirstChild.Value;

                        Colaboradores[_selectedIndexTemp] = _colaboradorTemp;

                        SelectedIndex = _selectedIndexTemp;

                    }));
                }
                _atualizandoFoto = false;

            }
            catch (Exception ex)
            {
                _atualizandoFoto = false;
            }
        }
        #endregion

        #region Data Access

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

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
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


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
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

        private void ExcluiColaboradorBD(int _ColaboradorID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Colaboradores where ColaboradorID=" + _ColaboradorID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);


            }
        }

        private string BuscaFoto(int colaboradorID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Colaboradores Where ColaboradorID = " + colaboradorID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Foto"].ToString())));
                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CriaXmlImagem ex: " + ex);
                return null;
            }
        }

        #endregion

        #region Metodos privados
        internal void SalvarAdicao2()
        {
            try
            {

                HabilitaEdicao = false;


                //var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                //_colaboradorService.Criar(colab);

                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradores));

                //ObservableCollection<ClasseColaboradores.Colaborador> _ColaboradoresPro = new ObservableCollection<ClasseColaboradores.Colaborador>();
                //ClasseColaboradores _ClasseColaboradoresTemp = new ClasseColaboradores();
                //ColaboradorSelecionado.Pendente = true;
                //ColaboradorSelecionado.Pendente21 = true;
                //ColaboradorSelecionado.Pendente22 = true;
                //ColaboradorSelecionado.Pendente23 = true;
                //ColaboradorSelecionado.Pendente24 = true;
                //ColaboradorSelecionado.Pendente25 = true;


                //_ColaboradoresPro.Add(ColaboradorSelecionado);
                //_ClasseColaboradoresTemp.Colaboradores = _ColaboradoresPro;

                //string xmlString;

                //using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                //{

                //    using (XmlTextWriter xw = new XmlTextWriter(sw))
                //    {
                //        xw.Formatting = Formatting.Indented;
                //        serializer.Serialize(xw, _ClasseColaboradoresTemp);
                //        xmlString = sw.ToString();
                //    }

                //}

                //int _novoColaboradorID = InsereColaboradoresBD(xmlString);

                //AtualizaPendencias(_novoColaboradorID);

                //ColaboradorSelecionado.ColaboradorID = _novoColaboradorID;

                //_ColaboradoresTemp.Clear();

                //_ColaboradoresTemp.Add(ColaboradorSelecionado);
                //Colaboradores = null;
                //Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>(_ColaboradoresTemp);
                //SelectedIndex = 0;
                //_ColaboradoresTemp.Clear();
                //_ColaboradoresPro.Clear();

                //_ClasseColaboradoresTemp = null;

                ////this._ColaboradoresTemp.Clear();
                ////_colaboradorTemp = null;


            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Alteração iniciada por Mihai & Máximo (28/11/2018)
        /// TranportarDados
        /// </summary>
        internal void SalvarAdicao()
        {
            try
            {
                HabilitaEdicao = false;

                ColaboradorSelecionado.Pendente = true;
                ColaboradorSelecionado.Pendente21 = true;
                ColaboradorSelecionado.Pendente22 = true;
                ColaboradorSelecionado.Pendente23 = true;
                ColaboradorSelecionado.Pendente24 = true;
                ColaboradorSelecionado.Pendente25 = true;


                var entity = ColaboradorSelecionado;
                var entityConv = Mapper.Map<IMOD.Domain.Entities.Colaborador>(entity);

                _service.Criar(entityConv);

                var id = ColaboradorSelecionado.ColaboradorID;

                _colaboradorTemp = null;

                Thread CarregaColecaoColaboradores_thr = new Thread(() => CarregaColecaoColaboradores(id, null, null, null, null));
                CarregaColecaoColaboradores_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void SalvarAdicao ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        /// <summary>
        /// Alteração iniciada por Mihai & Máximo (28/11/2018)
        /// TranportarDados
        /// </summary>
        internal void SalvarEdicao()
        {
            try
            {
                HabilitaEdicao = false;

                var entity = ColaboradorSelecionado;
                var entityConv = Mapper.Map<IMOD.Domain.Entities.Colaborador>(entity);

                _service.Alterar(entityConv);

                var id = ColaboradorSelecionado.ColaboradorID;

                _colaboradorTemp = null;

                Thread CarregaColecaoColaboradores_thr = new Thread(() => CarregaColecaoColaboradores(id, null, null, null, null));
                CarregaColecaoColaboradores_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void SalvarEdicao ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        internal void AbrePopupSalvando()
        {

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (_PopupSalvando != null)
                {
                    _PopupSalvando.ShowDialog();
                }


            }));

        }

        public bool ConsultarCpf(string doc)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(doc)) throw new ArgumentNullException("Informe um CPF para pesquisar");
                doc = doc.RetirarCaracteresEspeciais().Replace(" ", "");


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand cmd = new SqlCommand("Select * From Colaboradores Where cpf = '" + doc + "'", _Con);
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

        public void ValidarAdicao(ClasseColaboradores.Colaborador entity)
        {
            //if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            //if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            ValidarEdicao(entity);
            if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");


        }

        public void ValidarEdicao(ClasseColaboradores.Colaborador entity)
        {
            if (string.IsNullOrWhiteSpace(entity.CPF)) throw new InvalidOperationException("Informe CPF");
            if (!entity.CPF.IsValidCpf()) throw new InvalidOperationException("CPF inválido");
            //if (ConsultarCpf(entity.CPF)) throw new InvalidOperationException("CPF já cadastrado");


        }

        #endregion

        #region Testes

        #endregion

    }

}
