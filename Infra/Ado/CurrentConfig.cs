// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region

using System;
using System.Configuration;

#endregion

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
        //public static string ConexaoString
        //{
        //    get
        //    {

        //        try
        //        {
        //            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //            //var vr = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        //            //r strConfigPathf = new ConfigurationFileMap(System.Reflection.Assembly.GetExecutingAssembly().Location);

        //            //r c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
        //            //System.Configuration.ConfigurationFileMap fileMap = strConfigPathf;//new ConfigurationFileMap(strConfigPath); //Path to your config file

        //            //System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
        //            //ConfigurationManager.OpenExeConfiguration(c);

        //            //var  h = configuration.ConnectionStrings["Credenciamento"].ConnectionString;


        //            //ar t = Properties.Settings.Default.TestValnei;

        //            //ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
        //            //configMap.ExeConfigFilename = @"iModSCCredenciamento.dll.config";
        //            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        //            //var f = config.ConnectionStrings.ConnectionStrings["Credenciamento"].ConnectionString;
        //            //var t = ConfigurationManager.ConnectionStrings;
        //            //var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);


        //            //ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
        //            //configMap.ExeConfigFilename = @"iModSCCredenciamento.dll.config";
        //            ///Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

        //            //var f = config.ConnectionStrings.ConnectionStrings["Credenciamento"].ConnectionString;

        //            //Configuration config = ConfigurationManager.OpenExeConfiguration(@"C:\Path\WpfApplication1.exe");


        //            //return ConfigurationManager.ConnectionStrings["Credenciamento"].ConnectionString;

        //            //public static string ConexaoString => "Data Source=172.16.190.108\\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";

        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }


        //        return null;
        //    }
        //}

        public static string ConexaoString => "Data Source=172.16.190.108\\SQLEXPRESS;Initial Catalog=D_iModCredenciamento;User ID=imod;Password=imod;Min Pool Size=5;Max Pool Size=15;Connection Reset=True;Connection Lifetime=600;Trusted_Connection=no;MultipleActiveResultSets=True";

        #endregion
    }
}