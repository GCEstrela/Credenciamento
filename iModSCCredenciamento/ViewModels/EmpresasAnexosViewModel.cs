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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace iModSCCredenciamento.ViewModels
{
    public class EmpresasAnexosViewModel : ViewModelBase
    {

        #region Inicializacao
        public EmpresasAnexosViewModel()
        {

        }
        #endregion


        #region Variaveis Privadas
        
        private ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> _Anexos;

        private ClasseEmpresasAnexos.EmpresaAnexo _AnexoSelecionado;

        private ClasseEmpresasAnexos.EmpresaAnexo _anexoTemp = new ClasseEmpresasAnexos.EmpresaAnexo();

        private List<ClasseEmpresasAnexos.EmpresaAnexo> _AnexosTemp = new List<ClasseEmpresasAnexos.EmpresaAnexo>();

        PopupPesquisaEmpresasAnexos popupPesquisaEmpresasAnexos;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> Anexos
        {
            get
            {
                return _Anexos;
            }
            
            set
            {
                if (_Anexos != value)
                {
                    _Anexos = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseEmpresasAnexos.EmpresaAnexo AnexoSelecionado
        {
            get
            {
                return this._AnexoSelecionado;
            }
            set
            {
                this._AnexoSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (AnexoSelecionado != null)
                {
                    //OnEmpresaSelecionada();
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
            Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoAnexos(Convert.ToInt32(empresaID)));
            CarregaColecaoAnexos_thr.Start();
            //CarregaColecaoAnexos(Convert.ToInt32(empresaID));
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
                _arquivoPDF.Filter = "(*.pdf)|*.pdf|All Files (*.*)|*.*";
                _arquivoPDF.RestoreDirectory = true;
                _arquivoPDF.ShowDialog();

                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                _anexoTemp.NomeAnexo = _nomecompletodoarquivo;
                _anexoTemp.Anexo = _arquivoSTR;

                if (Anexos != null)
                {
                    Anexos[0].NomeAnexo = _nomecompletodoarquivo;
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
                    string _ArquivoPDF=null;
                    if (_anexoTemp != null)
                    {
                        if (_anexoTemp.Anexo != null && _anexoTemp.EmpresaAnexoID == AnexoSelecionado.EmpresaAnexoID)
                        {
                            _ArquivoPDF = _anexoTemp.Anexo;

                        }
                    }
                    if (_ArquivoPDF == null)
                    {
                        string _xmlstring = CriaXmlImagem(AnexoSelecionado.EmpresaAnexoID);

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
                    

                }
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
                _anexoTemp = AnexoSelecionado.CriaCopia(AnexoSelecionado);
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
                Anexos[_selectedIndexTemp] = _anexoTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasAnexos));

                ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> _EmpresasAnexosTemp = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>();
                ClasseEmpresasAnexos _ClasseEmpresasAnexosTemp = new ClasseEmpresasAnexos();
                _EmpresasAnexosTemp.Add(AnexoSelecionado);
                _ClasseEmpresasAnexosTemp.EmpresasAnexos = _EmpresasAnexosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasAnexosTemp);
                        xmlString = sw.ToString();
                    }

                }

                InsereAnexoBD(xmlString);

                _ClasseEmpresasAnexosTemp = null;

                _AnexosTemp.Clear();
                _anexoTemp = null;


            }
            catch (Exception ex)
            {
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Anexos)
                {
                    _AnexosTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Anexos.Clear();
                //ClasseEmpresasSeguros.EmpresaSeguro _seguro = new ClasseEmpresasSeguros.EmpresaSeguro();
                //_seguro.EmpresaID = EmpresaSelecionadaID;
                //Seguros.Add(_seguro);
                _anexoTemp = new ClasseEmpresasAnexos.EmpresaAnexo();
                _anexoTemp.EmpresaID = EmpresaSelecionadaID;
                Anexos.Add(_anexoTemp);
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasAnexos));

                ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo> _EmpresasAnexosPro = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>();
                ClasseEmpresasAnexos _ClasseEmpresasAnexosPro = new ClasseEmpresasAnexos();
                _EmpresasAnexosPro.Add(AnexoSelecionado);
                _ClasseEmpresasAnexosPro.EmpresasAnexos = _EmpresasAnexosPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasAnexosPro);
                        xmlString = sw.ToString();
                    }

                }

                InsereAnexoBD(xmlString);
                int _empresaID = AnexoSelecionado.EmpresaID;
                Thread CarregaColecaoAnexos_thr = new Thread(() => CarregaColecaoAnexos(_empresaID));
                CarregaColecaoAnexos_thr.Start();
                _AnexosTemp.Add(AnexoSelecionado);
                Anexos = null;
                Anexos = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>(_AnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _AnexosTemp.Clear();
                _ClasseEmpresasAnexosPro = null;

                _AnexosTemp.Clear();
                _anexoTemp = null;


            }
            catch (Exception ex)
            {
            }
        }
        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Anexos = null;
                Anexos = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>(_AnexosTemp);
                SelectedIndex = _selectedIndexTemp;
                _AnexosTemp.Clear();
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
                //        ExcluiSeguroBD(AnexoSelecionado.EmpresaAnexoID);
                //        Anexos.Remove(AnexoSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiSeguroBD(AnexoSelecionado.EmpresaAnexoID);
                        Anexos.Remove(AnexoSelecionado);
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
                popupPesquisaEmpresasAnexos = new PopupPesquisaEmpresasAnexos();
                popupPesquisaEmpresasAnexos.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaEmpresasAnexos.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            object vetor = popupPesquisaEmpresasAnexos.Criterio.Split((char)(20));
            int _empresaID = EmpresaSelecionadaID;
            string _descricao = ((string[])vetor)[0];
            CarregaColecaoAnexos(_empresaID, _descricao);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoAnexos(int empresaID, string _descricao = "")
        {
            try
            {
                string _xml = RequisitaAnexos(empresaID, _descricao);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasAnexos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresasAnexos classeAnexosEmpresa = new ClasseEmpresasAnexos();
                classeAnexosEmpresa = (ClasseEmpresasAnexos)deserializer.Deserialize(reader);
                Anexos = new ObservableCollection<ClasseEmpresasAnexos.EmpresaAnexo>();
                Anexos = classeAnexosEmpresa.EmpresasAnexos;
                SelectedIndex =-1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaAnexos(int _empresaID, string _DescricaoAnexo = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasAnexos = _xmlDocument.CreateElement("ClasseEmpresasAnexos");
                _xmlDocument.AppendChild(_ClasseEmpresasAnexos);

                XmlNode _EmpresasAnexos = _xmlDocument.CreateElement("EmpresasAnexos");
                _ClasseEmpresasAnexos.AppendChild(_EmpresasAnexos);

                string _strSql;

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                

                _DescricaoAnexo = "%" + _DescricaoAnexo + "%";

                _strSql = "select [EmpresaAnexoID],[EmpresaID],[Descricao],[NomeAnexo] " +
                    "from EmpresasAnexos where EmpresaID = " + _empresaID + " and Descricao Like '" +
                    _DescricaoAnexo  + "' order by EmpresaAnexoID desc";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _EmpresaAnexo = _xmlDocument.CreateElement("EmpresaAnexo");
                    _EmpresasAnexos.AppendChild(_EmpresaAnexo);

                    XmlNode _EmpresaAnexoID = _xmlDocument.CreateElement("EmpresaAnexoID");
                    _EmpresaAnexoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaAnexoID"].ToString())));
                    _EmpresaAnexo.AppendChild(_EmpresaAnexoID);

                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _EmpresaAnexo.AppendChild(_EmpresaID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString())));
                    _EmpresaAnexo.AppendChild(_Descricao);

                    XmlNode _NomeAnexo = _xmlDocument.CreateElement("NomeAnexo");
                    _NomeAnexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeAnexo"].ToString())));
                    _EmpresaAnexo.AppendChild(_NomeAnexo);

                    XmlNode _Anexo = _xmlDocument.CreateElement("Anexo");
                    //_Anexo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Anexo"].ToString())));
                    _EmpresaAnexo.AppendChild(_Anexo);

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

        private void InsereAnexoBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                //
                ClasseEmpresasAnexos.EmpresaAnexo _empresaAnexo = new ClasseEmpresasAnexos.EmpresaAnexo();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;

                _empresaAnexo.EmpresaAnexoID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaAnexoID")[i].InnerText);
                _empresaAnexo.EmpresaID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _empresaAnexo.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText.ToString().Trim();
                //_empresaAnexo.NomeAnexo = _xmlDoc.GetElementsByTagName("NomeSeguradora")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NomeSeguradora")[i].InnerText;
                //_empresaAnexo.NumeroApolice = _xmlDoc.GetElementsByTagName("NumeroApolice")[i] == null ? "" : _xmlDoc.GetElementsByTagName("NumeroApolice")[i].InnerText;
                //_empresaSeguro.ValorCobertura = _xmlDoc.GetElementsByTagName("ValorCobertura")[i] == null ? "" : _xmlDoc.GetElementsByTagName("ValorCobertura")[i].InnerText;
                //_empresaSeguro.Arquivo = _xmlDoc.GetElementsByTagName("Arquivo")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Arquivo")[i].InnerText;

                _empresaAnexo.NomeAnexo = _anexoTemp.NomeAnexo == null ? "" : _anexoTemp.NomeAnexo.ToString().Trim();
                _empresaAnexo.Anexo = _anexoTemp.Anexo == null ? "" : _anexoTemp.Anexo.ToString().Trim();


                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                //_Con.Close();
                

                SqlCommand _sqlCmd;
                if (_empresaAnexo.EmpresaAnexoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update EmpresasAnexos Set" +
                        " EmpresaID= " + _empresaAnexo.EmpresaID + "" +
                        ",Descricao= '" + _empresaAnexo.Descricao + "'" +
                        ",NomeAnexo= '" + _empresaAnexo.NomeAnexo + "'" +
                        ",Anexo= '" + _empresaAnexo.Anexo + "'" +
                        " Where EmpresaAnexoID = " + _empresaAnexo.EmpresaAnexoID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into EmpresasAnexos(EmpresaID,Descricao,NomeAnexo,Anexo) values (" +
                                                          _empresaAnexo.EmpresaID + ",'" + _empresaAnexo.Descricao + "','" +
                                                          _empresaAnexo.NomeAnexo + "','" + _empresaAnexo.Anexo + "')", _Con);


                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresaBD ex: " + ex);


            }
        }

        private void ExcluiSeguroBD(int _EmpresaAnexoID) // alterar para xml
        {
            try
            {

                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();
                //_Con.Close();
                

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasAnexos where EmpresaAnexoID=" + _EmpresaAnexoID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void EmpresasAnexos ex: " + ex);

            }
        }
        #endregion
        
        #region Metodos privados
        private string CriaXmlImagem(int empresaAnexoID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From EmpresasAnexos Where EmpresaAnexoID = " + empresaAnexoID + "", _Con);
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
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Anexo"].ToString())));
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
