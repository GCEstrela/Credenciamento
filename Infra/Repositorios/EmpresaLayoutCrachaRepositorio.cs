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
    public class EmpresaLayoutCrachaRepositorio : IEmpresaLayoutCrachaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaLayoutCrachaRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EmpresaLayoutCracha entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("EmpresasLayoutsCrachas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaLayoutCrachaID",
                            entity.EmpresaLayoutCrachaId, true)));
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutCrachaID",
                            entity.LayoutCrachaId, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaLayoutCrachaId = key;
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
        public EmpresaLayoutCracha BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresasLayoutsCrachas", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(
                                new ParamSelect("EmpresaLayoutCrachaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaLayoutCracha>();

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
        public ICollection<EmpresaLayoutCracha> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresasLayoutsCrachas", conn))

                {
                    try
                    {
                        //cmd.CreateParameterSelect(_dataBase.CreateParameter (new ParamSelect ("NomeArquivo", o, 0).Like()));
                        //cmd.CreateParameterSelect(_dataBase.CreateParameter (new ParamSelect ("ColaboradorID", o, 1).Igual()));

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(
                            new ParamSelect("EmpresaLayoutCrachaID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(
                            _dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, objects, 1).Igual()));
                        cmd.CreateParameterSelect(
                            _dataBase.CreateParameter(
                                new ParamSelect("LayoutCrachaID", DbType.Int32, objects, 2).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaLayoutCracha>();

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
        public void Alterar(EmpresaLayoutCracha entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("EmpresasLayoutsCrachas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaLayoutCrachaID",
                            entity.EmpresaLayoutCrachaId, true)));
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID",
                            entity.LayoutCrachaId, false)));

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
        public void Remover(EmpresaLayoutCracha entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("EmpresasLayoutsCrachas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(
                            new ParamDelete("EmpresaLayoutCrachaId", entity.EmpresaLayoutCrachaId).Igual()));

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