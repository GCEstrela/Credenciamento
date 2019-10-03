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
        private const string key = "iModEstrela2016";
        public static string GetConnectionString()
        {

            string returnValue = null;
            //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XmlDocument xmlDoc = new XmlDocument();

            //Conexao Apenas para Publicação
            //xmlDoc.Load("C:\\inetpub\\wwwroot\\Credenciamento\\Conexao.xml");

            //Conexao Rodando local
            xmlDoc.Load("C:\\Windows\\Temp\\Conexao\\Conexao.xml");

            //Conexao no Temp
            //xmlDoc.Load(path + "\\Conexao.xml");

            XmlNode nodestring = xmlDoc.SelectSingleNode("StringConexao");
            //Exempla de ecriptação de senha//////////////////
            //var str_1 = nodestring.InnerXml.Split(';');
            //var str_2 = str_1[3].Split('=');
            //string senha = Decrypt(str_2[1]+"=");
            /////////////////////////////////////////////////
            returnValue = nodestring.InnerXml;
            return returnValue;

        }
        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    //tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] Buffer = Convert.FromBase64String(cipher.Trim());
                        return ASCIIEncoding.ASCII.GetString(tdes.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
                    }
                }
            }
        }
        #endregion
    }
}