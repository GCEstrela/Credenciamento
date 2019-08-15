// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
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
    public class PendenciaRepositorio : IPendenciaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public PendenciaRepositorio()
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
        public void Criar(Pendencia entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("Pendencias", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("PendenciaID", entity.PendenciaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CodPendencia", entity.CodPendencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataLimite", entity.DataLimite, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Impeditivo", entity.Impeditivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorID", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("PendenciaSistema", entity.PendenciaSistema, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", true, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.PendenciaId = key;

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
        public Pendencia BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Pendencias", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("PendenciaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Pendencia>();

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
        public ICollection<Pendencia> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Pendencias", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, objects, 0).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataLimite", DbType.Date, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impeditivo", DbType.Boolean, objects, 2).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Pendencia>();

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
        public void Alterar(Pendencia entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("Pendencias", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PendenciaID", entity.PendenciaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CodPendencia", entity.CodPendencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataLimite", entity.DataLimite, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impeditivo", entity.Impeditivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorID", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PendenciaSistema", entity.PendenciaSistema, false)));
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
        /// <param name="entity"></param>
        public void Remover(Pendencia entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("Pendencias", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("PendenciaId", entity.PendenciaId).Igual()));

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
        ///     Desativar Pendência
        /// </summary>
        /// <param name="entity"></param>
        public void Desativar(Pendencia entity)
        {
            try
            {
                entity.Ativo = false;
                Alterar(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Listar Pendencia por Empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorEmpresa(int empresaId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("PendenciasView", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, empresaId).Igual()));
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Pendencia>();

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
        ///     Listar Pendencia por Colaborador
        /// </summary>
        /// <param name="colaboradorId"></param>
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorColaborador(int colaboradorId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("PendenciasView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, colaboradorId).Igual()));
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Pendencia>();

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
        ///     Listar Pendencia por Empresa
        /// </summary>
        /// <param name="veiculoId"></param>
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorVeiculo(int veiculoId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("Pendencias", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", DbType.Int32, veiculoId).Igual()));
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Pendencia>();

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
        ///     Criar pendência de sistema
        /// </summary>
        /// <param name="entity"></param>
        public void CriarPendenciaSistema(Pendencia entity)
        {
            try
            {
                entity.PendenciaSistema = true;
                if (string.IsNullOrWhiteSpace(entity.Descricao))
                    entity.Descricao = "Pendência de cadastro inicial";
                Criar(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Alterar pendência de sistema
        /// </summary>
        /// <param name="entity"></param>
        public void AlterarPendenciaSistema(Pendencia entity)
        {
            try
            {
                entity.PendenciaSistema = true;
                Alterar(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}