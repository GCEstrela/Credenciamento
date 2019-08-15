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
    public class RelatoriosGerenciaisRepositorio : IRelatoriosGerenciaisRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public RelatoriosGerenciaisRepositorio()
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
        public void Criar(RelatoriosGerenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("RelatoriosGerenciais", conn))
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
        public RelatoriosGerenciais BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatoriosGerenciais", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("RelatorioId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<RelatoriosGerenciais>();

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
        public ICollection<RelatoriosGerenciais> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatoriosGerenciais", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<RelatoriosGerenciais>();

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
        public void Alterar(RelatoriosGerenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("RelatoriosGerenciais", conn))
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
        public void Remover(RelatoriosGerenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("RelatoriosGerenciais", conn))
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