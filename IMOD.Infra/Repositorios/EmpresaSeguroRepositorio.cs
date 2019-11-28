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
    public class EmpresaSeguroRepositorio : IEmpresaSeguroRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaSeguroRepositorio()
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
        public void Criar(EmpresaSeguro entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EmpresasSeguros", conn))
                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaSeguroID", entity.EmpresaSeguroId, true)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeSeguradora", entity.NomeSeguradora, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NumeroApolice", entity.NumeroApolice, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ValorCobertura", entity.ValorCobertura, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Arquivo", entity.Arquivo, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivo", entity.NomeArquivo, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Emissao", entity.Emissao, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", entity.Validade, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaContratoId", entity.EmpresaContratoId, false)));
                            //
                            var key = Convert.ToInt32(cmd.ExecuteScalar());

                            entity.EmpresaSeguroId = key;
                        }
                        catch (Exception ex)
                        {
                            Utils.TraceException(ex);
                            throw;
                        }
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
        public EmpresaSeguro BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasSeguros", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaSeguroId", DbType.Int32, id).Igual()));

                            var reader = cmd.ExecuteReader();
                            var d1 = reader.MapToList<EmpresaSeguro>();

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
        public ICollection<EmpresaSeguro> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasSeguros", conn))

                    {
                        try
                        {
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NomeSeguradora", DbType.String, objects, 0).Like()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroApolice", DbType.String, objects, 1).Like()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Emissao", DbType.Date, objects, 2).Like()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Validade", DbType.Date, objects, 3).Like()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, objects, 4).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaContratoId", DbType.Int32, objects, 5).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaSeguroID", DbType.Int32, objects, 6).Igual()));

                            var reader = cmd.ExecuteReaderSelect();
                            var d1 = reader.MapToList<EmpresaSeguro>();

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaSeguro entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EmpresasSeguros", conn))
                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaSeguroID", entity.EmpresaSeguroId, true)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeSeguradora", entity.NomeSeguradora, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroApolice", entity.NumeroApolice, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ValorCobertura", entity.ValorCobertura, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Arquivo", entity.Arquivo, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao", entity.Emissao, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", entity.Validade, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaContratoId", entity.EmpresaContratoId, false)));
                            
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(EmpresaSeguro entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasSeguros", conn))
                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaSeguroId", entity.EmpresaSeguroId).Igual()));

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Utils.TraceException(ex);
                        }
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