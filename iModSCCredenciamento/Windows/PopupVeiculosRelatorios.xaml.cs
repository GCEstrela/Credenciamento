// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Xml;
using IMOD.Infra.Ado;

#endregion

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    ///     Lógica interna para PopupVeiculosRelatorios.xaml
    /// </summary>
    public partial class PopupVeiculosRelatorios : Window
    {
        private readonly string _connection = CurrentConfig.ConexaoString;

        public PopupVeiculosRelatorios()
        {
            InitializeComponent();
        }

        #region  Metodos

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            
        }

        private string RequisitaEmpresasVeiculos(int _empresaID = 0, string _placa = "", string _marca = "", string _modelo = "", int _ativo = 2)
        {
            try
            {
                var _xmlDocument = new XmlDocument();
                XmlNode _xmlNode = _xmlDocument.CreateXmlDeclaration ("1.0", "UTF-8", null);

                XmlNode _ClasseEmpresasVeiculos = _xmlDocument.CreateElement ("ClasseEmpresasVeiculos");
                _xmlDocument.AppendChild (_ClasseEmpresasVeiculos);

                XmlNode _EmpresasVeiculos = _xmlDocument.CreateElement ("EmpresasVeiculos");
                _ClasseEmpresasVeiculos.AppendChild (_EmpresasVeiculos);

                string _strSql;

                var _Con = new SqlConnection (_connection);
                _Con.Open();

                _placa = _placa == "" ? "" : " AND Placa like '%" + _placa + "%' ";
                _marca = _marca == "" ? "" : " AND Marca like '%" + _marca + "%' ";
                _modelo = _modelo == "" ? "" : " AND Modelo like '%" + _modelo + "%'";
                var _ativoStr = _ativo == 2 ? "" : " AND dbo.EmpresasVeiculos.Ativo = " + _ativo;

                _strSql = "SELECT dbo.EmpresasVeiculos.*, dbo.LayoutsCrachas.Nome FROM dbo.EmpresasVeiculos LEFT OUTER JOIN" +
                          " dbo.LayoutsCrachas ON dbo.EmpresasVeiculos.LayoutCrachaID = dbo.LayoutsCrachas.LayoutCrachaID  ";

                var _sqlcmd = new SqlCommand (_strSql, _Con);
                var _sqlreader = _sqlcmd.ExecuteReader (CommandBehavior.Default);
                while (_sqlreader.Read())
                {
                    XmlNode _EmpresaVeiculo = _xmlDocument.CreateElement ("EmpresaVeiculo");
                    _EmpresasVeiculos.AppendChild (_EmpresaVeiculo);

                    XmlNode _EmpresaVeiculoID = _xmlDocument.CreateElement ("EmpresaVeiculoID");
                    _EmpresaVeiculoID.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmpresaVeiculoID"].ToString()));
                    _EmpresaVeiculo.AppendChild (_EmpresaVeiculoID);

                    XmlNode _Node1 = _xmlDocument.CreateElement ("Validade");
                    _Node1.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Validade"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node1);

                    XmlNode _Node2 = _xmlDocument.CreateElement ("Descricao");
                    _Node2.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Descricao"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node2);

                    XmlNode _Node3 = _xmlDocument.CreateElement ("Tipo");
                    _Node3.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Tipo"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node3);

                    XmlNode _Node4 = _xmlDocument.CreateElement ("Marca");
                    _Node4.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Marca"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node4);

                    XmlNode _Node5 = _xmlDocument.CreateElement ("Modelo");
                    _Node5.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Modelo"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node5);

                    XmlNode _Node6 = _xmlDocument.CreateElement ("Ano");
                    _Node6.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Ano"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node6);

                    XmlNode _Node7 = _xmlDocument.CreateElement ("Cor");
                    _Node7.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Cor"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node7);

                    XmlNode _Node8 = _xmlDocument.CreateElement ("Placa");
                    _Node8.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Placa"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node8);

                    XmlNode _Node9 = _xmlDocument.CreateElement ("Renavam");
                    _Node9.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Renavam"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node9);

                    XmlNode _Node10 = _xmlDocument.CreateElement ("EstadoID");
                    _Node10.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EstadoID"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node10);

                    XmlNode _Node11 = _xmlDocument.CreateElement ("MunicipioID");
                    _Node11.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["MunicipioID"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node11);

                    XmlNode _Node12 = _xmlDocument.CreateElement ("Seguro");
                    _Node12.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Seguro"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node12);

                    XmlNode _Node13 = _xmlDocument.CreateElement ("EmpresaID");
                    _Node13.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["EmpresaID"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node13);

                    XmlNode _Node14 = _xmlDocument.CreateElement ("Ativo");
                    _Node14.AppendChild (_xmlDocument.CreateTextNode (Convert.ToInt32 ((bool) _sqlreader["Ativo"]).ToString()));
                    _EmpresaVeiculo.AppendChild (_Node14);

                    XmlNode _Node15 = _xmlDocument.CreateElement ("LayoutCrachaID");
                    _Node15.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["LayoutCrachaID"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node15);

                    XmlNode _Node16 = _xmlDocument.CreateElement ("LayoutCrachaNome");
                    _Node16.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["Nome"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node16);

                    XmlNode _Node17 = _xmlDocument.CreateElement ("FormatoCredencialID");
                    _Node17.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["FormatoCredencialID"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node17);

                    XmlNode _Node18 = _xmlDocument.CreateElement ("NumeroCredencial");
                    _Node18.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["NumeroCredencial"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node18);

                    XmlNode _Node19 = _xmlDocument.CreateElement ("FC");
                    _Node19.AppendChild (_xmlDocument.CreateTextNode (_sqlreader["FC"].ToString()));
                    _EmpresaVeiculo.AppendChild (_Node19);
                }

                _sqlreader.Close();

                _Con.Close();
                var _xml = _xmlDocument.InnerXml;
                _xmlDocument = null;
                return _xml;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        #endregion
    }
}