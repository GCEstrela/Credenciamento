// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

namespace Infra.Ado.Interfaces
{
    public interface IDataWorkerFactory
    {
        #region  Metodos

        /// <summary>
        ///     Obter banco de dados
        /// </summary>
        /// <param name="tipoDataBase">Tipo de base de dados</param>
        /// <param name="conectionstring">Nome da string de conexao</param>
        IDataBaseAdo ObterDataBaseSingleton(TipoDataBase tipoDataBase, string conectionstring);

        #endregion
    }
}