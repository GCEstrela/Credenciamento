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
    public class VeiculoWebRepositorio : IVeiculoWebRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculoWebRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro de Veículo
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoWeb entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("VeiculosWeb", conn))
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EquipamentoVeiculoWebID", entity.EquipamentoVeiculoWebId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EquipamentoVeiculoID", entity.EquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("PlacaIdentificador", entity.PlacaIdentificador, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Frota", entity.Frota, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Patrimonio", entity.Patrimonio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Tipo", entity.Tipo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cor", entity.Cor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("SerieChassi", entity.SerieChassi, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CombustivelID", entity.CombustivelId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Altura", entity.Altura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Comprimento", entity.Comprimento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Largura", entity.Largura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoEquipamentoVeiculoID", entity.TipoEquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Renavam", entity.Renavam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Foto", entity.Foto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", true, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DescricaoAnexo", entity.DescricaoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivoAnexo", entity.NomeArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ArquivoAnexo", entity.ArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente31", entity.Pendente31, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente32", entity.Pendente32, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente33", entity.Pendente33, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente34", entity.Pendente34, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Precadastro", entity.Precadastro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Categoria", entity.Categoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataLicenciameno", DbType.Date, entity.DataLicenciameno, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataVistoria", DbType.Date, entity.DataVistoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("StatusCadastro", entity.StatusCadastro, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EquipamentoVeiculoWebId = key;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Buscar Veículo pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoWeb BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosWeb", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoID", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoWeb>();

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
        ///     Listar Veículos
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoWeb> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosWeb", conn))
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Modelo", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("PlacaIdentificador", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("SerieChassi", DbType.String, objects, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Tipo", DbType.String, objects, 4).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, objects, 5).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Precadastro", DbType.Boolean, objects, 6).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EquipamentoVeiculoWebID", DbType.Int32, objects, 7).Igual()));

                       
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoWeb>();

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
        ///     Alterar registro de Veículo
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoWeb entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("VeiculosWeb", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EquipamentoVeiculoWebID", entity.EquipamentoVeiculoWebId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EquipamentoVeiculoID", entity.EquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PlacaIdentificador", entity.PlacaIdentificador, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Frota", entity.Frota, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Patrimonio", entity.Patrimonio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Marca", entity.Marca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Tipo", entity.Tipo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cor", entity.Cor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ano", entity.Ano, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("SerieChassi", entity.SerieChassi, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CombustivelID", entity.CombustivelId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Altura", entity.Altura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Comprimento", entity.Comprimento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Largura", entity.Largura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoEquipamentoVeiculoID", entity.TipoEquipamentoVeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Renavam", entity.Renavam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Foto", entity.Foto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DescricaoAnexo", entity.DescricaoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivoAnexo", entity.NomeArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ArquivoAnexo", entity.ArquivoAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente31", entity.Pendente31, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente32", entity.Pendente32, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente33", entity.Pendente33, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente34", entity.Pendente34, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Precadastro", entity.Precadastro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Categoria", entity.Categoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataLicenciameno", entity.DataLicenciameno, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataVistoria", entity.DataVistoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Observacao", entity.Observacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("StatusCadastro", entity.StatusCadastro, false)));

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
        /// <param name="entity">Entidade</param>   
        public void Remover(VeiculoWeb entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                var tran = conn.BeginTransaction();

                try
                {
                    using (var cmd = _dataBase.DeleteText("VeiculosAnexosWeb", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoId", DbType.Int32, entity.EquipamentoVeiculoId).Igual()));
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = _dataBase.DeleteText("VeiculosSegurosWeb", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoId", DbType.Int32, entity.EquipamentoVeiculoId).Igual()));
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = _dataBase.DeleteText("VeiculosEmpresasWeb", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoId", DbType.Int32, entity.EquipamentoVeiculoId).Igual()));
                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = _dataBase.DeleteText("VeiculosObservacoes", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoId", DbType.Int32, entity.EquipamentoVeiculoId).Igual()));
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = _dataBase.DeleteText("VeiculosWeb", conn))
                    {
                        cmd.Transaction = tran;
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EquipamentoVeiculoId", DbType.Int32, entity.EquipamentoVeiculoId).Igual()));
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        tran.Rollback();
                        conn.Close();
                    }
                    Utils.TraceException(ex);
                }

            }
        }

        #endregion
    }
}