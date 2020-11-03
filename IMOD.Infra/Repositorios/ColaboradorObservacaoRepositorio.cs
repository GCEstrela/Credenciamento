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
    public class ColaboradorObservacaoRepositorio : IColaboradorObservacaoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorObservacaoRepositorio()
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
        public void Alterar(ColaboradorObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("ColaboradoresObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorObservacaoId", entity.ColaboradorObservacaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impeditivo", entity.Impeditivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Resolvido", entity.Resolvido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("UsuarioRevisao", entity.UsuarioRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataRevisao", entity.DataRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoSituacao", entity.TipoSituacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorObservacaoRespostaID", entity.ColaboradorObservacaoRespostaID, false)));
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
        public ColaboradorObservacao BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresObservacoes", conn))

                    {

                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamSelect("ColaboradorObservacaoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorObservacao>();

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
        public void Criar(ColaboradorObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("ColaboradoresObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorObservacaoId", entity.ColaboradorObservacaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Observacao", entity.Observacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Impeditivo", entity.Impeditivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Resolvido", entity.Resolvido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("UsuarioRevisao", entity.UsuarioRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataRevisao", entity.DataRevisao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoSituacao", entity.TipoSituacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorObservacaoRespostaID", entity.ColaboradorObservacaoRespostaID, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ObservacaoResposta", entity.ObservacaoResposta, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorObservacaoId = key;
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
        public ICollection<ColaboradorObservacao> Listar(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradorObservacaoView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorObservacaoID", DbType.Int32, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Observacao", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorObservacaoRespostaID", DbType.Int32, o, 3).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ObservacaoResposta", DbType.String, o, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorObservacao>();

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
        public void Remover(ColaboradorObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("ColaboradoresObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorObservacaoId", entity.ColaboradorObservacaoId).Igual()));

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}