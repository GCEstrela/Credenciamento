// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

namespace Infra.Ado
{
    public class DataBaseInfo
    {
        #region  Propriedades

        /// <summary>
        ///     Nome do Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Versao do Servidor de Banco de Dados
        /// </summary>
        public string VersaoServidor { get; set; }

        /// <summary>
        ///     Versao do componente
        /// </summary>
        public string VersaoProvider { get; set; }

        /// <summary>
        ///     Nome do provider
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        ///     String de conexão
        /// </summary>
        public string StringConexao { get; set; }

        /// <summary>
        ///     Base de dados
        /// </summary>
        public string BaseDados { get; set; }

        /// <summary>
        ///     True, conexao estabelecida
        /// </summary>
        public bool ConnexaoEstabelecida { get; set; }

        #endregion

        #region Construtor

        /// <summary>
        ///     Banco de dados
        /// </summary>
        //public Tipos Database { get; set; }

        #region Construtor
        public DataBaseInfo()
        {
            ConnexaoEstabelecida = false;
        }

        #endregion

        #endregion
    }
}