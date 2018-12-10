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
using iModSCCredenciamento.Helpers;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using IMOD.CrossCutting;

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasSegurosViewModel : ViewModelBase
    {
        #region Inicializacao

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro> _Seguros;

        private ClasseEmpresasSeguros.EmpresaSeguro _SeguroSelecionado;

        private ClasseEmpresasSeguros.EmpresaSeguro _seguroTemp = new ClasseEmpresasSeguros.EmpresaSeguro();

        private List<ClasseEmpresasSeguros.EmpresaSeguro> _SegurosTemp = new List<ClasseEmpresasSeguros.EmpresaSeguro>();

        PopupPesquisaSeguro popupPesquisaSeguro;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao;

        private string _Criterios = "";

        private int _selectedIndexTemp;

        #endregion

        #region Contrutores
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

        public ClasseEmpresasSeguros.EmpresaSeguro SeguroSelecionado
        {
            get
            {
                return _SeguroSelecionado;
            }
            set
            {
                _SeguroSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (SeguroSelecionado != null)
                {
                    //OnEmpresaSelecionada();
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
        public void OnAtualizaCommand(object empresaID)
        {
            EmpresaSelecionadaID = Convert.ToInt32(empresaID);
            Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(empresaID)));
            CarregaColecaoSeguros_thr.Start();
            //CarregaColecaoSeguros(Convert.ToInt32(empresaID));
        }

        public void OnBuscarArquivoCommand()
        {
            try
            {
                var filtro = "Imagem files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                var arq = WpfHelp.UpLoadArquivoDialog(filtro, 700);
                if (arq == null) return;
                _seguroTemp.NomeArquivo = arq.Nome;
                _seguroTemp.Arquivo = arq.FormatoBase64;
                if (_SegurosTemp != null)
                    _SegurosTemp[0].NomeArquivo = arq.Nome;

            }
            catch (Exception ex)
            {
                WpfHelp.Mbox(ex.Message);
                Utils.TraceException(ex);
            }
             
        }

        public void OnAbrirArquivoCommand()
        {
                try
                {

                    string _ArquivoPDF = null;
                    if (_seguroTemp != null)
                    {
                        if (_seguroTemp.Arquivo != null && _seguroTemp.EmpresaSeguroID == SeguroSelecionado.EmpresaSeguroID)
                        {
                            _ArquivoPDF = _seguroTemp.Arquivo;

                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        string _xmlstring = CriaXmlImagem(SeguroSelecionado.EmpresaSeguroID);

                        XmlDocument xmldocument = new XmlDocument();
                        xmldocument.LoadXml(_xmlstring);
                        XmlNode node = xmldocument.DocumentElement;
                        XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                        _ArquivoPDF = arquivoNode.FirstChild.Value;
                    }
                Global.PopupPDF(_ArquivoPDF);
                //byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
                //    _ArquivoPDF = System.IO.Path.GetTempFileName();
                //    _ArquivoPDF = System.IO.Path.GetRandomFileName();
                //    _ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;

                //    //File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
                //    _ArquivoPDF = System.IO.Path.ChangeExtension(_ArquivoPDF, ".pdf");
                //    System.IO.File.WriteAllBytes(_ArquivoPDF, buffer);
                //    //Action<string> act = new Action<string>(Global.AbrirArquivoPDF);
                //    //act.BeginInvoke(_ArquivoPDF, null, null);
                //    Global.PopupPDF(_ArquivoPDF);
                //    System.IO.File.Delete(_ArquivoPDF);
                }
                catch (Exception ex)
                {
                    Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);

                }
        }

        public void OnEditarCommand()
        {
            try
            {
                //BuscaBadges();
                _seguroTemp = SeguroSelecionado.CriaCopia(SeguroSelecionado);
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
                Seguros[_selectedIndexTemp] = _seguroTemp;
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
                XmlSerializer serializer = new XmlSerializer(typeof(ClasseEmpresasSeguros));

                ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro> _EmpresasSegurosTemp = new ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro>();
                ClasseEmpresasSeguros _ClasseEmpresasSegurosTemp = new ClasseEmpresasSeguros();
                _EmpresasSegurosTemp.Add(SeguroSelecionado);
                _ClasseEmpresasSegurosTemp.EmpresasSeguros = _EmpresasSegurosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasSegurosTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereSeguroBD(xmlString);

                _ClasseEmpresasSegurosTemp = null;

                _SegurosTemp.Clear();
                _seguroTemp = null;


            }
            catch (Exception ex)
            {
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Seguros)
                {
                    _SegurosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Seguros.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _seguroTemp = new ClasseEmpresasSeguros.EmpresaSeguro();
                _seguroTemp.EmpresaID = EmpresaSelecionadaID;
                Seguros.Add(_seguroTemp);
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
                XmlSerializer serializer = new XmlSerializer(typeof(ClasseEmpresasSeguros));

                ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro> _EmpresasSegurosPro = new ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro>();
                ClasseEmpresasSeguros _ClasseEmpresasSegurosPro = new ClasseEmpresasSeguros();
                _EmpresasSegurosPro.Add(SeguroSelecionado);
                _ClasseEmpresasSegurosPro.EmpresasSeguros = _EmpresasSegurosPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasSegurosPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereSeguroBD(xmlString);
                Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(SeguroSelecionado.EmpresaID));
                CarregaColecaoSeguros_thr.Start();
                //_SegurosTemp.Add(SeguroSelecionado);
                //Seguros = null;
                //Seguros = new ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro>(_SegurosTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_SegurosTemp.Clear();
                //_ClasseEmpresasSegurosPro = null;

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
                Seguros = null;
                Seguros = new ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro>(_SegurosTemp);
                SelectedIndex = _selectedIndexTemp;
                _SegurosTemp.Clear();
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
                //        ExcluiSeguroBD(SeguroSelecionado.EmpresaSeguroID);
                //        Seguros.Remove(SeguroSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiSeguroBD(SeguroSelecionado.EmpresaSeguroID);
                        Seguros.Remove(SeguroSelecionado);
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
                popupPesquisaSeguro = new PopupPesquisaSeguro();
                popupPesquisaSeguro.EfetuarProcura += On_EfetuarProcura;
                popupPesquisaSeguro.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaSeguro.Criterio.Split((char)(20));
            int _empresaID = EmpresaSelecionadaID;
            string _seguradora = ((string[])vetor)[0];
            string _numeroapolice = ((string[])vetor)[1];
            CarregaColecaoSeguros(_empresaID,_seguradora, _numeroapolice);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoSeguros(int empresaID, string _seguradora="", string _numeroapolice="")
        {
            try
            {
                string _xml = RequisitaSeguros(empresaID, _seguradora, _numeroapolice);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasSeguros));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasSeguros classeSegurosEmpresa = new ClasseEmpresasSeguros();
                classeSegurosEmpresa = (ClasseEmpresasSeguros)deserializer.Deserialize(reader);
                Seguros = new ObservableCollection<ClasseEmpresasSeguros.EmpresaSeguro>();
                Seguros = classeSegurosEmpresa.EmpresasSeguros;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaSeguros(int _empresaID, string _seguradora = "", string _numeroapolice = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseSegurosEmpresas = _xmlDocument.CreateElement("ClasseEmpresasSeguros");
                _xmlDocument.AppendChild(_ClasseSegurosEmpresas);

                XmlNode _Seguros = _xmlDocument.CreateElement("EmpresasSeguros");
                _ClasseSegurosEmpresas.AppendChild(_Seguros);

                string _strSql;

                //
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _seguradora = "%" + _seguradora + "%";
                _numeroapolice = "%" + _numeroapolice + "%";

                _strSql = "select [EmpresaSeguroID],[NomeSeguradora],[NumeroApolice],[ValorCobertura],[EmpresaID],[NomeArquivo],[Emissao],[Validade]" +
                    " from EmpresasSeguros where EmpresaID = " + _empresaID + " and NomeSeguradora Like '" + _seguradora +
                    "' and NumeroApolice Like '" + _numeroapolice + "'    order by EmpresaSeguroID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Seguro = _xmlDocument.CreateElement("EmpresaSeguro");
                    _Seguros.AppendChild(_Seguro);

                    XmlNode _EmpresaSeguroID = _xmlDocument.CreateElement("EmpresaSeguroID");
                    _EmpresaSeguroID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaSeguroID"].ToString())));
                    _Seguro.AppendChild(_EmpresaSeguroID);

                    XmlNode _NomeSeguradora = _xmlDocument.CreateElement("NomeSeguradora");
                    _NomeSeguradora.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeSeguradora"].ToString())));
                    _Seguro.AppendChild(_NomeSeguradora);

                    XmlNode _NumeroApolice = _xmlDocument.CreateElement("NumeroApolice");
                    _NumeroApolice.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroApolice"].ToString())));
                    _Seguro.AppendChild(_NumeroApolice);

                    XmlNode _ValorCobertura = _xmlDocument.CreateElement("ValorCobertura");
                    _ValorCobertura.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ValorCobertura"].ToString())));
                    _Seguro.AppendChild(_ValorCobertura);

                    //XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
                    //_Emissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Emissao"].ToString())));
                    //_Seguro.AppendChild(_Emissao);

                    var dateStr = (_sqlreader["Emissao"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
                        //format valid for XML W3C yyyy-MM-ddTHH:mm:ss
                        _Emissao.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Seguro.AppendChild(_Emissao);
                    }

                    //XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                    //_Validade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString())));
                    //_Seguro.AppendChild(_Validade);

                    dateStr = (_sqlreader["Validade"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _Validade = _xmlDocument.CreateElement("Validade");
                        //format valid for XML W3C yyyy-MM-ddTHH:mm:ss
                        _Validade.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Seguro.AppendChild(_Validade);
                    }

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _Seguro.AppendChild(_EmpresaID);

                    XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
                    _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString())));
                    _Seguro.AppendChild(_NomeArquivo);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroArquivo"].ToString())));
                    _Seguro.AppendChild(_Arquivo);

                }

                _sqlreader.Close();

                _Con.Close();
                _Con.Dispose();

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
        //private string RequisitaSeguros(int _empresaID, string _seguradora = "", string _numeroapolice = "")
        //{
        //    try
        //    {
        //        XmlDocument _xmlDocument = new XmlDocument();
        //        XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

        //        XmlNode _ClasseSegurosEmpresas = _xmlDocument.CreateElement("ClasseEmpresasSeguros");
        //        _xmlDocument.AppendChild(_ClasseSegurosEmpresas);

        //        XmlNode _Seguros = _xmlDocument.CreateElement("EmpresasSeguros");
        //        _ClasseSegurosEmpresas.AppendChild(_Seguros);

        //        string _strSql;

        //        //
        //        // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();


        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

        //        _seguradora = "%" + _seguradora + "%";
        //        _numeroapolice = "%" + _numeroapolice + "%";

        //        _strSql = "select [EmpresaSeguroID],[NomeSeguradora],[NumeroApolice],[ValorCobertura],[EmpresaID],[NomeArquivo],[Emissao],[Validade]" +
        //            " from EmpresasSeguros where EmpresaID = " + _empresaID + " and NomeSeguradora Like '" + _seguradora +
        //            "' and NumeroApolice Like '" + _numeroapolice + "'    order by EmpresaSeguroID desc";

        //        SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
        //        SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
        //        while (_sqlreader.Read())
        //        {

        //            XmlNode _Seguro = _xmlDocument.CreateElement("EmpresaSeguro");
        //            _Seguros.AppendChild(_Seguro);

        //            XmlNode _EmpresaSeguroID = _xmlDocument.CreateElement("EmpresaSeguroID");
        //            _EmpresaSeguroID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaSeguroID"].ToString())));
        //            _Seguro.AppendChild(_EmpresaSeguroID);

        //            XmlNode _NomeSeguradora = _xmlDocument.CreateElement("NomeSeguradora");
        //            _NomeSeguradora.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeSeguradora"].ToString())));
        //            _Seguro.AppendChild(_NomeSeguradora);

        //            XmlNode _NumeroApolice = _xmlDocument.CreateElement("NumeroApolice");
        //            _NumeroApolice.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NumeroApolice"].ToString())));
        //            _Seguro.AppendChild(_NumeroApolice);

        //            XmlNode _ValorCobertura = _xmlDocument.CreateElement("ValorCobertura");
        //            _ValorCobertura.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ValorCobertura"].ToString())));
        //            _Seguro.AppendChild(_ValorCobertura);

        //            XmlNode _Emissao = _xmlDocument.CreateElement("Emissao");
        //            _Emissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Emissao"].ToString())));
        //            _Seguro.AppendChild(_Emissao);

        //            XmlNode _Validade = _xmlDocument.CreateElement("Validade");
        //            _Validade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Validade"].ToString())));
        //            _Seguro.AppendChild(_Validade);

        //            XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
        //            _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
        //            _Seguro.AppendChild(_EmpresaID);

        //            XmlNode _NomeArquivo = _xmlDocument.CreateElement("NomeArquivo");
        //            _NomeArquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivo"].ToString())));
        //            _Seguro.AppendChild(_NomeArquivo);

        //            XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
        //            //_Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["SeguroArquivo"].ToString())));
        //            _Seguro.AppendChild(_Arquivo);

        //        }

        //        _sqlreader.Close();

        //        _Con.Close();
        //        _Con.Dispose();

        //        string _xml = _xmlDocument.InnerXml;
        //        _xmlDocument = null;
        //        return _xml;
        //    }
        //    catch
        //    {

        //        return null;
        //    }
        //    return null;
        //}
        private void InsereSeguroBD(string xmlString)
        {
            try
            {


                XmlDocument _xmlDoc = new XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseEmpresasSeguros.EmpresaSeguro _empresaSeguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresaSeguro.EmpresaSeguroID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaSeguroID")[i].InnerText);
                _empresaSeguro.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _empresaSeguro.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
                _empresaSeguro.NomeSeguradora = _xmlDoc.GetElementsByTagName("NomeSeguradora")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeSeguradora")[i].InnerText;
                _empresaSeguro.NumeroApolice = _xmlDoc.GetElementsByTagName("NumeroApolice")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroApolice")[i].InnerText;
                _empresaSeguro.ValorCobertura = _xmlDoc.GetElementsByTagName("ValorCobertura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ValorCobertura")[i].InnerText;
                _empresaSeguro.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Emissao")[i].InnerText);
                _empresaSeguro.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);

                //_empresaSeguro.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;

                _empresaSeguro.NomeArquivo = _seguroTemp.NomeArquivo == null ? "" : _seguroTemp.NomeArquivo;
                _empresaSeguro.Arquivo = _seguroTemp.Arquivo == null ? "" : _seguroTemp.Arquivo;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_empresaSeguro.EmpresaSeguroID != 0)
                {


                    _sqlCmd = new SqlCommand("Update EmpresasSeguros Set EmpresaID=@v1,NomeArquivo=@v2" +
                                             ",NomeSeguradora= @v3,NumeroApolice=@v4,ValorCobertura=@v5,Emissao=@v6,Validade=@v7,Arquivo=@v8 Where EmpresaSeguroID =@v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _empresaSeguro.EmpresaSeguroID;
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _empresaSeguro.EmpresaID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _empresaSeguro.NomeArquivo;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.VarChar).Value = _empresaSeguro.NomeSeguradora;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _empresaSeguro.NumeroApolice;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.VarChar).Value = _empresaSeguro.ValorCobertura;
                    if (_empresaSeguro.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = _empresaSeguro.Emissao;
                    }
                    if (_empresaSeguro.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = _empresaSeguro.Validade;
                    }
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _empresaSeguro.Arquivo;
                }
                else
                {


                    _sqlCmd = new SqlCommand("Insert into EmpresasSeguros(EmpresaID,NomeArquivo,NomeSeguradora,NumeroApolice,ValorCobertura,Emissao,Validade,Arquivo) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)", _Con);
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _empresaSeguro.EmpresaID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _empresaSeguro.NomeArquivo;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.VarChar).Value = _empresaSeguro.NomeSeguradora;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _empresaSeguro.NumeroApolice;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.VarChar).Value = _empresaSeguro.ValorCobertura;
                    if (_empresaSeguro.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = _empresaSeguro.Emissao;
                    }
                    if (_empresaSeguro.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = _empresaSeguro.Validade;
                    }
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _empresaSeguro.Arquivo;


                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresaBD ex: " + ex);


            }
        }
        //private void InsereSeguroBD(string xmlString)
        //{
        //    try
        //    {


        //        System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

        //        _xmlDoc.LoadXml(xmlString);
        //        // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
        //        ClasseEmpresasSeguros.EmpresaSeguro _empresaSeguro = new ClasseEmpresasSeguros.EmpresaSeguro();
        //        //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
        //        //{
        //        int i = 0;

        //        _empresaSeguro.EmpresaSeguroID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaSeguroID")[i].InnerText);
        //        _empresaSeguro.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
        //        _empresaSeguro.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
        //        _empresaSeguro.NomeSeguradora = _xmlDoc.GetElementsByTagName("NomeSeguradora")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeSeguradora")[i].InnerText;
        //        _empresaSeguro.NumeroApolice = _xmlDoc.GetElementsByTagName("NumeroApolice")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroApolice")[i].InnerText;
        //        _empresaSeguro.ValorCobertura = _xmlDoc.GetElementsByTagName("ValorCobertura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ValorCobertura")[i].InnerText;
        //        _empresaSeguro.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText;
        //        _empresaSeguro.Validade = _xmlDoc.GetElementsByTagName("Validade")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Validade")[i].InnerText;

        //        //_empresaSeguro.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;

        //        _empresaSeguro.NomeArquivo = _seguroTemp.NomeArquivo == null ? "" : _seguroTemp.NomeArquivo;
        //        _empresaSeguro.Arquivo = _seguroTemp.Arquivo == null ? "" : _seguroTemp.Arquivo;


        //         SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

        //        SqlCommand _sqlCmd;
        //        if (_empresaSeguro.EmpresaSeguroID != 0)
        //        {
        //            _sqlCmd = new SqlCommand("Update EmpresasSeguros Set" +
        //                " EmpresaID= " + _empresaSeguro.EmpresaID + "" +
        //                ",NomeArquivo= '" + _empresaSeguro.NomeArquivo + "'" +
        //                ",NomeSeguradora= '" + _empresaSeguro.NomeSeguradora + "'" +
        //                ",NumeroApolice= '" + _empresaSeguro.NumeroApolice + "'" +
        //                ",ValorCobertura= '" + _empresaSeguro.ValorCobertura + "'" +
        //                ",Emissao= '" + _empresaSeguro.Emissao + "'" +
        //                ",Validade= '" + _empresaSeguro.Validade + "'" +
        //                ",Arquivo= '" + _empresaSeguro.Arquivo + "'" +
        //                " Where EmpresaSeguroID = " + _empresaSeguro.EmpresaSeguroID + "", _Con);
        //        }
        //        else
        //        {
        //            _sqlCmd = new SqlCommand("Insert into EmpresasSeguros(EmpresaID,NomeArquivo,NomeSeguradora,NumeroApolice,ValorCobertura,Emissao,Validade,Arquivo) values (" +
        //                                                  _empresaSeguro.EmpresaID + ",'" + _empresaSeguro.NomeArquivo + "','" +
        //                                                  _empresaSeguro.NomeSeguradora + "','" + _empresaSeguro.NumeroApolice + "','" +
        //                                                  _empresaSeguro.ValorCobertura + "','" + _empresaSeguro.Emissao + "','" + 
        //                                                  _empresaSeguro.Validade  + "','" + _empresaSeguro.Arquivo + "')", _Con);


        //        }

        //        _sqlCmd.ExecuteNonQuery();
        //        _Con.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Log("Erro na void InsereEmpresaBD ex: " + ex);


        //    }
        //}

        private void ExcluiSeguroBD(int _EmpresaSeguroID) // alterar para xml
        {
            try
            {

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasSeguros where EmpresaSeguroID=" + _EmpresaSeguroID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiSeguroBD ex: " + ex);
                

            }
        }
        #endregion

        #region Metodos privados
        private string CriaXmlImagem(int empresaSeguroID)
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseArquivosImagens = _xmlDocument.CreateElement("ClasseArquivosImagens");
                _xmlDocument.AppendChild(_ClasseArquivosImagens);

                XmlNode _ArquivosImagens = _xmlDocument.CreateElement("ArquivosImagens");
                _ClasseArquivosImagens.AppendChild(_ArquivosImagens);


                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                SqlCommand SQCMDXML = new SqlCommand("Select * From EmpresasSeguros Where EmpresaSeguroID = " + empresaSeguroID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    //XmlNode _ArquivoImagemID = _xmlDocument.CreateElement("ArquivoImagemID");
                    //_ArquivoImagemID.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["EmpresaSeguroID"].ToString())));
                    //_ArquivoImagem.AppendChild(_ArquivoImagemID);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Arquivo"].ToString())));
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


    }
}
