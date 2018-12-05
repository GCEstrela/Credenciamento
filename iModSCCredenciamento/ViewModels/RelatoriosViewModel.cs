using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace iModSCCredenciamento.ViewModels
{
    public class RelatoriosViewModel : ViewModelBase
    {
        #region Inicializacao
        public RelatoriosViewModel()
        {
            Thread CarregaUI_thr = new Thread(() => CarregaUI());
            CarregaUI_thr.Start();

            //CarregaUI();
        }

        private void CarregaUI()
        {
            CarregaColecaoRelatorios();
            CarregaColecaoEmpresas();
            CarregaColecaoAreasAcessos();
        }

        #endregion

        #region Variaveis Privadas

        private ObservableCollection<ClasseAreasAcessos.AreaAcesso> _AreasAcessos;

        private ObservableCollection<ClasseEmpresas.Empresa> _Empresas;


        private ObservableCollection<ClasseRelatorios.Relatorio> _Relatorios;

        private ClasseRelatorios.Relatorio _RelatorioSelecionado;

        private ClasseRelatorios.Relatorio _relatorioTemp = new ClasseRelatorios.Relatorio();

        private List<ClasseRelatorios.Relatorio> _RelatoriosTemp = new List<ClasseRelatorios.Relatorio>();

        PopupMensagem _PopupSalvando;

        private int _selectedIndex;

        private int _selectedIndexTemp = 0;

        private bool _atualizandoFoto = false;

        private BitmapImage _Waiting;
        private string formula;

        #endregion

        #region Contrutores

        public ObservableCollection<ClasseAreasAcessos.AreaAcesso> AreasAcessos
        {
            get
            {
                return _AreasAcessos;
            }

            set
            {
                if (_AreasAcessos != value)
                {
                    _AreasAcessos = value;
                    OnPropertyChanged();

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
        public ObservableCollection<ClasseRelatorios.Relatorio> Relatorios
        {
            get
            {
                return _Relatorios;
            }

            set
            {
                if (_Relatorios != value)
                {
                    _Relatorios = value;
                    OnPropertyChanged();

                }
            }
        }

        public ClasseRelatorios.Relatorio RelatorioSelecionado
        {
            get
            {

                return this._RelatorioSelecionado;
            }
            set
            {
                this._RelatorioSelecionado = value;
                base.OnPropertyChanged("SelectedItem");
                if (RelatorioSelecionado != null)
                {
                    //    //BitmapImage _img = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Carregando.png", UriKind.Absolute));
                    //    //string _imgstr = Conversores.IMGtoSTR(_img);
                    //    //ColaboradorSelecionado.Foto = _imgstr;
                    //    if (!_atualizandoFoto)
                    //    {
                    //        Thread CarregaRelatorio_thr = new Thread(() => CarregaRelatorio(RelatorioSelecionado.RelatorioID));
                    //        CarregaRelatorio_thr.Start();
                    //    }

                    //    //CarregaFoto(ColaboradorSelecionado.ColaboradorID);
                }

            }
        }


        //public int EmpresaSelecionadaID
        //{
        //    get
        //    {
        //        return this._EmpresaSelecionadaID;
        //    }
        //    set
        //    {
        //        this._EmpresaSelecionadaID = value;
        //        base.OnPropertyChanged();
        //        if (EmpresaSelecionadaID != null)
        //        {
        //            // OnEmpresaSelecionada();
        //        }

        //    }
        //}


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

        #region Carregamento das Colecoes
        private void CarregaColecaoRelatorios()
        {

            try
            {

                string _xml = RequisitaRelatorios();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseRelatorios));
                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);
                TextReader reader = new StringReader(_xml);
                ClasseRelatorios classeRelatorios = new ClasseRelatorios();
                classeRelatorios = (ClasseRelatorios)deserializer.Deserialize(reader);
                Relatorios = new ObservableCollection<ClasseRelatorios.Relatorio>();
                Relatorios = classeRelatorios.Relatorios;


            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CarregaColeçãoRelatorios ex: " + ex);
            }
        }

        private void CarregaColecaoAreasAcessos()
        {
            try
            {
                string _xml = RequisitaAreasAcessos();

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseAreasAcessos));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseAreasAcessos classeAreasAcessos = new ClasseAreasAcessos();
                classeAreasAcessos = (ClasseAreasAcessos)deserializer.Deserialize(reader);
                AreasAcessos = new ObservableCollection<ClasseAreasAcessos.AreaAcesso>();
                AreasAcessos = classeAreasAcessos.AreasAcessos;
            }
            catch (Exception ex)
            {
                Global.Log("Erro void CarregaColecaoAreasAcessos ex: " + ex.Message);
            }
        }

        private void CarregaColecaoEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        {

            try
            {
                string _xml = RequisitaEmpresas(_empresaID, _nome, _apelido, _cNPJ);

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresas classeEmpresas = new ClasseEmpresas();
                classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
                Empresas = new ObservableCollection<ClasseEmpresas.Empresa>();
                Empresas = classeEmpresas.Empresas;
            }
            catch (Exception)
            {
                //Global.Log("Erro void CarregaColecaoEmpresas ex: " + ex.Message);
            }
        }

        private string RequisitaEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", int _excluida = 0, string _quantidaderegistro = "500")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresas = _xmlDocument.CreateElement("ClasseEmpresas");
                _xmlDocument.AppendChild(_ClasseEmpresas);

                XmlNode _Empresas = _xmlDocument.CreateElement("Empresas");
                _ClasseEmpresas.AppendChild(_Empresas);

                string _strSql = " [EmpresaID],[Nome]";

                //            string _strSql = " [EmpresaID],[Nome],[Apelido],[CNPJ],[CEP],[Endereco]," +
                //"[Numero],[Complemento],[Bairro],[MunicipioID],[EstadoID],[Telefone],[Contato]," +
                //"[Celular],[Email],[Obs],[Responsavel],[InsEst],[InsMun],[Excluida]";

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();

                _empresaID = "%" + _empresaID + "%";
                _nome = "%" + _nome + "%";
                _apelido = "%" + _apelido + "%";
                _cNPJ = "%" + _cNPJ + "%";

                if (_quantidaderegistro == "0")
                {
                    _strSql = "select " + _strSql + " from Empresas where EmpresaID like '" + _empresaID + "' and EmpresaNome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + " order by EmpresaID desc";
                }
                else
                {
                    _strSql = "select Top " + _quantidaderegistro + _strSql + "  from Empresas where EmpresaID like '" + _empresaID + "' and Nome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + " order by EmpresaID desc";
                }


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

                    //XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
                    //_Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
                    //_Empresa.AppendChild(_Apelido);

                    //XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
                    //_CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString())));
                    //_Empresa.AppendChild(_CNPJ);

                    //XmlNode _InsEst = _xmlDocument.CreateElement("InsEst");
                    //_InsEst.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsEst"].ToString())));
                    //_Empresa.AppendChild(_InsEst);

                    //XmlNode _InsMun = _xmlDocument.CreateElement("InsMun");
                    //_InsMun.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsMun"].ToString())));
                    //_Empresa.AppendChild(_InsMun);

                    //XmlNode _Responsavel = _xmlDocument.CreateElement("Responsavel");
                    //_Responsavel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Responsavel"].ToString())));
                    //_Empresa.AppendChild(_Responsavel);

                    //XmlNode _CEP = _xmlDocument.CreateElement("CEP");
                    //_CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
                    //_Empresa.AppendChild(_CEP);

                    //XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    //_Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    //_Empresa.AppendChild(_Endereco);

                    //XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    //_Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    //_Empresa.AppendChild(_Numero);

                    //XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    //_Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    //_Empresa.AppendChild(_Complemento);

                    //XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    //_Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    //_Empresa.AppendChild(_Bairro);

                    //XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    //_EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    //_Empresa.AppendChild(_EstadoID);

                    //XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
                    //_MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    //_Empresa.AppendChild(_MunicipioID);

                    //XmlNode _Tel = _xmlDocument.CreateElement("Telefone");
                    //_Tel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone"].ToString())));
                    //_Empresa.AppendChild(_Tel);

                    //XmlNode _Cel = _xmlDocument.CreateElement("Celular");
                    //_Cel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular"].ToString())));
                    //_Empresa.AppendChild(_Cel);

                    //XmlNode _Contato = _xmlDocument.CreateElement("Contato");
                    //_Contato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato"].ToString())));
                    //_Empresa.AppendChild(_Contato);

                    //XmlNode _Email = _xmlDocument.CreateElement("Email");
                    //_Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
                    //_Empresa.AppendChild(_Email);

                    //XmlNode _Obs = _xmlDocument.CreateElement("Obs");
                    //_Obs.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Obs"].ToString())));
                    //_Empresa.AppendChild(_Obs);

                    //XmlNode _Logo = _xmlDocument.CreateElement("Logo");
                    ////_Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
                    //_Empresa.AppendChild(_Logo);

                    //XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
                    //_Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    //_Empresa.AppendChild(_Excluida);


                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

        #endregion

        #region Data Access
        private string RequisitaAreasAcessos()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseAreasAcessos = _xmlDocument.CreateElement("ClasseAreasAcessos");
                _xmlDocument.AppendChild(_ClasseAreasAcessos);

                XmlNode _AreasAcessos = _xmlDocument.CreateElement("AreasAcessos");
                _ClasseAreasAcessos.AppendChild(_AreasAcessos);

                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();
                SqlCommand _sqlcmd = new SqlCommand("select * from AreasAcessos order by AreaAcessoID", _Con);
                SqlDataReader _sqldatareader = _sqlcmd.ExecuteReader();
                while (_sqldatareader.Read())
                {
                    XmlNode _AreaAcesso = _xmlDocument.CreateElement("AreaAcesso");
                    _AreasAcessos.AppendChild(_AreaAcesso);

                    XmlNode _AreaAcessoID = _xmlDocument.CreateElement("AreaAcessoID");
                    _AreaAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["AreaAcessoID"].ToString())));
                    _AreaAcesso.AppendChild(_AreaAcessoID);

                    XmlNode _Descricao = _xmlDocument.CreateElement("Descricao");
                    _Descricao.AppendChild(_xmlDocument.CreateTextNode((_sqldatareader["Descricao"].ToString())));
                    _AreaAcesso.AppendChild(_Descricao);

                }
                _sqldatareader.Close();
                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void RequisitaAreasAcessos ex: " + ex);

                return null;
            }
        }
        private string RequisitaRelatorios()
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseRelatorios = _xmlDocument.CreateElement("ClasseRelatorios");
                _xmlDocument.AppendChild(_ClasseRelatorios);

                XmlNode _Relatorios = _xmlDocument.CreateElement("Relatorios");
                _ClasseRelatorios.AppendChild(_Relatorios);

                string _strSql;


                SqlConnection _Con = new SqlConnection(Global._connectionString); _Con.Open();


                _strSql = "Select [RelatorioID]" +
                          ",[Nome]" +
                          ",[NomeArquivoRPT], [Ativo]" +
                          "from Relatorios order by RelatorioID ASC";

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);

                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);


                while (_sqlreader.Read())
                {

                    XmlNode _Relatorio = _xmlDocument.CreateElement("Relatorio");
                    _Relatorios.AppendChild(_Relatorio);

                    XmlNode _RelatorioID = _xmlDocument.CreateElement("RelatorioID");
                    _RelatorioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RelatorioID"].ToString())));
                    _Relatorio.AppendChild(_RelatorioID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Relatorio.AppendChild(_Nome);

                    XmlNode _NomeArquivoRPT = _xmlDocument.CreateElement("NomeArquivoRPT");
                    _NomeArquivoRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeArquivoRPT"].ToString())));
                    _Relatorio.AppendChild(_NomeArquivoRPT);


                    XmlNode _Ativo = _xmlDocument.CreateElement("Ativo");
                    _Ativo.AppendChild(_xmlDocument.CreateTextNode((Convert.ToInt32((bool)_sqlreader["Ativo"])).ToString()));
                    _Relatorio.AppendChild(_Ativo);


                    XmlNode _ArquivoRPT = _xmlDocument.CreateElement("ArquivoRPT");
                    //_ArquivoRPT.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ArquivoRPT""].ToString())));
                    _Relatorio.AppendChild(_ArquivoRPT);


                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                //InsereRelatoriosBD("");

                return _xml;
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

        private string BuscaFoto(int relatorioID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Relatorios Where RelatorioID = " + relatorioID, _Con);
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
        #endregion

        #region Comandos dos Botoes

        #region Comandos dos Botoes (RELATÓRIOS GERENCIAIS)

        public void OnFiltroRelatorioCredencialCommand(bool _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                string _xmlstring;

                if (_tipo)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(1);
                }
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(5);
                }



                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);

                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);

                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                if (_tipo)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS CREDENCIAIS PERMANENTES E ATIVAS
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 and {CredenciaisStatus.CredencialStatusID} = 1 ";
                    }
                    else
                    {
                        formula = " {TiposCredenciais.TipoCredencialID} = 1 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS CREDENCIAIS TEMPORÁRIAS E ATIVAS
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 and {CredenciaisStatus.CredencialStatusID} = 1 ";
                    }
                    else
                    {
                        formula = " {TiposCredenciais.TipoCredencialID} = 2 " +
                                  " and {CredenciaisStatus.CredencialStatusID} = 1 " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }

                reportDocument.Load(_ArquivoRPT);

                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;

                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }


                Thread CarregaRel_thr = new Thread(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        reportDocument.RecordSelectionFormula = formula;

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltroRelatorioCredencialCommand ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnFiltroRelatorioAutorizacoesCommand(bool _check)
        {
            try
            {
                string _xmlstring = CriaXmlRelatoriosGerenciais(2);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;


                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                if (_check)
                {   //AUTORIZAÇÕES PERMANENTES E ATIVAS
                    formula = " {TiposCredenciais.TipoCredencialID} = 1 AND {CredenciaisStatus.CredencialStatusID} = 1 ";
                }
                else
                {
                    //AUTORIZAÇÕES TEMPORÁRIAS E ATIVAS
                    formula = " {TiposCredenciais.TipoCredencialID} = 2 AND {CredenciaisStatus.CredencialStatusID} = 1 ";
                }

                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        reportDocument.RecordSelectionFormula = formula;
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                });

                CarregaRel_thr.Start();
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltroRelatorioAutorizacoesCommand ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;
            }
        }

        public void OnRelatorioCredenciaisInvalidasFiltroCommand(int _status, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {
                _xmlstring = CriaXmlRelatoriosGerenciais(3);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;


                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                //DATA VAZIA (TODOS)
                if ((_dataFim == "" || _dataIni == "") && _status == 0)

                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                }
                //CAMPOS VAZIOS (TODOS)
                else if ((_dataFim == "" && _dataIni == "") && _status != 0)
                {
                    if (_status == 2)
                    {
                        //Credenciais Roubadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  "AND {CredenciaisMotivos.CredencialmotivoID} = 10";
                    }
                    else if (_status == 1)
                    {
                        //Credenciais Extraviadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  "AND {CredenciaisMotivos.CredencialmotivoID} = 9";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status + "";
                    }

                }
                else
                {
                    if (_status == 0)
                    {
                        formula = " ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }

                    else if (_status == 2)
                    {
                        //Credenciais Roubadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 10" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else if (_status == 1)
                    {
                        //Credenciais Extraviadas
                        formula = " {CredenciaisStatus.CredencialStatusID} = 2 " +
                                  " AND {CredenciaisMotivos.CredencialmotivoID} = 9" +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _status +
                                  " AND ({ColaboradoresCredenciais.Baixa} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Baixa} >= CDate ('" + _dataIni + "') ) ";
                    }
                }

                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;

                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        reportDocument.RecordSelectionFormula = formula;
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });
                });

                CarregaRel_thr.Start();

            }

            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioCredenciaisInvalidasFiltroCommand ex: " + ex);
                IMOD.CrossCutting.Utils.TraceException(ex);
                throw;

            }
        }

        public void OnRelatorioAutorizacoesInvalidasFiltroCommand(int _check, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {    //4_Relatório_AutorizacoesInvalidas.rpt
                _xmlstring = CriaXmlRelatoriosGerenciais(4);

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;


                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                //Esse ponto de implementação para a alteração da instancia do SQL, banco, usuário e senha
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                //DATA VAZIA (TODOS)
                if ((_dataFim == "" || _dataIni == "") && _check == 10)

                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 ";
                }
                //CAMPOS VAZIOS (TODOS)
                else if ((_dataFim == "" && _dataIni == "") && _check != 10)
                {
                    formula = " {CredenciaisStatus.CredencialStatusID} <> 1 " +
                    " AND {CredenciaisStatus.CredencialStatusID} = " + _check + "";
                }
                else
                {
                    if (_check == 10)
                    {
                        formula = " ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                    else
                    {
                        formula = " {CredenciaisStatus.CredencialStatusID} = " + _check +
                                  " AND ({VeiculosCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {VeiculosCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }
                }


                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;

                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {


                        reportDocument.RecordSelectionFormula = formula;

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }

            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioAutorizacoesInvalidasFiltroCommand ex: " + ex);

            }
        }

        public void OnRelatorioFiltroPorAreaCommand(string _area, bool _check)
        {
            string _xmlstring;

            try
            {
                //6_Relatorio_CredenciaisPorArea.rpt
                if (_check)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(6);
                }
                //7_Relatorio_AutorizacoesPorArea.rpt
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(7);
                }

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                //ReportDocument reportDocument = new ReportDocument();


                //File.Move(_caminhoArquivoPDF, Path.ChangeExtension(_caminhoArquivoPDF, ".pdf"));
                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                //Esse ponto de implementação para a alteração da instancia do SQL, banco, usuário e senha
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;


                reportDocument.Load(_ArquivoRPT);


                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {

                        if (_area != "")
                        {
                            reportDocument.RecordSelectionFormula = " {AreasAcessos_0.AreaAcessoID} = " + _area;
                        }

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                }

                );
                //CarregaRel_thr.SetApartmentState(ApartmentState.STA);
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioFiltroPorAreaCommand ex: " + ex);

            }
        }

        public void OnRelatorioFiltroPorEmpresaCommand(string empresa, bool _check, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {
                //8_Relatorio_CredenciaisNoPeriodoPorEntidade.rpt
                if (_check)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(8);
                }
                //9_Relatorio_AutorizacoesNoPeriodoPorEntidade.rpt
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(9);
                }

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                //Todas Empresas
                if (empresa == "" && _dataIni == "" && _dataFim == "")
                {
                    formula = "";
                    reportDocument.Load(_ArquivoRPT);
                }

                //Uma Empresa
                else if (_dataIni == "" && _dataFim == "" && _check)
                {
                    formula = " {Empresas.EmpresaID} = " + empresa;
                    reportDocument.Load(_ArquivoRPT);
                }

                //Credenciais
                else if (_check)
                {
                    formula = "{ColaboradoresEmpresas.Emissao}  <= '" + _dataFim + "' " +
                              " and {ColaboradoresEmpresas.Emissao} >= '" + _dataIni +
                              "' and {Empresas.EmpresaID} = " + empresa + "";

                    reportDocument.Load(_ArquivoRPT);
                }

                //Autorizacoes
                else
                {
                    formula = "{EmpresasVeiculos.Emissao}  <= '" + _dataFim + "' " +
                          " and {EmpresasVeiculos.Emissao} >= '" + _dataIni +
                          "' and {Empresas.EmpresaID} = " + empresa + "";

                    reportDocument.Load(_ArquivoRPT);
                }

                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {

                        if (empresa != "")
                        {
                            reportDocument.RecordSelectionFormula = formula;
                        }

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });
                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnRelatorioFiltroPorEmpresaCommand ex: " + ex);

            }
        }

        public void OnFiltrosImpressoesCommand(string _empresa, string _area, bool _check, string _dataIni, string _dataFim)
        {
            string _xmlstring;

            try
            {
                //10_Relatório_ImpressoesCredenciais.rpt
                if (_check)
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(10);
                }
                //11_Relatório_ImpressoesAutorizacoes.rpt
                else
                {
                    _xmlstring = CriaXmlRelatoriosGerenciais(11);
                }

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                //Tudo
                if (_empresa == "" || _dataIni == "" || _dataFim == "" || _area == "")
                {
                    formula = "";
                    reportDocument.Load(_ArquivoRPT);
                }

                //Autorizacoes
                else
                {
                    formula = " 1=1 and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataFim + "') " +
                          " and {ColaboradoresCredenciais.Emissao} <= cdate('" + _dataIni + "')" +
                          " and  {EmpresasAreasAcessos.AreaAcessoID} = " + _area + "" +
                          " and {Empresas.EmpresaID} = " + _empresa + "";

                    reportDocument.Load(_ArquivoRPT);
                }

                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;
                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {

                        if (_empresa != "")
                        {
                            reportDocument.RecordSelectionFormula = formula;
                        }

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltrosImpressoesCommand ex: " + ex);

            }
        }

        public void OnFiltroCredencialViasAdicionaisCommand(int _tipo, string _dataIni, string _dataFim)
        {
            try
            {
                string _xmlstring;

                //21_Relatório_ViasAdicionaisCredenciais.rpt
                _xmlstring = CriaXmlRelatoriosGerenciais(21);


                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);

                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);

                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;

                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                if (_tipo == 0)
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //TODAS AS VIAS ADICIONAIS EMITIDAS
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3]";
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID} in [2,3] " +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }
                else
                {
                    if (_dataIni == "" || _dataFim == "")
                    {
                        //VIAS ADICIONAIS EMITIDAS (1a,2a,3a VIA)
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo;
                    }
                    else
                    {
                        formula = " {CredenciaisMotivos.CredencialmotivoID}  =  " + _tipo +
                                  " AND ({ColaboradoresCredenciais.Emissao} <= CDate ('" + _dataFim + "')" +
                                  " AND {ColaboradoresCredenciais.Emissao} >= CDate ('" + _dataIni + "') ) ";
                    }

                }

                reportDocument.Load(_ArquivoRPT);

                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;

                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }


                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {

                        reportDocument.RecordSelectionFormula = formula;

                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnFiltroRelatorioCredencialCommand ex: " + ex);
            }
        }

        #endregion

        public void OnAbrirRelatorioCommand(string _Tag)
        {

            try
            {
                string _xmlstring = CriaXmlRelatorio(Convert.ToInt32(_Tag));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xmlstring);
                XmlNode node = xmldocument.DocumentElement;
                XmlNode arquivoNode = node.SelectSingleNode("ArquivosImagens/ArquivoImagem/Arquivo");

                string _ArquivoRPT = arquivoNode.FirstChild.Value;
                byte[] buffer = Convert.FromBase64String(_ArquivoRPT);
                _ArquivoRPT = System.IO.Path.GetTempFileName();
                _ArquivoRPT = System.IO.Path.GetRandomFileName();
                _ArquivoRPT = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + _ArquivoRPT;
                _ArquivoRPT = System.IO.Path.ChangeExtension(_ArquivoRPT, ".rpt");
                System.IO.File.WriteAllBytes(_ArquivoRPT, buffer);

                ReportDocument reportDocument = new ReportDocument();
                TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                reportDocument.Load(_ArquivoRPT);
                crConnectionInfo.ServerName = Global._instancia;
                crConnectionInfo.DatabaseName = Global._bancoDados;
                crConnectionInfo.UserID = Global._usuario;
                crConnectionInfo.Password = Global._senha;

                CrTables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                Thread CarregaRel_thr = new Thread(() =>
                {

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        PopupRelatorio _popupRelatorio = new PopupRelatorio(reportDocument);
                        _popupRelatorio.Show();
                    });

                });

                CarregaRel_thr.Start();

            }
            catch (Exception ex)
            {
                Global.Log("Erro na void OnAbrirArquivoCommand ex: " + ex);
            }

        }

        #endregion


        #region Metodos privados
        private string CriaXmlRelatorio(int relatorioID)
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

                SqlCommand SQCMDXML = new SqlCommand("Select * From Relatorios Where RelatorioID = " + relatorioID, _Con);
                SqlDataReader SQDR_XML;
                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["ArquivoRPT"].ToString())));
                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();
                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CriaXmlRelatorio ex: " + ex);
                return null;
            }
        }

        private string CriaXmlRelatoriosGerenciais(int relatorioID)
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


                SqlCommand SQCMDXML = new SqlCommand("Select * From RelatoriosGerenciais Where RelatorioID = " + relatorioID, _Con);
                SqlDataReader SQDR_XML;

                SQDR_XML = SQCMDXML.ExecuteReader(CommandBehavior.Default);
                while (SQDR_XML.Read())
                {
                    XmlNode _ArquivoImagem = _xmlDocument.CreateElement("ArquivoImagem");
                    _ArquivosImagens.AppendChild(_ArquivoImagem);

                    XmlNode _Arquivo = _xmlDocument.CreateElement("Arquivo");

                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((SQDR_XML["ArquivoRPT"].ToString())));

                    _ArquivoImagem.AppendChild(_Arquivo);

                }
                SQDR_XML.Close();

                _Con.Close();

                return _xmlDocument.InnerXml;
            }
            catch (Exception ex)
            {
                Global.Log("Erro na void CriaXmlRelatorio ex: " + ex);
                return null;
            }
        }
        #endregion


    }
}
