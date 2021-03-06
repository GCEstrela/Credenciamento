﻿// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
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
    public class EmpresaEquipamentoRepositorio : IEmpresaEquipamentoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaEquipamentoRepositorio()
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
        public void Criar(EmpresaEquipamento entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EmpresasEquipamentos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaEquipamentoID", entity.EmpresaEquipamentoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Patrimonio", entity.Patrimonio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Seguro", entity.Seguro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ApoliceSeguro", entity.ApoliceSeguro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ApoliceValor", entity.ApoliceValor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ApoliceVigencia", entity.ApoliceVigencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataEmissao", entity.DataEmissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataValidade", entity.DataValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Excluido", entity.Excluido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoEquipamentoID", entity.TipoEquipamentoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoAcessoID", entity.TipoAcessoId, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaEquipamentoId = key;

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
        public EmpresaEquipamento BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasEquipamentos", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaEquipamentoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaEquipamento>();

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
        public ICollection<EmpresaEquipamento> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasEquipamentos", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, objects, 0).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Marca", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Modelo", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ano", DbType.String, objects, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Patrimonio", DbType.String, objects, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaEquipamento>();

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
        public void Alterar(EmpresaEquipamento entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EmpresasEquipamentos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaEquipamentoID", entity.EmpresaEquipamentoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Patrimonio", entity.Patrimonio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Seguro", entity.Seguro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ApoliceSeguro", entity.ApoliceSeguro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ApoliceValor", entity.ApoliceValor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ApoliceVigencia", entity.ApoliceVigencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataEmissao", entity.DataEmissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataValidade", entity.DataValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Excluido", entity.Excluido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoEquipamentoID", entity.TipoEquipamentoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoAcessoID", entity.TipoAcessoId, false)));

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
        public void Remover(EmpresaEquipamento entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasEquipamentos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaEquipamentoId", entity.EmpresaEquipamentoId).Igual()));
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