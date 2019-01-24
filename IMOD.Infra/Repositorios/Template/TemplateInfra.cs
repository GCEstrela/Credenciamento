// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
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
    public class ColaboradorAnexoRepositorio : IColaboradorAnexoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorAnexoRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorAnexo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                
                using (var cmd = _dataBase.UpdateText ("ColaboradoresAnexos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("ColaboradorAnexoId", entity. ColaboradorAnexoId, true)));
                        //Coloque aqui todos os parâmetros (campos da tabela), seguindo o exemplo abaixo 
						//cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("ColaboradorEmpresaID",entity.ColaboradorEmpresaId, false)));
                       

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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorAnexo BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("ColaboradoresAnexos", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (
                            _dataBase.CreateParameter ( new ParamSelect ("ColaboradorAnexoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorAnexo>();

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
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorAnexo entity)
        {
            
            using (var conn = _dataBase.CreateOpenConnection())
            {
               
                using (var cmd = _dataBase.InsertText ("ColaboradoresAnexos", conn))
                {
                    try
                    {                       

                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("ColaboradorAnexoId", entity.ColaboradorAnexoId, true)));
                        //Coloque aqui todos os parâmetros (campos da tabela), seguindo o exemplo abaixo 
						//cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("ColaboradorEmpresaID", DbType.Int32, entity.ColaboradorEmpresaId, false)));
                        
						
                        var key = Convert.ToInt32 (cmd.ExecuteScalar());
                      
                        entity.ColaboradorAnexoId = key;
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
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> Listar(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("ColaboradoresAnexos", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("ColaboradorCredencialID", DbType.Int32, o, 0).Igual()));
						//Informe demais parametros se existir...
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("CampoA",o, 1).Igual()));
                        //cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("CampoB", DbType.Int32, o, 2).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorAnexo>();

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
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorAnexo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
               
                using (var cmd = _dataBase.DeleteText ("ColaboradoresAnexos", conn))
                {
                    try
                    {                      

                        cmd.Parameters.Add (
                            _dataBase.CreateParameter (new ParamDelete ("ColaboradorAnexoId", entity.ColaboradorAnexoId)));

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
 