using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Views.Model;
using iModSCCredenciamento.Windows;
using EstadoView = iModSCCredenciamento.Views.Model.EstadoView;

namespace iModSCCredenciamento.ViewModels
{
   public class EmpresasVeiculosViewModel : ViewModelBase
    {
        #region Inicializacao
        public EmpresasVeiculosViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
            //CarregaUI();
        }
        private void CarregaUI()
        {
            CarregaColeçãoFormatosCredenciais();
            CarregaColeçãoEstados();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo> _EmpresasVeiculos;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<EstadoView> _Estados;

        private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<EmpresaLayoutCrachaView> _EmpresasLayoutsCrachas;

        private ClasseEmpresasVeiculos.EmpresaVeiculo _EmpresaVeiculoSelecionado;

        private ClasseEmpresasVeiculos.EmpresaVeiculo _EmpresaVeiculoTemp = new ClasseEmpresasVeiculos.EmpresaVeiculo();

        private List<ClasseEmpresasVeiculos.EmpresaVeiculo> _EmpresasVeiculosTemp = new List<ClasseEmpresasVeiculos.EmpresaVeiculo>();

        PopupPesquisaEmpresasVeiculos popupPesquisaEmpresasVeiculos;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo> EmpresasVeiculos
        {
            get
            {
                return _EmpresasVeiculos;
            }

            set
            {
                if (_EmpresasVeiculos != value)
                {
                    _EmpresasVeiculos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseVinculos.Vinculo> Vinculos
        {
            get
            {
                return _Vinculos;
            }

            set
            {
                if (_Vinculos != value)
                {
                    _Vinculos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<EstadoView> Estados
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

        public ObservableCollection<EmpresaLayoutCrachaView> EmpresasLayoutsCrachas
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

        public ClasseEmpresasVeiculos.EmpresaVeiculo EmpresaVeiculoSelecionado
        {
            get
            {
                return _EmpresaVeiculoSelecionado;
            }
            set
            {
                _EmpresaVeiculoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (EmpresaVeiculoSelecionado != null)
                {
                    CarregaColeçãoEmpresasLayoutsCrachas(Convert.ToInt32(EmpresaVeiculoSelecionado.EmpresaID));
                }

            }
        }

        public ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> FormatosCredenciais

        {
            get
            {
                return _FormatosCredenciais;
            }

            set
            {
                if (_FormatosCredenciais != value)
                {
                    _FormatosCredenciais = value;
                    OnPropertyChanged();

                }
            }
        }

        public int EmpresaSelecionadaID
        {
            get
            {
                return _EmpresaSelecionadaID;

            }
            set
            {
                _EmpresaSelecionadaID = value;
                base.OnPropertyChanged();
                if (EmpresaSelecionadaID != null)
                {
                    //OnEmpresaSelecionada();
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
                return _HabilitaEdicao;
            }
            set
            {
                _HabilitaEdicao = value;
                base.OnPropertyChanged();
            }
        }

        public string Criterios
        {
            get
            {
                return _Criterios;
            }
            set
            {
                _Criterios = value;
                base.OnPropertyChanged();
            }
        }
        #endregion

        #region Comandos dos Botoes
        public void OnAtualizaCommand(object _EmpresaID)
        {
            try
            {
                EmpresaSelecionadaID = Convert.ToInt32(_EmpresaID);
                Thread CarregaColecaoEmpresasVeiculos_thr = new Thread(() => CarregaColecaoEmpresasVeiculos(Convert.ToInt32(_EmpresaID)));
                CarregaColecaoEmpresasVeiculos_thr.Start();
                //CarregaColecaoEmpresasVeiculos(Convert.ToInt32(_EmpresaID));

            }
            catch (Exception ex)
            {

            }

        }


        public void OnEditarCommand()
        {
            try
            {
                _EmpresaVeiculoTemp = EmpresaVeiculoSelecionado.CriaCopia(EmpresaVeiculoSelecionado);
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
                EmpresasVeiculos[_selectedIndexTemp] = _EmpresaVeiculoTemp;
                SelectedIndex = _selectedIndexTemp;
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {

            }
        }

        public void OnSalvarEdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                XmlSerializer serializer = new XmlSerializer(typeof(ClasseEmpresasVeiculos));

                ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo> _EmpresaVeiculoTemp = new ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo>();
                ClasseEmpresasVeiculos _ClasseEmpresasVeiculosTemp = new ClasseEmpresasVeiculos();
                _EmpresaVeiculoTemp.Add(EmpresaVeiculoSelecionado);
                _ClasseEmpresasVeiculosTemp.EmpresasVeiculos = _EmpresaVeiculoTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasVeiculosTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereEmpresaVeiculoBD(xmlString);

                //_ClasseEmpresasSegurosTemp = null;

                //_SegurosTemp.Clear();
                //_seguroTemp = null;


            }
            catch (Exception ex)
            {
            }
        }
        Global g = new Global();
        public void OnAdicionarCommand()
        {
            //if (g.iniciarFiljos = true)
            //    {
            //    return;
            //    }
                
            try
            {
                foreach (var x in EmpresasVeiculos)
                {
                    _EmpresasVeiculosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                EmpresasVeiculos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _EmpresaVeiculoTemp = new ClasseEmpresasVeiculos.EmpresaVeiculo();
                _EmpresaVeiculoTemp.EmpresaID = EmpresaSelecionadaID;
                EmpresasVeiculos.Add(_EmpresaVeiculoTemp);
                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {

            }

        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                XmlSerializer serializer = new XmlSerializer(typeof(ClasseEmpresasVeiculos));

                ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo> _EmpresaVeiculoPro = new ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo>();
                ClasseEmpresasVeiculos _ClasseEmpresasVeiculosPro = new ClasseEmpresasVeiculos();
                _EmpresaVeiculoPro.Add(EmpresaVeiculoSelecionado);
                _ClasseEmpresasVeiculosPro.EmpresasVeiculos = _EmpresaVeiculoPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasVeiculosPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereEmpresaVeiculoBD(xmlString);
                Thread CarregaColecaoEmpresasVeiculos_thr = new Thread(() => CarregaColecaoEmpresasVeiculos(EmpresaVeiculoSelecionado.EmpresaID));
                CarregaColecaoEmpresasVeiculos_thr.Start();
                _EmpresasVeiculosTemp.Add(EmpresaVeiculoSelecionado);
                EmpresasVeiculos = null;
                EmpresasVeiculos = new ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo>(_EmpresasVeiculosTemp);
                SelectedIndex = _selectedIndexTemp;
                _EmpresasVeiculosTemp.Clear();
                //_ClasseEmpresasSegurosTemp = null;

                //_SegurosTemp.Clear();
                //_seguroTemp = null;


            }
            catch (Exception ex)
            {
            }
        }
        public void OnCancelarAdicaoCommand()
        {
            try
            {
                EmpresasVeiculos = null;
                EmpresasVeiculos = new ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo>(_EmpresasVeiculosTemp);
                SelectedIndex = _selectedIndexTemp;
                _EmpresasVeiculosTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand()
        {
            try
            {
                //if (MessageBox.Show("Tem certeza que deseja excluir este veículo?", "Excluir Veículo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste veículo, inclusive histórico. Confirma exclusão?", "Excluir Veículo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        //ExcluiSeguroBD(EmpresaVeiculoSelecionado.ColaboradorID);
                //        EmpresasVeiculos.Remove(EmpresaVeiculoSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        EmpresasVeiculos.Remove(EmpresaVeiculoSelecionado);
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
                popupPesquisaEmpresasVeiculos = new PopupPesquisaEmpresasVeiculos();
                popupPesquisaEmpresasVeiculos.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaEmpresasVeiculos.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            try
            {
                object vetor = popupPesquisaEmpresasVeiculos.Criterio.Split((char)(20));
                int _empresaID = EmpresaSelecionadaID;
                string _placa = ((string[])vetor)[0];
                string _marca = ((string[])vetor)[1];
                string _modelo = ((string[])vetor)[2];
                int _ativo = Convert.ToInt32(((string[])vetor)[3]);

                CarregaColecaoEmpresasVeiculos(_empresaID, _modelo, _marca, _placa, _ativo);
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoEmpresasVeiculos(int _empresaID, string _placa = "", string _marca = "", string _modelo = "", int _ativo = 2)
        {
            try
            {
                string _xml = RequisitaEmpresasVeiculos(_empresaID, _placa, _marca, _modelo, _ativo);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasVeiculos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasVeiculos classeEmpresasVeiculos = new ClasseEmpresasVeiculos();
                classeEmpresasVeiculos = (ClasseEmpresasVeiculos)deserializer.Deserialize(reader);
                EmpresasVeiculos = new ObservableCollection<ClasseEmpresasVeiculos.EmpresaVeiculo>();
                EmpresasVeiculos = classeEmpresasVeiculos.EmpresasVeiculos;
                SelectedIndex = -1;
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

                //string _xml = RequisitaEstados();

                //XmlSerializer deserializer = new XmlSerializer(typeof(EstadoView));
                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);
                //TextReader reader = new StringReader(_xml);
                //EstadoView classeEstados = new EstadoView();
                //classeEstados = (EstadoView)deserializer.Deserialize(reader);
                //Estados = new ObservableCollection<EstadoView>();
                //Estados = EstadoViews;

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
        public void CarregaColeçãoEmpresasLayoutsCrachas(int _empresaID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaEmpresasLayoutsCrachas(_empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(EmpresaLayoutCrachaView));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                EmpresaLayoutCrachaView classeEmpresasLayoutsCrachas = new EmpresaLayoutCrachaView();
                classeEmpresasLayoutsCrachas = (EmpresaLayoutCrachaView)deserializer.Deserialize(reader);
                EmpresasLayoutsCrachas = new ObservableCollection<EmpresaLayoutCrachaView>();
               // EmpresasLayoutsCrachas = classeEmpresasLayoutsCrachas.EmpresasLayoutsCrachas;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        public void CarregaColeçãoFormatosCredenciais()
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaFormatosCredenciais();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseFormatosCredenciais));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseFormatosCredenciais classeFormatosCredenciais = new ClasseFormatosCredenciais();
                classeFormatosCredenciais = (ClasseFormatosCredenciais)deserializer.Deserialize(reader);
                FormatosCredenciais = new ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial>();
                FormatosCredenciais = classeFormatosCredenciais.FormatosCredenciais;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        public void CarregaColeçãoVinculos(int _EmpresaVeiculoID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaVinculos(_EmpresaVeiculoID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVinculos));
                XmlDocument DataFile = new XmlDocument();
                DataFile.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseVinculos classeVinculos = new ClasseVinculos();
                classeVinculos = (ClasseVinculos)deserializer.Deserialize(reader);
                Vinculos = new ObservableCollection<ClasseVinculos.Vinculo>();
                Vinculos = classeVinculos.Vinculos;

                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = false; }));
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoMunicipios ex: " + ex);
            }
        }

        #endregion

        #region Data Access
        private string RequisitaEmpresasVeiculos(int _empresaID, string _placa = "", string _marca = "", string _modelo = "", int _ativo = 2)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasVeiculos = _xmlDocument.CreateElement("ClasseEmpresasVeiculos");
                _xmlDocument.AppendChild(_ClasseEmpresasVeiculos);

                XmlNode _EmpresasVeiculos = _xmlDocument.CreateElement("EmpresasVeiculos");
                _ClasseEmpresasVeiculos.AppendChild(_EmpresasVeiculos);

                string _strSql;

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _placa = _placa == "" ? "" : " AND Placa like '%" + _placa + "%' ";
                _marca = _marca == "" ? "" : " AND Marca like '%" + _marca + "%' ";
                _modelo = _modelo == "" ? "" : " AND Modelo like '%" + _modelo + "%'";
                string _ativoStr = _ativo == 2 ? "" : " AND dbo.EmpresasVeiculos.Ativo = " + _ativo;

                _strSql = "SELECT dbo.EmpresasVeiculos.*, dbo.LayoutsCrachas.Nome FROM dbo.EmpresasVeiculos LEFT OUTER JOIN" +
                    " dbo.LayoutsCrachas ON dbo.EmpresasVeiculos.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID" +
                    "  WHERE EmpresaID =  " + _empresaID + _ativoStr + _placa + _marca + _modelo + " order by EmpresaVeiculoID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {


                    XmlNode _EmpresaVeiculo = _xmlDocument.CreateElement("EmpresaVeiculo");
                    _EmpresasVeiculos.AppendChild(_EmpresaVeiculo);

                    XmlNode _EmpresaVeiculoID = _xmlDocument.CreateElement("EmpresaVeiculoID");
                    _EmpresaVeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaVeiculoID"].ToString())));
                    _EmpresaVeiculo.AppendChild(_EmpresaVeiculoID);

                    XmlNode _Node1 = _xmlDocument.CreateElement("Validade");
                    _Node1.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node1);

                    XmlNode _Node2 = _xmlDocument.CreateElement("Descricao");
                    _Node2.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node2);

                    XmlNode _Node3 = _xmlDocument.CreateElement("Tipo");
                    _Node3.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Tipo"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node3);

                    XmlNode _Node4 = _xmlDocument.CreateElement("Marca");
                    _Node4.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Marca"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node4);

                    XmlNode _Node5 = _xmlDocument.CreateElement("Modelo");
                    _Node5.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Modelo"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node5);

                    XmlNode _Node6 = _xmlDocument.CreateElement("Ano");
                    _Node6.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Ano"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node6);

                    XmlNode _Node7 = _xmlDocument.CreateElement("Cor");
                    _Node7.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cor"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node7);

                    XmlNode _Node8 = _xmlDocument.CreateElement("Placa");
                    _Node8.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Placa"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node8);

                    XmlNode _Node9 = _xmlDocument.CreateElement("Renavam");
                    _Node9.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Renavam"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node9);

                    XmlNode _Node10 = _xmlDocument.CreateElement("EstadoID");
                    _Node10.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node10);

                    XmlNode _Node11 = _xmlDocument.CreateElement("MunicipioID");
                    _Node11.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node11);

                    XmlNode _Node12 = _xmlDocument.CreateElement("Seguro");
                    _Node12.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Seguro"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node12);

                    XmlNode _Node13 = _xmlDocument.CreateElement("EmpresaID");
                    _Node13.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node13);

                    XmlNode _Node14 = _xmlDocument.CreateElement("Ativo");
                    _Node14.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    _EmpresaVeiculo.AppendChild(_Node14);

                    XmlNode _Node15 = _xmlDocument.CreateElement("LayoutCrachaID");
                    _Node15.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["LayoutCrachaID"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node15);

                    XmlNode _Node16 = _xmlDocument.CreateElement("LayoutCrachaNome");
                    _Node16.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node16);

                    XmlNode _Node17 = _xmlDocument.CreateElement("FormatoCredencialID");
                    _Node17.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FormatoCredencialID"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node17);

                    XmlNode _Node18 = _xmlDocument.CreateElement("NumeroCredencial");
                    _Node18.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroCredencial"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node18);

                    XmlNode _Node19 = _xmlDocument.CreateElement("FC");
                    _Node19.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["FC"].ToString())));
                    _EmpresaVeiculo.AppendChild(_Node19);

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
                    " dbo.LayoutsCrachas.Nome, dbo.LayoutsCrachas.LayoutCrachaGUID FROM dbo.EmpresasLayoutsCrachas INNER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.EmpresasLayoutsCrachas.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID WHERE(dbo.EmpresasLayoutsCrachas.EmpresaID = " + _empresaID + ")";
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

                    XmlNode _LayoutCrachaGUID = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _LayoutCrachaGUID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_LayoutCrachaGUID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Nome");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_Descricao);

                    XmlNode _LayoutCrachaID = _xmlDocument.CreateElement("LayoutCrachaID");
                    _LayoutCrachaID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaID"].ToString())));
                    _EmpresaLayoutCracha.AppendChild(_LayoutCrachaID);
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

        private string RequisitaFormatosCredenciais(int _empresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseFormatosCredenciais = _xmlDocument.CreateElement("ClasseFormatosCredenciais");
                _xmlDocument.AppendChild(_ClasseFormatosCredenciais);

                XmlNode _FormatosCredenciais = _xmlDocument.CreateElement("FormatosCredenciais");
                _ClasseFormatosCredenciais.AppendChild(_FormatosCredenciais);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                string _SQL = "select * from FormatosCredenciais";
                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _FormatoCredencial = _xmlDocument.CreateElement("FormatoCredencial");
                    _FormatosCredenciais.AppendChild(_FormatoCredencial);

                    XmlNode _FormatoCredencialID = _xmlDocument.CreateElement("FormatoCredencialID");
                    _FormatoCredencialID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FormatoCredencialID"].ToString())));
                    _FormatoCredencial.AppendChild(_FormatoCredencialID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString().Trim())));
                    _FormatoCredencial.AppendChild(_Descricao);
                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaFormatosCredenciais ex: " + ex);
                
                return null;
            }
        }

        private string RequisitaVinculos(int _EmpresaVeiculoID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVinculos = _xmlDocument.CreateElement("ClasseVinculos");
                _xmlDocument.AppendChild(_ClasseVinculos);

                XmlNode _Vinculos = _xmlDocument.CreateElement("Vinculos");
                _ClasseVinculos.AppendChild(_Vinculos);

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                string _SQL = "SELECT dbo.EmpresasVeiculos.EmpresaVeiculoID, dbo.EmpresasVeiculos.Validade, dbo.EmpresasVeiculos.Placa, dbo.LayoutsCrachas.LayoutCrachaGUID," +
                    " dbo.FormatosCredenciais.Descricao, dbo.Empresas.CNPJ, dbo.Empresas.Nome, dbo.EmpresasVeiculos.Renavam, dbo.EmpresasVeiculos.NumeroCredencial, dbo.EmpresasVeiculos.FC" +
                    " FROM dbo.EmpresasVeiculos INNER JOIN dbo.Empresas ON dbo.EmpresasVeiculos.EmpresaID = dbo.Empresas.EmpresaID" +
                    " INNER JOIN dbo.FormatosCredenciais ON dbo.EmpresasVeiculos.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN " +
                    "dbo.LayoutsCrachas ON dbo.EmpresasVeiculos.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID WHERE(dbo.EmpresasVeiculos.EmpresaVeiculoID = " + _EmpresaVeiculoID + ")";


                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Vinculo = _xmlDocument.CreateElement("Vinculo");
                    _Vinculos.AppendChild(_Vinculo);

                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Renavam"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo9);

                    XmlNode _Vinculo10 = _xmlDocument.CreateElement("ColaboradorNome");
                    _Vinculo10.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Placa"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo10);

                    XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaNome");
                    _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Nome"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo12);

                    XmlNode _Vinculo15 = _xmlDocument.CreateElement("LayoutCrachaGUID");
                    _Vinculo15.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["LayoutCrachaGUID"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo15);

                    XmlNode _Vinculo16 = _xmlDocument.CreateElement("NumeroCredencial");
                    _Vinculo16.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["NumeroCredencial"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo16);

                    XmlNode _Vinculo17 = _xmlDocument.CreateElement("FC");
                    _Vinculo17.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FC"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo17);

                    XmlNode _Vinculo18 = _xmlDocument.CreateElement("Validade");
                    _Vinculo18.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Validade"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo18);

                    XmlNode _Vinculo19 = _xmlDocument.CreateElement("CNPJ");
                    _Vinculo19.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CNPJ"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo19);

                    XmlNode _Vinculo20 = _xmlDocument.CreateElement("FormatoCredencial");
                    _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo20);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaVinculos ex: " + ex);
                
                return null;
            }
        }

        private void InsereEmpresaVeiculoBD(string xmlString)
        {
            try
            {


                XmlDocument _xmlDoc = new XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseEmpresasVeiculos.EmpresaVeiculo _empresaVeiculo = new ClasseEmpresasVeiculos.EmpresaVeiculo();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresaVeiculo.EmpresaVeiculoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaVeiculoID")[i].InnerText);
                _empresaVeiculo.Validade = _xmlDoc.GetElementsByTagName("Validade")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Validade")[i].InnerText;
                _empresaVeiculo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
                _empresaVeiculo.Tipo = _xmlDoc.GetElementsByTagName("Tipo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Tipo")[i].InnerText;
                _empresaVeiculo.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
                _empresaVeiculo.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
                _empresaVeiculo.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
                _empresaVeiculo.Cor = _xmlDoc.GetElementsByTagName("Cor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cor")[i].InnerText;
                _empresaVeiculo.Placa = _xmlDoc.GetElementsByTagName("Placa")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Placa")[i].InnerText;
                _empresaVeiculo.Renavam = _xmlDoc.GetElementsByTagName("Renavam")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Renavam")[i].InnerText;
                _empresaVeiculo.EstadoID = _xmlDoc.GetElementsByTagName("EstadoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EstadoID")[i].InnerText);
                _empresaVeiculo.MunicipioID = _xmlDoc.GetElementsByTagName("MunicipioID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("MunicipioID")[i].InnerText);
                _empresaVeiculo.Seguro = _xmlDoc.GetElementsByTagName("Seguro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Seguro")[i].InnerText;
                _empresaVeiculo.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _empresaVeiculo.LayoutCrachaID = _xmlDoc.GetElementsByTagName("LayoutCrachaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("LayoutCrachaID")[i].InnerText);
                _empresaVeiculo.FormatoCredencialID = _xmlDoc.GetElementsByTagName("FormatoCredencialID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("FormatoCredencialID")[i].InnerText);
                _empresaVeiculo.NumeroCredencial = _xmlDoc.GetElementsByTagName("NumeroCredencial")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroCredencial")[i].InnerText;
                _empresaVeiculo.FC = _xmlDoc.GetElementsByTagName("FC")[i] == null ? "" : _xmlDoc.GetElementsByTagName("FC")[i].InnerText;
                bool _ativo;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativo")[i].InnerText, out _ativo);
                _empresaVeiculo.Ativo = _xmlDoc.GetElementsByTagName("Ativo")[i] == null ? false : _ativo;


                
                //_Con.Close();
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                if (_empresaVeiculo.EmpresaVeiculoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update EmpresasVeiculos Set" +
                        " Validade= '" + _empresaVeiculo.Validade + "'" +
                        ",Descricao= '" + _empresaVeiculo.Descricao + "'" +
                        ",Tipo= '" + _empresaVeiculo.Tipo + "'" +
                        ",Marca= '" + _empresaVeiculo.Marca + "'" +
                        ",Modelo= '" + _empresaVeiculo.Modelo + "'" +
                        ",Ano= '" + _empresaVeiculo.Ano + "'" +
                        ",Cor= '" + _empresaVeiculo.Cor + "'" +
                        ",Placa= '" + _empresaVeiculo.Placa + "'" +
                        ",Renavam= '" + _empresaVeiculo.Renavam + "'" +
                        ",EstadoID= " + _empresaVeiculo.EstadoID + "" +
                        ",MunicipioID= " + _empresaVeiculo.MunicipioID + "" +
                        ",Seguro= '" + _empresaVeiculo.Seguro + "'" +
                        ",EmpresaID= " + _empresaVeiculo.EmpresaID + "" +
                        ",LayoutCrachaID= " + _empresaVeiculo.LayoutCrachaID +
                        ",FormatoCredencialID= " + _empresaVeiculo.FormatoCredencialID + "" +
                        ",NumeroCredencial= '" + _empresaVeiculo.NumeroCredencial + "'" +
                        ",FC= '" + _empresaVeiculo.FC + "'" +
                        ",Ativo= '" + _empresaVeiculo.Ativo +
                        "' Where EmpresaVeiculoID = " + _empresaVeiculo.EmpresaVeiculoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into EmpresasVeiculos(Validade, Descricao, Tipo, Marca, Modelo, Ano, Cor, Placa" +
                                             ",Renavam,EstadoID,MunicipioID,Seguro,EmpresaID,Ativo,LayoutCrachaID,FormatoCredencialID,NumeroCredencial,FC) values ('" +
                                             _empresaVeiculo.Validade + "','" + _empresaVeiculo.Descricao + "','" + _empresaVeiculo.Tipo + "','" + _empresaVeiculo.Marca + "','"
                                             + _empresaVeiculo.Modelo + "','" + _empresaVeiculo.Ano + "','" + _empresaVeiculo.Cor + "','" + _empresaVeiculo.Placa +
                                             "','" + _empresaVeiculo.Renavam + "'," + _empresaVeiculo.EstadoID + "," + _empresaVeiculo.MunicipioID + ",'" + _empresaVeiculo.Seguro +
                                             "'," + _empresaVeiculo.EmpresaID + ",'" + _empresaVeiculo.Ativo + "'," + _empresaVeiculo.LayoutCrachaID + "," + _empresaVeiculo.FormatoCredencialID + ",'" +
                                                          _empresaVeiculo.NumeroCredencial + "','" + _empresaVeiculo.FC + "')", _Con);


                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresaVeiculoBD ex: " + ex);
                

            }
        }

        private void ExcluiEmpresaVeiculoBD(int _colaboradorID) // alterar para xml
        {
            try
            {

                
                //_Con.Close();
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasVeiculos where ColaboradorID=" + _colaboradorID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiEmpresaVeiculoBD ex: " + ex);
                

            }
        }
        #endregion

        #region Metodos Privados
        public void OnVincularCommand(string _credencialInfo)
        {
            try
            {
                //CarregaColeçãoVinculos(EmpresaVeiculoSelecionado.EmpresaVeiculoID);

                //Bitmap _Foto;
                //if (Vinculos[0].ColaboradorFoto != null)
                //{
                //    BitmapImage _img = Conversores.STRtoIMG(Vinculos[0].ColaboradorFoto) as BitmapImage;
                //    _Foto = Conversores.BitmapImageToBitmap(_img);
                //}
                //else
                //{
                //    _Foto = null;
                //}


                //bool _resposta = SCManager.Vincular(Vinculos[0].ColaboradorNome, Vinculos[0].CPF, Vinculos[0].CNPJ, Vinculos[0].EmpresaNome, Vinculos[0].Matricula,
                //                                          Vinculos[0].Cargo, Vinculos[0].FC, Vinculos[0].NumeroCredencial, Vinculos[0].FormatoCredencial,
                //                                          Vinculos[0].Validade, Vinculos[0].LayoutCrachaGUID, _Foto);
                //if (_resposta)
                //{
                //    MessageBox.Show("Vinculo Efetuado com Sucesso!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
            catch (Exception ex)
            {
            }
        }
        #endregion



    }
}
