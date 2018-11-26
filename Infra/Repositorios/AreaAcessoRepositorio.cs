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
    public class AreaAcessoRepositorio : IAreaAcesso
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public AreaAcessoRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(AreaAcesso entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText ("AreasAcessos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("AreaAcessoID", entity.AreaAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Identificacao", entity.Identificacao, false)));


                        var key = Convert.ToInt32 (cmd.ExecuteScalar());

                        entity.AreaAcessoId = key;
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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AreaAcesso BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("AreasAcessos", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("AreaAcessoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<AreaAcesso>();

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
        ///     Listar
        /// </summary>
        /// <param name="predicate">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<AreaAcesso> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("AreasAcessos", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("AreaAcessoID", objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", objects, 1).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<AreaAcesso>();

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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(AreaAcesso entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText ("AreasAcessos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("AreaAcessoID", entity.AreaAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Identificacao", entity.Identificacao, false)));

                        cmd.ExecuteNonQuery();
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
        ///     Deletar registro
        /// </summary>
        /// <param name="predicate"></param>
        public void Remover(AreaAcesso entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText ("AreasAcessos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamDelete ("AreaAcessoId", entity.AreaAcessoId).Igual()));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                    }
                }
            }
        }

        #endregion
    }
}