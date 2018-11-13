using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
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
using System.Xml;
using System.Xml.Serialization;

namespace iModSCCredenciamento.ViewModels
{
    class PopupPendenciasViewModel : ViewModelBase
    {
        #region Inicializacao
        public PopupPendenciasViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();
        }
        private void CarregaUI()
        {
            CarregaColecaoTiposPendencias();

        }


        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClassePendencias.Pendencia> _Pendencias;

        private ClassePendencias.Pendencia _PendenciaSelecionada;

        private ClassePendencias.Pendencia _PendenciaTemp = new ClassePendencias.Pendencia();

        private List<ClassePendencias.Pendencia> _PendenciasTemp = new List<ClassePendencias.Pendencia>();

        private ObservableCollection<ClasseTiposPendencias.TipoPendencia> _TiposPendencias;

        private int _selectedIndex;

        private int _PendenciaSelecionadaID;

        private bool _HabilitaEdicao = false;

        private string _Criterios = "";

        private int _selectedIndexTemp = 0;

        #endregion

        #region Contrutores

        public ObservableCollection<ClasseTiposPendencias.TipoPendencia> TiposPendencias
        {
            get
            {
                return _TiposPendencias;
            }

            set
            {
                if (_TiposPendencias != value)
                {
                    _TiposPendencias = value;
                    OnPropertyChanged();

                }
            }
        }

        public ObservableCollection<ClassePendencias.Pendencia> Pendencias
        {
            get
            {
                return _Pendencias;
            }

            set
            {
                if (_Pendencias != value)
                {
                    _Pendencias = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClassePendencias.Pendencia PendenciaSelecionada
        {
            get
            {
                return this._PendenciaSelecionada;
            }
            set
            {
                this._PendenciaSelecionada = value;
                base.OnPropertyChanged("SelectedItem");
                if (PendenciaSelecionada != null)
                {
                    //OnEmpresaSelecionada();
                }

            }
        }

         public int PendenciaSelecionadaID
        {
            get
            {
                return this._PendenciaSelecionadaID;
            }
            set
            {
                this._PendenciaSelecionadaID = value;
                base.OnPropertyChanged();
                if (PendenciaSelecionadaID != null)
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
        public void OnAtualizaCommand(int Cadastro, int Tag = 0, int ID = 0)
        {

            Thread CarregaUI_thr = new Thread(() => CarregaColecaoPendencias(Cadastro, Tag,  ID));
            CarregaUI_thr.Start();
            SelectedIndex = 0;
            //PendenciaSelecionadaID = Convert.ToInt32(_pendenciaID);
            //Thread CarregaColecaoSeguros_thr = new Thread(() => CarregaColecaoColaboradorerAnexos(Convert.ToInt32(_pendenciaID)));
            //CarregaColecaoSeguros_thr.Start();


        }

        public void OnPesquisarCommand()
        {
            try
            {
                //popupPesquisaColaboradorCurso = new PopupPesquisaColaboradorCurso();
                //popupPesquisaColaboradorCurso.EfetuarProcura += new EventHandler(On_EfetuarProcura);
                //popupPesquisaColaboradorCurso.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        public void On_EfetuarProcura(object sender, EventArgs e)
        {
            //object vetor = popupPesquisaColaboradorCurso.Criterio.Split((char)(20));
            //int _colaboradorID = ColaboradorCursoSelecionadaID;
            //string _id = ((string[])vetor)[0];
            //string _numeroapolice = ((string[])vetor)[1];
            //CarregaColecaoColaboradorerCursos(Convert.ToInt32(_id), _numeroapolice);
            //SelectedIndex = 0;
        }

        public void OnEditarCommand()
        {
            try
            {
                _PendenciaTemp = PendenciaSelecionada.CriaCopia(PendenciaSelecionada);
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
                Pendencias[_selectedIndexTemp] = _PendenciaTemp;
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClassePendencias));

                ObservableCollection<ClassePendencias.Pendencia> _PendenciasTemp = new ObservableCollection<ClassePendencias.Pendencia>();
                ClassePendencias _ClassePendenciasTemp = new ClassePendencias();
                _PendenciasTemp.Add(PendenciaSelecionada);
                _ClassePendenciasTemp.Pendencias = _PendenciasTemp;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClassePendenciasTemp);
                        xmlString = sw.ToString();
                    }

                }

                InserePendenciaBD(xmlString);

                _PendenciasTemp = null;

                _PendenciasTemp.Clear();
                _PendenciaTemp = null;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void OnSalvarAdicaoCommand()
        {
            try
            {
                HabilitaEdicao = false;
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ClassePendencias));

                ObservableCollection<ClassePendencias.Pendencia> _PendenciasPro = new ObservableCollection<ClassePendencias.Pendencia>();
                ClassePendencias _ClassePendenciasPro = new ClassePendencias();
                _PendenciasPro.Add(PendenciaSelecionada);
                _ClassePendenciasPro.Pendencias = _PendenciasPro;

                string xmlString;

                using (StringWriterWithEncoding sw = new StringWriterWithEncoding(System.Text.Encoding.UTF8))
                {

                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xw.Formatting = Formatting.Indented;
                        serializer.Serialize(xw, _ClassePendenciasPro);
                        xmlString = sw.ToString();
                    }

                }

                InserePendenciaBD(xmlString);

                //Thread CarregaUI_thr = new Thread(() => CarregaColecaoPendencias(Cadastro, Tag, ID));
                //CarregaUI_thr.Start();
 
                _PendenciasTemp.Add(PendenciaSelecionada);
                Pendencias = null;
                Pendencias = new ObservableCollection<ClassePendencias.Pendencia>(_PendenciasTemp);
                SelectedIndex = _selectedIndexTemp;
                _PendenciasTemp.Clear();


                _PendenciasPro = null;

                _PendenciasPro.Clear();
                _PendenciaTemp = null;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        public void OnAdicionarCommand(int Cadastro, int _TipoPendenciaID, int EntidadeID)
        {
            try
            {
                if (Pendencias == null)
                {
                    Pendencias = new ObservableCollection<ClassePendencias.Pendencia>();
                }
                foreach (var x in Pendencias)
                {
                    _PendenciasTemp.Add(x);
                }
                _selectedIndexTemp = SelectedIndex;
                Pendencias.Clear();
                _PendenciaTemp = new ClassePendencias.Pendencia();
                if (_TipoPendenciaID != 0)
                {
                _PendenciaTemp.TipoPendenciaID = _TipoPendenciaID;
                }
                if (Cadastro == 1) { _PendenciaTemp.EmpresaID = EntidadeID; }
                else if (Cadastro == 2) { _PendenciaTemp.ColaboradorID = EntidadeID; }
                else if (Cadastro == 3) { _PendenciaTemp.VeiculoID = EntidadeID; }
                Pendencias.Add(_PendenciaTemp);
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
                Pendencias = null;
                Pendencias = new ObservableCollection<ClassePendencias.Pendencia>(_PendenciasTemp);
                SelectedIndex = _selectedIndexTemp;
                _PendenciasTemp.Clear();
                HabilitaEdicao = false;
            }
            catch (Exception ex)
            {
            }
        }

        public void OnExcluirCommand(int Cadastro, int _TipoPendenciaID, int EntidadeID)
        {
            try
            {

                if (Global.PopupBox("Tem certeza que deseja excluir?", 2))
                {

                        ExcluiPendenciaBD(PendenciaSelecionada.PendenciaID, Cadastro,  _TipoPendenciaID,  EntidadeID);

                        Pendencias.Remove(PendenciaSelecionada);

                }

            }
            catch (Exception ex)
            {
            }

        }

        #endregion

        #region Carregamento das Colecoes

        private void CarregaColecaoTiposPendencias()
        {
            try
            {

                TiposPendencias = new ObservableCollection<ClasseTiposPendencias.TipoPendencia>();
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 11, Tipo = "Geral" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 12, Tipo = "Signatários" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 13, Tipo = "Seguros" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 14, Tipo = "Contratos" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 15, Tipo = "Veículos" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 16, Tipo = "Equipamentos" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 17, Tipo = "Anexos" });

                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 21, Tipo = "Geral" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 22, Tipo = "Empresas Vínculo" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 23, Tipo = "Treinamentos e Certificações" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 24, Tipo = "Anexos" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 25, Tipo = "Credenciais" });

                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 31, Tipo = "Geral" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 32, Tipo = "Empresas Vínculo" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 33, Tipo = "Seguros" });
                TiposPendencias.Add(new ClasseTiposPendencias.TipoPendencia() { TipoPendenciaID = 34, Tipo = "Credenciais" });


            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }

        }

        public void CarregaColecaoPendencias(int Cadastro, int Tag = 0, int ID = 0)
        {
            try
            {
                string _xml = RequisitaPendencias(Cadastro, Tag, ID);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClassePendencias));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClassePendencias classePendencias = new ClassePendencias();
                classePendencias = (ClassePendencias)deserializer.Deserialize(reader);
                Pendencias = new ObservableCollection<ClassePendencias.Pendencia>();
                Pendencias = classePendencias.Pendencias;
                SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        #endregion

        #region Data Access
        private string RequisitaPendencias(int Cadastro, int Tag = 0, int ID = 0)
        {
            try
            {

                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClassePendencias = _xmlDocument.CreateElement("ClassePendencias");
                _xmlDocument.AppendChild(_ClassePendencias);

                XmlNode _Pendencias = _xmlDocument.CreateElement("Pendencias");
                _ClassePendencias.AppendChild(_Pendencias);

                string _TipoPendenciaIDSTR;
                string IdSTR;
                string CadastroSTR="";
                string _strSql;

                if (Cadastro == 1) { CadastroSTR = " EmpresaID = "; }
                else if (Cadastro == 2) { CadastroSTR = " ColaboradorID = "; }
                else if (Cadastro == 3) { CadastroSTR = " VeiculoID = "; }
                _TipoPendenciaIDSTR = Tag == 0 ? "" : " AND TipoPendenciaID = " + Tag;

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _strSql = "select * from Pendencias where " + CadastroSTR + ID + _TipoPendenciaIDSTR + " order by PendenciaID desc"; // and NumeroApolice Like '" + _numeroapolice + "'


                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Pendencia = _xmlDocument.CreateElement("Pendencia");
                    _Pendencias.AppendChild(_Pendencia);

                    XmlNode _PendenciaID = _xmlDocument.CreateElement("PendenciaID");
                    _PendenciaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["PendenciaID"].ToString())));
                    _Pendencia.AppendChild(_PendenciaID);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _Pendencia.AppendChild(_ColaboradorID);


                    XmlNode _EmpresaID = _xmlDocument.CreateElement("EmpresaID");
                    _EmpresaID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EmpresaID"].ToString())));
                    _Pendencia.AppendChild(_EmpresaID);

                    XmlNode _VeiculoID = _xmlDocument.CreateElement("VeiculoID");
                    _VeiculoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["VeiculoID"].ToString())));
                    _Pendencia.AppendChild(_VeiculoID);

                    XmlNode _Tipo = _xmlDocument.CreateElement("TipoPendenciaID");
                    _Tipo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoPendenciaID"].ToString()).Trim()));
                    _Pendencia.AppendChild(_Tipo);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Descricao"].ToString()).Trim()));
                    _Pendencia.AppendChild(_Descricao);

                    var dateStr = (_sqlreader["DataLimite"].ToString());
                    if (!string.IsNullOrWhiteSpace(dateStr))
                    {
                        var dt2 = Convert.ToDateTime(dateStr);
                        XmlNode _DataLimite = _xmlDocument.CreateElement("DataLimite");
                        //format valid for XML W3C yyyy-MM-ddTHH:mm:ss
                        _DataLimite.AppendChild(_xmlDocument.CreateTextNode(dt2.ToString("yyyy-MM-ddTHH:mm:ss")));
                        _Pendencia.AppendChild(_DataLimite);
                    }

                    //XmlNode _DataLimite = _xmlDocument.CreateElement("DataLimite");
                    ////_DataLimite.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataLimite"].ToString())));
                    //_DataLimite.AppendChild(_xmlDocument.CreateTextNode("30/10/2018"));
                    //_Pendencia.AppendChild(_DataLimite);

                    XmlNode _Impeditivo = _xmlDocument.CreateElement("Impeditivo");
                    _Impeditivo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Impeditivo"])).ToString()));
                    _Pendencia.AppendChild(_Impeditivo);
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
        }

        private void InserePendenciaBD(string xmlString)
        {
            try
            {


                System.Xml.XmlDocument _xmlDoc = new System.Xml.XmlDocument();
                _xmlDoc.LoadXml(xmlString);

                ClassePendencias.Pendencia _Pendencia = new ClassePendencias.Pendencia();

                int i = 0;

                _Pendencia.PendenciaID = _xmlDoc.GetElementsByTagName("PendenciaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("PendenciaID")[i].InnerText);
                _Pendencia.TipoPendenciaID = _xmlDoc.GetElementsByTagName("TipoPendenciaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("TipoPendenciaID")[i].InnerText);
                _Pendencia.Descricao = _xmlDoc.GetElementsByTagName("Descricao")[i] == null ? "" : _xmlDoc.GetElementsByTagName("Descricao")[i].InnerText;
                _Pendencia.DataLimite = _xmlDoc.GetElementsByTagName("DataLimite")[i].InnerText == "" ? null : (DateTime?)Convert.ToDateTime(_xmlDoc.GetElementsByTagName("DataLimite")[i].InnerText);
                bool _impeditivo;
                Boolean.TryParse(_xmlDoc.GetElementsByTagName("Impeditivo")[i].InnerText, out _impeditivo);
                _Pendencia.ColaboradorID = _xmlDoc.GetElementsByTagName("ColaboradorID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("ColaboradorID")[i].InnerText);
                _Pendencia.EmpresaID = _xmlDoc.GetElementsByTagName("EmpresaID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("EmpresaID")[i].InnerText);
                _Pendencia.VeiculoID = _xmlDoc.GetElementsByTagName("VeiculoID")[i] == null ? 0 : Convert.ToInt32(_xmlDoc.GetElementsByTagName("VeiculoID")[i].InnerText);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                if (_Pendencia.PendenciaID != 0)
                {

                    _sqlCmd = new SqlCommand("Update Pendencias set TipoPendenciaID=@v1, Descricao=@v2, " +
                            "DataLimite=@v3, Impeditivo=@v4, ColaboradorID=@v5, EmpresaID=@v6, VeiculoID=@7 where PendenciaID=@v8", _Con);

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _Pendencia.TipoPendenciaID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _Pendencia.Descricao;
                    if (_Pendencia.DataLimite == null)
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = _Pendencia.DataLimite;
                    }
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.Bit).Value = _impeditivo;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _Pendencia.ColaboradorID;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _Pendencia.EmpresaID;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _Pendencia.VeiculoID;
                    _sqlCmd.Parameters.Add("@V8", SqlDbType.Int).Value = _Pendencia.PendenciaID;
                }
                else
                {

                    _sqlCmd = new SqlCommand("Insert into Pendencias(TipoPendenciaID, Descricao, " +
                        "DataLimite, Impeditivo, ColaboradorID, EmpresaID,VeiculoID) VALUES (@v1,@v2,@v3,@v4,@v5,@v6,@v7)", _Con);

                    _sqlCmd.Parameters.Add("@V1", SqlDbType.Int).Value = _Pendencia.TipoPendenciaID;
                    _sqlCmd.Parameters.Add("@V2", SqlDbType.VarChar).Value = _Pendencia.Descricao;
                    if (_Pendencia.DataLimite == null)
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        _sqlCmd.Parameters.Add("@V3", SqlDbType.DateTime).Value = _Pendencia.DataLimite;
                    }
                    _sqlCmd.Parameters.Add("@V4", SqlDbType.Bit).Value = _impeditivo;
                    _sqlCmd.Parameters.Add("@V5", SqlDbType.Int).Value = _Pendencia.ColaboradorID;
                    _sqlCmd.Parameters.Add("@V6", SqlDbType.Int).Value = _Pendencia.EmpresaID;
                    _sqlCmd.Parameters.Add("@V7", SqlDbType.Int).Value = _Pendencia.VeiculoID;

                }

                _sqlCmd.ExecuteNonQuery();

                if (_Pendencia.EmpresaID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Empresas Set " +
                                                "Pendente" + _Pendencia.TipoPendenciaID + "= 'True' " +
                                                " Where EmpresaID = " + _Pendencia.EmpresaID , _Con);
                    
                }
                else if (_Pendencia.ColaboradorID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Colaboradores Set " +
                            "Pendente" + _Pendencia.TipoPendenciaID + "= 'True' " +
                            " Where ColaboradorID = " + _Pendencia.ColaboradorID, _Con);
                }
                else if (_Pendencia.VeiculoID != 0)
                {
                    _sqlCmd = new SqlCommand("Update Veiculos Set " +
                            "Pendente" + _Pendencia.TipoPendenciaID + "= 'True' " +
                            " Where VeiculoID = " + _Pendencia.VeiculoID, _Con);
                }
                _sqlCmd.ExecuteNonQuery();


                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void InserePendenciaBD ex: " + ex);


            }
        }

        private void ExcluiPendenciaBD(int _PendenciaID, int Cadastro, int _TipoPendenciaID, int EntidadeID) // alterar para xml
        {
            try
            {


                //_Con.Close();
                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                SqlCommand _sqlCmd;
                _sqlCmd = new SqlCommand("Delete from Pendencias where PendenciaID=" + _PendenciaID, _Con);
                _sqlCmd.ExecuteNonQuery();

                if (Cadastro == 1)
                {
                    _sqlCmd = new SqlCommand("SELECT * FROM Pendencias where EmpresaID = " + EntidadeID + " AND TipoPendenciaID = " + _TipoPendenciaID, _Con);

                    SqlDataReader _sqlreader = _sqlCmd.ExecuteReader(CommandBehavior.Default);
                    if (!_sqlreader.Read())
                    {
                        _sqlCmd = new SqlCommand("Update Empresas Set " +
                            "Pendente" + _TipoPendenciaID + "= 'False' " +
                            " Where EmpresaID = " + EntidadeID, _Con);
                        _sqlCmd.ExecuteNonQuery();
                    }

                }
                else if (Cadastro == 2)
                {
                    _sqlCmd = new SqlCommand("SELECT * FROM Pendencias where ColaboradorID = " + EntidadeID + " AND TipoPendenciaID = " + _TipoPendenciaID , _Con);
                    SqlDataReader _sqlreader = _sqlCmd.ExecuteReader(CommandBehavior.Default);
                    if (!_sqlreader.Read())
                    {
                        _sqlCmd = new SqlCommand("Update Colaboradores Set " +
                            "Pendente" + _TipoPendenciaID + "= 'False' " +
                            " Where ColaboradorID = " + EntidadeID, _Con);
                        _sqlCmd.ExecuteNonQuery();
                    }
                }
                else if (Cadastro == 3)
                {
                    _sqlCmd = new SqlCommand("SELECT * FROM Pendencias where VeiculoID = " + EntidadeID + " AND TipoPendenciaID = " + _TipoPendenciaID, _Con);
                    SqlDataReader _sqlreader = _sqlCmd.ExecuteReader(CommandBehavior.Default);
                    if (!_sqlreader.Read())
                    {
                        _sqlCmd = new SqlCommand("Update Veiculos Set " +
                            "Pendente" + _TipoPendenciaID + "= 'False' " +
                            " Where VeiculoID = " + EntidadeID, _Con);
                        _sqlCmd.ExecuteNonQuery();
                    }
                }

                _Con.Close();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void ExcluiPendenciaBD ex: " + ex);


            }
        }
        #endregion

        #region Metodos privados
        private string CriaXmlImagem(int colaboradorID, int pendenciaID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Pendencias Where ColaboradorID = " + colaboradorID + " And PendenciaID = " + pendenciaID + "", _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

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
