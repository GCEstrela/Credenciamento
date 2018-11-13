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
    /// Lógica interna para PopupColaboradoresRelatorios.xaml
    /// </summary>
    public partial class PopupColaboradoresRelatorios : Window
    {
        public PopupColaboradoresRelatorios()
        {
            InitializeComponent();
        }
        private void ReportViewer_Load(object sender, EventArgs e)
        {

            string _xml = RequisitaColaboradores();

            XmlSerializer deserializer = new XmlSerializer(typeof(ClasseColaboradores));

            XmlDocument xmldocument = new XmlDocument();
            xmldocument.LoadXml(_xml);

            TextReader reader = new StringReader(_xml);
            ClasseColaboradores classeColaboradores = new ClasseColaboradores();
            classeColaboradores = (ClasseColaboradores)deserializer.Deserialize(reader);
            var dadosRelatorio = new ObservableCollection<ClasseColaboradores.Colaborador>();
            dadosRelatorio = classeColaboradores.Colaboradores;


            var dataSource = new Microsoft.Reporting.WinForms.ReportDataSource("DataSetColaboradores", dadosRelatorio);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            ReportViewer.LocalReport.ReportEmbeddedResource = "iModSCCredenciamento.Relatorios.Report_Colaboradores.rdlc";
           
            ReportViewer.RefreshReport();
        }
        private string RequisitaColaboradores(string _colaboradorID = "", string _nome = "", string _apelido = "", string _cpf = "", int _excluida = 0, string _quantidaderegistro = "500")
        {
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode _ClasseColaboradores = _xmlDocument.CreateElement("ClasseColaboradores");
                _xmlDocument.AppendChild(_ClasseColaboradores);

                XmlNode _Colaboradores = _xmlDocument.CreateElement("Colaboradores");
                _ClasseColaboradores.AppendChild(_Colaboradores);

                string _strSql;

                
                 SqlConnection _Con = new SqlConnection(Global._connectionString);_Con.Open();

                _colaboradorID = _colaboradorID == "" ? "" : " AND ColaboradorID = " + _colaboradorID;
                _nome = _nome == "" ? "" : " AND Nome like '%" + _nome + "%' ";
                _apelido = _apelido == "" ? "" : "AND Apelido like '%" + _apelido + "%' ";
                _cpf = _cpf == "" ? "" : " AND CPF like '%" + _cpf + "%'";

                if (_quantidaderegistro == "0")
                {
                    _strSql = "select * from Colaboradores where Excluida  = " + _excluida + _colaboradorID + _nome + _apelido + _cpf;
                }
                else
                {
                    _strSql = "select Top " + _quantidaderegistro + " * from Colaboradores where Excluida  = " + _excluida + _colaboradorID + _nome + _apelido + _cpf;
                }

                SqlCommand _sqlcmd = new SqlCommand(_strSql, _Con);
                SqlDataReader _sqlreader = _sqlcmd.ExecuteReader(CommandBehavior.Default);
                while (_sqlreader.Read())
                {

                    XmlNode _Colaborador = _xmlDocument.CreateElement("Colaborador");
                    _Colaboradores.AppendChild(_Colaborador);

                    XmlNode _ColaboradorID = _xmlDocument.CreateElement("ColaboradorID");
                    _ColaboradorID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ColaboradorID"].ToString())));
                    _Colaborador.AppendChild(_ColaboradorID);

                    XmlNode _Nome = _xmlDocument.CreateElement("Nome");
                    _Nome.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nome"].ToString())));
                    _Colaborador.AppendChild(_Nome);

                    XmlNode _Apelido = _xmlDocument.CreateElement("Apelido");
                    _Apelido.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Apelido"].ToString())));
                    _Colaborador.AppendChild(_Apelido);

                    XmlNode _DataNascimento = _xmlDocument.CreateElement("DataNascimento");
                    _DataNascimento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataNascimento"].ToString())));
                    _Colaborador.AppendChild(_DataNascimento);

                    XmlNode _NomePai = _xmlDocument.CreateElement("NomePai");
                    _NomePai.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomePai"].ToString())));
                    _Colaborador.AppendChild(_NomePai);

                    XmlNode _NomeMae = _xmlDocument.CreateElement("NomeMae");
                    _NomeMae.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["NomeMae"].ToString())));
                    _Colaborador.AppendChild(_NomeMae);

                    XmlNode _Nacionalidade = _xmlDocument.CreateElement("Nacionalidade");
                    _Nacionalidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Nacionalidade"].ToString())));
                    _Colaborador.AppendChild(_Nacionalidade);

                    XmlNode _Foto = _xmlDocument.CreateElement("Foto");
                    _Foto.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Foto"].ToString())));
                    _Colaborador.AppendChild(_Foto);

                    XmlNode _EstadoCivil = _xmlDocument.CreateElement("EstadoCivil");
                    _EstadoCivil.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoCivil"].ToString())));
                    _Colaborador.AppendChild(_EstadoCivil);

                    XmlNode _CPF = _xmlDocument.CreateElement("CPF");
                    _CPF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CPF"].ToString())));
                    _Colaborador.AppendChild(_CPF);

                    XmlNode _RG = _xmlDocument.CreateElement("RG");
                    _RG.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RG"].ToString())));
                    _Colaborador.AppendChild(_RG);

                    XmlNode _RGEmissao = _xmlDocument.CreateElement("RGEmissao");
                    _RGEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGEmissao"].ToString())));
                    _Colaborador.AppendChild(_RGEmissao);

                    XmlNode _RGOrgLocal = _xmlDocument.CreateElement("RGOrgLocal");
                    _RGOrgLocal.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgLocal"].ToString())));
                    _Colaborador.AppendChild(_RGOrgLocal);

                    XmlNode _RGOrgUF = _xmlDocument.CreateElement("RGOrgUF");
                    _RGOrgUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["RGOrgUF"].ToString())));
                    _Colaborador.AppendChild(_RGOrgUF);

                    XmlNode _Passaporte = _xmlDocument.CreateElement("Passaporte");
                    _Passaporte.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Passaporte"].ToString())));
                    _Colaborador.AppendChild(_Passaporte);

                    XmlNode _PassaporteValidade = _xmlDocument.CreateElement("PassaporteValidade");
                    _PassaporteValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["PassaporteValidade"].ToString())));
                    _Colaborador.AppendChild(_PassaporteValidade);

                    XmlNode _TelefoneFixo = _xmlDocument.CreateElement("TelefoneFixo");
                    _TelefoneFixo.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneFixo"].ToString())));
                    _Colaborador.AppendChild(_TelefoneFixo);

                    XmlNode _TelefoneCelular = _xmlDocument.CreateElement("TelefoneCelular");
                    _TelefoneCelular.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneCelular"].ToString())));
                    _Colaborador.AppendChild(_TelefoneCelular);

                    XmlNode _Email = _xmlDocument.CreateElement("Email");
                    _Email.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Email"].ToString())));
                    _Colaborador.AppendChild(_Email);

                    XmlNode _ContatoEmergencia = _xmlDocument.CreateElement("ContatoEmergencia");
                    _ContatoEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["ContatoEmergencia"].ToString())));
                    _Colaborador.AppendChild(_ContatoEmergencia);

                    XmlNode _TelefoneEmergencia = _xmlDocument.CreateElement("TelefoneEmergencia");
                    _TelefoneEmergencia.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TelefoneEmergencia"].ToString())));
                    _Colaborador.AppendChild(_TelefoneEmergencia);

                    XmlNode _Cep = _xmlDocument.CreateElement("Cep");
                    _Cep.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Cep"].ToString())));
                    _Colaborador.AppendChild(_Cep);

                    XmlNode _Endereco = _xmlDocument.CreateElement("Endereco");
                    _Endereco.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Endereco"].ToString())));
                    _Colaborador.AppendChild(_Endereco);

                    XmlNode _Numero = _xmlDocument.CreateElement("Numero");
                    _Numero.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Numero"].ToString())));
                    _Colaborador.AppendChild(_Numero);

                    XmlNode _Complemento = _xmlDocument.CreateElement("Complemento");
                    _Complemento.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Complemento"].ToString())));
                    _Colaborador.AppendChild(_Complemento);

                    XmlNode _Bairro = _xmlDocument.CreateElement("Bairro");
                    _Bairro.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bairro"].ToString())));
                    _Colaborador.AppendChild(_Bairro);

                    XmlNode _MunicipioId = _xmlDocument.CreateElement("MunicipioID");
                    _MunicipioId.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["MunicipioID"].ToString())));
                    _Colaborador.AppendChild(_MunicipioId);

                    XmlNode _EstadoID = _xmlDocument.CreateElement("EstadoID");
                    _EstadoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["EstadoID"].ToString())));
                    _Colaborador.AppendChild(_EstadoID);

                    XmlNode _Motorista = _xmlDocument.CreateElement("Motorista");
                    _Motorista.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Motorista"].ToString())));
                    _Colaborador.AppendChild(_Motorista);

                    XmlNode _CNH = _xmlDocument.CreateElement("CNH");
                    _CNH.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNH"].ToString())));
                    _Colaborador.AppendChild(_CNH);

                    XmlNode _CNHValidade = _xmlDocument.CreateElement("CNHValidade");
                    _CNHValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CHNValidade"].ToString())));
                    _Colaborador.AppendChild(_CNHValidade);

                    XmlNode _CNHEmissor = _xmlDocument.CreateElement("CNHEmissor");
                    _CNHEmissor.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHEmissor"].ToString())));
                    _Colaborador.AppendChild(_CNHEmissor);

                    XmlNode _CNHUF = _xmlDocument.CreateElement("CNHUF");
                    _CNHUF.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["CNHUF"].ToString())));
                    _Colaborador.AppendChild(_CNHUF);

                    XmlNode _Bagagem = _xmlDocument.CreateElement("Bagagem");
                    _Bagagem.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Bagagem"].ToString())));
                    _Colaborador.AppendChild(_Bagagem);

                    XmlNode _DataEmissao = _xmlDocument.CreateElement("DataEmissao");
                    _DataEmissao.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataEmissao"].ToString())));
                    _Colaborador.AppendChild(_DataEmissao);

                    XmlNode _DataValidade = _xmlDocument.CreateElement("DataValidade");
                    _DataValidade.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["DataValidade"].ToString())));
                    _Colaborador.AppendChild(_DataValidade);

                    XmlNode _Excluida = _xmlDocument.CreateElement("Excluida");
                    _Excluida.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["Excluida"].ToString())));
                    _Colaborador.AppendChild(_Excluida);

                    XmlNode _StatusID = _xmlDocument.CreateElement("StatusID");
                    _StatusID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["StatusID"].ToString())));
                    _Colaborador.AppendChild(_StatusID);

                    XmlNode _TipoAcessoID = _xmlDocument.CreateElement("TipoAcessoID");
                    _TipoAcessoID.AppendChild(_xmlDocument.CreateTextNode((_sqlreader["TipoAcessoID"].ToString())));
                    _Colaborador.AppendChild(_TipoAcessoID);

                }

                _sqlreader.Close();

                _Con.Close();
                string _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                //InsereColaboradoresBD("");

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
