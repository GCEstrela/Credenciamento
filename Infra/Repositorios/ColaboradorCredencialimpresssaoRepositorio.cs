// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
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
    public class ColaboradorCredencialimpresssaoRepositorio : IColaboradorCredencialimpresssaoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public ColaboradorCredencialimpresssaoRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(ColaboradorCredencialimpresssao entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("ColaboradoresCredenciaisImpressoes", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialImpressaoID",
                            DbType.Int32, entity.CredencialImpressaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorCredencialID",
                            DbType.Int32, entity.ColaboradorCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataImpressao", DbType.Date,
                            entity.DataImpressao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cobrar", DbType.Boolean,
                            entity.Cobrar, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.CredencialImpressaoId = key;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ColaboradorCredencialimpresssao BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradorCredencialimpresssao", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(
                                new ParamSelect("CredencialImpressaoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorCredencialimpresssao>();

                        return d1.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<ColaboradorCredencialimpresssao> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresCredenciaisImpressoes", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(
                            new ParamSelect("ColaboradorCredencialID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(
                            _dataBase.CreateParameter(new ParamSelect("DataImpressao", DbType.Date, objects, 1)
                                .Igual()));
                        cmd.CreateParameterSelect(
                            _dataBase.CreateParameter(new ParamSelect("Cobrar", DbType.Boolean, objects, 2).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorCredencialimpresssao>();

                        return d1;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorCredencialimpresssao entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("ColaboradoresCredenciaisImpressoes", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialImpressaoID",
                            DbType.Int32, entity.CredencialImpressaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorCredencialID",
                            DbType.Int32, entity.ColaboradorCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataImpressao", DbType.Date,
                            entity.DataImpressao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cobrar", DbType.Boolean,
                            entity.Cobrar, false)));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(ColaboradorCredencialimpresssao entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("ColaboradorCredencialimpresssao", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(
                            new ParamDelete("CredencialImpressaoId", entity.CredencialImpressaoId).Igual()));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                    }
                }
            }
        }

        #endregion
    }
}