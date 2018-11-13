using iModSCCredenciamento.Funcoes;
using Microsoft.Win32;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.ViewModels;
using iModSCCredenciamento.Views;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Windows;

namespace iModSCCredenciamento.ViewModels
{
    class VeiculosSegurosViewModel : ViewModelBase
    {
        #region Inicializacao
        public VeiculosSegurosViewModel()
        {

        }
        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> _Seguros;

        private ClasseVeiculosSeguros.VeiculoSeguro _SeguroSelecionado;

        private ClasseVeiculosSeguros.VeiculoSeguro _seguroTemp = new ClasseVeiculosSeguros.VeiculoSeguro();

        private List<ClasseVeiculosSeguros.VeiculoSeguro> _SegurosTemp = new List<ClasseVeiculosSeguros.VeiculoSeguro>();

        PopupPesquisaSeguro popupPesquisaSeguro;

        private int _selectedIndex;

        private int _VeiculoSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> Seguros
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

        public ClasseVeiculosSeguros.VeiculoSeguro SeguroSelecionado
        {
            get
            {
                return this._SeguroSelecionado;
            }
            set
            {
                this._SeguroSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (SeguroSelecionado != null)
                {
                    //OnVeiculoSelecionada();
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
                    //OnVeiculoSelecionada();
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
        public void OnAtualizaCommand(object veiculoID)
        {
            VeiculoSelecionadaID = Convert.ToInt32(veiculoID);
            Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(Convert.ToInt32(veiculoID)));
            CarregaColecaoSeguros_thr.Start();
            //CarregaColecaoSeguros(Convert.ToInt32(veiculoID));
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
                _seguroTemp.NomeArquivo = _nomecompletodoarquivo;
                _seguroTemp.Arquivo = _arquivoSTR;

                if (Seguros != null)
                    Seguros[0].NomeArquivo = _nomecompletodoarquivo;
                //InsereArquivoBD(Convert.ToInt32(veiculoID), _nomecompletodoarquivo, _arquivoSTR);

                //AtualizaListaAnexos(_resp);

                //}
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
                    string _ArquivoPDF;
                    if (_seguroTemp.Arquivo != null && _seguroTemp.VeiculoSeguroID == SeguroSelecionado.VeiculoSeguroID)
                    {
                        _ArquivoPDF = _seguroTemp.Arquivo;

                    }
                    else
                    {
                        string _xmlstring = CriaXmlImagem(SeguroSelecionado.VeiculoSeguroID);

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
                    Global.Log("Erro na void ListaARQColaboradorAnexo_lv_PreviewMouseDoubleClick ex: " + ex);

                }
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosSeguros));

                ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> _VeiculosSegurosTemp = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>();
                ClasseVeiculosSeguros _ClasseVeiculosSegurosTemp = new ClasseVeiculosSeguros();
                _VeiculosSegurosTemp.Add(SeguroSelecionado);
                _ClasseVeiculosSegurosTemp.VeiculosSeguros = _VeiculosSegurosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculosSegurosTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereSeguroBD(xmlString);

                _ClasseVeiculosSegurosTemp = null;

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
                //ClasseVeiculosSeguros.VeiculoSeguro _seguro = new ClasseVeiculosSeguros.VeiculoSeguro();
                //_seguro.VeiculoID = VeiculoSelecionadaID;
                //Seguros.Add(_seguro);
                _seguroTemp = new ClasseVeiculosSeguros.VeiculoSeguro();
                _seguroTemp.VeiculoID = VeiculoSelecionadaID;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseVeiculosSeguros));

                ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro> _VeiculosSegurosPro = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>();
                ClasseVeiculosSeguros _ClasseVeiculosSegurosPro = new ClasseVeiculosSeguros();
                _VeiculosSegurosPro.Add(SeguroSelecionado);
                _ClasseVeiculosSegurosPro.VeiculosSeguros = _VeiculosSegurosPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseVeiculosSegurosPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereSeguroBD(xmlString);
                Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoSeguros(SeguroSelecionado.VeiculoID));
                CarregaColecaoSeguros_thr.Start();
                //_SegurosTemp.Add(SeguroSelecionado);
                //Seguros = null;
                //Seguros = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>(_SegurosTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_SegurosTemp.Clear();
                //_ClasseVeiculosSegurosPro = null;

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
                Seguros = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>(_SegurosTemp);
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
                //        ExcluiSeguroBD(SeguroSelecionado.VeiculoSeguroID);
                //        Seguros.Remove(SeguroSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiSeguroBD(SeguroSelecionado.VeiculoSeguroID);
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
                popupPesquisaSeguro.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaSeguro.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaSeguro.Criterio.Split((char)(20));
            int _veiculoID = VeiculoSelecionadaID;
            string _seguradora = ((string[])vetor)[0];
            string _numeroapolice = ((string[])vetor)[1];
            CarregaColecaoSeguros(_veiculoID, _seguradora, _numeroapolice);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoSeguros(int veiculoID, string _seguradora = "", string _numeroapolice = "")
        {
            try
            {
                string _xml = RequisitaSeguros(veiculoID, _seguradora, _numeroapolice);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseVeiculosSeguros));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseVeiculosSeguros classeSegurosVeiculo = new ClasseVeiculosSeguros();
                classeSegurosVeiculo = (ClasseVeiculosSeguros)deserializer.Deserialize(reader);
                Seguros = new ObservableCollection<ClasseVeiculosSeguros.VeiculoSeguro>();
                Seguros = classeSegurosVeiculo.VeiculosSeguros;
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoVeiculos ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaSeguros(int _veiculoID, string _seguradora = "", string _numeroapolice = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseSegurosVeiculos = _xmlDocument.CreateElement("ClasseVeiculosSeguros");
                _xmlDocument.AppendChild(_ClasseSegurosVeiculos);

                XmlNode _Seguros = _xmlDocument.CreateElement("VeiculosSeguros");
                _ClasseSegurosVeiculos.AppendChild(_Seguros);

                string _strSql;

                //
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _seguradora = "%" + _seguradora + "%";
                _numeroapolice = "%" + _numeroapolice + "%";

                _strSql = "select [VeiculoSeguroID],[NomeSeguradora],[NumeroApolice],[ValorCobertura],[VeiculoID],[NomeArquivo],[Emissao],[Validade]" +
                    " from VeiculosSeguros where VeiculoID = " + _veiculoID + " and NomeSeguradora Like '" + _seguradora +
                    "' and NumeroApolice Like '" + _numeroapolice + "'    order by VeiculoSeguroID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Seguro = _xmlDocument.CreateElement("VeiculoSeguro");
                    _Seguros.AppendChild(_Seguro);

                    XmlNode _VeiculoSeguroID = _xmlDocument.CreateElement("VeiculoSeguroID");
                    _VeiculoSeguroID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoSeguroID"].ToString())));
                    _Seguro.AppendChild(_VeiculoSeguroID);

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

                    XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    _Seguro.AppendChild(_VeiculoID);

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

        private void InsereSeguroBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                // SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                ClasseVeiculosSeguros.VeiculoSeguro _veiculoSeguro = new ClasseVeiculosSeguros.VeiculoSeguro();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("VeiculoID").Count - 1; i++)
                //{
                int i = 0;

                _veiculoSeguro.VeiculoSeguroID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoSeguroID")[i].InnerText);
                _veiculoSeguro.VeiculoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);
                _veiculoSeguro.NomeArquivo = _xmlDoc.GetElementsByTagName("NomeArquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeArquivo")[i].InnerText;
                _veiculoSeguro.NomeSeguradora = _xmlDoc.GetElementsByTagName("NomeSeguradora")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeSeguradora")[i].InnerText;
                _veiculoSeguro.NumeroApolice = _xmlDoc.GetElementsByTagName("NumeroApolice")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroApolice")[i].InnerText;
                _veiculoSeguro.ValorCobertura = _xmlDoc.GetElementsByTagName("ValorCobertura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ValorCobertura")[i].InnerText;
                _veiculoSeguro.Emissao = _xmlDoc.GetElementsByTagName("Emissao")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Emissao")[i].InnerText);
                _veiculoSeguro.Validade = _xmlDoc.GetElementsByTagName("Validade")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("Validade")[i].InnerText);

                //_veiculoSeguro.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;

                _veiculoSeguro.NomeArquivo = _seguroTemp.NomeArquivo == null ? "" : _seguroTemp.NomeArquivo;
                _veiculoSeguro.Arquivo = _seguroTemp.Arquivo == null ? "" : _seguroTemp.Arquivo;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_veiculoSeguro.VeiculoSeguroID != 0)
                {


                    _sqlCmd = new SqlCommand("Update VeiculosSeguros Set VeiculoID=@v1,NomeArquivo=@v2" +
                                             ",NomeSeguradora= @v3,NumeroApolice=@v4,ValorCobertura=@v5,Emissao=@v6,Validade=@v7,Arquivo=@v8 Where VeiculoSeguroID =@v0", _Con);

                    _sqlCmd.Parameters.Add("@V0", SqlDbType.Int).Value = _veiculoSeguro.VeiculoSeguroID;
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _veiculoSeguro.VeiculoID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _veiculoSeguro.NomeArquivo;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.VarChar).Value = _veiculoSeguro.NomeSeguradora;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _veiculoSeguro.NumeroApolice;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.VarChar).Value = _veiculoSeguro.ValorCobertura;
                    if (_veiculoSeguro.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = _veiculoSeguro.Emissao;
                    }
                    if (_veiculoSeguro.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = _veiculoSeguro.Validade;
                    }
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _veiculoSeguro.Arquivo;
                }
                else
                {


                    _sqlCmd = new SqlCommand("Insert into VeiculosSeguros(VeiculoID,NomeArquivo,NomeSeguradora,NumeroApolice,ValorCobertura,Emissao,Validade,Arquivo) values (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)", _Con);
                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _veiculoSeguro.VeiculoID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _veiculoSeguro.NomeArquivo;
                    _sqlCmd.Parameters.Add("@V3", SqlDbType.VarChar).Value = _veiculoSeguro.NomeSeguradora;
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.VarChar).Value = _veiculoSeguro.NumeroApolice;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.VarChar).Value = _veiculoSeguro.ValorCobertura;
                    if (_veiculoSeguro.Emissao == null)
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V6", SqlDbType.DateTime).Value = _veiculoSeguro.Emissao;
                    }
                    if (_veiculoSeguro.Validade == null)
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V7", SqlDbType.DateTime).Value = _veiculoSeguro.Validade;
                    }
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.VarChar).Value = _veiculoSeguro.Arquivo;


                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereVeiculoBD ex: " + ex);


            }
        }


        private void ExcluiSeguroBD(int _VeiculoSeguroID) // alterar para xml
        {
            try
            {

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from VeiculosSeguros where VeiculoSeguroID=" + _VeiculoSeguroID, _Con);
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
        private string CriaXmlImagem(int veiculoSeguroID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From VeiculosSeguros Where VeiculoSeguroID = " + veiculoSeguroID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    //XmlNode _ArquivoImagemID = _xmlDocument.CreateElement("ArquivoImagemID");
                    //_ArquivoImagemID.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["VeiculoSeguroID"].ToString())));
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
