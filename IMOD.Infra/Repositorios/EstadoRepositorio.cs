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
            try
            {
                _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  Metodos

        /// <summary>
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        public ICollection<Estados> Listar()
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Estados", conn))

                    {

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Estados>();

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
        ///     Buscar Estado por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public Estados BuscarEstadoPorUf(string uf)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Estados", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("Uf", DbType.String, uf).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Estados>();

                        return d1.FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public EstadoView BuscarEstadoMunicipiosPorUf(string uf)
        {
            try
            {
                var municipioRepositorio = new MunicipioRepositorio();
                var est = BuscarEstadoPorUf(uf); //Obter Estado
                var listMun = municipioRepositorio.Listar("", uf); //Listar municipios por Uf
                var result = new EstadoView
                {
                    Nome = est.Nome,
                    EstadoId = est.EstadoId,
                    Uf = est.Uf,
                    Municipios = listMun
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}