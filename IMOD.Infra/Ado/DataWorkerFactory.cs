// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.SQLServer;

#endregion

namespace IMOD.Infra.Ado
{
    public class DataWorkerFactory : IDataWorkerFactory
    {
        /// <summary>
        ///     Retorna instancia do banco de dados
        /// </summary>
        private static IDataBaseAdo _instanciaSqlServer;

        #region  Metodos

        /// <summary>
        ///     Retorna instancia do banco de dados
        /// </summary>
        /// <summary>
        ///     Obter banco de dados
        /// </summary>
        /// <param name="tipoDataBase">Tipo de base de dados</param>
        /// <param name="conectionstring">Nome da string de conexao</param>
        public IDataBaseAdo ObterDataBaseSingleton(TipoDataBase tipoDataBase, string conectionstring)
        {
            switch (tipoDataBase)
            {
                //case TipoDataBase.PostgreSql:
                //    if (_instanciaPostgres != null) return _instanciaPostgres;

                //    _instanciaPostgres = new PostgreDataBase();
                //    _instanciaPostgres.Connectionstring = conectionstring;
                //    return _instanciaPostgres;

                case TipoDataBase.SqlServer:
                    if (_instanciaSqlServer != null) return _instanciaSqlServer;
                    _instanciaSqlServer = new SqlServerDataBase();
                    _instanciaSqlServer.Connectionstring = conectionstring;
                    return _instanciaSqlServer;
                default:
                    throw new Exception("Não foi encontrado um tipo de banco de dados");
            }
        }

        #endregion
    }
}