// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region



#endregion

using System.Configuration;

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
        public static string ConexaoString => "Data Source=172.16.190.108\\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";
        //public static string ConexaoString
        //{
        //    get
        //    {
        //        //return ConfigurationManager.ConnectionStrings["Credenciamento"].ConnectionString;
        //        //return ConfigurationManager.ConnectionStrings["Credenciamento"].ConnectionString;
        //        //return ConfigurationManager.ConnectionStrings["Credenciamento"].ConnectionString;
        //    }
        //}


        #endregion
    }
}