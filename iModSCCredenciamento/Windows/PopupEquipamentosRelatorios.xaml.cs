using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupEquipamentosRelatorios.xaml
    /// </summary>
    public partial class PopupEquipamentosRelatorios : Window
    {
        public PopupEquipamentosRelatorios()
        {
            InitializeComponent();
        }
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            //string _xml = RequisitaEquipamentos();

            //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresasEquipamentos));

            //XmlDocument xmldocument = new XmlDocument();
            //xmldocument.LoadXml(_xml);

            //TextReader reader = new StringReader(_xml);
            //ClasseEmpresasEquipamentos classeEquipamentosEmpresa = new ClasseEmpresasEquipamentos();
            //classeEquipamentosEmpresa = (ClasseEmpresasEquipamentos)deserializer.Deserialize(reader);
            //var dadosRelatorio = new ObservableCollection<ClasseEmpresasEquipamentos.EmpresaEquipamento>();
            //dadosRelatorio = classeEquipamentosEmpresa.EmpresasEquipamentos;

            //var dataSource = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetEquipamentos", dadosRelatorio);
            //ReportViewer.LocalReport.DataSources.Add(dataSource);
            //ReportViewer.LocalReport.ReportEmbeddedResource = "iModSCCredenciamento.Relatorios.Report_Equipamentos.rdlc";
            //// ReportViewer.LocalReport.ReportEmbeddedResource = "ModuloCredenciamento.Report1.rdlc";
            //ReportViewer.RefreshReport();
        }

        private string RequisitaEquipamentos(string _empresaID = "", string _descriao = "", string _marca = "")
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

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _empresaID = "%" + _empresaID + "%";
                _descriao = "%" + _descriao + "%";
                _marca = "%" + _marca + "%";

                _strSql = "select * from EmpresasEquipamentos where EmpresaID Like '" + _empresaID + "' and Descricao Like '" + _descriao + "' and Marca Like '" + _marca + "'";

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

                    XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
                    _DataEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataEmissao"].ToString())));
                    _EmpresaEquipamento.AppendChild(_DataEmissao);

                    XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
                    _DataValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataValidade"].ToString())));
                    _EmpresaEquipamento.AppendChild(_DataValidade);

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
            catch (Exception ex)
            {
                
                return null;
            }
            return null;
        }
    }
}
