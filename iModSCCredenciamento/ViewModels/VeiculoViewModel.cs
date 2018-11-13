﻿using iModSCCredenciamento.Models;
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

namespace iModSCCredenciamento.ViewModels
{
    class VeiculoViewModel : ViewModelBase
    {
        #region Inicializacao
        public VeiculoViewModel()
        {
            //CarregaUI();
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
            //CarregaColecaoVeiculos();
        }
        private void CarregaUI()
        {
            CarregaColecaoVeiculos();
            CarregaColeçãoEstados();
            //CarregaColecaoTiposAtividades();
            //CarregaColecaoAreasAcessos();
            //CarregaColecaoLayoutsCrachas();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseVeiculos.Veiculo> _Veiculos;

        private ClasseVeiculos.Veiculo _VeiculoSelecionado;

        private ClasseVeiculos.Veiculo _veiculoTemp = new ClasseVeiculos.Veiculo();

        private List<ClasseVeiculos.Veiculo> _VeiculosTemp = new List<ClasseVeiculos.Veiculo>();

        private ObservableCollection<ClasseEstados.Estado> _Estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        PopupPesquisaVeiculos popupPesquisaVeiculos;

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
        public ObservableCollection<ClasseVeiculos.Veiculo> Veiculos
        {
            get
            {
                return _Veiculos;
            }

            set
            {
                if (_Veiculos != value)
                {
                    _Veiculos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseVeiculos.Veiculo VeiculoSelecionado
        {
            get
            {

                return this._VeiculoSelecionado;
            }
            set
            {
                this._VeiculoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (VeiculoSelecionado != null)
                {
                    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                    //string _imgstr = Conversores.IMGtoSTR(_img);
                    //VeiculoSelecionado.Foto = _imgstr;
                    if (!_atualizandoFoto)
                    {
                        Thread CarregaFoto_thr = new Thread(() => CarregaFoto(VeiculoSelecionado.VeiculoID));
                        CarregaFoto_thr.Start();
                    }

                    //CarregaFoto(VeiculoSelecionado.VeiculoID);
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
        public void OnAtualizaCommand(object veiculoID)
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

                string _nomecompletodoarquivo;
                string _arquivoSTR;
                _arquivoPDF.InitialDirectory = "c:\\\\";
                _arquivoPDF.Filter = "(*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();

                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);

                _veiculoTemp.NomeArquivoAnexo = _nomecompletodoarquivo;
                _veiculoTemp.ArquivoAnexo = _arquivoSTR;

                if (Veiculos != null)
                {
                    Veiculos[0].NomeArquivoAnexo = _nomecompletodoarquivo;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void OnAbrirArquivoCommand()
        {
            try
            {
                try
                {
                    string _ArquivoPDF = null;
                    if (_veiculoTemp != null)
                    {
                        if (_veiculoTemp.ArquivoAnexo != null && _veiculoTemp.VeiculoID == _VeiculoSelecionado.VeiculoID)
                        {
                            _ArquivoPDF = _veiculoTemp.ArquivoAnexo;

                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        string _xmlstring = CriaXmlImagem(_VeiculoSelecionado.VeiculoID);

                        XmlDocument xmldocument = new XmlDocument();
                        xmldocument.LoadXml(_xmlstring);
                        XmlNode node = (XmlNode)xmldocument.DocumentElement;
                        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                        _ArquivoPDF = arquivoNode.FirstChild.Value;
                    }
                    Global.PopupPDF(_ArquivoPDF);
                    //byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
                    //_ArquivoPDF = System.IO.Path.GetTempFileName();
                    //_ArquivoPDF = System.IO.Path.GetRandomFileName();
                    //_ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;

                    ////File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
                    //_ArquivoPDF = System.IO.Path.ChangeExtension(_ArquivoPDF, ".pdf");
                    //System.IO.File.WriteAllBytes(_ArquivoPDF, buffer);
                    ////Action<string> act = new Action<string>(Global.AbrirArquivoPDF);
                    ////act.BeginInvoke(_ArquivoPDF, null, null);
                    //Global.PopupPDF(_ArquivoPDF);
                    //System.IO.File.Delete(_ArquivoPDF);
                }
                catch (Exception ex)
                {
                    Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);

                }
            }
            catch (Exception ex)
            {

            }
        }

        //public void OnBuscarArquivoCommand()
        //{
        //    try
        //    {
        //        System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
        //        string _sql;
        //        string _nomecompletodoarquivo;
        //        string _arquivoSTR;
        //        _arquivoPDF.InitialDirectory = "c:\\\\";
        //        _arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        //        _arquivoPDF.RestoreDirectory = true;
        //        _arquivoPDF.ShowDialog();
        //        //if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
        //        //{
        //        _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
        //        _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
        //        //_seguroTemp.NomeArquivo = _nomecompletodoarquivo;
        //        //_seguroTemp.Arquivo = _arquivoSTR;
        //        //InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

        //        //AtualizaListaAnexos(_resp);

        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _veiculoTemp = VeiculoSelecionado.CriaCopia(VeiculoSelecionado);
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
                Veiculos[_selectedIndexTemp] = _veiculoTemp;
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

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Veiculos)
                {
                    _VeiculosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Veiculos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _veiculoTemp = new ClasseVeiculos.Veiculo();
                ////////////////////////////////////////////////////////
                _veiculoTemp.VeiculoID = EmpresaSelecionadaID;  //OBS
                ////////////////////////////////////////////////////////
                Veiculos.Add(_veiculoTemp);
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
                Veiculos = null;
                Veiculos = new ObservableCollection<ClasseVeiculos.Veiculo>(_VeiculosTemp);
                SelectedIndex = _selectedIndexTemp;
                _VeiculosTemp.Clear();
                HabilitaEdicao = false;
                _atualizandoFoto = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este veiculo?", "Excluir Veiculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste veiculo, inclusive histórico. Confirma exclusão?", "Excluir Veiculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        ExcluiVeiculoBD(VeiculoSelecionado.VeiculoID);
                //        Veiculos.Remove(VeiculoSelecionado);

                //    }
                //}

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiVeiculoBD(VeiculoSelecionado.VeiculoID);
                        Veiculos.Remove(VeiculoSelecionado);
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

                popupPesquisaVeiculos = new PopupPesquisaVeiculos();
                popupPesquisaVeiculos.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaVeiculos.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaVeiculos.Criterio.Split((char)(20));
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

            CarregaColecaoVeiculos(_codigo, _nome);
            SelectedIndex = 0;
        }

        public void OnAbrirPendencias(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupPendencias popupPendencias = new PopupPendencias(3, ((System.Windows.FrameworkElement)e.OriginalSource).Tag, VeiculoSelecionado.VeiculoID, VeiculoSelecionado.Placa);
                popupPendencias.ShowDialog();
                popupPendencias = null;
                CarregaColecaoVeiculos(VeiculoSelecionado.VeiculoID);


            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Carregamento das Colecoes

        private void CarregaColecaoVeiculos(int _VeiculoID = 0, string _nome = "", string _apelido = "", string _cpf = "", string _quantidaderegistro = "500")
        {
            try
            {
                string _xml = RequisitaVeiculos(_VeiculoID, _nome, _apelido, _cpf);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseVeiculos classeVeiculos = new ClasseVeiculos();
                classeVeiculos = (ClasseVeiculos)deserializer.Deserialize(reader);
                Veiculos = new ObservableCollection<ClasseVeiculos.Veiculo>();
                Veiculos = classeVeiculos.Veiculos;
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

        private void CarregaFoto(int _VeiculoID)
        {
            try
            {
                _atualizandoFoto = true; //para que o evento de VeiculoSelecionado não entre em looping
                ///
                //                BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                //                string _imgstr = Conversores.IMGtoSTR(_img);
                //                VeiculoSelecionado.Foto = _imgstr;

                //                System.Windows.Application.Current.Dispatcher.Invoke(
                //(Action)(() => {
                //_veiculoTemp = VeiculoSelecionado.CriaCopia(VeiculoSelecionado);
                //_selectedIndexTemp = SelectedIndex;

                //_veiculoTemp.Foto = _imgstr;
                //Veiculos[_selectedIndexTemp] = _veiculoTemp;

                //SelectedIndex = _selectedIndexTemp;



                //}));

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    Waiting = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Waitng.gif", UriKind.Absolute));

                    Waiting.Freeze();
                }));

                string _xmlstring = BuscaFoto(_VeiculoID);

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { Waiting = null; }));

                XmlDocument xmldocument = new XmlDocument();

                xmldocument.LoadXml(_xmlstring);

                XmlNode node = (XmlNode)xmldocument.DocumentElement;

                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                if (arquivoNode.HasChildNodes)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        _veiculoTemp = VeiculoSelecionado.CriaCopia(VeiculoSelecionado);

                        _selectedIndexTemp = SelectedIndex;

                        _veiculoTemp.Foto = arquivoNode.FirstChild.Value;

                        Veiculos[_selectedIndexTemp] = _veiculoTemp;

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
        private string RequisitaVeiculos(int _veiculoID = 0, string _placa = "", string _renavam = "", string _descricao = "", int _excluida = 0, string _quantidaderegistro = "500")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVeiculos = _xmlDocument.CreateElement("ClasseVeiculos");
                _xmlDocument.AppendChild(_ClasseVeiculos);

                XmlNode _Veiculos = _xmlDocument.CreateElement("Veiculos");
                _ClasseVeiculos.AppendChild(_Veiculos);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _veiculoIDSTR = "";

                _veiculoIDSTR = _veiculoID == 0 ? "" : " AND VeiculoID = " + _veiculoID;
                _placa = _placa == "" ? "" : " AND Placa like '%" + _placa + "%' ";
                _renavam = _renavam == "" ? "" : "AND Renavam like '%" + _renavam + "%' ";
                _descricao = _descricao == "" ? "" : " AND Descricao like '%" + _descricao + "%'";

                _strSql = "VeiculoID,Descricao,Tipo,Marca,Modelo,Ano,Cor,Placa,Renavam,EstadoID,MunicipioID,Foto,Excluida," +
                    "StatusID,TipoAcessoID,DescricaoAnexo,NomeArquivoAnexo,Pendente31,Pendente32,Pendente33,Pendente34 " +
                     "from Veiculos where Excluida  = " + _excluida + _veiculoIDSTR + _placa + _renavam + _descricao + " order by VeiculoID desc";

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

                    XmlNode _Veiculo = _xmlDocument.CreateElement("Veiculo");
                    _Veiculos.AppendChild(_Veiculo);

                    XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    _Veiculo.AppendChild(_VeiculoID);

                    XmlNode _Node2 = _xmlDocument.CreateElement("Descricao");
                    _Node2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _Veiculo.AppendChild(_Node2);

                    XmlNode _Node3 = _xmlDocument.CreateElement("Tipo");
                    _Node3.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Tipo"].ToString())));
                    _Veiculo.AppendChild(_Node3);

                    XmlNode _Node4 = _xmlDocument.CreateElement("Marca");
                    _Node4.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Marca"].ToString())));
                    _Veiculo.AppendChild(_Node4);

                    XmlNode _Node5 = _xmlDocument.CreateElement("Modelo");
                    _Node5.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Modelo"].ToString())));
                    _Veiculo.AppendChild(_Node5);

                    XmlNode _Node6 = _xmlDocument.CreateElement("Ano");
                    _Node6.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Ano"].ToString())));
                    _Veiculo.AppendChild(_Node6);

                    XmlNode _Node7 = _xmlDocument.CreateElement("Cor");
                    _Node7.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cor"].ToString())));
                    _Veiculo.AppendChild(_Node7);

                    XmlNode _Node8 = _xmlDocument.CreateElement("Placa");
                    _Node8.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Placa"].ToString())));
                    _Veiculo.AppendChild(_Node8);

                    XmlNode _Node9 = _xmlDocument.CreateElement("Renavam");
                    _Node9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Renavam"].ToString())));
                    _Veiculo.AppendChild(_Node9);

                    XmlNode _Node10 = _xmlDocument.CreateElement("EstadoID");
                    _Node10.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _Veiculo.AppendChild(_Node10);

                    XmlNode _Node11 = _xmlDocument.CreateElement("MunicipioID");
                    _Node11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _Veiculo.AppendChild(_Node11);

                    XmlNode _Node12 = _xmlDocument.CreateElement("Foto");
                    //_Node12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString())));
                    _Veiculo.AppendChild(_Node12);

                    XmlNode _Node13 = _xmlDocument.CreateElement("Excluida");
                    _Node13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    _Veiculo.AppendChild(_Node13);

                    XmlNode _Node15 = _xmlDocument.CreateElement("StatusID");
                    _Node15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
                    _Veiculo.AppendChild(_Node15);

                    XmlNode _Node16 = _xmlDocument.CreateElement("TipoAcessoID");
                    _Node16.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
                    _Veiculo.AppendChild(_Node16);

                    XmlNode _DescricaoAnexo = _xmlDocument.CreateElement("DescricaoAnexo");
                    _DescricaoAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DescricaoAnexo"].ToString()).Trim()));
                    _Veiculo.AppendChild(_DescricaoAnexo);

                    XmlNode _NomeArquivoAnexo = _xmlDocument.CreateElement("NomeArquivoAnexo");
                    _NomeArquivoAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivoAnexo"].ToString()).Trim()));
                    _Veiculo.AppendChild(_NomeArquivoAnexo);

                    XmlNode _ArquivoAnexo = _xmlDocument.CreateElement("ArquivoAnexo");
                    //_ArquivoAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ArquivoAnexo"].ToString())));
                    _Veiculo.AppendChild(_ArquivoAnexo);

                    XmlNode _Pendente1 = _xmlDocument.CreateElement("Pendente31");
                    _Pendente1.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente31"])).ToString()));
                    _Veiculo.AppendChild(_Pendente1);

                    XmlNode _Pendente2 = _xmlDocument.CreateElement("Pendente32");
                    _Pendente2.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente32"])).ToString()));
                    _Veiculo.AppendChild(_Pendente2);

                    XmlNode _Pendente3 = _xmlDocument.CreateElement("Pendente33");
                    _Pendente3.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente33"])).ToString()));
                    _Veiculo.AppendChild(_Pendente3);

                    XmlNode _Pendente4 = _xmlDocument.CreateElement("Pendente34");
                    _Pendente4.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Pendente34"])).ToString()));
                    _Veiculo.AppendChild(_Pendente4);


                    bool _pend = false;
                    _pend = (bool)_sqlreader["Pendente31"] ||
                        (bool)_sqlreader["Pendente32"] ||
                        (bool)_sqlreader["Pendente33"] ||
                        (bool)_sqlreader["Pendente34"];

                    XmlNode _Pendente = _xmlDocument.CreateElement("Pendente");
                    _Pendente.AppendChild(_xmlDocument.CreateTextNode(Convert.ToInt32(_pend).ToString()));
                    _Veiculo.AppendChild(_Pendente);


                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                //InsereVeiculosBD("");

                return _xml;
            }
            catch (Exception ex)
            {

                return null;
            }
            return null;
        }

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

        private int InsereVeiculosBD(string xmlString)
        {
            try
            {
                int _novID = 0;

                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseVeiculos.Veiculo _Veiculo = new ClasseVeiculos.Veiculo();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;
                _Veiculo.VeiculoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);
                _Veiculo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
                _Veiculo.Tipo = _xmlDoc.GetElementsByTagName("Tipo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Tipo")[i].InnerText;
                _Veiculo.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
                _Veiculo.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
                _Veiculo.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
                _Veiculo.Cor = _xmlDoc.GetElementsByTagName("Cor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cor")[i].InnerText;
                _Veiculo.Placa = _xmlDoc.GetElementsByTagName("Placa")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Placa")[i].InnerText;
                _Veiculo.Renavam = _xmlDoc.GetElementsByTagName("Renavam")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Renavam")[i].InnerText;
                _Veiculo.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
                _Veiculo.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);

                _Veiculo.Foto = _xmlDoc.GetElementsByTagName("Foto")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Foto")[i].InnerText;
                _Veiculo.Excluida = _xmlDoc.GetElementsByTagName("Excluida")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
                _Veiculo.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                _Veiculo.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);

                _Veiculo.NomeArquivoAnexo = _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i].InnerText;
                _Veiculo.DescricaoAnexo = _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i].InnerText;
                _Veiculo.ArquivoAnexo = _veiculoTemp.ArquivoAnexo == null ? "" : _veiculoTemp.ArquivoAnexo;

                bool _controlado1, _controlado2, _controlado3, _controlado4;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente31")[i].InnerText, out _controlado1);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente32")[i].InnerText, out _controlado2);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente33")[i].InnerText, out _controlado3);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente34")[i].InnerText, out _controlado4);



                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_Veiculo.VeiculoID == 0)
                {
                    _sqlCmd = new SqlCommand("Insert into Veiculos(Descricao,Tipo,Marca,Modelo,Ano,Cor,Placa,Renavam," +
                        "EstadoID,MunicipioID,Foto,Excluida,StatusID,TipoAcessoID,NomeArquivoAnexo,DescricaoAnexo,ArquivoAnexo," +
                        "Pendente31,Pendente32,Pendente33,Pendente34) " +
                        " values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15" +
                         ",@v16,@v17,@v18,@v19,@v20,@v21);SELECT SCOPE_IDENTITY();", _Con);

                    _sqlCmd.Parameters.Add("@v1", SqlDbType.VarChar).Value = _Veiculo.Descricao;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = _Veiculo.Tipo;
                    _sqlCmd.Parameters.Add("@v3", SqlDbType.VarChar).Value = _Veiculo.Marca;
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.VarChar).Value = _Veiculo.Modelo;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.VarChar).Value = _Veiculo.Ano;
                    _sqlCmd.Parameters.Add("@v6", SqlDbType.VarChar).Value = _Veiculo.Cor;
                    _sqlCmd.Parameters.Add("@v7", SqlDbType.VarChar).Value = _Veiculo.Placa;
                    _sqlCmd.Parameters.Add("@v8", SqlDbType.VarChar).Value = _Veiculo.Renavam;
                    _sqlCmd.Parameters.Add("@v9", SqlDbType.Int).Value = _Veiculo.EstadoID;
                    _sqlCmd.Parameters.Add("@v10", SqlDbType.Int).Value = _Veiculo.MunicipioID;
                    _sqlCmd.Parameters.Add("@v11", SqlDbType.VarChar).Value = _Veiculo.Foto;
                    _sqlCmd.Parameters.Add("@v12", SqlDbType.Int).Value = _Veiculo.Excluida;
                    _sqlCmd.Parameters.Add("@v13", SqlDbType.Int).Value = _Veiculo.StatusID;
                    _sqlCmd.Parameters.Add("@v14", SqlDbType.Int).Value = _Veiculo.TipoAcessoID;
                    _sqlCmd.Parameters.Add("@v15", SqlDbType.VarChar).Value = _Veiculo.NomeArquivoAnexo;
                    _sqlCmd.Parameters.Add("@v16", SqlDbType.VarChar).Value = _Veiculo.DescricaoAnexo;
                    _sqlCmd.Parameters.Add("@v17", SqlDbType.VarChar).Value = _Veiculo.ArquivoAnexo;
                    _sqlCmd.Parameters.Add("@v18", SqlDbType.Bit).Value = _controlado1;
                    _sqlCmd.Parameters.Add("@v19", SqlDbType.Bit).Value = _controlado2;
                    _sqlCmd.Parameters.Add("@v20", SqlDbType.Bit).Value = _controlado3;
                    _sqlCmd.Parameters.Add("@v21", SqlDbType.Bit).Value = _controlado4;

                    _novID = Convert.ToInt32(_sqlCmd.ExecuteScalar());
                    _Con.Close();

                }

                return _novID;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereVeiculosBD ex: " + ex);
                return 0;

            }
        }

        private void AtualizaVeiculosBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseVeiculos.Veiculo _Veiculo = new ClasseVeiculos.Veiculo();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _Veiculo.VeiculoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);
                _Veiculo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
                _Veiculo.Tipo = _xmlDoc.GetElementsByTagName("Tipo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Tipo")[i].InnerText;
                _Veiculo.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
                _Veiculo.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
                _Veiculo.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
                _Veiculo.Cor = _xmlDoc.GetElementsByTagName("Cor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cor")[i].InnerText;
                _Veiculo.Placa = _xmlDoc.GetElementsByTagName("Placa")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Placa")[i].InnerText;
                _Veiculo.Renavam = _xmlDoc.GetElementsByTagName("Renavam")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Renavam")[i].InnerText;
                _Veiculo.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
                _Veiculo.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);

                _Veiculo.Foto = _xmlDoc.GetElementsByTagName("Foto")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Foto")[i].InnerText;
                _Veiculo.Excluida = _xmlDoc.GetElementsByTagName("Excluida")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Excluida")[i].InnerText);
                _Veiculo.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                _Veiculo.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);

                _Veiculo.NomeArquivoAnexo = _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivoAnexo")[i].InnerText;
                _Veiculo.DescricaoAnexo = _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("DescricaoAnexo")[i].InnerText;
                _Veiculo.ArquivoAnexo = _veiculoTemp.ArquivoAnexo == null ? "" : _veiculoTemp.ArquivoAnexo;

                bool _controlado1, _controlado2, _controlado3, _controlado4;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente31")[i].InnerText, out _controlado1);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente32")[i].InnerText, out _controlado2);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente33")[i].InnerText, out _controlado3);
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Pendente34")[i].InnerText, out _controlado4);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_Veiculo.VeiculoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Veiculos Set" +
                        " Descricao= @v1" +
                        ",Tipo= @v2" +
                        ",Marca= @v3" +
                        ",Modelo= @v4" +
                        ",Ano= @v5" +
                        ",Cor= @v6" +
                        ",Placa= @v7" +
                        ",Renavam= @v8" +
                        ",EstadoID= @v9" +
                        ",MunicipioID= @v10" +
                        ",Foto= @v11" +
                        ",Excluida= @v12" +
                        ",StatusID= @v13" +
                        ",TipoAcessoID= @v14" +
                        ",NomeArquivoAnexo= @v15" +
                        ",DescricaoAnexo= @v16" +
                        ",ArquivoAnexo= @v17" +
                        ",Pendente31= @v18" +
                        ",Pendente32= @v19" +
                        ",Pendente33= @v20" +
                        ",Pendente34= @v21" +
                        " Where VeiculoID = @v0", _Con);

                    _sqlCmd.Parameters.Add("@v0", SqlDbType.Int).Value = _Veiculo.VeiculoID;
                    _sqlCmd.Parameters.Add("@v1", SqlDbType.VarChar).Value = _Veiculo.Descricao;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = _Veiculo.Tipo;
                    _sqlCmd.Parameters.Add("@v3", SqlDbType.VarChar).Value = _Veiculo.Marca;
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.VarChar).Value = _Veiculo.Modelo;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.VarChar).Value = _Veiculo.Ano;
                    _sqlCmd.Parameters.Add("@v6", SqlDbType.VarChar).Value = _Veiculo.Cor;
                    _sqlCmd.Parameters.Add("@v7", SqlDbType.VarChar).Value = _Veiculo.Placa;
                    _sqlCmd.Parameters.Add("@v8", SqlDbType.VarChar).Value = _Veiculo.Renavam;
                    _sqlCmd.Parameters.Add("@v9", SqlDbType.Int).Value = _Veiculo.EstadoID;
                    _sqlCmd.Parameters.Add("@v10", SqlDbType.Int).Value = _Veiculo.MunicipioID;
                    _sqlCmd.Parameters.Add("@v11", SqlDbType.VarChar).Value = _Veiculo.Foto;
                    _sqlCmd.Parameters.Add("@v12", SqlDbType.Int).Value = _Veiculo.Excluida;
                    _sqlCmd.Parameters.Add("@v13", SqlDbType.Int).Value = _Veiculo.StatusID;
                    _sqlCmd.Parameters.Add("@v14", SqlDbType.Int).Value = _Veiculo.TipoAcessoID;
                    _sqlCmd.Parameters.Add("@v15", SqlDbType.VarChar).Value = _Veiculo.NomeArquivoAnexo;
                    _sqlCmd.Parameters.Add("@v16", SqlDbType.VarChar).Value = _Veiculo.DescricaoAnexo;
                    _sqlCmd.Parameters.Add("@v17", SqlDbType.VarChar).Value = _Veiculo.ArquivoAnexo;
                    _sqlCmd.Parameters.Add("@v18", SqlDbType.Bit).Value = _controlado1;
                    _sqlCmd.Parameters.Add("@v19", SqlDbType.Bit).Value = _controlado2;
                    _sqlCmd.Parameters.Add("@v20", SqlDbType.Bit).Value = _controlado3;
                    _sqlCmd.Parameters.Add("@v21", SqlDbType.Bit).Value = _controlado4;

                    _sqlCmd.ExecuteNonQuery();
                    _Con.Close();
                }
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void AtualizaVeiculoBD ex: " + ex);


            }
        }


        private void ExcluiVeiculoBD(int _VeiculoID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Veiculos where VeiculoID=" + _VeiculoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);


            }
        }

        private string BuscaFoto(int veiculoID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Veiculos Where VeiculoID = " + veiculoID, _Con);
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


        private void AtualizaPendencias(int _VeiculoID)
        {
            try
            {

                if (_VeiculoID == 0)
                {
                    return;
                }

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlCmd;
                for (int i = 31; i < 35; i++)
                {
                    _sqlCmd = new SqlCommand("Insert into Pendencias (TipoPendenciaID,Descricao,DataLimite ,Impeditivo,VeiculoID) values (" +
                                                      "@v1,@v2, @v3,@v4,@v5)", _Con);

                    _sqlCmd.Parameters.Add("@v1", SqlDbType.Int).Value = i;
                    _sqlCmd.Parameters.Add("@v2", SqlDbType.VarChar).Value = "Cadastro novo!";
                    _sqlCmd.Parameters.Add("@v3", SqlDbType.DateTime).Value = DateTime.Now;
                    _sqlCmd.Parameters.Add("@v4", SqlDbType.Bit).Value = 1;
                    _sqlCmd.Parameters.Add("@v5", SqlDbType.Int).Value = _VeiculoID;
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

        private string CriaXmlImagem(int veiculoID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Veiculos Where VeiculoID = " + veiculoID , _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["ArquivoAnexo"].ToString())));
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

        internal void SalvarAdicao()
        {
            try
            {

                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculos));

                ObservableCollection<ClasseVeiculos.Veiculo> _VeiculosPro = new ObservableCollection<ClasseVeiculos.Veiculo>();
                ClasseVeiculos _ClasseVeiculosTemp = new ClasseVeiculos();
                VeiculoSelecionado.Pendente = true;
                VeiculoSelecionado.Pendente31 = true;
                VeiculoSelecionado.Pendente32 = true;
                VeiculoSelecionado.Pendente33 = true;
                VeiculoSelecionado.Pendente34 = true;

                _VeiculosPro.Add(VeiculoSelecionado);
                _ClasseVeiculosTemp.Veiculos = _VeiculosPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculosTemp);
                        xmlString = sw.ToString();
                    }

                }

                int _novoVeiculoID = InsereVeiculosBD(xmlString);

                AtualizaPendencias(_novoVeiculoID);

                VeiculoSelecionado.VeiculoID = _novoVeiculoID;

                _VeiculosTemp.Clear();

                _VeiculosTemp.Add(VeiculoSelecionado);
                Veiculos = null;
                Veiculos = new ObservableCollection<ClasseVeiculos.Veiculo>(_VeiculosTemp);
                SelectedIndex = 0;
                _VeiculosTemp.Clear();
                _VeiculosPro.Clear();
                _veiculoTemp = null;
                _ClasseVeiculosTemp = null;

                //this._VeiculosTemp.Clear();
                //_veiculoTemp = null;


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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculos));

                ObservableCollection<ClasseVeiculos.Veiculo> _VeiculosTemp = new ObservableCollection<ClasseVeiculos.Veiculo>();
                ClasseVeiculos _ClasseVeiculosTemp = new ClasseVeiculos();
                _VeiculosTemp.Add(VeiculoSelecionado);
                _ClasseVeiculosTemp.Veiculos = _VeiculosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculosTemp);
                        xmlString = sw.ToString();
                    }

                }

                AtualizaVeiculosBD(xmlString);

                _ClasseVeiculosTemp = null;

                this._VeiculosTemp.Clear();
                _veiculoTemp = null;


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

        #endregion

        #region Testes

        #endregion
    }
}
