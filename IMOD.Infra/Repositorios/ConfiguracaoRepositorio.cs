// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 09 - 2019
// ***********************************************************************

#region

using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Interfaces;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class ConfiguracaoRepositorio : IConfiguracaoRepositorio,IInfoDataBase
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ConfiguracaoRepositorio()
        {
            try
            {
                _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obter informações do banco de dados
        /// </summary>
        public DataBaseInfo ObterInformacaoBancoDeDados
        {
            get { return _dataBase.Info (_connection); }
        }
    }
}