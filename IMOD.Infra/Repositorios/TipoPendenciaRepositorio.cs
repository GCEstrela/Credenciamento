// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 17 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class TipoPendenciaRepositorio : ITipoPendenciaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public TipoPendenciaRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<TipoPendencia> Listar()
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("TipoPendencia", conn))

                    {

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<TipoPendencia>();

                        return d1;

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Obter Tipo Pendencia por codigo
        /// </summary>
        /// <param name="codPendencia"></param>
        /// <returns></returns>
        public TipoPendencia BuscarPorCodigo(string codPendencia)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("TipoPendencia", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("CodPendencia", DbType.Int32, codPendencia).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<TipoPendencia>();

                        return d1.FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}