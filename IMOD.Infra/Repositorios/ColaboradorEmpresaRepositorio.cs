// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
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
    public class ColaboradorEmpresaRepositorio : IColaboradorEmpresaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorEmpresaRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("ColaboradoresEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorEmpresaId", entity.ColaboradorEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CardHolderGuid", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaId", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaContratoId", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeAnexo", entity.NomeAnexo, false))); 
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Anexo", entity.Anexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ManuseioBagagem", entity.ManuseioBagagem, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FlagCcam", entity.FlagCcam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("OperadorPonteEmbarque", entity.OperadorPonteEmbarque, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Motorista", entity.Motorista, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FlagAuditoria", entity.Auditado, false)));

                        cmd.ExecuteNonQuery();

                        ////Gerar matricula
                        //if (entity.Matricula == null)
                        //    CriarNumeroMatricula(entity);
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
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorEmpresa BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresEmpresas", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("ColaboradoresEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorEmpresaId", entity.ColaboradorEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CardHolderGuid", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaId", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaContratoId", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeAnexo", entity.NomeAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Anexo", entity.Anexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ManuseioBagagem", entity.ManuseioBagagem, false))); 
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FlagCcam", entity.FlagCcam, false))); 
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("OperadorPonteEmbarque", entity.OperadorPonteEmbarque, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Motorista", entity.Motorista, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FlagAuditoria", entity.Auditado, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorEmpresaId = key;
                        //Gerar matricula
                        CriarNumeroMatricula (entity);
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
        /// Criar numero de matricula
        /// </summary>
        /// <param name="entity"></param>
        public void CriarNumeroMatricula(ColaboradorEmpresa entity)
        {
            
            var data = DateTime.Now.ToString("yy");
            entity.Matricula = $"{entity.ColaboradorEmpresaId}-{data}";//entity
            Alterar (entity);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> Listar(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradorEmpresaView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativo", DbType.Boolean, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cargo", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Matricula", DbType.String, o, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 4).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, o, 5).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaContratoId", DbType.Int32, o, 6).Igual()));
                        

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("ColaboradoresEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorEmpresaId", entity.ColaboradorEmpresaId).Igual()));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                    }
                }
            }
        }

        /// <summary>
        ///     Listar View
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> ListarView(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradorEmpresaView", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativo", DbType.Boolean, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cargo", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Matricula", DbType.String, o, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        #endregion
    }
}