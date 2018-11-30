using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;

namespace iModSCCredenciamento.ViewModels
{

    public class EmpresasSignatariosViewModel : ViewModelBase
    {
        public EmpresasSignatariosViewModel()
        {
            //CarregaColecaoEmpresasSignatarios(1);
        }

        #region Variaveis Privadas

        private ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> _Signatarios;

        private ClasseEmpresasSignatarios.EmpresaSignatario _SignatarioSelecionado;

        private ClasseEmpresasSignatarios.EmpresaSignatario _signatarioTemp = new ClasseEmpresasSignatarios.EmpresaSignatario();

        private List<ClasseEmpresasSignatarios.EmpresaSignatario> _SignatarioTemp = new List<ClasseEmpresasSignatarios.EmpresaSignatario>();

        //private ObservableCollection<ClasseEstados.Estado> _Estados;

        //private ObservableCollection<ClasseMunicipios.Municipio> _Municipios;

        //private ObservableCollection<ClasseStatus.Status> _Statuss;

        //private ObservableCollection<ClasseTiposAcessos.TipoAcesso> _TiposAcessos;

        //private ObservableCollection<ClasseTiposCobrancas.TipoCobranca> _TiposCobrancas;

        PopupPesquisaContrato PopupPesquisaSignatarios;

        private int _selectedIndex;

        private int _EmpresaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores
        public ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> Signatarios
        {
            get
            {
                return _Signatarios;
            }

            set
            {
                if (_Signatarios != value)
                {
                    _Signatarios = value;
                    OnPropertyChanged();

                }
            }
        }

        //public ObservableCollection<ClasseEstados.Estado> Estados
        //{
        //    get
        //    {
        //        return _Estados;
        //    }

        //    set
        //    {
        //        if (_Estados != value)
        //        {
        //            _Estados = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseMunicipios.Municipio> Municipios
        //{
        //    get
        //    {
        //        return _Municipios;
        //    }

        //    set
        //    {
        //        if (_Municipios != value)
        //        {
        //            _Municipios = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseStatus.Status> Statuss
        //{
        //    get
        //    {
        //        return _Statuss;
        //    }

        //    set
        //    {
        //        if (_Statuss != value)
        //        {
        //            _Statuss = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseTiposAcessos.TipoAcesso> TiposAcessos
        //{
        //    get
        //    {
        //        return _TiposAcessos;
        //    }

        //    set
        //    {
        //        if (_TiposAcessos != value)
        //        {
        //            _TiposAcessos = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        //public ObservableCollection<ClasseTiposCobrancas.TipoCobranca> TiposCobrancas
        //{
        //    get
        //    {
        //        return _TiposCobrancas;
        //    }

        //    set
        //    {
        //        if (_TiposCobrancas != value)
        //        {
        //            _TiposCobrancas = value;
        //            OnPropertyChanged();

        //        }
        //    }
        //}

        public ClasseEmpresasSignatarios.EmpresaSignatario SignatarioSelecionado
        {
            get
            {
                return this._SignatarioSelecionado;
            }
            set
            {
                this._SignatarioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (SignatarioSelecionado != null)
                {
                    OnSignatarioSelecionado();
                }

            }
        }

        private void OnSignatarioSelecionado()
        {
            try
            {



            }
            catch (Exception ex)
            {
                //Global.Log("Erro void OnEmpresaSelecionada ex: " + ex.Message);
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
            Thread CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(Convert.ToInt32(empresaID)));
            CarregaColecaoEmpresasSignatarios_thr.Start();
            //CarregaColecaoEmpresasSignatarios(Convert.ToInt32(empresaID));
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
                //if (_arquivoPDF.ShowDialog()) //System.Windows.Forms.DialogResult.Yes
                //{
                _nomecompletodoarquivo = _arquivoPDF.SafeFileName;
                _arquivoSTR = Conversores.PDFtoString(_arquivoPDF.FileName);
                _signatarioTemp.Assinatura = _arquivoSTR;


            }
            catch (Exception ex)
            {

            }
        }

        public void OnAbrirArquivoCommand()
        {
            try
            {

                string _ArquivoPDF = null;
                if (_signatarioTemp != null)
                {
                    if (_signatarioTemp.Assinatura != null && _signatarioTemp.EmpresaSignatarioID == SignatarioSelecionado.EmpresaSignatarioID)
                    {
                        _ArquivoPDF = _signatarioTemp.Assinatura;

                    }
                }
                if (string.IsNullOrWhiteSpace(_ArquivoPDF))
                {
                    string _xmlstring = CriaXmlImagem(SignatarioSelecionado.EmpresaSignatarioID);

                    XmlDocument xmldocument = new XmlDocument();
                    xmldocument.LoadXml(_xmlstring);
                    XmlNode node = (XmlNode)xmldocument.DocumentElement;
                    XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                    _ArquivoPDF = arquivoNode.FirstChild.Value;
                }

                Global.PopupPDF(_ArquivoPDF, false);


                //byte[] buffer = Conversores.StringToPDF(_ArquivoPDF);
                //_ArquivoPDF = System.IO.Path.GetTempFileName();
                //_ArquivoPDF = System.IO.Path.GetRandomFileName();
                //_ArquivoPDF = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoPDF;

                ////File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
                //_ArquivoPDF = System.IO.Path.ChangeExtension(_ArquivoPDF, ".pdf");
                //System.IO.File.WriteAllBytes(_ArquivoPDF, buffer);
                ////Action<string> act = new Action<string>(Global.AbrirArquivoPDF);
                ////act.BeginInvoke(_ArquivoPDF, null, null);
                //Global.PopupPDF(_ArquivoPDF,false);
                //System.IO.File.Delete(_ArquivoPDF);
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
                _signatarioTemp = SignatarioSelecionado.CriaCopia(SignatarioSelecionado);
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
                Signatarios[_selectedIndexTemp] = _signatarioTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasSignatarios));

                ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> _EmpresasSignatariosTemp = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>();
                ClasseEmpresasSignatarios _ClasseEmpresasSegnatariosTemp = new ClasseEmpresasSignatarios();
                _EmpresasSignatariosTemp.Add(SignatarioSelecionado);
                _ClasseEmpresasSegnatariosTemp.EmpresasSignatarios = _EmpresasSignatariosTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasSegnatariosTemp);
                        xmlString = sw.ToString();
                    }
                }

                InsereEmpresasSignatariosBD(xmlString);

                _ClasseEmpresasSegnatariosTemp = null;

                _EmpresasSignatariosTemp.Clear();
                //_contratoTemp = null;


            }
            catch (Exception ex)
            {
            }
        }

        public void OnAdicionarCommand()
        {
            try
            {
                foreach (var x in Signatarios)
                {
                    _SignatarioTemp.Add(x);
                }

                _selectedIndexTemp = SelectedIndex;
                Signatarios.Clear();

                _signatarioTemp = new ClasseEmpresasSignatarios.EmpresaSignatario();
                _signatarioTemp.EmpresaId = EmpresaSelecionadaID;
                Signatarios.Add(_signatarioTemp);
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClasseEmpresasSignatarios));

                ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario> _EmpresasSignatariosPro = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>();
                ClasseEmpresasSignatarios _ClasseEmpresasSegnatariosPro = new ClasseEmpresasSignatarios();
                _EmpresasSignatariosPro.Add(SignatarioSelecionado);
                _ClasseEmpresasSegnatariosPro.EmpresasSignatarios = _EmpresasSignatariosPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClasseEmpresasSegnatariosPro);
                        xmlString = sw.ToString();
                    }
                }

                InsereEmpresasSignatariosBD(xmlString);
                Thread CarregaColecaoEmpresasSignatarios_thr = new Thread(() => CarregaColecaoEmpresasSignatarios(SignatarioSelecionado.EmpresaId));
                CarregaColecaoEmpresasSignatarios_thr.Start();
                //_SignatarioTemp.Add(SignatarioSelecionado);
                //Signatarios = null;
                //Signatarios = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>(_SignatarioTemp);
                //SelectedIndex = _selectedIndexTemp;
                //_SignatarioTemp.Clear();
                //_ClasseEmpresasSegnatariosPro = null;

                //_EmpresasSignatariosPro.Clear();



            }
            catch (Exception ex)
            {
            }
        }

        public void OnCancelarAdicaoCommand()
        {
            try
            {
                Signatarios = null;
                Signatarios = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>(_SignatarioTemp);
                SelectedIndex = _selectedIndexTemp;
                _SignatarioTemp.Clear();
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
                //if (System.Windows.Forms.MessageBox.Show("Tem certeza que deseja excluir este signatário?", "Excluir Signatário", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //    if (System.Windows.Forms.MessageBox.Show("Você perderá todos os dados deste signatário, inclusive histórico. Confirma exclusão?", "Excluir Signatário", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        ExcluiEmpresasSignatariosBD(SignatarioSelecionado.EmpresaSignatarioID);
                //        Signatarios.Remove(SignatarioSelecionado);

                //    }
                //}
                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {
                    if (Global.PopupBox("Você perderá todos os dados, inclusive histórico. Confirma exclusão?", 2))
                    {
                        ExcluiEmpresasSignatariosBD(SignatarioSelecionado.EmpresaSignatarioID);
                        Signatarios.Remove(SignatarioSelecionado);
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
                PopupPesquisaSignatarios popupPesquisaSegnatarios = new PopupPesquisaSignatarios();
                popupPesquisaSegnatarios.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                popupPesquisaSegnatarios.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            int _empresaID = EmpresaSelecionadaID;
            string _nome = ((iModSCCredenciamento.Windows.PopupPesquisaSignatarios)sender).Criterio.Trim();
            CarregaColecaoEmpresasSignatarios(_empresaID, _nome);
            //CarregaColecaoContratos(_empresaID, _descricao, _numerocontrato);
            SelectedIndex = 0;
        }

        #endregion

        #region Carregamento das Colecoes
        private void CarregaColecaoEmpresasSignatarios(int empresaID, string nome = "")
        {
            try
            {




                //string _xml = RequisitaEmpresasSignatarios(empresaID, nome);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasSignatarios));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseEmpresasSignatarios classeEmpresaSignatarios = new ClasseEmpresasSignatarios();
                //classeEmpresaSignatarios = (ClasseEmpresasSignatarios)deserializer.Deserialize(reader);
                //Signatarios = new ObservableCollection<ClasseEmpresasSignatarios.EmpresaSignatario>();
                //Signatarios = classeEmpresaSignatarios.EmpresasSignatarios;

                //Mihai(30/11/2018 - 13:54)
                SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }
        #endregion

        #region Data Access
        private string RequisitaEmpresasSignatarios(int _empresaID, string _nome = "")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasSignatarios = _xmlDocument.CreateElement("ClasseEmpresasSignatarios");
                _xmlDocument.AppendChild(_ClasseEmpresasSignatarios);

                XmlNode _EmpresasSignatarios = _xmlDocument.CreateElement("EmpresasSignatarios");
                _ClasseEmpresasSignatarios.AppendChild(_EmpresasSignatarios);

                string _strSql;

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _nome = "%" + _nome + "%";
                //_numerocontrato = "%" + _numerocontrato + "%";

                _strSql = "select * from EmpresasSignatarios where EmpresaID = " + _empresaID + " and Nome Like '" + _nome + "' order by Principal desc";

                //and Descricao Like '" + _descricao + "' and NumeroContrato Like '" + _numerocontrato + "'
                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _EmpresaSignatario = _xmlDocument.CreateElement("EmpresaSignatario");
                    _EmpresasSignatarios.AppendChild(_EmpresaSignatario);

                    XmlNode _EmpresaSignatarioID = _xmlDocument.CreateElement("EmpresaSignatarioID");
                    _EmpresaSignatarioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaSignatarioID"].ToString())));
                    _EmpresaSignatario.AppendChild(_EmpresaSignatarioID);

                    XmlNode _EmpresaId = _xmlDocument.CreateElement("EmpresaId");
                    _EmpresaId.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaId"].ToString())));
                    _EmpresaSignatario.AppendChild(_EmpresaId);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString().Trim())));
                    _EmpresaSignatario.AppendChild(_Nome);

                    XmlNode _CPF = _xmlDocument.CreateElement("CPF");
                    _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString().Trim())));
                    _EmpresaSignatario.AppendChild(_CPF);

                    XmlNode _Email = _xmlDocument.CreateElement("Email");
                    _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString().Trim())));
                    _EmpresaSignatario.AppendChild(_Email);

                    XmlNode _Telefone = _xmlDocument.CreateElement("Telefone");
                    _Telefone.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone"].ToString().Trim())));
                    _EmpresaSignatario.AppendChild(_Telefone);

                    XmlNode _Celular = _xmlDocument.CreateElement("Celular");
                    _Celular.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular"].ToString().Trim())));
                    _EmpresaSignatario.AppendChild(_Celular);

                    XmlNode _Principal = _xmlDocument.CreateElement("Principal");
                    _Principal.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Principal"])).ToString()));
                    _EmpresaSignatario.AppendChild(_Principal);

                    XmlNode _Assinatura = _xmlDocument.CreateElement("Assinatura");
                    //_Assinatura.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Assinatura"].ToString())));
                    _EmpresaSignatario.AppendChild(_Assinatura);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception er)
            {

                return null;
            }

        }

        private void InsereEmpresasSignatariosBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();

                _xmlDoc.LoadXml(xmlString);
                //
                ClasseEmpresasSignatarios.EmpresaSignatario _EmpresaSignatario = new ClasseEmpresasSignatarios.EmpresaSignatario();
                //for (int i = 0; i <= _xmlDoc.GetElementsByTagName("EmpresaID").Count - 1; i++)
                //{
                int i = 0;
                _EmpresaSignatario.EmpresaSignatarioID = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaSignatarioID")[i].InnerText);
                _EmpresaSignatario.EmpresaId = Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaId")[i].InnerText);

                _EmpresaSignatario.Nome = _xmlDoc.GetElementsByTagName("Nome")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("Nome")[i].InnerText);
                _EmpresaSignatario.CPF = _xmlDoc.GetElementsByTagName("CPF")[i] == null ? "" : (_xmlDoc.GetElementsByTagName("CPF")[i].InnerText);
                _EmpresaSignatario.Email = _xmlDoc.GetElementsByTagName("Email")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Email")[i].InnerText;
                _EmpresaSignatario.Telefone = _xmlDoc.GetElementsByTagName("Telefone")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Telefone")[i].InnerText;
                _EmpresaSignatario.Celular = _xmlDoc.GetElementsByTagName("Celular")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Celular")[i].InnerText;
                //_EmpresaSignatario.Principal = _xmlDoc.GetElementsByTagName("Principal")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("Principal")[i].InnerText);
                bool _Principal;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Principal")[i].InnerText, out _Principal);

                _EmpresaSignatario.Principal = _xmlDoc.GetElementsByTagName("Principal")[i] == null ? false : _Principal;
                _EmpresaSignatario.Assinatura = _signatarioTemp.Assinatura == null ? "" : _signatarioTemp.Assinatura;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                //_Con.Close();


                SqlCommand _sqlCmd;
                if (_EmpresaSignatario.EmpresaSignatarioID != 0)
                {
                    _sqlCmd = new SqlCommand("Update EmpresasSignatarios Set" +
                        " EmpresaId= " + _EmpresaSignatario.EmpresaId + "" +
                        ",Nome= '" + _EmpresaSignatario.Nome.ToString().Trim() + "'" +
                        ",CPF= '" + _EmpresaSignatario.CPF.ToString().Trim() + "'" +
                        ",Email= '" + _EmpresaSignatario.Email.ToString().Trim() + "'" +
                        ",Telefone= '" + _EmpresaSignatario.Telefone.ToString().Trim() + "'" +
                        ",Celular= '" + _EmpresaSignatario.Celular.ToString().Trim() + "'" +
                        ",Principal= '" + _EmpresaSignatario.Principal + "'" +
                        ",Assinatura= '" + _EmpresaSignatario.Assinatura + "'" +
                        " Where EmpresaSignatarioID = " + _EmpresaSignatario.EmpresaSignatarioID + "", _Con);
                }
                else
                {
                    _sqlCmd = new SqlCommand("Insert into EmpresasSignatarios(EmpresaID,Nome,CPF,Email,Telefone,Celular,Principal,Assinatura) values (" +
                        _EmpresaSignatario.EmpresaId + ",'" +
                        _EmpresaSignatario.Nome.ToString().Trim() + "','" +
                        _EmpresaSignatario.CPF.ToString().Trim() + "','" +
                        _EmpresaSignatario.Email.ToString().Trim() + "','" +
                        _EmpresaSignatario.Telefone.ToString().Trim() + "','" +
                        _EmpresaSignatario.Celular.ToString().Trim() + "','" +
                        _EmpresaSignatario.Principal + "','" +
                        _EmpresaSignatario.Assinatura.ToString().Trim() + "')", _Con);

                }

                _sqlCmd.ExecuteNonQuery();
                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InsereEmpresasSignatariosBD ex: " + ex);


            }
        }

        private void ExcluiEmpresasSignatariosBD(int _EmpresaSignatarioID) // alterar para xml
        {
            try
            {

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                //_Con.Close();


                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from EmpresasSignatarios where EmpresaSignatarioID=" + _EmpresaSignatarioID, _Con);
                _sqlCmd.ExecuteNonQuery();

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiEmpresasSignatariosBD ex: " + ex);


            }
        }
        #endregion

        #region Metodos privados
        private string CriaXmlImagem(int empresaSignatarioID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From EmpresasSignatarios Where EmpresaSignatarioID = " + empresaSignatarioID + "", _Con);
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
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["Assinatura"].ToString())));
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
