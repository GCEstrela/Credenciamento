﻿// ***********************************************************************
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
    public class RelatoriosRepositorio : IRelatoriosRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public RelatoriosRepositorio()
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
        public void Criar(Relatorios entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("Relatorios", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RelatorioID", entity.RelatorioId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivoRPT", entity.NomeArquivoRpt, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ArquivoRPT", entity.ArquivoRpt, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", entity.Ativo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.RelatorioId = key;

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
        public Relatorios BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Relatorios", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("RelatorioID", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Relatorios>();

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
        public ICollection<Relatorios> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Relatorios", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Relatorios>();

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
        public void Alterar(Relatorios entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("Relatorios", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RelatorioID", entity.RelatorioId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivoRPT", entity.NomeArquivoRpt, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ArquivoRPT", entity.ArquivoRpt, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));

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
        public void Remover(Relatorios entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("Relatorios", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("RelatorioID", entity.RelatorioId).Igual()));
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