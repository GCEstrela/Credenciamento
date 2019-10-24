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
            //string path = @"C:\Projetos\IMOD\IMOD.Infra";
            //string path = @"C:\Users\renatomaximo\Desktop\Credenciamento\IMOD.Infra";
            XmlDocument xmlDoc = new XmlDocument();

            //Conexao Apenas para Publicação Pré Cadastro
            //xmlDoc.Load("C:\\inetpub\\wwwroot\\Credenciamento\\Conexao.xml");

            //Conexao Apenas para Publicação ValidaCredencial QRcode
            //xmlDoc.Load("C:\\inetpub\\wwwroot\\ValidaCredencial\\Conexao.xml");

            //Conexao Rodando local
            //xmlDoc.Load("C:\\Windows\\Temp\\Conexao\\Conexao.xml");

            //Conexao no Temp
            xmlDoc.Load(path + "\\Conexao.xml");

            XmlNode nodestring = xmlDoc.SelectSingleNode("StringConexao");
            XmlNode instancia = nodestring.SelectSingleNode("InstanciaSQL");
            XmlNode banco = nodestring.SelectSingleNode("Banco");
            XmlNode usuario = nodestring.SelectSingleNode("UsuarioDB");
            XmlNode senha = nodestring.SelectSingleNode("SenhaDB");
            string senhaDecryptada = senha.InnerXml;

            EstrelaEncryparDecrypitar.Decrypt ESTRELA_EMCRYPTAR = new EstrelaEncryparDecrypitar.Decrypt();
            EstrelaEncryparDecrypitar.Variavel.key = "CREDENCIAMENTO2019";
            senhaDecryptada = ESTRELA_EMCRYPTAR.EstrelaDecrypt(senhaDecryptada);

            XmlNode complemento = nodestring.SelectSingleNode("Complemento");
            returnValue = "Data Source=" + instancia.InnerXml + ";Initial Catalog="+ banco.InnerXml + ";User ID="+ usuario.InnerXml + ";Password="+ senhaDecryptada + ";"+ complemento.InnerXml;
                      

            return returnValue;

        }
       
        #endregion
    }
}