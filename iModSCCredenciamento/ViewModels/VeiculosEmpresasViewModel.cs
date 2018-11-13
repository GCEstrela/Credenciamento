using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using iModSCCredenciamento.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
namespace iModSCCredenciamento.ViewModels
{
    public class VeiculosEmpresasViewModel : ViewModelBase
    {
        #region Inicializacao
        public VeiculosEmpresasViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

        }

        private void CarregaUI()
        {
            CarregaColecaoEmpresas();
            CarregaColeçãoFormatosCredenciais();

        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> _VeiculosEmpresas;

        private ObservableCollection<ClasseVinculos.Vinculo> _Vinculos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;

        private ObservableCollection<ClasseFormatosCredenciais.FormatoCredencial> _FormatosCredenciais;

        private ObservableCollection<ClasseEmpresasLayoutsCrachas.EmpresaLayoutCracha> _EmpresasLayoutsCrachas;

        private ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> _Contratos;

        private ClasseVeiculosEmpresas.VeiculoEmpresa _VeiculoEmpresaSelecionado;

        private ClasseVeiculosEmpresas.VeiculoEmpresa _VeiculoEmpresaTemp = new ClasseVeiculosEmpresas.VeiculoEmpresa();

        private List<ClasseVeiculosEmpresas.VeiculoEmpresa> _VeiculosEmpresasTemp = new List<ClasseVeiculosEmpresas.VeiculoEmpresa>();

        PopupPesquisaVeiculos popupPesquisaVeiculos;

        private int _selectedIndex;

        private int _VeiculoSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        private string _Validade;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> VeiculosEmpresas
        {
            get
            {
                return _VeiculosEmpresas;
            }

            set
            {
                if (_VeiculosEmpresas != value)
                {
                    _VeiculosEmpresas = value;
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

        public ClasseVeiculosEmpresas.VeiculoEmpresa VeiculoEmpresaSelecionado
        {
            get
            {
                return this._VeiculoEmpresaSelecionado;
            }
            set
            {
                this._VeiculoEmpresaSelecionado = value;
                //base.OnPropertyChanged("SelectedItem");
                base.OnPropertyChanged();
                if (VeiculoEmpresaSelecionado != null)
                {
                    CarregaColeçãoEmpresasLayoutsCrachas(Convert.ToInt32(VeiculoEmpresaSelecionado.EmpresaID));
                }

            }
        }

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
        public ObservableCollection<ClasseEmpresasContratos.EmpresaContrato> Contratos
        {
            get
            {
                return _Contratos;
            }

            set
            {
                if (_Contratos != value)
                {
                    _Contratos = value;
                    OnPropertyChanged();

                }
            }
        }
        public int VeiculoSelecionadaID
        {
            get
            {
                return this._VeiculoSelecionadaID;

            }
            set
            {
                this._VeiculoSelecionadaID = value;
                base.OnPropertyChanged();
                if (VeiculoSelecionadaID != null)
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
                return this._HabilitaEdicao;
            }
            set
            {
                this._HabilitaEdicao = value;
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

        public string Validade
        {
            get
            {
                return this._Validade;
            }
            set
            {
                this._Validade = value;
                base.OnPropertyChanged();
            }
        }

        //public string ComboEmpresaSelecionado
        //{
        //    get
        //    {
        //        return this._ComboEmpresaSelecionado;
        //    }
        //    set
        //    {
        //        this._ComboEmpresaSelecionado = value;
        //        base.OnPropertyChanged();
        //    }
        //}
        #endregion

        #region Comandos dos Botoes

        public void OnAtualizaCommand(object _VeiculoID)
        {
            try
            {
                VeiculoSelecionadaID = Convert.ToInt32(_VeiculoID);
                Thread CarregaColecaoVeiculosEmpresas_thr = new Thread(() => CarregaColecaoVeiculosEmpresas(Convert.ToInt32(_VeiculoID)));
                CarregaColecaoVeiculosEmpresas_thr.Start();
                //CarregaColecaoVeiculosEmpresas(Convert.ToInt32(_VeiculoID));

            }
            catch (Exception ex)
            {

            }

        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                //System.Windows.Forms.OpenFileDialog _arquivoPDF = new System.Windows.Forms.OpenFileDialog();
                //string _sql;
                //string _nomecompletodoarquivo;
                //string _arquivoSTR;
                //_arquivoPDF.InitialDirectory = "c:\\\\";
                //_arquivoPDF.Filter = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                //_arquivoPDF.RestoreDirectory = true;
                //_arquivoPDF.ShowDialog();
                ////if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                ////{
                //_nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                //_arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                //_VeiculoEmpresaTemp.Cargo = _nomecompletodoarquivo;
                //_VeiculoEmpresaTemp.Arquivo = _arquivoSTR;
                ////InsereArquivoBD(Convert.ToInt32(empresaID), _nomecompletodoarquivo, _arquivoSTR);

                ////AtualizaListaAnexos(_resp);

                ////}
            }
            catch (Exception ex)
            {

            }
        }

        public void OnEditarCommand()
        {
            try
            {

                _VeiculoEmpresaTemp = VeiculoEmpresaSelecionado.CriaCopia(VeiculoEmpresaSelecionado);
                _selectedIndexTemp = SelectedIndex;

                //CarregaColecaoContratos(_VeiculoEmpresaTemp.EmpresaID);

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
                VeiculosEmpresas[_selectedIndexTemp] = _VeiculoEmpresaTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosEmpresas));

                ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> _VeiculoEmpresaTemp = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                ClasseVeiculosEmpresas _ClasseVeiculoerEmpresasTemp = new ClasseVeiculosEmpresas();
                _VeiculoEmpresaTemp.Add(VeiculoEmpresaSelecionado);
                _ClasseVeiculoerEmpresasTemp.VeiculosEmpresas = _VeiculoEmpresaTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculoerEmpresasTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereVeiculoEmpresaBD(xmlString);

                //_ClasseEmpresasSegurosTemp = null;

                //_SegurosTemp.Clear();
                //_seguroTemp = null;


            }
            catch (Exception ex)
            {
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in VeiculosEmpresas)
                {
                    _VeiculosEmpresasTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                VeiculosEmpresas.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _VeiculoEmpresaTemp = new ClasseVeiculosEmpresas.VeiculoEmpresa();
                _VeiculoEmpresaTemp.VeiculoID = VeiculoSelecionadaID;
                VeiculosEmpresas.Add(_VeiculoEmpresaTemp);

                SelectedIndex = 0;
                HabilitaEdicao = true;
            }
            catch (Exception ex)
            {

            }

        }
        //public void OnSelecionaEmpresaCommand(int _empresaID)
        //{
        //    CarregaColecaoContratos(_empresaID);
        //}
        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosEmpresas));

                ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa> _VeiculoEmpresaPro = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                ClasseVeiculosEmpresas _ClasseVeiculoerEmpresasPro = new ClasseVeiculosEmpresas();
                _VeiculoEmpresaPro.Add(VeiculoEmpresaSelecionado);
                _ClasseVeiculoerEmpresasPro.VeiculosEmpresas = _VeiculoEmpresaPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculoerEmpresasPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereVeiculoEmpresaBD(xmlString);
                Thread CarregaColecaoVeiculosEmpresas_thr = new Thread(() => CarregaColecaoVeiculosEmpresas(VeiculoEmpresaSelecionado.VeiculoID));
                CarregaColecaoVeiculosEmpresas_thr.Start();
                _VeiculosEmpresasTemp.Add(VeiculoEmpresaSelecionado);
                VeiculosEmpresas = null;
                VeiculosEmpresas = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>(_VeiculosEmpresasTemp);
                SelectedIndex = _selectedIndexTemp;
                _VeiculosEmpresasTemp.Clear();



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
                VeiculosEmpresas = null;
                VeiculosEmpresas = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>(_VeiculosEmpresasTemp);
                SelectedIndex = _selectedIndexTemp;
                _VeiculosEmpresasTemp.Clear();
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
                //if (MessageBox.Show("Tem certeza que deseja excluir este vínculo?", "Excluir Vínculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados deste vínculo, inclusive histórico. Confirma exclusão?", "Excluir Vínculo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        //ExcluiSeguroBD(VeiculoEmpresaSelecionado.VeiculoID);

                //        ExcluiVeiculoEmpresaBD(VeiculoEmpresaSelecionado.VeiculoEmpresaID);
                //        VeiculosEmpresas.Remove(VeiculoEmpresaSelecionado);
                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiVeiculoEmpresaBD(VeiculoEmpresaSelecionado.VeiculoEmpresaID);
                        VeiculosEmpresas.Remove(VeiculoEmpresaSelecionado);
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
            try
            {
                object vetor = popupPesquisaVeiculos.Criterio.Split((char)(20));
                int _veiculoID = VeiculoSelecionadaID;
                string _matricula = ((string[])vetor)[0];
                string _empresaNome = ((string[])vetor)[1];
                string _cargo = ((string[])vetor)[2];
                int _ativo = Convert.ToInt32(((string[])vetor)[3]);

                CarregaColecaoVeiculosEmpresas(_veiculoID, _cargo, _empresaNome, _matricula, _ativo);
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoVeiculosEmpresas(int _veiculoID, string _empresaNome = "", string _cargo = "", string _matricula = "", int _ativo = 2)
        {
            try
            {
                string _xml = RequisitaVeiculosEmpresas(_veiculoID, _empresaNome, _cargo, _matricula, _ativo);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculosEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseVeiculosEmpresas classeVeiculosEmpresas = new ClasseVeiculosEmpresas();
                classeVeiculosEmpresas = (ClasseVeiculosEmpresas)deserializer.Deserialize(reader);
                VeiculosEmpresas = new ObservableCollection<ClasseVeiculosEmpresas.VeiculoEmpresa>();
                VeiculosEmpresas = classeVeiculosEmpresas.VeiculosEmpresas;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private void CarregaColecaoEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        {
            try
            {
                string _xml = RequisitaEmpresas();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresas classeEmpresas = new ClasseEmpresas();
                classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
                Empresas = new ObservableCollection<ClasseEmpresas.Empresa>();
                Empresas = classeEmpresas.Empresas;
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

        public void CarregaColeçãoVinculos(int _VeiculoEmpresaID = 0)
        {

            try
            {
                //this.Dispatcher.Invoke(new Action(() => { LoadingAdorner.IsAdornerVisible = true; }));

                string _xml = RequisitaVinculos(_VeiculoEmpresaID);

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

        public void CarregaColecaoContratos(int empresaID = 0)
        {

            try
            {

                string _xml = RequisitaContratos(empresaID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasContratos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasContratos classeContratosEmpresa = new ClasseEmpresasContratos();
                classeContratosEmpresa = (ClasseEmpresasContratos)deserializer.Deserialize(reader);
                Contratos = new ObservableCollection<ClasseEmpresasContratos.EmpresaContrato>();
                Contratos = classeContratosEmpresa.EmpresasContratos;
                SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaVeiculosEmpresas(int _veiculoID, string _empresaNome = "", string _cargo = "", string _matricula = "", int _ativo = 2)//Possibilidade de criar a pesquisa por Matriculatambem
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVeiculosEmpresas = _xmlDocument.CreateElement("ClasseVeiculosEmpresas");
                _xmlDocument.AppendChild(_ClasseVeiculosEmpresas);

                XmlNode _VeiculosEmpresas = _xmlDocument.CreateElement("VeiculosEmpresas");
                _ClasseVeiculosEmpresas.AppendChild(_VeiculosEmpresas);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _empresaNome = _empresaNome == "" ? "" : " AND Nome like '%" + _empresaNome + "%' ";
                _cargo = _cargo == "" ? "" : " AND Apelido like '%" + _cargo + "%' ";
                _matricula = _matricula == "" ? "" : " AND CPF like '%" + _matricula + "%'";
                string _ativoStr = _ativo == 2 ? "" : " AND dbo.VeiculosEmpresas.Ativo = " + _ativo;

                _strSql = "SELECT dbo.VeiculosEmpresas.Ativo, dbo.VeiculosEmpresas.VeiculoEmpresaID, dbo.VeiculosEmpresas.EmpresaContratoID," +
                    " dbo.VeiculosEmpresas.Cargo, dbo.VeiculosEmpresas.Matricula, dbo.VeiculosEmpresas.VeiculoID, dbo.Empresas.Nome, " +
                    "dbo.Empresas.EmpresaID, dbo.EmpresasContratos.Descricao" +
                    " FROM dbo.VeiculosEmpresas INNER JOIN dbo.Empresas ON dbo.VeiculosEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN" +
                    " dbo.EmpresasContratos ON dbo.VeiculosEmpresas.EmpresaContratoID = dbo.EmpresasContratos.EmpresaContratoID " +
                    "WHERE dbo.VeiculosEmpresas.VeiculoID =  " + _veiculoID + _ativoStr + _empresaNome + _cargo + _matricula;

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _VeiculoEmpresa = _xmlDocument.CreateElement("VeiculoEmpresa");
                    _VeiculosEmpresas.AppendChild(_VeiculoEmpresa);

                    XmlNode _VeiculoEmpresaID = _xmlDocument.CreateElement("VeiculoEmpresaID");
                    _VeiculoEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoEmpresaID"].ToString())));
                    _VeiculoEmpresa.AppendChild(_VeiculoEmpresaID);

                    XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    _VeiculoEmpresa.AppendChild(_VeiculoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _VeiculoEmpresa.AppendChild(_EmpresaID);

                    XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
                    _EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
                    _VeiculoEmpresa.AppendChild(_EmpresaContratoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _VeiculoEmpresa.AppendChild(_Descricao);

                    XmlNode _Empresa = _xmlDocument.CreateElement("EmpresaNome");
                    _Empresa.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _VeiculoEmpresa.AppendChild(_Empresa);

                    XmlNode _Cargo = _xmlDocument.CreateElement("Cargo");
                    _Cargo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cargo"].ToString())));
                    _VeiculoEmpresa.AppendChild(_Cargo);

                    XmlNode _Matricula = _xmlDocument.CreateElement("Matricula");
                    _Matricula.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Matricula"].ToString())));
                    _VeiculoEmpresa.AppendChild(_Matricula);

                    XmlNode _Ativo = _xmlDocument.CreateElement("Ativo");
                    _Ativo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    _VeiculoEmpresa.AppendChild(_Ativo);

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

        private string RequisitaEmpresas()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresas = _xmlDocument.CreateElement("ClasseEmpresas");
                _xmlDocument.AppendChild(_ClasseEmpresas);

                XmlNode _Empresas = _xmlDocument.CreateElement("Empresas");
                _ClasseEmpresas.AppendChild(_Empresas);

                string _strSql = " Select [EmpresaID],[Nome] from Empresas where Excluida=0";


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Empresa = _xmlDocument.CreateElement("Empresa");
                    _Empresas.AppendChild(_Empresa);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _Empresa.AppendChild(_EmpresaID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Empresa.AppendChild(_Nome);

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


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

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


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

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

        private string RequisitaVinculos(int _VeiculoEmpresaID = 0)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseVinculos = _xmlDocument.CreateElement("ClasseVinculos");
                _xmlDocument.AppendChild(_ClasseVinculos);

                XmlNode _Vinculos = _xmlDocument.CreateElement("Vinculos");
                _ClasseVinculos.AppendChild(_Vinculos);


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _SQL = "SELECT dbo.VeiculosEmpresas.VeiculoEmpresaID, dbo.VeiculosEmpresas.VeiculoID, dbo.Veiculos.Nome AS VeiculoNome," +
                    " dbo.Veiculos.CPF,dbo.Veiculos.Apelido AS VeiculoApelido, dbo.Veiculos.Motorista, dbo.Veiculos.Foto, dbo.VeiculosEmpresas.EmpresaID," +
                    " dbo.Empresas.Nome AS EmpresaNome, dbo.Empresas.Apelido AS EmpresaApelido,dbo.Empresas.CNPJ," +
                    " dbo.FormatosCredenciais.Descricao AS FormatoCredencial, dbo.VeiculosEmpresas.Cargo, dbo.VeiculosEmpresas.Matricula, dbo.VeiculosEmpresas.LayoutCrachaID," +
                    " dbo.VeiculosEmpresas.NumeroCredencial, dbo.VeiculosEmpresas.FC, dbo.VeiculosEmpresas.Validade, dbo.LayoutsCrachas.LayoutCrachaGUID FROM" +
                    " dbo.VeiculosEmpresas INNER JOIN dbo.Veiculos ON dbo.VeiculosEmpresas.VeiculoID = dbo.Veiculos.VeiculoID INNER JOIN" +
                    " dbo.Empresas ON dbo.VeiculosEmpresas.EmpresaID = dbo.Empresas.EmpresaID INNER JOIN dbo.FormatosCredenciais ON" +
                    " dbo.VeiculosEmpresas.FormatoCredencialID = dbo.FormatosCredenciais.FormatoCredencialID INNER JOIN dbo.LayoutsCrachas ON" +
                    " dbo.VeiculosEmpresas.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID WHERE(dbo.VeiculosEmpresas.VeiculoEmpresaID = " + _VeiculoEmpresaID + ")";


                SqlCommand _sqlcmd = new SqlCommand(_SQL, _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Vinculo = _xmlDocument.CreateElement("Vinculo");
                    _Vinculos.AppendChild(_Vinculo);

                    XmlNode _Vinculo24 = _xmlDocument.CreateElement("VeiculoID");
                    _Vinculo24.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["VeiculoID"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo24);

                    XmlNode _Vinculo9 = _xmlDocument.CreateElement("CPF");
                    _Vinculo9.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["CPF"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo9);

                    XmlNode _Vinculo10 = _xmlDocument.CreateElement("VeiculoNome");
                    _Vinculo10.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["VeiculoNome"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo10);

                    XmlNode _Vinculo22 = _xmlDocument.CreateElement("Motorista");
                    _Vinculo22.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqldatareader["Motorista"])).ToString()));
                    _Vinculo.AppendChild(_Vinculo22);

                    XmlNode _Vinculo21 = _xmlDocument.CreateElement("VeiculoApelido");
                    _Vinculo21.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["VeiculoApelido"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo21);

                    XmlNode _Vinculo11 = _xmlDocument.CreateElement("VeiculoFoto");
                    _Vinculo11.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Foto"].ToString())));
                    _Vinculo.AppendChild(_Vinculo11);

                    XmlNode _Vinculo12 = _xmlDocument.CreateElement("EmpresaNome");
                    _Vinculo12.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaNome"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo12);


                    XmlNode _Vinculo23 = _xmlDocument.CreateElement("EmpresaApelido");
                    _Vinculo23.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["EmpresaApelido"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo23);

                    XmlNode _Vinculo13 = _xmlDocument.CreateElement("Cargo");
                    _Vinculo13.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Cargo"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo13);

                    XmlNode _Vinculo14 = _xmlDocument.CreateElement("Matricula");
                    _Vinculo14.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Matricula"].ToString().Trim())));
                    _Vinculo.AppendChild(_Vinculo14);

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
                    _Vinculo20.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["FormatoCredencial"].ToString().Trim())));
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

        private string RequisitaContratos(int _empresaID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseContratosEmpresas = _xmlDocument.CreateElement("ClasseEmpresasContratos");
                _xmlDocument.AppendChild(_ClasseContratosEmpresas);

                XmlNode _EmpresasContratos = _xmlDocument.CreateElement("EmpresasContratos");
                _ClasseContratosEmpresas.AppendChild(_EmpresasContratos);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _strSql = "select * from EmpresasContratos where EmpresaID = " + _empresaID + " and StatusID=1 order by EmpresaContratoID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _EmpresaContrato = _xmlDocument.CreateElement("EmpresaContrato");
                    _EmpresasContratos.AppendChild(_EmpresaContrato);

                    XmlNode _EmpresaContratoID = _xmlDocument.CreateElement("EmpresaContratoID");
                    _EmpresaContratoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaContratoID"].ToString())));
                    _EmpresaContrato.AppendChild(_EmpresaContratoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _EmpresaContrato.AppendChild(_EmpresaID);

                    XmlNode _NumeroContrato = _xmlDocument.CreateElement("NumeroContrato");
                    _NumeroContrato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroContrato"].ToString())));
                    _EmpresaContrato.AppendChild(_NumeroContrato);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _EmpresaContrato.AppendChild(_Descricao);

                    XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
                    _Emissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Emissao"].ToString())));
                    _EmpresaContrato.AppendChild(_Emissao);

                    XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                    _Validade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString())));
                    _EmpresaContrato.AppendChild(_Validade);

                    XmlNode _Terceirizada = _xmlDocument.CreateElement("Terceirizada");
                    _Terceirizada.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Terceirizada"].ToString())));
                    _EmpresaContrato.AppendChild(_Terceirizada);

                    XmlNode _Contratante = _xmlDocument.CreateElement("Contratante");
                    _Contratante.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contratante"].ToString())));
                    _EmpresaContrato.AppendChild(_Contratante);

                    XmlNode _IsencaoCobranca = _xmlDocument.CreateElement("IsencaoCobranca");
                    _IsencaoCobranca.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["IsencaoCobranca"].ToString())));
                    _EmpresaContrato.AppendChild(_IsencaoCobranca);

                    XmlNode _TipoCobrancaID = _xmlDocument.CreateElement("TipoCobrancaID");
                    _TipoCobrancaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoCobrancaID"].ToString())));
                    _EmpresaContrato.AppendChild(_TipoCobrancaID);

                    XmlNode _CobrancaEmpresaID = _xmlDocument.CreateElement("CobrancaEmpresaID");
                    _CobrancaEmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CobrancaEmpresaID"].ToString())));
                    _EmpresaContrato.AppendChild(_CobrancaEmpresaID);

                    XmlNode _CEP = _xmlDocument.CreateElement("CEP");
                    _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
                    _EmpresaContrato.AppendChild(_CEP);

                    XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    _EmpresaContrato.AppendChild(_Endereco);

                    XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    _EmpresaContrato.AppendChild(_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    _EmpresaContrato.AppendChild(_Bairro);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _EmpresaContrato.AppendChild(_MunicipioID);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _EmpresaContrato.AppendChild(_EstadoID);

                    XmlNode _NomeResp = _xmlDocument.CreateElement("NomeResp");
                    _NomeResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeResp"].ToString())));
                    _EmpresaContrato.AppendChild(_NomeResp);

                    XmlNode _TelefoneResp = _xmlDocument.CreateElement("TelefoneResp");
                    _TelefoneResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneResp"].ToString())));
                    _EmpresaContrato.AppendChild(_TelefoneResp);

                    XmlNode _CelularResp = _xmlDocument.CreateElement("CelularResp");
                    _CelularResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CelularResp"].ToString())));
                    _EmpresaContrato.AppendChild(_CelularResp);

                    XmlNode _EmailResp = _xmlDocument.CreateElement("EmailResp");
                    _EmailResp.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmailResp"].ToString())));
                    _EmpresaContrato.AppendChild(_EmailResp);

                    XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    _EmpresaContrato.AppendChild(_Numero);

                    XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
                    _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
                    _EmpresaContrato.AppendChild(_StatusID);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
                    _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
                    _EmpresaContrato.AppendChild(_TipoAcessoID);

                    XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
                    _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString())));
                    _EmpresaContrato.AppendChild(_NomeArquivo);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
                    _EmpresaContrato.AppendChild(_Arquivo);

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

        private void InsereVeiculoEmpresaBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseVeiculosEmpresas.VeiculoEmpresa _empresaVeiculoEmpresa = new ClasseVeiculosEmpresas.VeiculoEmpresa();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresaVeiculoEmpresa.VeiculoEmpresaID = _xmlDoc.GetElementsByTagName("VeiculoEmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoEmpresaID")[i].InnerText);
                _empresaVeiculoEmpresa.VeiculoID = _xmlDoc.GetElementsByTagName("VeiculoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);
                _empresaVeiculoEmpresa.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _empresaVeiculoEmpresa.EmpresaContratoID = _xmlDoc.GetElementsByTagName("EmpresaContratoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaContratoID")[i].InnerText);
                _empresaVeiculoEmpresa.Cargo = _xmlDoc.GetElementsByTagName("Cargo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Cargo")[i].InnerText;
                _empresaVeiculoEmpresa.Matricula = _xmlDoc.GetElementsByTagName("Matricula")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Matricula")[i].InnerText;
                bool _ativo;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Ativo")[i].InnerText, out _ativo);
                _empresaVeiculoEmpresa.Ativo = _xmlDoc.GetElementsByTagName("Ativo")[i] == null ? false : _ativo;



                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_empresaVeiculoEmpresa.VeiculoEmpresaID != 0)
                {
                    _sqlCmd = new SqlCommand("Update VeiculosEmpresas Set" +
                        " VeiculoID= " + _empresaVeiculoEmpresa.VeiculoID +
                        ",EmpresaID= " + _empresaVeiculoEmpresa.EmpresaID +
                        ",EmpresaContratoID= " + _empresaVeiculoEmpresa.EmpresaContratoID +
                        ",Cargo= '" + _empresaVeiculoEmpresa.Cargo + "'" +
                        ",Matricula= '" + _empresaVeiculoEmpresa.Matricula + "'" +
                        ",Ativo= '" + _empresaVeiculoEmpresa.Ativo +
                        "' Where VeiculoEmpresaID = " + _empresaVeiculoEmpresa.VeiculoEmpresaID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into VeiculosEmpresas(VeiculoID,EmpresaID,EmpresaContratoID,Ativo,Cargo,Matricula) values (" +
                                                           +_empresaVeiculoEmpresa.VeiculoID + "," + +_empresaVeiculoEmpresa.EmpresaID + "," + +_empresaVeiculoEmpresa.EmpresaContratoID + ",'" +
                                                          _empresaVeiculoEmpresa.Ativo + "','" + _empresaVeiculoEmpresa.Cargo + "','" + _empresaVeiculoEmpresa.Matricula + "')", _Con);


                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereVeiculoEmpresaBD ex: " + ex);


            }
        }

        private void ExcluiVeiculoEmpresaBD(int _VeiculoEmpresaID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from VeiculosEmpresas where VeiculoEmpresaID=" + _VeiculoEmpresaID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiVeiculoEmpresaBD ex: " + ex);


            }
        }

        private DateTime validadeCursoContrato(int _veiculo = 0)
        {
            try
            {

                //DateTime _menorDataCurso = Convert.ToDateTime("01-01-2999");
                //DateTime _menorDataContrato = Convert.ToDateTime("01-01-2999");

                string _menorDataCurso = "";
                string _menorDataContrato = "";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                string _strSql = "SELECT dbo.Veiculos.VeiculoID, dbo.Veiculos.Nome,CONVERT(datetime, dbo.VeiculosCursos.Validade, 103) " +
                                 "as ValidadeCurso,DATEDIFF(DAY, GETDATE(), CONVERT(datetime, dbo.VeiculosCursos.Validade, 103)) AS Dias FROM dbo.Veiculos " +
                                 "INNER JOIN dbo.VeiculosCursos ON dbo.Veiculos.VeiculoID = dbo.VeiculosCursos.VeiculoID where dbo.Veiculos.Excluida = 0 And dbo.VeiculosCursos.Controlado = 1 And dbo.VeiculosCursos.VeiculoID = " + _veiculo + " Order By Dias";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                if (_sqlreader.Read())
                {

                    //if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                    //{
                    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _menorDataCurso = _sqlreader["ValidadeCurso"].ToString();
                    // break;
                    //}

                }
                _sqlreader.Close();


                //_strSql = "SELECT dbo.Veiculos.VeiculoID, dbo.Veiculos.Nome, dbo.EmpresasContratos.EmpresaID, dbo.EmpresasContratos.NumeroContrato, " +
                //          "CONVERT(datetime,dbo.EmpresasContratos.Validade,103) as DataContrato, DATEDIFF ( DAY , GETDATE(),  CONVERT(datetime, dbo.EmpresasContratos.Validade,103))  AS Dias " +
                //          "FROM  dbo.EmpresasContratos INNER JOIN dbo.VeiculosEmpresas ON dbo.EmpresasContratos.EmpresaID = dbo.VeiculosEmpresas.EmpresaID INNER JOIN dbo.Veiculos " +
                //          "ON dbo.VeiculosEmpresas.VeiculoID = dbo.Veiculos.VeiculoID WHERE (dbo.Veiculos.Excluida = 0) And dbo.Veiculos.VeiculoID = " + _veiculo + " Order By Dias";

                //_sqlcmd = new SqlCommand(_strSql, _Con);
                //_sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                //if (_sqlreader.Read())
                //{

                //    // if (Convert.ToInt32(_sqlreader["Dias"]) < 30)
                //    //{
                //    //MessageBox.Show("Data de Vinculo!", "Sucesso ao Vincular", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    _menorDataContrato = _sqlreader["DataContrato"].ToString();
                //    // break;
                //    //}

                //}
                //_sqlreader.Close();



                //if (Convert.ToDateTime(_menorDataCurso) < Convert.ToDateTime(_menorDataContrato))
                //{
                return Convert.ToDateTime(_menorDataCurso);
                //}
                //else if (Convert.ToDateTime(_menorDataCurso) > Convert.ToDateTime(_menorDataContrato))
                //{
                //    return Convert.ToDateTime(_menorDataContrato);
                //}

                //return DateTime.Now;

            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }
        #endregion

        #region Metodos Privados

        #endregion
    }
}
