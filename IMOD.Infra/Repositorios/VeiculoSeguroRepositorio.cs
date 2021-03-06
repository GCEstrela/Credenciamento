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
    public class VeiculoSeguroRepositorio : IVeiculoSeguroRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculoSeguroRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro de VeiculoSeguro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoSeguro entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("VeiculosSeguros", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoSeguroID", entity.VeiculoSeguroId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeSeguradora", entity.NomeSeguradora, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NumeroApolice", entity.NumeroApolice, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ValorCobertura", entity.ValorCobertura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Arquivo", entity.Arquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Emissao", entity.Emissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", entity.Validade, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.VeiculoSeguroId = key;

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        ///     Buscar pela chave primaria VeiculoSeguro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoSeguro BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosSeguros", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("VeiculoSeguroId", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoSeguro>();

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
        ///     Listar VeiculoSeguro
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoSeguro> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosSegurosView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NomeSeguradora", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroApolice", DbType.String, objects, 2).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoSeguro>();

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
        ///     Alterar registro VeiculoSeguro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoSeguro entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("VeiculosSeguros", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoSeguroID", entity.VeiculoSeguroId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeSeguradora", entity.NomeSeguradora, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroApolice", entity.NumeroApolice, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ValorCobertura", entity.ValorCobertura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Arquivo", entity.Arquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao", entity.Emissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", entity.Validade, false)));

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
        ///     Deletar registro VeiculoSeguro
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(VeiculoSeguro entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("VeiculosSeguros", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoSeguroId", entity.VeiculoSeguroId).Igual()));

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