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
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class EquipamentoVeiculoTipoServicoRepositorio : IEquipamentoVeiculoTipoServicoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EquipamentoVeiculoTipoServicoRepositorio()
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
        public void Criar(EquipamentoVeiculoTipoServico entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EquipamentoVeiculoTiposServicos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EquipamentoVeiculoTipoServicoID", entity.EquipamentoVeiculoTipoServicoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EquipamentoVeiculoID", entity.EquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoServicoID", entity.TipoServicoId, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EquipamentoVeiculoTipoServicoId = key;

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
        public EquipamentoVeiculoTipoServico BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EquipamentoVeiculoTiposServicos", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoTipoServicoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EquipamentoVeiculoTipoServico>();

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
        public ICollection<EquipamentoVeiculoTipoServico> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EquipamentoVeiculoTiposServicos", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoTipoServicoID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoID", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoServicoID", DbType.Int32, objects, 2).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EquipamentoVeiculoTipoServico>();

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
        public void Alterar(EquipamentoVeiculoTipoServico entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EquipamentoVeiculoTiposServicos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EquipamentoVeiculoTipoServicoID", entity.EquipamentoVeiculoTipoServicoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EquipamentoVeiculoID", entity.EquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoServicoID", entity.TipoServicoId, false)));

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
        /// <param name="entity"></param>
        public void Remover(EquipamentoVeiculoTipoServico entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EquipamentoVeiculoTiposServicos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EquipamentoVeiculoTipoServicoId", entity.EquipamentoVeiculoTipoServicoId).Igual()));

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
        /// Remover 
        /// </summary>
        /// <param name="equipamentoVeiculoId"></param>
        public void RemoverPorVeiculo(int equipamentoVeiculoId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EquipamentoVeiculoTiposServicos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EquipamentoVeiculoId", equipamentoVeiculoId).Igual()));

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<EquipamentoVeiculoTipoServicoView> ListarEquipamentoVeiculoTipoServicoView(int equipamentoVeiculoId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EquipamentoVeiculoTipoServicoView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoID", DbType.Int32, equipamentoVeiculoId).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EquipamentoVeiculoTipoServicoView>();
                        return d1;

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