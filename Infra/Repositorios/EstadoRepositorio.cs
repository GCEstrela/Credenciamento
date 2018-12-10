// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class EstadoRepositorio : IEstadosRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public EstadoRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        public ICollection<Estados> Listar()
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("Estados", conn))

                {
                    try
                    {
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Estados>();

                        return d1;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Buscar Estado por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public Estados BuscarEstadoPorUf(string uf)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("Estados", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("Uf", DbType.String, uf).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Estados>();

                        return d1.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public EstadoView BuscarEstadoMunicipiosPorUf(string uf)
        {
            var municipioRepositorio = new MunicipioRepositorio();
            var est = BuscarEstadoPorUf (uf); //Obter Estado
            var listMun = municipioRepositorio.Listar ("", uf); //Listar municipios por Uf
            var result = new EstadoView
            {
                Nome = est.Nome,
                EstadoId = est.EstadoId,
                Uf = est.Uf,
                Municipios = listMun
            };
            return result;
        }

        #endregion
    }
}