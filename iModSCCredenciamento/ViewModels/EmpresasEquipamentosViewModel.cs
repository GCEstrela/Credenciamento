using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using iModSCCredenciamento.Helpers;
using IMOD.CrossCutting;

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasEquipamentosViewModel : ViewModelBase
    {

        #region Inicializacao
        public EmpresasEquipamentosViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

            //Global.ValidadeEmpresa();
        }

        private void CarregaUI()
        {
            CarregaColecaoTiposEquipamentos();
            CarregaColecaoStatus();
            CarregaColecaoTiposAcesso();
        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento> _EmpresasEquipamentos;

        private ClasseEmpresasEquipamentos.EmpresaEquipamento _EmpresaEquipamentoSelecionado;

        private ClasseEmpresasEquipamentos.EmpresaEquipamento _EmpresaEquipamentoTemp = new ClasseEmpresasEquipamentos.EmpresaEquipamento();

        private List<ClasseEmpresasEquipamentos.EmpresaEquipamento> _EmpresasEquipamentosTemp = new List<ClasseEmpresasEquipamentos.EmpresaEquipamento>();

        private ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> _TiposEquipamentos;
        private ObservableCollection<ClasseStatus.Status> _TiposStatus;
        private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessos;

        PopupPesquisaEquipamentos popupPesquisaEquipamentos;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento> EmpresaEquipamentos
        {
            get
            {
                return _EmpresasEquipamentos;
            }

            set
            {
                if (_EmpresasEquipamentos != value)
                {
                    _EmpresasEquipamentos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ClasseEmpresasEquipamentos.EmpresaEquipamento EmpresaEquipamentoSelecionado
        {
            get
            {
                return this._EmpresaEquipamentoSelecionado;
            }
            set
            {
                this._EmpresaEquipamentoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (EmpresaEquipamentoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }
        public ObservableCollection<ClasseTiposEquipamento.TipoEquipamento> TiposEquipamento
        {
            get
            {
                return _TiposEquipamentos;
            }

            set
            {
                if (_TiposEquipamentos != value)
                {
                    _TiposEquipamentos = value;
                    OnPropertyChanged();

                }
            }
        }
        public ObservableCollection<ClasseStatus.Status> TiposStatus
        {
            get
            {
                return _TiposStatus;
            }

            set
            {
                if (_TiposStatus != value)
                {
                    _TiposStatus = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
        {
            get
            {
                return _TiposAcessos;
            }

            set
            {
                if (_TiposAcessos != value)
                {
                    _TiposAcessos = value;
                    OnPropertyChanged();

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
        #endregion

        #region Comandos dos Botoes
        public void OnAtualizaCommand(object empresaID)
        {
            EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            Thread CarregaColecaoEquipamentos_thr = new Thread(() => CarregaColecaoEquipamentos(Convert.ToInt32(empresaID)));
            CarregaColecaoEquipamentos_thr.Start();
            //CarregaColecaoEquipamentos(Convert.ToInt32(empresaID));
        }
         

        public void OnAbrirArquivoCommand()
        {
            //try
            //{
            //    try
            //    {
            //        string _xmlstring = CriaXmlImagem(SeguroSelecionado.EmpresaSeguroID);

            //        XmlDocument xmldocument = new XmlDocument();
            //        xmldocument.LoadXml(_xmlstring);
            //        XmlNode node = (XmlNode)xmldocument.DocumentElement;
            //        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

            //        string _ArquivoPDF = arquivoNode.FirstChild.Value;
            //        byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
            //        _ArquivoPDF = System.IO.Path.GetTempFileName();
            //        _ArquivoPDF = System.IO.Path.GetRandomFileName();
            //        _ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;

            //        //File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
            //        _ArquivoPDF = System.IO.Path.ChangeExtension(_ArquivoPDF, ".pdf");
            //        System.IO.File.WriteAllBytes(_ArquivoPDF, buffer);
            //        Action<string> act = new Action<string>(Global.AbrirArquivoPDF);
            //        act.BeginInvoke(_ArquivoPDF, null, null);
            //        //Global.AbrirArquivoPDF(_caminhoArquivoPDF);
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Log("Erro na void ListaARQColaboradorAnexo_lv_PreviewMouseDoubleClick ex: " + ex);

            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _EmpresaEquipamentoTemp = EmpresaEquipamentoSelecionado.CriaCopia(EmpresaEquipamentoSelecionado);
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
                EmpresaEquipamentos[_selectedIndexTemp] = _EmpresaEquipamentoTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasEquipamentos));

                ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento> _EmpresasEquipamentosTemp = new ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento>();
                ClasseEmpresasEquipamentos _ClasseEmpresasEquipamentosTemp = new ClasseEmpresasEquipamentos();
                _EmpresasEquipamentosTemp.Add(EmpresaEquipamentoSelecionado);
                _ClasseEmpresasEquipamentosTemp.EmpresasEquipamentos = _EmpresasEquipamentosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasEquipamentosTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereEquipamentoBD(xmlString);

                _EmpresasEquipamentosTemp = null;

                _EmpresasEquipamentosTemp.Clear();
                _EmpresaEquipamentoTemp = null;

            }
            catch (Exception ex)
            {
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in EmpresaEquipamentos)
                {
                    _EmpresasEquipamentosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                EmpresaEquipamentos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _EmpresaEquipamentoTemp = new ClasseEmpresasEquipamentos.EmpresaEquipamento();
                _EmpresaEquipamentoTemp.EmpresaID = EmpresaSelecionadaID;
                EmpresaEquipamentos.Add(_EmpresaEquipamentoTemp);
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasEquipamentos));

                ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento> _EmpresasEquipamentosPro = new ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento>();
                ClasseEmpresasEquipamentos _ClasseEmpresasEquipamentosPro = new ClasseEmpresasEquipamentos();
                _EmpresasEquipamentosPro.Add(EmpresaEquipamentoSelecionado);
                _ClasseEmpresasEquipamentosPro.EmpresasEquipamentos = _EmpresasEquipamentosPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasEquipamentosPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereEquipamentoBD(xmlString);
                Thread CarregaColecaoEquipamentos_thr = new Thread(() => CarregaColecaoEquipamentos(EmpresaEquipamentoSelecionado.EmpresaID));
                CarregaColecaoEquipamentos_thr.Start();
                _EmpresasEquipamentosTemp.Add(EmpresaEquipamentoSelecionado);
                EmpresaEquipamentos = null;
                EmpresaEquipamentos = new ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento>(_EmpresasEquipamentosTemp);
                SelectedIndex = _selectedIndexTemp;
                _EmpresasEquipamentosTemp.Clear();
                _EmpresasEquipamentosPro = null;

                _EmpresasEquipamentosPro.Clear();
                _EmpresaEquipamentoTemp = null;

            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                EmpresaEquipamentos = null;
                EmpresaEquipamentos = new ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento>(_EmpresasEquipamentosTemp);
                SelectedIndex = _selectedIndexTemp;
                _EmpresasEquipamentosTemp.Clear();
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
                //if (MessageBox.Show("Tem certeza que deseja excluir esta apólice?", "Excluir Apólice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (MessageBox.Show("Você perderá todos os dados desta apólice, inclusive histórico. Confirma exclusão?", "Excluir Apólice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        ExcluiEquipamentoBD(EmpresaEquipamentoSelecionado.EmpresaEquipamentoID);
                //        EmpresaEquipamentos.Remove(EmpresaEquipamentoSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiEquipamentoBD(EmpresaEquipamentoSelecionado.EmpresaEquipamentoID);
                        EmpresaEquipamentos.Remove(EmpresaEquipamentoSelecionado);
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
                PopupPesquisaEquipamentos popupPesquisaEquipamentos  = new PopupPesquisaEquipamentos();
                popupPesquisaEquipamentos.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaEquipamentos.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaEquipamentos.Criterio.Split((char)(20));
            int _empresaID = EmpresaSelecionadaID;
            string _seguradora = ((string[])vetor)[0];
            string _numeroapolice = ((string[])vetor)[1];
            CarregaColecaoEquipamentos(_empresaID, _seguradora, _numeroapolice);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoEquipamentos(int empresaID, string _equipamento = "", string _numeroapolice = "")
        {
            try
            {
                string _xml = RequisitaEquipamentos(empresaID, _equipamento, _numeroapolice);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasEquipamentos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasEquipamentos classeEquipamentosEmpresa = new ClasseEmpresasEquipamentos();
                classeEquipamentosEmpresa = (ClasseEmpresasEquipamentos)deserializer.Deserialize(reader);
                EmpresaEquipamentos = new ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento>();
                EmpresaEquipamentos = classeEquipamentosEmpresa.EmpresasEquipamentos;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoTiposEquipamentos()
        {
            try
            {
                string _xml = RequisitaTiposEquipamentos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposEquipamento));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseTiposEquipamento classeTiposEquipamentos = new ClasseTiposEquipamento();
                classeTiposEquipamentos = (ClasseTiposEquipamento)deserializer.Deserialize(reader);
                TiposEquipamento = new ObservableCollection<ClasseTiposEquipamento.TipoEquipamento>();
                TiposEquipamento = classeTiposEquipamentos.TiposEquipamentos;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoStatus()
        {
            try
            {
                string _xml = RequisitaTiposStatus();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseStatus));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseStatus classeTiposStatus = new ClasseStatus();
                classeTiposStatus = (ClasseStatus)deserializer.Deserialize(reader);
                TiposStatus = new ObservableCollection<ClasseStatus.Status>();
                TiposStatus = classeTiposStatus.Statuss;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        private void CarregaColecaoTiposAcesso()
        {
            try
            {
                string _xml = RequisitaTiposAcessos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseTiposAcessos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseTiposAcessos classeTiposAcessos = new ClasseTiposAcessos();
                classeTiposAcessos = (ClasseTiposAcessos)deserializer.Deserialize(reader);
                TiposAcessos = new ObservableCollection<ClasseTiposAcessos.TipoAcesso>();
                TiposAcessos = classeTiposAcessos.TiposAcessos;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaEquipamentos(int _empresaID, string _descriao = "", string _marca = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasEquipamentos = _xmlDocument.CreateElement("ClasseEmpresasEquipamentos");
                _xmlDocument.AppendChild(_ClasseEmpresasEquipamentos);

                XmlNode _EmpresasEquipamentos = _xmlDocument.CreateElement("EmpresasEquipamentos");
                _ClasseEmpresasEquipamentos.AppendChild(_EmpresasEquipamentos);

                string _strSql;

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _descriao = "%" + _descriao + "%";
                _marca = "%" + _marca + "%";

                _strSql = "select * from EmpresasEquipamentos where EmpresaID = " + _empresaID + " and Descricao Like '" +
                    _descriao + "' and Marca Like '" + _marca + "'   order by EmpresaEquipamentoID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _EmpresaEquipamento = _xmlDocument.CreateElement("EmpresaEquipamento");
                    _EmpresasEquipamentos.AppendChild(_EmpresaEquipamento);

                    XmlNode _EmpresaEquipamentoID = _xmlDocument.CreateElement("EmpresaEquipamentoID");
                    _EmpresaEquipamentoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaEquipamentoID"].ToString())));
                    _EmpresaEquipamento.AppendChild(_EmpresaEquipamentoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Descricao);

                    XmlNode _Marca = _xmlDocument.CreateElement("Marca");
                    _Marca.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Marca"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Marca);

                    XmlNode _Modelo = _xmlDocument.CreateElement("Modelo");
                    _Modelo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Modelo"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Modelo);

                    XmlNode _Ano = _xmlDocument.CreateElement("Ano");
                    _Ano.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Ano"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Ano);

                    XmlNode _Patrimonio = _xmlDocument.CreateElement("Patrimonio");
                    _Patrimonio.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Patrimonio"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Patrimonio);

                    XmlNode _Seguro = _xmlDocument.CreateElement("Seguro");
                    _Seguro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Seguro"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Seguro);

                    XmlNode _ApoliceSeguro = _xmlDocument.CreateElement("ApoliceSeguro");
                    _ApoliceSeguro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ApoliceSeguro"].ToString())));
                    _EmpresaEquipamento.AppendChild(_ApoliceSeguro);

                    XmlNode _ApoliceValor = _xmlDocument.CreateElement("ApoliceValor");
                    _ApoliceValor.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ApoliceValor"].ToString())));
                    _EmpresaEquipamento.AppendChild(_ApoliceValor);

                    XmlNode _ApoliceVigencia = _xmlDocument.CreateElement("ApoliceVigencia");
                    _ApoliceVigencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ApoliceVigencia"].ToString())));
                    _EmpresaEquipamento.AppendChild(_ApoliceVigencia);

                    var dateStr = (_sqlreader["DataEmissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
                        _DataEmissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _EmpresaEquipamento.AppendChild(_DataEmissao);
                    }

                    dateStr = (_sqlreader["DataValidade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
                        _DataValidade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _EmpresaEquipamento.AppendChild(_DataValidade);
                    }

                    XmlNode _Excluido = _xmlDocument.CreateElement("Excluido");
                    _Excluido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluido"].ToString())));
                    _EmpresaEquipamento.AppendChild(_Excluido);

                    XmlNode _TipoEquipamentoID = _xmlDocument.CreateElement("TipoEquipamentoID");
                    _TipoEquipamentoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoEquipamentoID"].ToString())));
                    _EmpresaEquipamento.AppendChild(_TipoEquipamentoID);

                    XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
                    _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
                    _EmpresaEquipamento.AppendChild(_StatusID);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
                    _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
                    _EmpresaEquipamento.AppendChild(_TipoAcessoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _EmpresaEquipamento.AppendChild(_EmpresaID);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch
            {
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                return null;
            }
            return null;
        }

        //private string RequisitaEquipamentos(int _empresaID, string _descriao = "", string _marca = "")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseEmpresasEquipamentos = _xmlDocument.CreateElement("ClasseEmpresasEquipamentos");
        //        _xmlDocument.AppendChild(_ClasseEmpresasEquipamentos);

        //        XmlNode _EmpresasEquipamentos = _xmlDocument.CreateElement("EmpresasEquipamentos");
        //        _ClasseEmpresasEquipamentos.AppendChild(_EmpresasEquipamentos);

        //        string _strSql;

        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();


        //        _descriao = "%" + _descriao + "%";
        //        _marca = "%" + _marca + "%";

        //        _strSql = "select * from EmpresasEquipamentos where EmpresaID = " + _empresaID + " and Descricao Like '" +
        //            _descriao + "' and Marca Like '" + _marca + "'   order by EmpresaEquipamentoID desc";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _EmpresaEquipamento = _xmlDocument.CreateElement("EmpresaEquipamento");
        //            _EmpresasEquipamentos.AppendChild(_EmpresaEquipamento);

        //            XmlNode _EmpresaEquipamentoID = _xmlDocument.CreateElement("EmpresaEquipamentoID");
        //            _EmpresaEquipamentoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaEquipamentoID"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_EmpresaEquipamentoID);

        //            XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
        //            _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Descricao);

        //            XmlNode _Marca = _xmlDocument.CreateElement("Marca");
        //            _Marca.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Marca"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Marca);

        //            XmlNode _Modelo = _xmlDocument.CreateElement("Modelo");
        //            _Modelo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Modelo"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Modelo);

        //            XmlNode _Ano = _xmlDocument.CreateElement("Ano");
        //            _Ano.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Ano"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Ano);

        //            XmlNode _Patrimonio = _xmlDocument.CreateElement("Patrimonio");
        //            _Patrimonio.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Patrimonio"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Patrimonio);

        //            XmlNode _Seguro = _xmlDocument.CreateElement("Seguro");
        //            _Seguro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Seguro"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Seguro);

        //            XmlNode _ApoliceSeguro = _xmlDocument.CreateElement("ApoliceSeguro");
        //            _ApoliceSeguro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ApoliceSeguro"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_ApoliceSeguro);

        //            XmlNode _ApoliceValor = _xmlDocument.CreateElement("ApoliceValor");
        //            _ApoliceValor.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ApoliceValor"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_ApoliceValor);

        //            XmlNode _ApoliceVigencia = _xmlDocument.CreateElement("ApoliceVigencia");
        //            _ApoliceVigencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ApoliceVigencia"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_ApoliceVigencia);

        //            XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
        //            _DataEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataEmissao"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_DataEmissao);

        //            XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
        //            _DataValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataValidade"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_DataValidade);

        //            XmlNode _Excluido = _xmlDocument.CreateElement("Excluido");
        //            _Excluido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluido"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_Excluido);

        //            XmlNode _TipoEquipamentoID = _xmlDocument.CreateElement("TipoEquipamentoID");
        //            _TipoEquipamentoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoEquipamentoID"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_TipoEquipamentoID);

        //            XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
        //            _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_StatusID);

        //            XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
        //            _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_TipoAcessoID);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
        //            _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
        //            _EmpresaEquipamento.AppendChild(_EmpresaID);

        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        return _xml;
        //    }
        //    catch
        //    {
        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
        //        return null;
        //    }
        //    return null;
        //}
        private string RequisitaTiposEquipamentos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposEquipamento = _xmlDocument.CreateElement("ClasseTiposEquipamento");
                _xmlDocument.AppendChild(_ClasseTiposEquipamento);

                XmlNode _TiposEquipamentos = _xmlDocument.CreateElement("TiposEquipamentos");
                _ClasseTiposEquipamento.AppendChild(_TiposEquipamentos);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                
                SqlCommand _sqlcmd = new SqlCommand("select * from TiposEquipamentos order by TipoEquipamentoID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoEquipamento = _xmlDocument.CreateElement("TipoEquipamento");
                    _TiposEquipamentos.AppendChild(_TipoEquipamento);

                    XmlNode _TipoEquipamentoID = _xmlDocument.CreateElement("TipoEquipamentoID");
                    _TipoEquipamentoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoEquipamentoID"].ToString())));
                    _TipoEquipamento.AppendChild(_TipoEquipamentoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoEquipamento.AppendChild(_Descricao);

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
        private string RequisitaTiposStatus()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseStatus = _xmlDocument.CreateElement("ClasseStatus");
                _xmlDocument.AppendChild(_ClasseStatus);

                XmlNode _Status = _xmlDocument.CreateElement("Statuss");
                _ClasseStatus.AppendChild(_Status);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
               
                SqlCommand _sqlcmd = new SqlCommand("select * from Status order by StatusID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _Statu = _xmlDocument.CreateElement("Status");
                    _Status.AppendChild(_Statu);

                    XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
                    _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["StatusID"].ToString())));
                    _Statu.AppendChild(_StatusID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _Statu.AppendChild(_Descricao);

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


        private string RequisitaTiposAcessos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseTiposAcessos = _xmlDocument.CreateElement("ClasseTiposAcessos");
                _xmlDocument.AppendChild(_ClasseTiposAcessos);

                XmlNode _TiposAcessos = _xmlDocument.CreateElement("TiposAcessos");
                _ClasseTiposAcessos.AppendChild(_TiposAcessos);

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
               
                SqlCommand _sqlcmd = new SqlCommand("select * from TiposAcessos order by TipoAcessoID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _TipoAcesso = _xmlDocument.CreateElement("TipoAcesso");
                    _TiposAcessos.AppendChild(_TipoAcesso);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
                    _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["TipoAcessoID"].ToString())));
                    _TipoAcesso.AppendChild(_TipoAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _TipoAcesso.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaTiposAcessos ex: " + ex);

                return null;
            }
        }

        private void InsereEquipamentoBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                //_Con.Open();
                ClasseEmpresasEquipamentos.EmpresaEquipamento _EmpresaEquipamento = new ClasseEmpresasEquipamentos.EmpresaEquipamento();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _EmpresaEquipamento.EmpresaEquipamentoID = _xmlDoc.GetElementsByTagName("EmpresaEquipamentoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaEquipamentoID")[i].InnerText);
                _EmpresaEquipamento.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
                _EmpresaEquipamento.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
                _EmpresaEquipamento.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
                _EmpresaEquipamento.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
                _EmpresaEquipamento.Patrimonio = _xmlDoc.GetElementsByTagName("Patrimonio")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Patrimonio")[i].InnerText;
                _EmpresaEquipamento.Seguro = _xmlDoc.GetElementsByTagName("Seguro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Seguro")[i].InnerText;
                _EmpresaEquipamento.ApoliceSeguro = _xmlDoc.GetElementsByTagName("ApoliceSeguro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ApoliceSeguro")[i].InnerText;
                _EmpresaEquipamento.ApoliceValor = _xmlDoc.GetElementsByTagName("ApoliceValor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ApoliceValor")[i].InnerText;
                _EmpresaEquipamento.ApoliceVigencia = _xmlDoc.GetElementsByTagName("ApoliceVigencia")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ApoliceVigencia")[i].InnerText;
                _EmpresaEquipamento.DataEmissao = _xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText);
                _EmpresaEquipamento.DataValidade = _xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText);
                _EmpresaEquipamento.Excluido = _xmlDoc.GetElementsByTagName("Excluido")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Excluido")[i].InnerText;
                _EmpresaEquipamento.TipoEquipamentoID = _xmlDoc.GetElementsByTagName("TipoEquipamentoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoEquipamentoID")[i].InnerText);
                _EmpresaEquipamento.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
                _EmpresaEquipamento.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoAcessoID")[i].InnerText);
                _EmpresaEquipamento.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);




                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                SqlCommand _sqlCmd;
                if (_EmpresaEquipamento.EmpresaEquipamentoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update EmpresasEquipamentos Set" +
                        " Descricao=@v1" +
                        ",Marca=@v2" +
                        ",Modelo=@v3" +
                        ",Ano=@v4" +
                        ",Patrimonio=@v5" +
                        ",Seguro=@v6" +
                        ",ApoliceSeguro=@v7" +
                        ",ApoliceValor=@v8" +
                        ",ApoliceVigencia=@v9" +
                        ",DataEmissao=@v10" +
                        ",DataValidade=@v11" +
                        ",Excluido=@v12" +
                        ",TipoEquipamentoID=@v13" +
                        ",StatusID=@v14" +
                        ",TipoAcessoID=@v15" +
                        ",EmpresaID=@v16" +
                        " Where EmpresaEquipamentoID =@v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.VarChar).Value = _EmpresaEquipamento.EmpresaEquipamentoID;
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.VarChar).Value = _EmpresaEquipamento.Descricao;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _EmpresaEquipamento.Marca;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.VarChar).Value = _EmpresaEquipamento.Modelo;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _EmpresaEquipamento.Ano;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.VarChar).Value = _EmpresaEquipamento.Patrimonio;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.VarChar).Value = _EmpresaEquipamento.Seguro;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.VarChar).Value = _EmpresaEquipamento.ApoliceSeguro;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _EmpresaEquipamento.ApoliceValor;
                    _sqlCmd.Parameters.Add("@V9", SqlDbType.VarChar).Value = _EmpresaEquipamento.ApoliceVigencia;
                    if (_EmpresaEquipamento.DataEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _EmpresaEquipamento.DataEmissao;
                    }
                    if (_EmpresaEquipamento.DataEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _EmpresaEquipamento.DataValidade;
                    }

                    _sqlCmd.Parameters.Add("@V12", SqlDbType.VarChar).Value = _EmpresaEquipamento.Excluido;
                    _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _EmpresaEquipamento.TipoEquipamentoID;
                    _sqlCmd.Parameters.Add("@V14", SqlDbType.Int).Value = _EmpresaEquipamento.StatusID;
                    _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _EmpresaEquipamento.TipoAcessoID;
                    _sqlCmd.Parameters.Add("@V16", SqlDbType.Int).Value = _EmpresaEquipamento.EmpresaID;

                }
                else
                {

                    _sqlCmd = new SqlCommand("Insert into EmpresasEquipamentos(Descricao,Marca" +
                                                                        ",Modelo,Ano" +
                                                                        ",Patrimonio,Seguro" +
                                                                        ",ApoliceSeguro,ApoliceValor" +
                                                                        ",ApoliceVigencia,DataEmissao" +
                                                                        ",DataValidade,Excluido" +
                                                                        ",TipoEquipamentoID,StatusID" +
                                                                        ",TipoAcessoID,EmpresaID) " +
                                                                        "values (@V1,@V2,@V3,@V4,@V5,@V6,@V7,@V8,@V9,@V10,@V11,@V12,@V13,@V14,@V15,@V16)", _Con);

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.VarChar).Value = _EmpresaEquipamento.Descricao;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _EmpresaEquipamento.Marca;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.VarChar).Value = _EmpresaEquipamento.Modelo;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _EmpresaEquipamento.Ano;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.VarChar).Value = _EmpresaEquipamento.Patrimonio;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.VarChar).Value = _EmpresaEquipamento.Seguro;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.VarChar).Value = _EmpresaEquipamento.ApoliceSeguro;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _EmpresaEquipamento.ApoliceValor;
                    _sqlCmd.Parameters.Add("@V9", SqlDbType.VarChar).Value = _EmpresaEquipamento.ApoliceVigencia;
                    if (_EmpresaEquipamento.DataEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V10", SqlDbType.DateTime).Value = _EmpresaEquipamento.DataEmissao;
                    }
                    if (_EmpresaEquipamento.DataEmissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V11", SqlDbType.DateTime).Value = _EmpresaEquipamento.DataValidade;
                    }

                    _sqlCmd.Parameters.Add("@V12", SqlDbType.VarChar).Value = _EmpresaEquipamento.Excluido;
                    _sqlCmd.Parameters.Add("@V13", SqlDbType.Int).Value = _EmpresaEquipamento.TipoEquipamentoID;
                    _sqlCmd.Parameters.Add("@V14", SqlDbType.Int).Value = _EmpresaEquipamento.StatusID;
                    _sqlCmd.Parameters.Add("@V15", SqlDbType.Int).Value = _EmpresaEquipamento.TipoAcessoID;
                    _sqlCmd.Parameters.Add("@V16", SqlDbType.Int).Value = _EmpresaEquipamento.EmpresaID;
                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEquipamentoBD ex: " + ex);


            }
        }
        //private void InsereEquipamentoBD(string xmlString)
        //{
        //    try
        //    {


        //        System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);
        //        //_Con.Open();
        //        ClasseEmpresasEquipamentos.EmpresaEquipamento _EmpresaEquipamento = new ClasseEmpresasEquipamentos.EmpresaEquipamento();
        //        //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        //{
        //        int i = 0;

        //        _EmpresaEquipamento.EmpresaEquipamentoID = _xmlDoc.GetElementsByTagName("EmpresaEquipamentoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaEquipamentoID")[i].InnerText);
        //        _EmpresaEquipamento.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
        //        _EmpresaEquipamento.Marca = _xmlDoc.GetElementsByTagName("Marca")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Marca")[i].InnerText;
        //        _EmpresaEquipamento.Modelo = _xmlDoc.GetElementsByTagName("Modelo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Modelo")[i].InnerText;
        //        _EmpresaEquipamento.Ano = _xmlDoc.GetElementsByTagName("Ano")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Ano")[i].InnerText;
        //        _EmpresaEquipamento.Patrimonio = _xmlDoc.GetElementsByTagName("Patrimonio")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Patrimonio")[i].InnerText;
        //        _EmpresaEquipamento.Seguro = _xmlDoc.GetElementsByTagName("Seguro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Seguro")[i].InnerText;
        //        _EmpresaEquipamento.ApoliceSeguro = _xmlDoc.GetElementsByTagName("ApoliceSeguro")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ApoliceSeguro")[i].InnerText;
        //        _EmpresaEquipamento.ApoliceValor = _xmlDoc.GetElementsByTagName("ApoliceValor")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ApoliceValor")[i].InnerText;
        //        _EmpresaEquipamento.ApoliceVigencia = _xmlDoc.GetElementsByTagName("ApoliceVigencia")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ApoliceVigencia")[i].InnerText;
        //        _EmpresaEquipamento.DataEmissao = _xmlDoc.GetElementsByTagName("DataEmissao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("DataEmissao")[i].InnerText;
        //        _EmpresaEquipamento.DataValidade = _xmlDoc.GetElementsByTagName("DataValidade")[i] == null ? "" : _xmlDoc.GetElementsByTagName("DataValidade")[i].InnerText;
        //        _EmpresaEquipamento.Excluido = _xmlDoc.GetElementsByTagName("Excluido")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Excluido")[i].InnerText;
        //        _EmpresaEquipamento.TipoEquipamentoID = _xmlDoc.GetElementsByTagName("TipoEquipamentoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoEquipamentoID")[i].InnerText);
        //        _EmpresaEquipamento.StatusID = _xmlDoc.GetElementsByTagName("StatusID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("StatusID")[i].InnerText);
        //        _EmpresaEquipamento.TipoAcessoID = _xmlDoc.GetElementsByTagName("TipoAcessoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoAcessoID")[i].InnerText);
        //        _EmpresaEquipamento.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);




        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();


        //        SqlCommand _sqlCmd;
        //        if (_EmpresaEquipamento.EmpresaEquipamentoID != 0)
        //        {
        //            _sqlCmd = new SqlCommand("Update EmpresasEquipamentos Set" +
        //                " Descricao= '" + _EmpresaEquipamento.Descricao + "'" +
        //                ",Marca= '" + _EmpresaEquipamento.Marca + "'" +
        //                ",Modelo= '" + _EmpresaEquipamento.Modelo + "'" +
        //                ",Ano= '" + _EmpresaEquipamento.Ano + "'" +
        //                ",Patrimonio= '" + _EmpresaEquipamento.Patrimonio + "'" +
        //                ",Seguro= '" + _EmpresaEquipamento.Seguro + "'" +
        //                ",ApoliceSeguro= '" + _EmpresaEquipamento.ApoliceSeguro + "'" +
        //                ",ApoliceValor= '" + _EmpresaEquipamento.ApoliceValor + "'" +
        //                ",ApoliceVigencia= '" + _EmpresaEquipamento.ApoliceVigencia + "'" +
        //                ",DataEmissao= '" + _EmpresaEquipamento.DataEmissao + "'" +
        //                ",DataValidade= '" + _EmpresaEquipamento.DataValidade + "'" +
        //                ",Excluido= '" + _EmpresaEquipamento.Excluido + "'" +
        //                ",TipoEquipamentoID= " + _EmpresaEquipamento.TipoEquipamentoID + "" +
        //                ",StatusID= " + _EmpresaEquipamento.StatusID + "" +
        //                ",TipoAcessoID= " + _EmpresaEquipamento.TipoAcessoID + "" +
        //                ",EmpresaID= " + _EmpresaEquipamento.EmpresaID + "" +
        //                " Where EmpresaEquipamentoID = " + _EmpresaEquipamento.EmpresaEquipamentoID + "", _Con);
        //        }
        //        else
        //        {
        //            _sqlCmd = new SqlCommand("Insert into EmpresasEquipamentos(Descricao,Marca" +
        //                                                                ",Modelo,Ano" +
        //                                                                ",Patrimonio,Seguro" +
        //                                                                ",ApoliceSeguro,ApoliceValor" +
        //                                                                ",ApoliceVigencia,DataEmissao" +
        //                                                                ",DataValidade,Excluido" +
        //                                                                ",TipoEquipamentoID,StatusID" +
        //                                                                ",TipoAcessoID,EmpresaID) values ('" +
        //                                                                _EmpresaEquipamento.Descricao + "','" + _EmpresaEquipamento.Marca + "','" +
        //                                                                _EmpresaEquipamento.Modelo + "','" + _EmpresaEquipamento.Ano + "','" +
        //                                                                _EmpresaEquipamento.Patrimonio + "','" + _EmpresaEquipamento.Seguro + "','" +
        //                                                                _EmpresaEquipamento.ApoliceSeguro + "','" + _EmpresaEquipamento.ApoliceValor + "','" +
        //                                                                _EmpresaEquipamento.ApoliceVigencia + "','" + _EmpresaEquipamento.DataEmissao + "','" +
        //                                                                _EmpresaEquipamento.DataValidade + "','" + _EmpresaEquipamento.Excluido + "'," +
        //                                                                _EmpresaEquipamento.TipoEquipamentoID + "," + _EmpresaEquipamento.StatusID + "," +
        //                                                                _EmpresaEquipamento.TipoAcessoID + "," + _EmpresaEquipamento.EmpresaID + ")", _Con);


        //        }

        //        _sqlCmd.ExecuteNonQuery();
        //        _Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void InsereEquipamentoBD ex: " + ex);


        //    }
        //}

        private void ExcluiEquipamentoBD(int _EmpresaEquipamentoID) // alterar para xml
        {
            try
            {

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                //_Con.Close();
                _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasEquipamentos where EmpresaSeguroID=" + _EmpresaEquipamentoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiEquipamentoBD ex: " + ex);


            }
        }
        #endregion

        #region Metodos privados
         

        #endregion
    }
}
