
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
    public class VeiculoCredencialRepositorio :IVeiculoCredencialRepositorio 
    {
	    private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public  VeiculoCredencialRepositorio()
        {
          _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoCredencial entity)
        {
             using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText ("VeiculoCredencial", conn))
                {
                    try
                    {
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("ColaboradorAnexoId", entity.ColaboradorAnexoId, true)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Descricao", entity.Descricao, false)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("NomeArquivo", entity.NomeArquivo, false)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("ColaboradorID", entity.ColaboradorId, false)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Arquivo", entity.Arquivo, false)));

                        var key = Convert.ToInt32 (cmd.ExecuteScalar());

                        entity.VeiculoCredencialId = key;
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
        public VeiculoCredencial BuscarPelaChave(int id)
        {
           using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculoCredencial", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoCredencial>();

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
        public ICollection<VeiculoCredencial> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculoCredencial", conn))

                {
                    try
                    {
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("NomeArquivo", o, 0).Like()));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("ColaboradorID", o, 1).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoCredencial>();

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
        public void Alterar(VeiculoCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText ("VeiculoCredencial", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoCredencialId", entity.VeiculoCredencialId, true)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Descricao", entity.Descricao, false)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("NomeArquivo", entity.NomeArquivo, false)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("ColaboradorID", entity.ColaboradorId, false)));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Arquivo", entity.Arquivo, false)));

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
        public void Remover(VeiculoCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText ("VeiculoCredencial", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamDelete ("VeiculoCredencialId", entity.VeiculoCredencialId).Igual()));

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

 