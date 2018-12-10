using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Models;
using Microsoft.Win32;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupEmpresasRelatorios.xaml
    /// </summary>
    public partial class PopupEmpresasRelatorios : Window
    {
        public PopupEmpresasRelatorios()
        {
            InitializeComponent();

            try
            {
                ReportDocument reportDocument = new ReportDocument(); 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Imagem files (*.rpt)|*.jpg|All Files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    reportDocument.Load(filePath);

                }
                string _xml = RequisitaEmpresas("4");

                XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

                XmlDocument xmldocument = new XmlDocument();
                xmldocument.LoadXml(_xml);

                TextReader reader = new StringReader(_xml);
                ClasseEmpresas classeEmpresas = new ClasseEmpresas();
                classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
                var dadosRelatorio = new ObservableCollection<ClasseEmpresas.Empresa>();
                dadosRelatorio = classeEmpresas.Empresas;
                reportDocument.SetDataSource(dadosRelatorio);
                ReportViewer.ViewerCore.ReportSource = reportDocument;

            }
            catch (Exception ex)
            {

            }




            //var reportDocument2 = new Relatorio_Empresa();  // Relatorio_Empresas();
            //reportDocument2.SetDataSource(dadosRelatorio);

            //ReportViewer.ViewerCore.ReportSource = reportDocument2;
        }

        //private void ReportViewer_Load(object sender, EventArgs e)
        //{
        //    string _xml = RequisitaEmpresas();

        //    XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

        //    XmlDocument xmldocument = new XmlDocument();
        //    xmldocument.LoadXml(_xml);

        //    TextReader reader = new StringReader(_xml);
        //    ClasseEmpresas classeEmpresas = new ClasseEmpresas();
        //    classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
        //    var dadosRelatorio = new ObservableCollection<ClasseEmpresas.Empresa>();
        //    dadosRelatorio = classeEmpresas.Empresas;

            ////var dataSource = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetEmpresas", dadosRelatorio);
            ////ReportViewer.LocalReport.DataSources.Add(dataSource);
            ////ReportViewer.LocalReport.ReportEmbeddedResource = "iModSCCredenciamento.Relatorios.Report_Empresas.rdlc";
            ////// ReportViewer.LocalReport.ReportEmbeddedResource = "ModuloCredenciamento.Report1.rdlc";
            ////ReportViewer.RefreshReport();
        //}
        private void CarregaColecaoEmpresas(string _empresaID = "", string _nome = "", string _apelido = "", string _cNPJ = "", string _quantidaderegistro = "500")
        {
            try
            {
                //string _xml = RequisitaEmpresas(_empresaID, _nome, _apelido, _cNPJ);

                //XmlSerializer deserializer = new XmlSerializer(typeof(ClasseEmpresas));

                //XmlDocument xmldocument = new XmlDocument();
                //xmldocument.LoadXml(_xml);

                //TextReader reader = new StringReader(_xml);
                //ClasseEmpresas classeEmpresas = new ClasseEmpresas();
                //classeEmpresas = (ClasseEmpresas)deserializer.Deserialize(reader);
                //Empresas = new ObservableCollection<ClasseEmpresas.Empresa>();
                //Empresas = classeEmpresas.Empresas;
            }
            catch (Exception ex)
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

                string _strSql;

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _empresaID = "%" + _empresaID + "%";
                _nome = "%" + _nome + "%";
                _apelido = "%" + _apelido + "%";
                _cNPJ = "%" + _cNPJ + "%";

                if (_quantidaderegistro == "0")
                {
                    _strSql = "select * from Empresas where EmpresaID like '" + _empresaID + "' and EmpresaNome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + "";
                }
                else
                {
                    _strSql = "select Top " + _quantidaderegistro + " * from Empresas where EmpresaID like '" + _empresaID + "' and Nome like '" + _nome + "' and Apelido like '" + _apelido + "' and CNPJ like '" + _cNPJ + "' and Excluida  = " + _excluida + "";
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

                    XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
                    _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
                    _Empresa.AppendChild(_Apelido);

                    XmlNode _CNPJ = _xmlDocument.CreateElement("CNPJ");
                    _CNPJ.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNPJ"].ToString())));
                    _Empresa.AppendChild(_CNPJ);

                    XmlNode _InsEst = _xmlDocument.CreateElement("InsEst");
                    _InsEst.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsEst"].ToString())));
                    _Empresa.AppendChild(_InsEst);

                    XmlNode _InsMun = _xmlDocument.CreateElement("InsMun");
                    _InsMun.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["InsMun"].ToString())));
                    _Empresa.AppendChild(_InsMun);

                    XmlNode _Responsavel = _xmlDocument.CreateElement("Responsavel");
                    _Responsavel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Responsavel"].ToString())));
                    _Empresa.AppendChild(_Responsavel);

                    XmlNode _CEP = _xmlDocument.CreateElement("CEP");
                    _CEP.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CEP"].ToString())));
                    _Empresa.AppendChild(_CEP);

                    XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    _Empresa.AppendChild(_Endereco);

                    XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    _Empresa.AppendChild(_Numero);

                    XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    _Empresa.AppendChild(_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    _Empresa.AppendChild(_Bairro);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _Empresa.AppendChild(_EstadoID);

                    XmlNode _MunicipioID = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _Empresa.AppendChild(_MunicipioID);

                    XmlNode _Tel = _xmlDocument.CreateElement("Telefone");
                    _Tel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Telefone"].ToString())));
                    _Empresa.AppendChild(_Tel);

                    XmlNode _Cel = _xmlDocument.CreateElement("Celular");
                    _Cel.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Celular"].ToString())));
                    _Empresa.AppendChild(_Cel);

                    XmlNode _Contato = _xmlDocument.CreateElement("Contato");
                    _Contato.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Contato"].ToString())));
                    _Empresa.AppendChild(_Contato);

                    XmlNode _Email = _xmlDocument.CreateElement("Email");
                    _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
                    _Empresa.AppendChild(_Email);

                    XmlNode _Obs = _xmlDocument.CreateElement("Obs");
                    _Obs.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Obs"].ToString())));
                    _Empresa.AppendChild(_Obs);

                    XmlNode _Logo = _xmlDocument.CreateElement("Logo");
                    _Logo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Logo"].ToString())));
                    _Empresa.AppendChild(_Logo);

                    XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
                    _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    _Empresa.AppendChild(_Excluida);


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
