// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
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
    public class TipoAtividadeRepositorio : ITipoAtividadeRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public TipoAtividadeRepositorio()
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

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(TipoAtividade entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("TiposAtividades", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoAtividadeID", entity.TipoAtividadeId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.TipoAtividadeId = key;

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
        /// <param name="id"></param>
        /// <returns></returns>
        public TipoAtividade BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("TiposAtividades", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("TipoAtividadeId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<TipoAtividade>();

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
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<TipoAtividade> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("TiposAtividades", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<TipoAtividade>();

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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(TipoAtividade entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("TiposAtividades", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoAtividadeID", entity.TipoAtividadeId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));

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
        ///     Deletar registro
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(TipoAtividade entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("TiposAtividades", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("TipoAtividadeId", entity.TipoAtividadeId).Igual()));
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