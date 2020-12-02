// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
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
    public class VeiculoObservacaoRepositorio : IVeiculoObservacaoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public VeiculoObservacaoRepositorio()
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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("VeiculosObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoObservacaoID", entity.VeiculoObservacaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Observacao", entity.Observacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impeditivo", entity.Impeditivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Resolvido", entity.Resolvido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("UsuarioRevisao", entity.UsuarioRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataRevisao", entity.DataRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoObservacaoRespostaID", entity.VeiculoObservacaoRespostaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ObservacaoResposta", entity.ObservacaoResposta, false)));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public VeiculoObservacao BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosObservacoes", conn))

                    {

                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamSelect("VeiculoObservacaoID", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoObservacao>();

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
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(VeiculoObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("VeiculosObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoObservacaoID", entity.VeiculoObservacaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Observacao", entity.Observacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Impeditivo", entity.Impeditivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Resolvido", entity.Resolvido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("UsuarioRevisao", entity.UsuarioRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataRevisao", entity.DataRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoObservacaoRespostaID", entity.VeiculoObservacaoRespostaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ObservacaoResposta", entity.ObservacaoResposta, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.VeiculoObservacaoId = key;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<VeiculoObservacao> Listar(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosObservacoes", conn))
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoObservacaoID", DbType.Int32, o, 1).Igual()));                        
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Observacao", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoObservacaoRespostaID", DbType.Int32, o, 3).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ObservacaoResposta", DbType.String, o, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoObservacao>();

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
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(VeiculoObservacao entity)
        {            
            using (var conn = _dataBase.CreateOpenConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    using (var cmd = _dataBase.DeleteText("VeiculosObservacoes", conn))
                    {
                        cmd.Transaction = tran;
                        var listarRespostas = Listar(entity.VeiculoId, entity.VeiculoObservacaoId).Where(ver => ver.VeiculoObservacaoRespostaId != null);
                        foreach (var resposta in listarRespostas)
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoObservacaoRespostaID", resposta.VeiculoObservacaoRespostaId).Igual()));
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (var cmd = _dataBase.DeleteText("VeiculosObservacoes", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoObservacaoID", entity.VeiculoObservacaoId).Igual()));
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        tran.Rollback();
                        conn.Close();
                    }
                    Utils.TraceException(ex);
                }
                    
            }            
        }

        #endregion
    }
}