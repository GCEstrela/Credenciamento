using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using iModSCCredenciamento.Views.Model;
using Microsoft.Reporting.WinForms;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupContratosRelatorios.xaml
    /// </summary>
    public partial class PopupContratosRelatorios : Window
    {
        public PopupContratosRelatorios()
        {
            InitializeComponent();
        }
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            string _xml = RequisitaContratos();

            XmlSerializer deserializer = new XmlSerializer(typeof(EmpresaContratoView));

            XmlDocument xmldocument = new XmlDocument();
            xmldocument.LoadXml(_xml);

            TextReader reader = new StringReader(_xml);
            EmpresaContratoView classeEmpresasContratos = new EmpresaContratoView();
            classeEmpresasContratos = (EmpresaContratoView)deserializer.Deserialize(reader);
            var dadosRelatorio = new ObservableCollection<EmpresaContratoView>();
            //dadosRelatorio = classeEmpresasContratos.EmpresasContratos;

            var dataSource = new ReportDataSource("DataSetContratos", dadosRelatorio);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.ReportEmbeddedResource = "iModSCCredenciamento.Relatorios.Report_Contratos.rdlc";
            // ReportViewer.LocalReport.ReportEmbeddedResource = "ModuloCredenciamento.Report1.rdlc";
            ReportViewer.RefreshReport();
        }
        private string RequisitaContratos(int _empresaID = 0, string _descricao = "", string _numerocontrato = "")
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

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _descricao = "%" + _descricao + "%";
                _numerocontrato = "%" + _numerocontrato + "%";

                _strSql = "select * from EmpresasContratos";
                // where EmpresaID = " + _empresaID + " and Descricao Like '" + _descricao + "' and NumeroContrato Like '" + _numerocontrato + "'";
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
                    _Arquivo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Arquivo"].ToString())));
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
    }
}
