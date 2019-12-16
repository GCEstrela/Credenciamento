// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region

#endregion

using IMOD.Infra.Properties;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Xml.Linq;
using System.Linq;
using IMOD.Domain.EntitiesCustom;

namespace IMOD.Infra.Ado
{

    /// <summary>
    ///     Obtem configurações correntes
    /// </summary>
    public static class CurrentConfig
    {
        #region  Propriedades

        /// <summary>
        ///     String de conexao com o banco de dados
        /// </summary>
        //public static string ConexaoString => Settings.Default.Credenciamento;
        public static string ConexaoString => GetConnectionString();
        // private readonly TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
        //private readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
        //private const string key = "iModEstrela2016";
        public static string GetConnectionString()
        {

            string returnValue = null;
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XmlDocument xmlDoc = new XmlDocument();
            ////////////////////
            //string nodestring = "";
            string instancia = "";
            string banco = "";
            string usuario = "";
            string senha = "";
            string complemento = "";
            // cria a consulta
            var prods = from p in XElement.Load(path + "\\Conexao.xml").Elements("Produto")
                        where p.Element("SystemID").Value == UsuarioLogado.sdiLicenca
                        select new
                        {
                            instancia = p.Element("InstanciaSQL").Value,
                            banco = p.Element("Banco").Value,
                            usuarioDB = p.Element("UsuarioDB").Value,
                            SenhaDB = p.Element("SenhaDB").Value,
                            complemento = p.Element("Complemento").Value,
                        };

            // Executa a consulta
            foreach (var produto in prods)
            {
                //lbProdutos.Items.Add(produto.NomeProduto);
                instancia = produto.instancia;
                banco = produto.banco;
                usuario = produto.usuarioDB;
                senha = produto.SenhaDB;
                complemento = produto.complemento;
            }
            

            ////Conexao no Temp
            //xmlDoc.Load(path + "\\Conexao.xml");

            //XmlNode nodestring = xmlDoc.SelectSingleNode("StringConexao");
            //XmlNode instancia = nodestring.SelectSingleNode("InstanciaSQL");
            //XmlNode banco = nodestring.SelectSingleNode("Banco");
            //XmlNode usuario = nodestring.SelectSingleNode("UsuarioDB");
            //XmlNode senha = nodestring.SelectSingleNode("SenhaDB");
            //string senhaDecryptada = senha.InnerXml;

            EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
            EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
            string senhaDecryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(senha);

            //XmlNode complemento = nodestring.SelectSingleNode("Complemento");
            returnValue = "Data Source=" + instancia + ";Initial Catalog="+ banco + ";User ID="+ usuario + ";Password="+ senhaDecryptada + ";"+ complemento;
            UsuarioLogado.InstanciaSQL = returnValue;

            return returnValue;

        }
       
        #endregion
    }
}