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
    public class VeiculosRepositorio : IVeiculosRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculosRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro de Veículo
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Veiculo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("Veiculos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EquipamentoVeiculoID", entity.EquipamentoVeiculoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Placa_Identificador", entity.PlacaIdentificador, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Frota", entity.Frota, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Patrimonio", entity.Patrimonio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Tipo", entity.Tipo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cor", entity.Cor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Serie_Chassi", entity.SerieChassi, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CombustivelID", entity.CombustivelId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Altura", entity.Altura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Comprimento", entity.Comprimento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Largura", entity.Largura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoEquipamentoVeiculoID", entity.TipoEquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Renavam", entity.Renavam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Foto", entity.Foto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Excluida", entity.Excluida, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DescricaoAnexo", entity.DescricaoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivoAnexo", entity.NomeArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ArquivoAnexo", entity.ArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente31", entity.Pendente31, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente32", entity.Pendente32, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente33", entity.Pendente33, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente34", entity.Pendente34, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EquipamentoVeiculoId = key;
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
        ///     Buscar Veículo pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Veiculo BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Veiculos", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoID", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Veiculo>();

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
        ///     Listar Veículos
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Veiculo> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Veiculos", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, objects, 0).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Modelo", DbType.String, objects, 1).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Veiculo>();

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
        ///     Alterar registro de Veículo
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Veiculo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("Veiculos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EquipamentoVeiculoID", entity.EquipamentoVeiculoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Placa_Identificador", entity.PlacaIdentificador, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Frota", entity.Frota, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Patrimonio", entity.Patrimonio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Tipo", entity.Tipo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cor", entity.Cor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Serie_Chassi", entity.SerieChassi, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CombustivelID", entity.CombustivelId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Altura", entity.Altura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Comprimento", entity.Comprimento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Largura", entity.Largura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoEquipamentoVeiculoID", entity.TipoEquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Renavam", entity.Renavam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Foto", entity.Foto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Excluida", entity.Excluida, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DescricaoAnexo", entity.DescricaoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivoAnexo", entity.NomeArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ArquivoAnexo", entity.ArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente31", entity.Pendente31, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente32", entity.Pendente32, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente33", entity.Pendente33, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente34", entity.Pendente34, false)));

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
        ///     Deletar registro de Veículo
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(Veiculo entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("Veiculos", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EquipamentoVeiculoID", entity.EquipamentoVeiculoId).Igual()));
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