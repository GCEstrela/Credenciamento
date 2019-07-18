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

        public static string GetConnectionString()
        {

            string returnValue = null;
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path + "\\Conexao.xml");
            XmlNode nodestring = xmlDoc.SelectSingleNode("StringConexao");
            returnValue = nodestring.InnerXml;

            return returnValue;

        }
        #endregion
    }
}