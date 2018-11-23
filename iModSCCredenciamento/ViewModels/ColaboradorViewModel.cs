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
using iModSCCredenciamento.Views;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using IMOD.Application.Interfaces;
using IMOD.Application.Service;

//using IMOD.Application.Service;

namespace iModSCCredenciamento.ViewModels
{
    public class ColaboradorViewModel : ViewModelBase
    {
        private IColaboradorService _colaboradorService = new ColaboradorService();
        public IMOD.Domain.Entities.Colaborador Colaborador { get; set; }







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
             var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
            _colaboradorService.Alterar(colab);
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

                var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                _colaboradorService.Remover(colab);

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

        private void CarregaColecaoColaboradores(int _ColaboradorID = 0, string _nome = "", string _apelido = "", string _cpf = "", string _quantidaderegistro = "500")
        {
            try
            {
                string _xml = RequisitaColaboradores(_ColaboradorID, _nome, _apelido, _cpf);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradores));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseColaboradores classeColaboradores = new ClasseColaboradores();
                classeColaboradores = (ClasseColaboradores)deserializer.Deserialize(reader);
                Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>();
                Colaboradores = classeColaboradores.Colaboradores;
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
        private string RequisitaColaboradores(int _colaboradorID = 0, string _nome = "", string _apelido = "", string _cpf = "", int _excluida = 0, string _quantidaderegistro = "500")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradores = _xmlDocument.CreateElement("ClasseColaboradores");
                _xmlDocument.AppendChild(_ClasseColaboradores);

                XmlNode _Colaboradores = _xmlDocument.CreateElement("Colaboradores");
                _ClasseColaboradores.AppendChild(_Colaboradores);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _colaboradorIDSTR = "";

                _colaboradorIDSTR = _colaboradorID == 0 ? "" : " AND ColaboradorID = " + _colaboradorID;
                _nome = _nome == "" ? "" : " AND Nome like '%" + _nome + "%' ";
                _apelido = _apelido == "" ? "" : "AND Apelido like '%" + _apelido + "%' ";
                _cpf = _cpf == "" ? "" : " AND CPF like '%" + _cpf.RetirarCaracteresEspeciais() + "%'";

                _strSql = "[ColaboradorID]" +
                          ",[Nome]" +
                          ",[Apelido]" +
                          ",[DataNascimento]" +
                          ",[NomePai]" +
                          ",[NomeMae]" +
                          ",[Nacionalidade]" +
                          ",[EstadoCivil]" +
                          ",[CPF]" +
                          ",[RG]" +
                          ",[RGEmissao]" +
                          ",[RGOrgLocal]" +
                          ",[RGOrgUF]" +
                          ",[Passaporte]" +
                          ",[PassaporteValidade]" +
                          ",[RNE]" +
                          ",[TelefoneFixo]" +
                          ",[TelefoneCelular]" +
                          ",[Email]" +
                          ",[ContatoEmergencia]" +
                          ",[TelefoneEmergencia]" +
                          ",[Cep]" +
                          ",[Endereco]" +
                          ",[Numero]" +
                          ",[Complemento]" +
                          ",[Bairro]" +
                          ",[MunicipioID]" +
                          ",[EstadoID]" +
                          ",[Motorista]" +
                          ",[CNHCategoria]" +
                          ",[CNH]" +
                          ",[CNHValidade]" +
                          ",[CNHEmissor]" +
                          ",[CNHUF]" +
                          ",[Bagagem]" +
                          ",[DataEmissao]" +
                          ",[DataValidade]" +
                          ",[Excluida]" +
                          ",[StatusID]" +
                          ",[TipoAcessoID]" +
                          ",[Pendente21],[Pendente22],[Pendente23],[Pendente24]" +
                          ",[Pendente25] " +
                             "from Colaboradores where Excluida  = " + _excluida + _colaboradorIDSTR + _nome + _apelido + _cpf + " order by ColaboradorID desc";

                if (_quantidaderegistro == "0")
                {
                    _strSql = "Select " + _strSql;
                }
                else
                {
                    _strSql = "select Top " + _quantidaderegistro + " " + _strSql;
                }

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);

                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);

                string dt1;
                DateTime dt2;

                while (_sqlreader.Read())
                {

                    XmlNode _Colaborador = _xmlDocument.CreateElement("Colaborador");
                    _Colaboradores.AppendChild(_Colaborador);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _Colaborador.AppendChild(_ColaboradorID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Colaborador.AppendChild(_Nome);

                    XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
                    _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
                    _Colaborador.AppendChild(_Apelido);

                    //XmlNode _DataNascimento = _xmlDocument.CreateElement("DataNascimento");
                    //_DataNascimento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataNascimento"].ToString())));
                    //_Colaborador.AppendChild(_DataNascimento);


                    dt1 = (_sqlreader["DataNascimento"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        dt2 = Convert.ToDateTime(dt1);
                        XmlNode _DataNascimento = _xmlDocument.CreateElement("DataNascimento");
                        _DataNascimento.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Colaborador.AppendChild(_DataNascimento);
                    }

                    XmlNode _NomePai = _xmlDocument.CreateElement("NomePai");
                    _NomePai.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomePai"].ToString())));
                    _Colaborador.AppendChild(_NomePai);

                    XmlNode _NomeMae = _xmlDocument.CreateElement("NomeMae");
                    _NomeMae.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeMae"].ToString())));
                    _Colaborador.AppendChild(_NomeMae);

                    XmlNode _Nacionalidade = _xmlDocument.CreateElement("Nacionalidade");
                    _Nacionalidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nacionalidade"].ToString())));
                    _Colaborador.AppendChild(_Nacionalidade);

                    XmlNode _Foto = _xmlDocument.CreateElement("Foto");
                    //_Foto.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString())));
                    _Colaborador.AppendChild(_Foto);

                    XmlNode _EstadoCivil = _xmlDocument.CreateElement("EstadoCivil");
                    _EstadoCivil.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoCivil"].ToString())));
                    _Colaborador.AppendChild(_EstadoCivil);

                    XmlNode _CPF = _xmlDocument.CreateElement("CPF");
                    _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().FormatarCpf())));
                    _Colaborador.AppendChild(_CPF);

                    XmlNode _RG = _xmlDocument.CreateElement("RG");
                    _RG.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RG"].ToString())));
                    _Colaborador.AppendChild(_RG);

                    dt1 = (_sqlreader["RGEmissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        dt2 = Convert.ToDateTime(dt1);
                        XmlNode _RGEmissao = _xmlDocument.CreateElement("RGEmissao");
                        _RGEmissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Colaborador.AppendChild(_RGEmissao);
                    }

                    XmlNode _RGOrgLocal = _xmlDocument.CreateElement("RGOrgLocal");
                    _RGOrgLocal.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgLocal"].ToString())));
                    _Colaborador.AppendChild(_RGOrgLocal);

                    XmlNode _RGOrgUF = _xmlDocument.CreateElement("RGOrgUF");
                    _RGOrgUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgUF"].ToString())));
                    _Colaborador.AppendChild(_RGOrgUF);

                    XmlNode _Passaporte = _xmlDocument.CreateElement("Passaporte");
                    _Passaporte.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Passaporte"].ToString())));
                    _Colaborador.AppendChild(_Passaporte);

                    //XmlNode _PassaporteValidade = _xmlDocument.CreateElement("PassaporteValidade");
                    //_PassaporteValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["PassaporteValidade"].ToString())));
                    //_Colaborador.AppendChild(_PassaporteValidade);

                    dt1 = (_sqlreader["PassaporteValidade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        dt2 = Convert.ToDateTime(dt1);
                        XmlNode _PassaporteValidade = _xmlDocument.CreateElement("PassaporteValidade");
                        _PassaporteValidade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Colaborador.AppendChild(_PassaporteValidade);
                    }

                    XmlNode _RNE = _xmlDocument.CreateElement("RNE");
                    _RNE.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RNE"].ToString())));
                    _Colaborador.AppendChild(_RNE);

                    XmlNode _TelefoneFixo = _xmlDocument.CreateElement("TelefoneFixo");
                    _TelefoneFixo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneFixo"].ToString())));
                    _Colaborador.AppendChild(_TelefoneFixo);

                    XmlNode _TelefoneCelular = _xmlDocument.CreateElement("TelefoneCelular");
                    _TelefoneCelular.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneCelular"].ToString())));
                    _Colaborador.AppendChild(_TelefoneCelular);

                    XmlNode _Email = _xmlDocument.CreateElement("Email");
                    _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
                    _Colaborador.AppendChild(_Email);

                    XmlNode _ContatoEmergencia = _xmlDocument.CreateElement("ContatoEmergencia");
                    _ContatoEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContatoEmergencia"].ToString())));
                    _Colaborador.AppendChild(_ContatoEmergencia);

                    XmlNode _TelefoneEmergencia = _xmlDocument.CreateElement("TelefoneEmergencia");
                    _TelefoneEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneEmergencia"].ToString())));
                    _Colaborador.AppendChild(_TelefoneEmergencia);

                    XmlNode _Cep = _xmlDocument.CreateElement("Cep");
                    _Cep.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cep"].ToString())));
                    _Colaborador.AppendChild(_Cep);

                    XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    _Colaborador.AppendChild(_Endereco);

                    XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    _Colaborador.AppendChild(_Numero);

                    XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    _Colaborador.AppendChild(_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    _Colaborador.AppendChild(_Bairro);

                    XmlNode _MunicipioId = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioId.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _Colaborador.AppendChild(_MunicipioId);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _Colaborador.AppendChild(_EstadoID);

                    XmlNode _Motorista = _xmlDocument.CreateElement("Motorista");
                    _Motorista.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString()));
                    _Colaborador.AppendChild(_Motorista);

                    XmlNode _CNHCategoria = _xmlDocument.CreateElement("CNHCategoria");
                    _CNHCategoria.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHCategoria"].ToString())));
                    _Colaborador.AppendChild(_CNHCategoria);

                    XmlNode _CNH = _xmlDocument.CreateElement("CNH");
                    _CNH.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNH"].ToString())));
                    _Colaborador.AppendChild(_CNH);

                    //XmlNode _CNHValidade = _xmlDocument.CreateElement("CNHValidade");
                    //_CNHValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHValidade"].ToString())));
                    //_Colaborador.AppendChild(_CNHValidade);

                    dt1 = (_sqlreader["CNHValidade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        dt2 = Convert.ToDateTime(dt1);
                        XmlNode _CNHValidade = _xmlDocument.CreateElement("CNHValidade");
                        _CNHValidade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Colaborador.AppendChild(_CNHValidade);
                    }

                    XmlNode _CNHEmissor = _xmlDocument.CreateElement("CNHEmissor");
                    _CNHEmissor.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHEmissor"].ToString())));
                    _Colaborador.AppendChild(_CNHEmissor);

                    XmlNode _CNHUF = _xmlDocument.CreateElement("CNHUF");
                    _CNHUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHUF"].ToString())));
                    _Colaborador.AppendChild(_CNHUF);

                    XmlNode _Bagagem = _xmlDocument.CreateElement("Bagagem");
                    _Bagagem.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bagagem"].ToString())));
                    _Colaborador.AppendChild(_Bagagem);

                    //XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
                    //_DataEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataEmissao"].ToString())));
                    //_Colaborador.AppendChild(_DataEmissao);

                    dt1 = (_sqlreader["DataEmissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        dt2 = Convert.ToDateTime(dt1);
                        XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
                        _DataEmissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Colaborador.AppendChild(_DataEmissao);
                    }


                    //XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
                    //_DataValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataValidade"].ToString())));
                    //_Colaborador.AppendChild(_DataValidade);

                    dt1 = (_sqlreader["DataValidade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dt1))
                    {
                        dt2 = Convert.ToDateTime(dt1);
                        XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
                        _DataValidade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Colaborador.AppendChild(_DataValidade);
                    }


                    XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
                    _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    _Colaborador.AppendChild(_Excluida);

                    XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
                    _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
                    _Colaborador.AppendChild(_StatusID);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
                    _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
                    _Colaborador.AppendChild(_TipoAcessoID);

                    XmlNode _Pendente1 = _xmlDocument.CreateElement("Pendente21");
                    _Pendente1.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente21"])).ToString()));
                    _Colaborador.AppendChild(_Pendente1);

                    XmlNode _Pendente2 = _xmlDocument.CreateElement("Pendente22");
                    _Pendente2.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente22"])).ToString()));
                    _Colaborador.AppendChild(_Pendente2);

                    XmlNode _Pendente3 = _xmlDocument.CreateElement("Pendente23");
                    _Pendente3.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente23"])).ToString()));
                    _Colaborador.AppendChild(_Pendente3);

                    XmlNode _Pendente4 = _xmlDocument.CreateElement("Pendente24");
                    _Pendente4.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente24"])).ToString()));
                    _Colaborador.AppendChild(_Pendente4);

                    XmlNode _Pendente5 = _xmlDocument.CreateElement("Pendente25");
                    _Pendente5.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente25"])).ToString()));
                    _Colaborador.AppendChild(_Pendente5);


                    bool _pend = false;
                    _pend = (bool)_sqlreader["Pendente21"] ||
                        (bool)_sqlreader["Pendente22"] ||
                        (bool)_sqlreader["Pendente23"] ||
                        (bool)_sqlreader["Pendente24"] ||
                        (bool)_sqlreader["Pendente25"];

                    XmlNode _Pendente = _xmlDocument.CreateElement("Pendente");
                    _Pendente.AppendChild(_xmlDocument.CreateTextNode(Convert.ToInt32(_pend).ToString()));
                    _Colaborador.AppendChild(_Pendente);


                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                //InsereColaboradoresBD("");

                return _xml;
            }
            catch (Exception ex)
            {

                return null;
            }
            return null;
        }
        //private string RequisitaColaboradores(string _colaboradorID = "", string _nome = "", string _apelido = "", string _cpf = "", int _excluida = 0, string _quantidaderegistro = "500")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseColaboradores = _xmlDocument.CreateElement("ClasseColaboradores");
        //        _xmlDocument.AppendChild(_ClasseColaboradores);

        //        XmlNode _Colaboradores = _xmlDocument.CreateElement("Colaboradores");
        //        _ClasseColaboradores.AppendChild(_Colaboradores);

        //        string _strSql;


        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

        //        _colaboradorID = _colaboradorID == "" ? "" : " AND ColaboradorID = " + _colaboradorID;
        //        _nome = _nome == "" ? "" : " AND Nome like '%" + _nome + "%' ";
        //        _apelido = _apelido == "" ? "" : "AND Apelido like '%" + _apelido + "%' ";
        //        _cpf = _cpf == "" ? "" : " AND CPF like '%" + _cpf + "%'";

        //        _strSql = "[ColaboradorID]" +
        //                  ",[Nome]" +
        //                  ",[Apelido]" +
        //                  ",[DataNascimento]" +
        //                  ",[NomePai]" +
        //                  ",[NomeMae]" +
        //                  ",[Nacionalidade]" +
        //                  ",[EstadoCivil]" +
        //                  ",[CPF]" +
        //                  ",[RG]" +
        //                  ",[RGEmissao]" +
        //                  ",[RGOrgLocal]" +
        //                  ",[RGOrgUF]" +
        //                  ",[Passaporte]" +
        //                  ",[PassaporteValidade]" +
        //                  ",[TelefoneFixo]" +
        //                  ",[TelefoneCelular]" +
        //                  ",[Email]" +
        //                  ",[ContatoEmergencia]" +
        //                  ",[TelefoneEmergencia]" +
        //                  ",[Cep]" +
        //                  ",[Endereco]" +
        //                  ",[Numero]" +
        //                  ",[Complemento]" +
        //                  ",[Bairro]" +
        //                  ",[MunicipioID]" +
        //                  ",[EstadoID]" +
        //                  ",[Motorista]" +
        //                  ",[CNH]" +
        //                  ",[CNHValidade]" +
        //                  ",[CNHEmissor]" +
        //                  ",[CNHUF]" +
        //                  ",[Bagagem]" +
        //                  ",[DataEmissao]" +
        //                  ",[DataValidade]" +
        //                  ",[Excluida]" +
        //                  ",[StatusID]" +
        //                  ",[TipoAcessoID]" +
        //                     "from Colaboradores where Excluida  = " + _excluida + _colaboradorID + _nome + _apelido + _cpf + " order by ColaboradorID desc";

        //        if (_quantidaderegistro == "0")
        //        {
        //            _strSql = "Select " + _strSql;
        //        }
        //        else
        //        {
        //            _strSql = "select Top " + _quantidaderegistro + " " + _strSql;
        //        }

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);

        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);


        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _Colaborador = _xmlDocument.CreateElement("Colaborador");
        //            _Colaboradores.AppendChild(_Colaborador);

        //            XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
        //            _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
        //            _Colaborador.AppendChild(_ColaboradorID);

        //            XmlNode _Nome = _xmlDocument.CreateElement("Nome");
        //            _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
        //            _Colaborador.AppendChild(_Nome);

        //            XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
        //            _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
        //            _Colaborador.AppendChild(_Apelido);

        //            XmlNode _DataNascimento = _xmlDocument.CreateElement("DataNascimento");
        //            _DataNascimento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataNascimento"].ToString())));
        //            _Colaborador.AppendChild(_DataNascimento);

        //            XmlNode _NomePai = _xmlDocument.CreateElement("NomePai");
        //            _NomePai.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomePai"].ToString())));
        //            _Colaborador.AppendChild(_NomePai);

        //            XmlNode _NomeMae = _xmlDocument.CreateElement("NomeMae");
        //            _NomeMae.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeMae"].ToString())));
        //            _Colaborador.AppendChild(_NomeMae);

        //            XmlNode _Nacionalidade = _xmlDocument.CreateElement("Nacionalidade");
        //            _Nacionalidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nacionalidade"].ToString())));
        //            _Colaborador.AppendChild(_Nacionalidade);

        //            XmlNode _Foto = _xmlDocument.CreateElement("Foto");
        //            //_Foto.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString())));
        //            _Colaborador.AppendChild(_Foto);

        //            XmlNode _EstadoCivil = _xmlDocument.CreateElement("EstadoCivil");
        //            _EstadoCivil.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoCivil"].ToString())));
        //            _Colaborador.AppendChild(_EstadoCivil);

        //            XmlNode _CPF = _xmlDocument.CreateElement("CPF");
        //            _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString())));
        //            _Colaborador.AppendChild(_CPF);

        //            XmlNode _RG = _xmlDocument.CreateElement("RG");
        //            _RG.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RG"].ToString())));
        //            _Colaborador.AppendChild(_RG);

        //            XmlNode _RGEmissao = _xmlDocument.CreateElement("RGEmissao");
        //            _RGEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGEmissao"].ToString())));
        //            _Colaborador.AppendChild(_RGEmissao);

        //            XmlNode _RGOrgLocal = _xmlDocument.CreateElement("RGOrgLocal");
        //            _RGOrgLocal.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgLocal"].ToString())));
        //            _Colaborador.AppendChild(_RGOrgLocal);

        //            XmlNode _RGOrgUF = _xmlDocument.CreateElement("RGOrgUF");
        //            _RGOrgUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgUF"].ToString())));
        //            _Colaborador.AppendChild(_RGOrgUF);

        //            XmlNode _Passaporte = _xmlDocument.CreateElement("Passaporte");
        //            _Passaporte.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Passaporte"].ToString())));
        //            _Colaborador.AppendChild(_Passaporte);

        //            XmlNode _PassaporteValidade = _xmlDocument.CreateElement("PassaporteValidade");
        //            _PassaporteValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["PassaporteValidade"].ToString())));
        //            _Colaborador.AppendChild(_PassaporteValidade);

        //            XmlNode _TelefoneFixo = _xmlDocument.CreateElement("TelefoneFixo");
        //            _TelefoneFixo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneFixo"].ToString())));
        //            _Colaborador.AppendChild(_TelefoneFixo);

        //            XmlNode _TelefoneCelular = _xmlDocument.CreateElement("TelefoneCelular");
        //            _TelefoneCelular.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneCelular"].ToString())));
        //            _Colaborador.AppendChild(_TelefoneCelular);

        //            XmlNode _Email = _xmlDocument.CreateElement("Email");
        //            _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
        //            _Colaborador.AppendChild(_Email);

        //            XmlNode _ContatoEmergencia = _xmlDocument.CreateElement("ContatoEmergencia");
        //            _ContatoEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContatoEmergencia"].ToString())));
        //            _Colaborador.AppendChild(_ContatoEmergencia);

        //            XmlNode _TelefoneEmergencia = _xmlDocument.CreateElement("TelefoneEmergencia");
        //            _TelefoneEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneEmergencia"].ToString())));
        //            _Colaborador.AppendChild(_TelefoneEmergencia);

        //            XmlNode _Cep = _xmlDocument.CreateElement("Cep");
        //            _Cep.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cep"].ToString())));
        //            _Colaborador.AppendChild(_Cep);

        //            XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
        //            _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
        //            _Colaborador.AppendChild(_Endereco);

        //            XmlNode _Numero = _xmlDocument.CreateElement("Numero");
        //            _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
        //            _Colaborador.AppendChild(_Numero);

        //            XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
        //            _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
        //            _Colaborador.AppendChild(_Complemento);

        //            XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
        //            _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
        //            _Colaborador.AppendChild(_Bairro);

        //            XmlNode _MunicipioId = _xmlDocument.CreateElement("MunicipioID");
        //            _MunicipioId.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
        //            _Colaborador.AppendChild(_MunicipioId);

        //            XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
        //            _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
        //            _Colaborador.AppendChild(_EstadoID);

        //            XmlNode _Motorista = _xmlDocument.CreateElement("Motorista");
        //            _Motorista.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Motorista"])).ToString()));
        //            _Colaborador.AppendChild(_Motorista);

        //            XmlNode _CNH = _xmlDocument.CreateElement("CNH");
        //            _CNH.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNH"].ToString())));
        //            _Colaborador.AppendChild(_CNH);

        //            XmlNode _CNHValidade = _xmlDocument.CreateElement("CNHValidade");
        //            _CNHValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHValidade"].ToString())));
        //            _Colaborador.AppendChild(_CNHValidade);

        //            XmlNode _CNHEmissor = _xmlDocument.CreateElement("CNHEmissor");
        //            _CNHEmissor.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHEmissor"].ToString())));
        //            _Colaborador.AppendChild(_CNHEmissor);

        //            XmlNode _CNHUF = _xmlDocument.CreateElement("CNHUF");
        //            _CNHUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHUF"].ToString())));
        //            _Colaborador.AppendChild(_CNHUF);

        //            XmlNode _Bagagem = _xmlDocument.CreateElement("Bagagem");
        //            _Bagagem.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bagagem"].ToString())));
        //            _Colaborador.AppendChild(_Bagagem);

        //            XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
        //            _DataEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataEmissao"].ToString())));
        //            _Colaborador.AppendChild(_DataEmissao);

        //            XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
        //            _DataValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataValidade"].ToString())));
        //            _Colaborador.AppendChild(_DataValidade);

        //            XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
        //            _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
        //            _Colaborador.AppendChild(_Excluida);

        //            XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
        //            _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
        //            _Colaborador.AppendChild(_StatusID);

        //            XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
        //            _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
        //            _Colaborador.AppendChild(_TipoAcessoID);

        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        //InsereColaboradoresBD("");

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

        private int InsereColaboradoresBD(string xmlString)
        {
            try
            {
                int _novID = 0;

                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseColaboradores.Colaborador _Colaborador = new ClasseColaboradores.Colaborador();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _Colaborador.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
                _Colaborador.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Nome")[i].InnerText;
                _Colaborador.Apelido = _xmlDoc.GetElementsByTagName("Apelido")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Apelido")[i].InnerText;
                _Colaborador.DataNascimento = _xmlDoc.GetElementsByTagName("DataNascimento")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataNascimento")[i].InnerText);
                _Colaborador.NomePai = _xmlDoc.GetElementsByTagName("NomePai")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomePai")[i].InnerText;
                _Colaborador.NomeMae = _xmlDoc.GetElementsByTagName("NomeMae")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeMae")[i].InnerText;
                _Colaborador.Nacionalidade = _xmlDoc.GetElementsByTagName("Nacionalidade")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Nacionalidade")[i].InnerText;
                _Colaborador.Foto = _xmlDoc.GetElementsByTagName("Foto")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Foto")[i].InnerText;
                _Colaborador.EstadoCivil = _xmlDoc.GetElementsByTagName("EstadoCivil")[i] == null ? "" : _xmlDoc.GetElementsByTagName("EstadoCivil")[i].InnerText;
                _Colaborador.CPF = _xmlDoc.GetElementsByTagName("CPF")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CPF")[i].InnerText;
                _Colaborador.RG = _xmlDoc.GetElementsByTagName("RG")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RG")[i].InnerText;
                _Colaborador.RGEmissao = _xmlDoc.GetElementsByTagName("RGEmissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("RGEmissao")[i].InnerText);
                _Colaborador.RGOrgLocal = _xmlDoc.GetElementsByTagName("RGOrgLocal")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RGOrgLocal")[i].InnerText;
                _Colaborador.RGOrgUF = _xmlDoc.GetElementsByTagName("RGOrgUF")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RGOrgUF")[i].InnerText;
                _Colaborador.Passaporte = _xmlDoc.GetElementsByTagName("Passaporte")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Passaporte")[i].InnerText;
                _Colaborador.PassaporteValidade = _xmlDoc.GetElementsByTagName("PassaporteValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("PassaporteValidade")[i].InnerText);
                _Colaborador.RNE = _xmlDoc.GetElementsByTagName("RNE")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RNE")[i].InnerText;
                _Colaborador.TelefoneFixo = _xmlDoc.GetElementsByTagName("TelefoneFixo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneFixo")[i].InnerText;
                _Colaborador.TelefoneCelular = _xmlDoc.GetElementsByTagName("TelefoneCelular")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneCelular")[i].InnerText;
                _Colaborador.Email = _xmlDoc.GetElementsByTagName("Email")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email")[i].InnerText;
                _Colaborador.ContatoEmergencia = _xmlDoc.GetElementsByTagName("ContatoEmergencia")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ContatoEmergencia")[i].InnerText;
                _Colaborador.TelefoneEmergencia = _xmlDoc.GetElementsByTagName("TelefoneEmergencia")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneEmergencia")[i].InnerText;

                _Colaborador.Cep = _xmlDoc.GetElementsByTagName("Cep")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cep")[i].InnerText;
                _Colaborador.Endereco = _xmlDoc.GetElementsByTagName("Endereco")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Endereco")[i].InnerText;
                _Colaborador.Numero = _xmlDoc.GetElementsByTagName("Numero")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Numero")[i].InnerText;
                _Colaborador.Complemento = _xmlDoc.GetElementsByTagName("Complemento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Complemento")[i].InnerText;
                _Colaborador.Bairro = _xmlDoc.GetElementsByTagName("Bairro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bairro")[i].InnerText;
                _Colaborador.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);
                _Colaborador.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);

                bool _motorista;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Motorista")[i].InnerText, out _motorista);
                _Colaborador.Motorista = _xmlDoc.GetElementsByTagName("Motorista")[i] == null ? false : _motorista;
                _Colaborador.CNHCategoria = _xmlDoc.GetElementsByTagName("CNHCategoria")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNHCategoria")[i].InnerText;

                _Colaborador.CNH = _xmlDoc.GetElementsByTagName("CNH")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNH")[i].InnerText;
                _Colaborador.CNHValidade = _xmlDoc.GetElementsByTagName("CNHValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("CNHValidade")[i].InnerText);
                _Colaborador.CNHEmissor = _xmlDoc.GetElementsByTagName("CNHEmissor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNHEmissor")[i].InnerText;
                _Colaborador.CNHUF = _xmlDoc.GetElementsByTagName("CNHUF")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNHUF")[i].InnerText;
                _Colaborador.Bagagem = _xmlDoc.GetElementsByTagName("Bagagem")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bagagem")[i].InnerText;
                _Colaborador.DataEmissao = _xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText);
                _Colaborador.DataValidade = _xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText);
                _Colaborador.Excluida = _xmlDoc.GetElementsByTagName("Excluida")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
                _Colaborador.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                _Colaborador.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                bool _controlado1, _controlado2, _controlado3, _controlado4, _controlado5, _controlado6, _controlado7;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente21")[i].InnerText, out _controlado1);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente22")[i].InnerText, out _controlado2);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente23")[i].InnerText, out _controlado3);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente24")[i].InnerText, out _controlado4);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente25")[i].InnerText, out _controlado5);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente26")[i].InnerText, out _controlado6);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente27")[i].InnerText, out _controlado7);


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_Colaborador.ColaboradorID == 0)
                {
                    _sqlCmd = new SqlCommand("Insert into Colaboradores(Nome,Apelido,DataNascimento,NomePai" +
                                                                        ",NomeMae,Nacionalidade,Foto,EstadoCivil" +
                                                                        ",CPF,RG,RGEmissao,RGOrgLocal" +
                                                                        ",RGOrgUF,Passaporte,PassaporteValidade,RNE,TelefoneFixo" +
                                                                        ",TelefoneCelular,Email,ContatoEmergencia,TelefoneEmergencia" +
                                                                        ",Cep,Endereco,Numero,Complemento" +
                                                                        ",Bairro,MunicipioID,EstadoID,Motorista" +
                                                                        ",CNH,CNHValidade,CNHEmissor,CNHUF" +
                                                                        ",Bagagem,DataEmissao,DataValidade,Excluida" +
                                                                        ",StatusID,TipoAcessoID,CNHCategoria) " +
                                                                        " values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15" +
                                                                        ",@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v28,@v29,@v30" +
                                                                        ",@v31,@v32,@v33,@v34,@v35,@v36,@v37,@v38,@v39,@v40)" +
                                                                        ";SELECT SCOPE_IDENTITY();", _Con);



                    _sqlCmd.Parameters.Add("@v1", SqlDbType.VarChar).Value = _Colaborador.Nome;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = _Colaborador.Apelido;
                    if (_Colaborador.DataNascimento == null)
                    {
                        _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = _Colaborador.DataNascimento;
                    }
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.VarChar).Value = _Colaborador.NomePai;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.VarChar).Value = _Colaborador.NomeMae;
                    _sqlCmd.Parameters.Add("@v6", SqlDbType.VarChar).Value = _Colaborador.Nacionalidade;
                    _sqlCmd.Parameters.Add("@v7", SqlDbType.VarChar).Value = _Colaborador.Foto;
                    _sqlCmd.Parameters.Add("@v8", SqlDbType.VarChar).Value = _Colaborador.EstadoCivil;
                    _sqlCmd.Parameters.Add("@v9", SqlDbType.VarChar).Value = _Colaborador.CPF.RetirarCaracteresEspeciais();
                    _sqlCmd.Parameters.Add("@v10", SqlDbType.VarChar).Value = _Colaborador.RG;
                    if (_Colaborador.RGEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@v11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v11", SqlDbType.DateTime).Value = _Colaborador.RGEmissao;
                    }

                    _sqlCmd.Parameters.Add("@v12", SqlDbType.VarChar).Value = _Colaborador.RGOrgLocal;
                    _sqlCmd.Parameters.Add("@v13", SqlDbType.VarChar).Value = _Colaborador.RGOrgUF;
                    _sqlCmd.Parameters.Add("@v14", SqlDbType.VarChar).Value = _Colaborador.Passaporte;
                    if (_Colaborador.PassaporteValidade == null)
                    {
                        _sqlCmd.Parameters.Add("@v15", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v15", SqlDbType.DateTime).Value = _Colaborador.PassaporteValidade;
                    }
                    _sqlCmd.Parameters.Add("@v16", SqlDbType.VarChar).Value = _Colaborador.RNE;
                    _sqlCmd.Parameters.Add("@v17", SqlDbType.VarChar).Value = _Colaborador.TelefoneFixo;
                    _sqlCmd.Parameters.Add("@v18", SqlDbType.VarChar).Value = _Colaborador.TelefoneCelular;
                    _sqlCmd.Parameters.Add("@v19", SqlDbType.VarChar).Value = _Colaborador.Email;
                    _sqlCmd.Parameters.Add("@v20", SqlDbType.VarChar).Value = _Colaborador.ContatoEmergencia;
                    _sqlCmd.Parameters.Add("@v21", SqlDbType.VarChar).Value = _Colaborador.TelefoneEmergencia;
                    _sqlCmd.Parameters.Add("@v22", SqlDbType.VarChar).Value = _Colaborador.Cep;
                    _sqlCmd.Parameters.Add("@v23", SqlDbType.VarChar).Value = _Colaborador.Endereco;
                    _sqlCmd.Parameters.Add("@v24", SqlDbType.VarChar).Value = _Colaborador.Numero;
                    _sqlCmd.Parameters.Add("@v25", SqlDbType.VarChar).Value = _Colaborador.Complemento;
                    _sqlCmd.Parameters.Add("@v26", SqlDbType.VarChar).Value = _Colaborador.Bairro;
                    _sqlCmd.Parameters.Add("@v27", SqlDbType.Int).Value = _Colaborador.MunicipioID;
                    _sqlCmd.Parameters.Add("@v28", SqlDbType.Int).Value = _Colaborador.EstadoID;
                    _sqlCmd.Parameters.Add("@v29", SqlDbType.VarChar).Value = _Colaborador.Motorista;
                    _sqlCmd.Parameters.Add("@v30", SqlDbType.VarChar).Value = _Colaborador.CNH;

                    if (_Colaborador.CNHValidade == null)
                    {
                        _sqlCmd.Parameters.Add("@v31", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v31", SqlDbType.DateTime).Value = _Colaborador.CNHValidade;
                    }
                    _sqlCmd.Parameters.Add("@v32", SqlDbType.VarChar).Value = _Colaborador.CNHEmissor;
                    _sqlCmd.Parameters.Add("@v33", SqlDbType.VarChar).Value = _Colaborador.CNHUF;
                    _sqlCmd.Parameters.Add("@v34", SqlDbType.VarChar).Value = _Colaborador.Bagagem;

                    if (_Colaborador.DataEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@v35", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v35", SqlDbType.DateTime).Value = _Colaborador.DataEmissao;
                    }

                    if (_Colaborador.DataValidade == null)
                    {
                        _sqlCmd.Parameters.Add("@v36", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v36", SqlDbType.DateTime).Value = _Colaborador.DataValidade;
                    }
                    _sqlCmd.Parameters.Add("@v37", SqlDbType.Int).Value = _Colaborador.Excluida;
                    _sqlCmd.Parameters.Add("@v38", SqlDbType.Int).Value = _Colaborador.StatusID;
                    _sqlCmd.Parameters.Add("@v39", SqlDbType.Int).Value = _Colaborador.TipoAcessoID;
                    _sqlCmd.Parameters.Add("@v40", SqlDbType.VarChar).Value = _Colaborador.CNHCategoria;



                    _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
                    _Con.Close();

                }

                return _novID;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereColaboradoresBD ex: " + ex);
                return 0;

            }
        }

        private void AtualizaColaboradoresBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseColaboradores.Colaborador _Colaborador = new ClasseColaboradores.Colaborador();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _Colaborador.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
                _Colaborador.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Nome")[i].InnerText;
                _Colaborador.Apelido = _xmlDoc.GetElementsByTagName("Apelido")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Apelido")[i].InnerText;
                _Colaborador.DataNascimento = _xmlDoc.GetElementsByTagName("DataNascimento")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataNascimento")[i].InnerText);
                _Colaborador.NomePai = _xmlDoc.GetElementsByTagName("NomePai")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomePai")[i].InnerText;
                _Colaborador.NomeMae = _xmlDoc.GetElementsByTagName("NomeMae")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeMae")[i].InnerText;
                _Colaborador.Nacionalidade = _xmlDoc.GetElementsByTagName("Nacionalidade")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Nacionalidade")[i].InnerText;
                _Colaborador.Foto = _xmlDoc.GetElementsByTagName("Foto")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Foto")[i].InnerText;
                _Colaborador.EstadoCivil = _xmlDoc.GetElementsByTagName("EstadoCivil")[i] == null ? "" : _xmlDoc.GetElementsByTagName("EstadoCivil")[i].InnerText;
                _Colaborador.CPF = _xmlDoc.GetElementsByTagName("CPF")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CPF")[i].InnerText;
                _Colaborador.RG = _xmlDoc.GetElementsByTagName("RG")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RG")[i].InnerText;
                _Colaborador.RGEmissao = _xmlDoc.GetElementsByTagName("RGEmissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("RGEmissao")[i].InnerText);
                _Colaborador.RGOrgLocal = _xmlDoc.GetElementsByTagName("RGOrgLocal")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RGOrgLocal")[i].InnerText;
                _Colaborador.RGOrgUF = _xmlDoc.GetElementsByTagName("RGOrgUF")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RGOrgUF")[i].InnerText;
                _Colaborador.Passaporte = _xmlDoc.GetElementsByTagName("Passaporte")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Passaporte")[i].InnerText;
                _Colaborador.PassaporteValidade = _xmlDoc.GetElementsByTagName("PassaporteValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("PassaporteValidade")[i].InnerText);
                _Colaborador.RNE = _xmlDoc.GetElementsByTagName("RNE")[i] == null ? "" : _xmlDoc.GetElementsByTagName("RNE")[i].InnerText;
                _Colaborador.TelefoneFixo = _xmlDoc.GetElementsByTagName("TelefoneFixo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneFixo")[i].InnerText;
                _Colaborador.TelefoneCelular = _xmlDoc.GetElementsByTagName("TelefoneCelular")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneCelular")[i].InnerText;
                _Colaborador.Email = _xmlDoc.GetElementsByTagName("Email")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email")[i].InnerText;
                _Colaborador.ContatoEmergencia = _xmlDoc.GetElementsByTagName("ContatoEmergencia")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ContatoEmergencia")[i].InnerText;
                _Colaborador.TelefoneEmergencia = _xmlDoc.GetElementsByTagName("TelefoneEmergencia")[i] == null ? "" : _xmlDoc.GetElementsByTagName("TelefoneEmergencia")[i].InnerText;

                _Colaborador.Cep = _xmlDoc.GetElementsByTagName("Cep")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cep")[i].InnerText;
                _Colaborador.Endereco = _xmlDoc.GetElementsByTagName("Endereco")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Endereco")[i].InnerText;
                _Colaborador.Numero = _xmlDoc.GetElementsByTagName("Numero")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Numero")[i].InnerText;
                _Colaborador.Complemento = _xmlDoc.GetElementsByTagName("Complemento")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Complemento")[i].InnerText;
                _Colaborador.Bairro = _xmlDoc.GetElementsByTagName("Bairro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bairro")[i].InnerText;
                _Colaborador.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);
                _Colaborador.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);

                bool _motorista;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Motorista")[i].InnerText, out _motorista);
                _Colaborador.Motorista = _xmlDoc.GetElementsByTagName("Motorista")[i] == null ? false : _motorista;
                _Colaborador.CNHCategoria = _xmlDoc.GetElementsByTagName("CNHCategoria")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNHCategoria")[i].InnerText;

                _Colaborador.CNH = _xmlDoc.GetElementsByTagName("CNH")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNH")[i].InnerText;
                _Colaborador.CNHValidade = _xmlDoc.GetElementsByTagName("CNHValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("CNHValidade")[i].InnerText);
                _Colaborador.CNHEmissor = _xmlDoc.GetElementsByTagName("CNHEmissor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNHEmissor")[i].InnerText;
                _Colaborador.CNHUF = _xmlDoc.GetElementsByTagName("CNHUF")[i] == null ? "" : _xmlDoc.GetElementsByTagName("CNHUF")[i].InnerText;
                _Colaborador.Bagagem = _xmlDoc.GetElementsByTagName("Bagagem")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Bagagem")[i].InnerText;
                _Colaborador.DataEmissao = _xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText);
                _Colaborador.DataValidade = _xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText);
                _Colaborador.Excluida = _xmlDoc.GetElementsByTagName("Excluida")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
                _Colaborador.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                _Colaborador.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);

                //                " values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15" +
                //",@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v28,@v29,@v30," +
                //"@v31,@v32,@v33,@v34,@v35,@v36,@v37,@v38,@v39,@v40,@v41,@v42,@v43,@v44,@v45,@v46)" +


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_Colaborador.ColaboradorID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Colaboradores Set" +
                        " Nome= @v1" +
                        ",Apelido= @v2" +
                        ",DataNascimento= @v3" +
                        ",NomePai= @v4" +
                        ",NomeMae= @v5" +
                        ",Nacionalidade= @v6" +
                        ",Foto= @v7" +
                        ",EstadoCivil= @v8" +
                        ",CPF= @v9" +
                        ",RG= @v10" +
                        ",RGEmissao= @v11" +
                        ",RGOrgLocal= @v12" +
                        ",RGOrgUF= @v13" +
                        ",Passaporte= @v14" +
                        ",PassaporteValidade= @v15" +
                        ",RNE= @v16" +
                        ",TelefoneFixo= @v17" +
                        ",TelefoneCelular= @v18" +
                        ",Email= @v19" +
                        ",ContatoEmergencia= @v20" +
                        ",TelefoneEmergencia= @v21" +
                        ",Cep= @v22" +
                        ",Endereco= @v23" +
                        ",Numero= @v24" +
                        ",Complemento= @v25" +
                        ",Bairro= @v26" +
                        ",MunicipioID= @v27" +
                        ",EstadoID= @v28" +
                        ",Motorista= @v29" +                        
                        ",CNH= @v30" +
                        ",CNHValidade= @v31" +
                        ",CNHEmissor= @v32" +
                        ",CNHUF= @v33" +
                        ",Bagagem= @v34" +
                        ",DataEmissao= @v35" +
                        ",DataValidade= @v36" +
                        ",Excluida= @v37" +
                        ",StatusID= @v38" +
                        ",TipoAcessoID= @v39" +
                        ",CNHCategoria = @v40" +
                        " Where ColaboradorID = @v0", _Con);

                    _sqlCmd.Parameters.Add("@v0", SqlDbType.Int).Value = _Colaborador.ColaboradorID;
                    _sqlCmd.Parameters.Add("@v1", SqlDbType.VarChar).Value = _Colaborador.Nome;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = _Colaborador.Apelido;
                    if (_Colaborador.DataNascimento == null)
                    {
                        _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = _Colaborador.DataNascimento;
                    }
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.VarChar).Value = _Colaborador.NomePai;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.VarChar).Value = _Colaborador.NomeMae;
                    _sqlCmd.Parameters.Add("@v6", SqlDbType.VarChar).Value = _Colaborador.Nacionalidade;
                    _sqlCmd.Parameters.Add("@v7", SqlDbType.VarChar).Value = _Colaborador.Foto;
                    _sqlCmd.Parameters.Add("@v8", SqlDbType.VarChar).Value = _Colaborador.EstadoCivil;
                    _sqlCmd.Parameters.Add("@v9", SqlDbType.VarChar).Value = _Colaborador.CPF.RetirarCaracteresEspeciais();
                    _sqlCmd.Parameters.Add("@v10", SqlDbType.VarChar).Value = _Colaborador.RG;
                    if (_Colaborador.RGEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@v11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v11", SqlDbType.DateTime).Value = _Colaborador.RGEmissao;
                    }
                    _sqlCmd.Parameters.Add("@v12", SqlDbType.VarChar).Value = _Colaborador.RGOrgLocal;
                    _sqlCmd.Parameters.Add("@v13", SqlDbType.VarChar).Value = _Colaborador.RGOrgUF;
                    _sqlCmd.Parameters.Add("@v14", SqlDbType.VarChar).Value = _Colaborador.Passaporte;
                    if (_Colaborador.PassaporteValidade == null)
                    {
                        _sqlCmd.Parameters.Add("@v15", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v15", SqlDbType.DateTime).Value = _Colaborador.PassaporteValidade;
                    }
                    _sqlCmd.Parameters.Add("@v16", SqlDbType.VarChar).Value = _Colaborador.RNE;
                    _sqlCmd.Parameters.Add("@v17", SqlDbType.VarChar).Value = _Colaborador.TelefoneFixo;
                    _sqlCmd.Parameters.Add("@v18", SqlDbType.VarChar).Value = _Colaborador.TelefoneCelular;
                    _sqlCmd.Parameters.Add("@v19", SqlDbType.VarChar).Value = _Colaborador.Email;
                    _sqlCmd.Parameters.Add("@v20", SqlDbType.VarChar).Value = _Colaborador.ContatoEmergencia;
                    _sqlCmd.Parameters.Add("@v21", SqlDbType.VarChar).Value = _Colaborador.TelefoneEmergencia;
                    _sqlCmd.Parameters.Add("@v22", SqlDbType.VarChar).Value = _Colaborador.Cep;
                    _sqlCmd.Parameters.Add("@v23", SqlDbType.VarChar).Value = _Colaborador.Endereco;
                    _sqlCmd.Parameters.Add("@v24", SqlDbType.VarChar).Value = _Colaborador.Numero;
                    _sqlCmd.Parameters.Add("@v25", SqlDbType.VarChar).Value = _Colaborador.Complemento;
                    _sqlCmd.Parameters.Add("@v26", SqlDbType.VarChar).Value = _Colaborador.Bairro;
                    _sqlCmd.Parameters.Add("@v27", SqlDbType.Int).Value = _Colaborador.MunicipioID;
                    _sqlCmd.Parameters.Add("@v28", SqlDbType.Int).Value = _Colaborador.EstadoID;
                    _sqlCmd.Parameters.Add("@v29", SqlDbType.VarChar).Value = _Colaborador.Motorista;
                    _sqlCmd.Parameters.Add("@v30", SqlDbType.VarChar).Value = _Colaborador.CNH;

                    if (_Colaborador.CNHValidade == null)
                    {
                        _sqlCmd.Parameters.Add("@v31", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v31", SqlDbType.DateTime).Value = _Colaborador.CNHValidade;
                    }
                    _sqlCmd.Parameters.Add("@v32", SqlDbType.VarChar).Value = _Colaborador.CNHEmissor;
                    _sqlCmd.Parameters.Add("@v33", SqlDbType.VarChar).Value = _Colaborador.CNHUF;
                    _sqlCmd.Parameters.Add("@v34", SqlDbType.VarChar).Value = _Colaborador.Bagagem;

                    if (_Colaborador.DataEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@v35", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v35", SqlDbType.DateTime).Value = _Colaborador.DataEmissao;
                    }

                    if (_Colaborador.DataValidade == null)
                    {
                        _sqlCmd.Parameters.Add("@v36", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@v36", SqlDbType.DateTime).Value = _Colaborador.DataValidade;
                    }
                    _sqlCmd.Parameters.Add("@v37", SqlDbType.Int).Value = _Colaborador.Excluida;
                    _sqlCmd.Parameters.Add("@v38", SqlDbType.Int).Value = _Colaborador.StatusID;
                    _sqlCmd.Parameters.Add("@v39", SqlDbType.Int).Value = _Colaborador.TipoAcessoID;
                    _sqlCmd.Parameters.Add("@v40", SqlDbType.VarChar).Value = _Colaborador.CNHCategoria;

                    _sqlCmd.ExecuteNonQuery();
                    _Con.Close();
                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void AtualizaColaboradorBD ex: " + ex);


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


        private void AtualizaPendencias(int _ColaboradorID)
        {
            try
            {

                if (_ColaboradorID == 0)
                {
                    return;
                }

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlCmd;
                for (int i = 21; i < 28; i++)
                {
                    _sqlCmd = new SqlCommand("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,ColaboradorID) values (" +
                                                      "@v1,@v2, @v3,@v4,@v5)", _Con);

                    _sqlCmd.Parameters.Add("@v1", SqlDbType.Int).Value = i;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
                    _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DateTime.Now;
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.Bit).Value = 1;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.Int).Value = _ColaboradorID;
                    _sqlCmd.ExecuteNonQuery();
                    //Thread.Sleep(200);
                }

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void AtualizaPendencias ex: " + ex);


            }

        }
        #endregion

        #region Metodos privados

        internal void SalvarAdicao2()
        {
            try
            {

                HabilitaEdicao = false;


                var colab = Mapper.Map<IMOD.Domain.Entities.Colaborador>(ColaboradorSelecionado);
                _colaboradorService.Criar(colab);

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
        internal void SalvarAdicao()
        {
            try
            {

                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradores));

                ObservableCollection<ClasseColaboradores.Colaborador> _ColaboradoresPro = new ObservableCollection<ClasseColaboradores.Colaborador>();
                ClasseColaboradores _ClasseColaboradoresTemp = new ClasseColaboradores();
                ColaboradorSelecionado.Pendente = true;
                ColaboradorSelecionado.Pendente21 = true;
                ColaboradorSelecionado.Pendente22 = true;
                ColaboradorSelecionado.Pendente23 = true;
                ColaboradorSelecionado.Pendente24 = true;
                ColaboradorSelecionado.Pendente25 = true;


                _ColaboradoresPro.Add(ColaboradorSelecionado);
                _ClasseColaboradoresTemp.Colaboradores = _ColaboradoresPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseColaboradoresTemp);
                        xmlString = sw.ToString();
                    }

                }

                int _novoColaboradorID = InsereColaboradoresBD(xmlString);

                AtualizaPendencias(_novoColaboradorID);

                ColaboradorSelecionado.ColaboradorID = _novoColaboradorID;

                _ColaboradoresTemp.Clear();

                _ColaboradoresTemp.Add(ColaboradorSelecionado);
                Colaboradores = null;
                Colaboradores = new ObservableCollection<ClasseColaboradores.Colaborador>(_ColaboradoresTemp);
                SelectedIndex = 0;
                _ColaboradoresTemp.Clear();
                _ColaboradoresPro.Clear();

                _ClasseColaboradoresTemp = null;

                //this._ColaboradoresTemp.Clear();
                //_colaboradorTemp = null;


            }
            catch (Exception ex)
            {

            }
        }

        internal void SalvarEdicao()
        {
            try
            {

                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseColaboradores));

                ObservableCollection<ClasseColaboradores.Colaborador> _ColaboradoresTemp = new ObservableCollection<ClasseColaboradores.Colaborador>();
                ClasseColaboradores _ClasseColaboradoresTemp = new ClasseColaboradores();
                _ColaboradoresTemp.Add(ColaboradorSelecionado);
                _ClasseColaboradoresTemp.Colaboradores = _ColaboradoresTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseColaboradoresTemp);
                        xmlString = sw.ToString();
                    }

                }

                AtualizaColaboradoresBD(xmlString);

                _ClasseColaboradoresTemp = null;

                this._ColaboradoresTemp.Clear();
                _colaboradorTemp = null;


            }
            catch (Exception ex)
            {

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
