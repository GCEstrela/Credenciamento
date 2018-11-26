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
    public class VeiculoCredencialRepositorio : IVeiculoCredencialRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculoCredencialRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText ("VeiculosCredenciais", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoCredencialID", entity.VeiculoCredencialId, true)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoEmpresaID", entity.VeiculoEmpresaId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("TecnologiaCredencialID", entity.TecnologiaCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("TipoCredencialID", entity.TipoCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("LayoutCrachaID", entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("FormatoCredencialID", entity.FormatoCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("FC", entity.Fc, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Emissao", DbType.Date, entity.Emissao, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Validade", DbType.Date, entity.Validade, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialStatusID", entity.CredencialStatusId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CardHolderGUID", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialGUID", entity.CredencialGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoPrivilegio1ID", entity.VeiculoPrivilegio1Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoPrivilegio2ID", entity.VeiculoPrivilegio2Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Ativa", entity.Ativa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Colete", entity.Colete, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialmotivoID", entity.CredencialmotivoId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Baixa", DbType.Date, entity.Baixa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Impressa", entity.Impressa, false)));

                        var key = Convert.ToInt32 (cmd.ExecuteScalar());

                        entity.VeiculoCredencialId = key;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
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
        public VeiculoCredencial BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciais", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoCredencial>();

                        return d1.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Listar VeiculoCredencial
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoCredencial> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciais", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("NumeroCredencial", objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoCredencial>();

                        return d1;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Alterar registro VeiculoCredencial
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText ("VeiculosCredenciais", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoCredencialID", entity.VeiculoCredencialId, true)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoEmpresaID", entity.VeiculoEmpresaId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("TecnologiaCredencialID", entity.TecnologiaCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("TipoCredencialID", entity.TipoCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("LayoutCrachaID", entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("FormatoCredencialID", entity.FormatoCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("FC", entity.Fc, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Emissao", entity.Emissao, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Validade", entity.Validade, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialStatusID", entity.CredencialStatusId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CardHolderGUID", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialGUID", entity.CredencialGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoPrivilegio1ID", entity.VeiculoPrivilegio1Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoPrivilegio2ID", entity.VeiculoPrivilegio2Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Ativa", entity.Ativa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Colete", entity.Colete, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialmotivoID", entity.CredencialmotivoId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Baixa", entity.Baixa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Impressa", entity.Impressa, false)));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Deletar registro VeiculoCredencial
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(VeiculoCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText ("VeiculosCredenciais", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamDelete ("VeiculoCredencialId", entity.VeiculoCredencialId).Igual()));
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                    }
                }
            }
        }

        #endregion
    }
}