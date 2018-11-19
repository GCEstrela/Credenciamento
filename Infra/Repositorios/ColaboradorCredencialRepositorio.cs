// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using CrossCutting;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Ado;
using Infra.Ado.Interfaces;
using Infra.Ado.Interfaces.ParamSql;

#endregion

namespace Infra.Repositorios
{
    public class ColaboradorCredencialRepositorio : IColaboradorCredencialRepositorio
    {
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorCredencialRepositorio()
        {
            //TODO:Retirar este trecho (testes)
            var connection = ConfigurationManager.ConnectionStrings["Credenciamento"].ConnectionString;

            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, connection);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
               

                var trans = conn.BeginTransaction();
                using (var cmd = _dataBase.UpdateText("ColaboradoresCredenciais", conn))
                {
                    try
                    {
                        cmd.Transaction = trans;

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorCredencialID",entity.ColaboradorCredencialId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorEmpresaID", DbType.Int32, entity.ColaboradorEmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TecnologiaCredencialID",DbType.Int32, entity.TecnologiaCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoCredencialID", DbType.Int32,entity.TipoCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID", DbType.Int32, entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FormatoCredencialID", DbType.Int32, entity.FormatoCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FC", DbType.Int32, entity.Fc, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao",DbType.DateTime,  entity.Emissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade",DbType.DateTime, entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialStatusID", DbType.Int32, entity.CredencialStatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CardHolderGUID", DbType.String, entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialGUID", DbType.String, entity.CredencialGuid, false)));

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio1ID", DbType.Int32, entity.ColaboradorPrivilegio1Id, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio2ID", DbType.Int32, entity.ColaboradorPrivilegio2Id, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativa", entity.Ativa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Colete", DbType.String, entity.Colete, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialmotivoID", DbType.Int32, entity.CredencialMotivoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Baixa", DbType.DateTime, entity.Baixa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impressa", entity.Impressa, false)));


                        cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Utils.TraceException(ex);
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
        public ColaboradorCredencial BuscarPelaChave(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="key">Primary key gerada no banco de dados</param>
        public void Criar(ColaboradorCredencial entity, out int key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColaboradorCredencial> Listar(params object[] objects)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
               
                var trans = conn.BeginTransaction();
                using (var cmd = _dataBase.DeleteText("ColaboradoresCredenciais", conn))
                {
                    try
                    {
                        cmd.Transaction = trans;

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorCredencialID",entity.ColaboradorCredencialId)));
                       
                        cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Utils.TraceException(ex);
                    }
                }
            }
        }
    }
}


//Template 1 ************************************************************
//var trans = conn.BeginTransaction();
//using (var cmd = _dataBase.CreateCommand())
//{
//    try
//    {
//        cmd.Transaction = trans;
//        cmd.Connection = conn;
//        cmd.CommandText = "Update ColaboradoresCredenciais Set NumeroCredencial=@v2 Where ColaboradorPrivilegio1ID=@v1";
//        cmd.Parameters.Add(_dataBase.CreateParameter("@v1", DbType.Int32, ParameterDirection.Input, 1));
//        cmd.Parameters.Add(_dataBase.CreateParameter("@v2", DbType.String, ParameterDirection.Input, "MIhai"));


//        cmd.ExecuteNonQuery();
//        trans.Commit();
//    }
//    catch (Exception ex)
//    {
//        Utils.TraceException(ex);
//        trans.Rollback();
//    }
//}

//Template 2 ************************************************************
 //using (var conn = _dataBase.CreateOpenConnection())
 //           {
               

 //               var trans = conn.BeginTransaction();
 //               using (var cmd = _dataBase.UpdateText("ColaboradoresCredenciais", conn))
 //               {
 //                   try
 //                   {
 //                       cmd.Transaction = trans;

 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorCredencialID", entity.ColaboradorCredencialId, true)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorEmpresaID", DbType.Int32, entity.ColaboradorEmpresaId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TecnologiaCredencialID", DbType.Int32, entity.TecnologiaCredencialId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoCredencialID", DbType.Int32, entity.TipoCredencialId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID", DbType.Int32, entity.LayoutCrachaId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FormatoCredencialID", DbType.Int32, entity.FormatoCredencialId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroCredencial", entity.NumeroCredencial, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FC", DbType.Int32, entity.Fc, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao", DbType.DateTime, entity.Emissao, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", DbType.DateTime, entity.Validade, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialStatusID", DbType.Int32, entity.CredencialStatusId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CardHolderGUID", DbType.String, entity.CardHolderGuid, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialGUID", DbType.String, entity.CredencialGuid, false)));

 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio1ID", DbType.Int32, entity.ColaboradorPrivilegio1Id, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio2ID", DbType.Int32, entity.ColaboradorPrivilegio2Id, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativa", entity.Ativa, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Colete", DbType.String, entity.Colete, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialmotivoID", DbType.Int32, entity.CredencialMotivoId, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Baixa", DbType.DateTime, entity.Baixa, false)));
 //                       cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impressa", entity.Impressa, false)));


 //                       cmd.ExecuteNonQuery();
 //                       trans.Commit();
 //                   }
 //                   catch (Exception ex)
 //                   {
 //                       trans.Rollback();
 //                       Utils.TraceException(ex);
 //                       throw;
 //                   }
 //               }
 //           }