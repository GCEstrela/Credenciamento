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
    public class TipoCobrancaRepositorio : ITipoCobrancaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public TipoCobrancaRepositorio()
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
        public void Criar(TipoCobranca entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("TiposCobrancas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoCobrancaId", entity.TipoCobrancaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.TipoCobrancaId = key;

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
        public TipoCobranca BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("TiposCobrancas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("TipoCobrancaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<TipoCobranca>();

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
        public ICollection<TipoCobranca> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("TiposCobrancas", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<TipoCobranca>();

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
        public void Alterar(TipoCobranca entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("TiposCobrancas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoCobrancaId", entity.TipoCobrancaId, true)));
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
        public void Remover(TipoCobranca entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("TiposCobrancas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("TipoCobrancaId", entity.TipoCobrancaId).Igual()));
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