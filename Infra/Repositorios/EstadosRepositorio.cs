
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
    public class EstadosRepositorio :IEstadosRepositorio 
    {
	    private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public  EstadosRepositorio()
        {
          _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Estados entity)
        {
             using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText ("Estados", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("UF", entity.Uf, false)));


                        var key = Convert.ToInt32 (cmd.ExecuteScalar());

                        entity.EstadoId = key;
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
        public Estados BuscarPelaChave(int id)
        {
           using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("Estados", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("EstadoId", DbType.Int32, id).Igual()));
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
        ///     Listar
        /// </summary>
        /// <param name="predicate">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Estados> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("Estados", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EstadoID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("UF",DbType.String, objects, 2).Like()));

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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Estados entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText ("Estados", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("UF", entity.Uf, false)));

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
        public void Remover(Estados entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText ("Estados", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamDelete ("EstadoId", entity.EstadoId).Igual()));

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

 