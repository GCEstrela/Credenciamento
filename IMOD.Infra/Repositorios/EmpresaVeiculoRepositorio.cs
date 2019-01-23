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
    public class EmpresaVeiculoRepositorio : IEmpresaVeiculoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaVeiculoRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EmpresaVeiculo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("EmpresaVeiculos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaVeiculoID", entity.EmpresaVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Tipo", entity.Tipo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cor", entity.Cor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Placa", entity.Placa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Renavam", entity.Renavam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Seguro", entity.Seguro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutCrachaID", entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FC", entity.Fc, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FormatoCredencialID", entity.FormatoCredencialId, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaVeiculoId = key;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaVeiculo BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresaVeiculos", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaVeiculoId", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaVeiculo>();

                        return d1.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaVeiculo> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresaVeiculos", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Validade", DbType.Date, objects, 0)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, objects, 1)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Tipo", DbType.String, objects, 2)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Marca", DbType.String, objects, 3)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Modelo", DbType.String, objects, 4)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ano", DbType.String, objects, 5)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cor", DbType.String, objects, 6)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Placa", DbType.String, objects, 7)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Renavam", DbType.String, objects, 8)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Seguro", DbType.String, objects, 9)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativo", DbType.String, objects, 10)));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaVeiculo>();

                        return d1;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaVeiculo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("EmpresaVeiculos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaVeiculoID", entity.EmpresaVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Tipo", entity.Tipo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cor", entity.Cor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Placa", entity.Placa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Renavam", entity.Renavam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Seguro", entity.Seguro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID", entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FC", entity.Fc, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FormatoCredencialID", entity.FormatoCredencialId, false)));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(EmpresaVeiculo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("EmpresaVeiculos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaVeiculoId", entity.EmpresaVeiculoId).Igual()));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                    }
                }
            }
        }

        #endregion
    }
}