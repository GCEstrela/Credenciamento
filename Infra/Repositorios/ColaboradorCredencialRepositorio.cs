// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
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
    public class ColaboradorCredencialRepositorio : IColaboradorCredencialRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorCredencialRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("ColaboradoresCredenciais", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorCredencialID", entity.ColaboradorCredencialId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorEmpresaID", DbType.Int32, entity.ColaboradorEmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TecnologiaCredencialID", DbType.Int32, entity.TecnologiaCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoCredencialID", DbType.Int32, entity.TipoCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID", DbType.Int32, entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FormatoCredencialID", DbType.Int32, entity.FormatoCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FC", DbType.Int32, entity.Fc, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao", DbType.DateTime, entity.Emissao, false)));
                        //cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", DbType.DateTime, entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialStatusID", DbType.Int32, entity.CredencialStatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CardHolderGUID", DbType.String, entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialGUID", DbType.String, entity.CredencialGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio1ID", DbType.Int32, entity.ColaboradorPrivilegio1Id, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio2ID", DbType.Int32, entity.ColaboradorPrivilegio2Id, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativa", entity.Ativa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Colete", DbType.String, entity.Colete, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialmotivoID", DbType.Int32, entity.CredencialMotivoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Baixa", DbType.DateTime, entity.Baixa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impressa", entity.Impressa, false)));

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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorCredencial BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresCredenciais", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorCredencial>();

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
        public void Criar(ColaboradorCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("ColaboradoresCredenciais", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorCredencialID", entity.ColaboradorCredencialId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorEmpresaID", DbType.Int32, entity.ColaboradorEmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TecnologiaCredencialID", DbType.Int32, entity.TecnologiaCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoCredencialID", DbType.Int32, entity.TipoCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutCrachaID", DbType.Int32, entity.LayoutCrachaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FormatoCredencialID", DbType.Int32, entity.FormatoCredencialId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NumeroCredencial", entity.NumeroCredencial, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FC", DbType.Int32, entity.Fc, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Emissao", DbType.DateTime, entity.Emissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialStatusID", DbType.Int32, entity.CredencialStatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CardHolderGUID", DbType.String, entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialGUID", DbType.String, entity.CredencialGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorPrivilegio1ID", DbType.Int32, entity.ColaboradorPrivilegio1Id, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorPrivilegio2ID", DbType.Int32, entity.ColaboradorPrivilegio2Id, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativa", entity.Ativa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Colete", DbType.String, entity.Colete, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialmotivoID", DbType.Int32, entity.CredencialMotivoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Baixa", DbType.DateTime, entity.Baixa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Impressa", false, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorCredencialId = key;
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
        /// <returns></returns>
        public ICollection<ColaboradorCredencial> Listar(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresCredenciaisView", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusID", DbType.Int32, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("FormatoCredencialID", DbType.Int32, o, 2).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 3).Like()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorCredencial>();

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
        public void Remover(ColaboradorCredencial entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("ColaboradoresCredenciais", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorCredencialID", entity.ColaboradorCredencialId).Igual()));

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
        ///    Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresCredenciaisView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialID", DbType.String, o, 2).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusID", DbType.String, o, 3).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 4).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradoresCredenciaisView>();
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
        ///    Listar dados de Credencial (Impressão)
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<CredencialView> ListarCredencialView(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("CredencialView", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<CredencialView>();
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
        /// Obter dados da credencial
        /// </summary>
        /// <param name="colaboradorCredencialId">Identificador</param>
        /// <returns></returns>
        public ColaboradoresCredenciaisView BuscarCredencialPelaChave(int colaboradorCredencialId)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresCredenciaisView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, colaboradorCredencialId, 0).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradoresCredenciaisView>();
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
        ///    Listar Veículos e seus contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresaView> ListarContratoView(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradorEmpresaView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroContrato", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, o, 3).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresaView>();
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
