﻿// ***********************************************************************
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
            try
            {
                _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  Metodos

        /// <summary>
        ///     Obtem um único numero de contrato vinculado ao colaborador
        ///     <para>Não é permitido vincular mais de uma número de codntrato igual para o colaborador.</para>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <returns></returns>
        private EmpresaContratoCredencialView ObterNumeroContrato(ColaboradorCredencial entity, int colaboradorId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresaContratoCredencialColaboradorView", conn))
                    {
                        try
                        {
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaID", entity.ColaboradorEmpresaId).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", colaboradorId).Igual()));

                            var reader = cmd.ExecuteReaderSelect();
                            var d1 = reader.MapToList<EmpresaContratoCredencialView>();
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
        ///     Obtém a menor data de entre um curso do tipo controlado e uma data de validade do contrato
        /// </summary>
        /// <param name="colaboradorId">Identificador</param>
        /// <param name="numContrato">Número do contrato</param>
        /// <returns></returns>
        private DateTime? ObterMenorData(int colaboradorId, string numContrato)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.CreateCommand("Select dbo.fnc_Colaborador_Obter_Menor_Data (@colaboradorId,@NumContrato)", conn))
                    {
                        try
                        {
                            var param1 = _dataBase.CreateParameter("@colaboradorId", DbType.Int32, ParameterDirection.Input, colaboradorId);
                            var param2 = _dataBase.CreateParameter("@numContrato", DbType.String, ParameterDirection.Input, numContrato);
                            cmd.Parameters.Add(param1);
                            cmd.Parameters.Add(param2);

                            var returns = cmd.ExecuteScalar();
                            var dt = returns == null ? (DateTime?)null : Convert.ToDateTime(returns);

                            return dt;
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
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um curso controlado e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(ColaboradorCredencial entity, int colaboradorId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            return ObterDataValidadeCredencial(entity.TipoCredencialId, colaboradorId, numContrato, credencialRepositorio);
        }

        /// <summary>
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um curso controlado e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="tipoCredencialId"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(int tipoCredencialId, int colaboradorId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            try
            {
                if (credencialRepositorio == null) throw new ArgumentNullException(nameof(credencialRepositorio));

                //Verificar se o contrato é temporário ou permanente
                var tipoCredencial = credencialRepositorio.BuscarPelaChave(tipoCredencialId);
                if (tipoCredencial == null) throw new InvalidOperationException("Um tipo de credencial é necessário.");
                if (tipoCredencial.CredPermanente) //Sendo uma credencial do tipo permanente, então vale a regra da menor data
                    return ObterMenorData(colaboradorId, numContrato);
                //Caso contrario, trata-se de uma credencial temporária, então vale o prazo de 90 dias em relação a data atual
                return DateTime.Today.AddDays(90);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Criar registro credencial e obter data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="colaboradorId">Identificador</param>
        /// <param name="credencialRepositorio"></param>
        public void Criar(ColaboradorCredencial entity, int colaboradorId, ITipoCredencialRepositorio credencialRepositorio)
        {
            try
            {
                Criar(entity);
                //Setar a data de validade, caso Status Ativo
                if (!entity.Ativa) return;
                //Obter contrato
                var contrato = ObterNumeroContrato(entity, colaboradorId);
                var numContrato = contrato.NumeroContrato;
                //Obter uma data de validade (menor data entre um curso do tipo controlado e uma data de vencimento de um determinado contrato
                var dataCredencial = ObterDataValidadeCredencial(entity, colaboradorId, numContrato, credencialRepositorio);
                //Setando a data de vencimento uma credencial
                entity.Validade = dataCredencial;
                Alterar(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Listar Veículos e seus contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresaView> ListarContratos(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradorEmpresaView", conn))
                    {
                        try
                        {
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, o, 0).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroContrato", DbType.String, o, 1).Igual()));

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorCredencial entity)
        {
            try
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
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroCredencial", entity.NumeroCredencial.RetirarCaracteresEspeciais(), false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FC", DbType.Int32, entity.Fc, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao", DbType.DateTime, entity.Emissao, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", DbType.DateTime, entity.Validade, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialStatusID", DbType.Int32, entity.CredencialStatusId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CardHolderGUID", DbType.String, entity.CardHolderGuid, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialGUID", DbType.String, entity.CredencialGuid, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio1ID", DbType.Int32, entity.ColaboradorPrivilegio1Id, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorPrivilegio2ID", DbType.Int32, entity.ColaboradorPrivilegio2Id, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativa", DbType.Boolean, entity.Ativa, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Colete", DbType.String, entity.Colete, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialmotivoID", DbType.Int32, entity.CredencialMotivoId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Baixa", DbType.DateTime, entity.Baixa, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impressa", entity.Impressa, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataStatus", DbType.DateTime, entity.DataStatus, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Policiafederal", DbType.Boolean, entity.Policiafederal, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Receitafederal", DbType.Boolean, entity.Receitafederal, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Segurancatrabalho", DbType.Boolean, entity.Segurancatrabalho, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Obs", DbType.String, entity.Obs, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialVia", DbType.Int32, entity.CredencialVia, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialmotivoViaAdicionalID", DbType.Int32, entity.CredencialmotivoViaAdicionalID, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CredencialmotivoIDanterior", DbType.Int32, entity.CredencialmotivoIDanterior, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Identificacao1", DbType.String, entity.Identificacao1, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Identificacao2", DbType.String, entity.Identificacao2, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Usuario", DbType.String, entity.Usuario, false)));

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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorCredencial BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("colaboradorescredenciaisview", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, id).Igual()));
                            //cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorId", DbType.Int32, id).Igual()));
                            var reader = cmd.ExecuteReader();
                            var d1 = reader.MapToList<ColaboradorCredencial>();
                            //
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
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorCredencial entity)
        {
            try
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
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NumeroCredencial", entity.NumeroCredencial.RetirarCaracteresEspeciais(), false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FC", DbType.Int32, entity.Fc, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Emissao", DbType.DateTime, entity.Emissao, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialStatusID", DbType.Int32, entity.CredencialStatusId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CardHolderGUID", DbType.String, entity.CardHolderGuid, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialGUID", DbType.String, entity.CredencialGuid, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorPrivilegio1ID", DbType.Int32, entity.ColaboradorPrivilegio1Id, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorPrivilegio2ID", DbType.Int32, entity.ColaboradorPrivilegio2Id, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativa", DbType.Boolean, entity.Ativa, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Colete", DbType.String, entity.Colete, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialmotivoID", DbType.Int32, entity.CredencialMotivoId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Baixa", DbType.DateTime, entity.Baixa, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Impressa", DbType.Boolean, false, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", DbType.DateTime, entity.Validade, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataStatus", DbType.DateTime, DateTime.Today.Date, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Policiafederal", entity.Policiafederal, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Receitafederal", entity.Receitafederal, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Segurancatrabalho", entity.Segurancatrabalho, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Identificacao1", DbType.String, entity.Identificacao1, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Identificacao2", DbType.String, entity.Identificacao2, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Obs", DbType.String, entity.Obs, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialVia", DbType.Int32, entity.CredencialVia, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialmotivoViaAdicionalID", DbType.Int32, entity.CredencialmotivoViaAdicionalID, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CredencialmotivoIDanterior", DbType.Int32, entity.CredencialmotivoIDanterior, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Usuario", DbType.String, entity.Usuario, false)));

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="colaboradorId"></param>
        /// <param name="credencialRepositorio"></param>
        public void Alterar(ColaboradorCredencial entity, int colaboradorId, ITipoCredencialRepositorio credencialRepositorio)
        {
            try
            {
                Alterar(entity);
                //Setar a data de validade, caso Status Ativo
                if (!entity.Ativa) return;
                //Obter contrato
                var contrato = ObterNumeroContrato(entity, colaboradorId);
                var numContrato = contrato.NumeroContrato;
                //Obter uma data de validade (menor data entre um curso do tipo controlado e uma data de vencimento de um determinado contrato
                var dataCredencial = ObterDataValidadeCredencial(entity, colaboradorId, numContrato, credencialRepositorio);
                //Setando a data de vencimento uma credencial
                entity.Validade = dataCredencial;
                Alterar(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorCredencial> Listar(params object[] o)
        {
            try
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
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 4).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, o, 5).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativa", DbType.Boolean, o, 6).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroContrato", DbType.String, o, 7).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, o, 8).Diferente()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Colete", DbType.String, o, 9).Like()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaID", DbType.Int32, o, 10).Igual()));

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorCredencial entity)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public ColaboradorCredencial ObterCredencialPeloNumeroCredencial(string numCredencial)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresCredenciais", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("NumeroCredencial", numCredencial).Igual()));
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o)
        {
            try
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
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaID", DbType.Int32, o, 5).Igual()));

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Obter credencial
        /// </summary>
        /// <param name="colaboradorCredencialId"></param>
        /// <returns></returns>
        public CredencialView ObterCredencialView(int colaboradorCredencialId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("CredencialView", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, colaboradorCredencialId).Igual()));
                            var reader = cmd.ExecuteReaderSelect();
                            var d1 = reader.MapToList<CredencialView>();
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
        ///     Obter dados da credencial
        /// </summary>
        /// <param name="colaboradorCredencialId">Identificador</param>
        /// <returns></returns>
        public ColaboradoresCredenciaisView BuscarCredencialPelaChave(int colaboradorCredencialId)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ColaboradorCredencial ObterNumeroColete(int colaboradorid, string numColete)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresCredenciaisView", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", colaboradorid).Diferente()));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("Colete", numColete).Igual()));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("Ativa", true).Igual()));

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
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        ///    Listar Colaboradores credenciais - concedidas
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialConcedidasView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.ColaboradorCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, entity.ColaboradorCredencialId).Igual()));
                            }
                            if (entity != null && entity.TipoCredencialId > 0)
                            {
                                //entity.Matricula = null;
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialId", DbType.Int32, entity.TipoCredencialId).Igual()));
                            }
                            if (entity != null && entity.CredencialStatusId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusId", DbType.Int32, entity.CredencialStatusId).Igual()));
                            }
                            if (entity != null && entity.EmpresaId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaId", DbType.Int32, entity.EmpresaId).Igual()));
                            }

                            if (entity.emissaoValidade == 0 || entity.emissaoValidade == 1)
                            {
                                //Busca faixa de data
                                if (entity.Emissao != null || entity.EmissaoFim != null)
                                {
                                    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Emissao", DbType.DateTime, entity.Emissao).MaiorIgual()));
                                    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmissaoFim", DbType.DateTime, entity.EmissaoFim).MenorIgual()));
                                }
                            }
                            else if (entity.emissaoValidade == 2)
                            {
                                if (entity.Validade != null || entity.ValidadeFim != null)
                                {
                                    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Validade", DbType.DateTime, entity.Validade).MaiorIgual()));
                                    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ValidadeFim", DbType.DateTime, entity.ValidadeFim).MenorIgual()));
                                }
                            }


                            if (entity.Impressa != null && entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - vias adicionais
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialViaAdicionaisView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.ColaboradorCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, entity.ColaboradorCredencialId).Igual()));
                            }
                            if (entity != null && entity.TipoCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialId", DbType.Int32, entity.TipoCredencialId).Igual()));
                            }
                            if (entity != null && entity.CredencialStatusId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusId", DbType.Int32, entity.CredencialStatusId).Igual()));
                            }

                            //Busca por faixa de data
                            if (entity.Emissao != null || entity.EmissaoFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Emissao", DbType.DateTime, entity.Emissao).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmissaoFim", DbType.DateTime, entity.EmissaoFim).MenorIgual()));
                            }

                            if (entity.Impressa != null && entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - invalidas 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialInvalidasView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.ColaboradorCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, entity.ColaboradorCredencialId).Igual()));
                            }
                            if (entity != null && entity.TipoCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialId", DbType.Int32, entity.TipoCredencialId).Igual()));
                            }
                            if (entity != null && entity.CredencialStatusId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusId", DbType.Int32, entity.CredencialStatusId).Igual()));
                            }
                            if (entity.Impeditivo)
                            {
                                //cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialMotivoId", DbType.Int32, entity.CredencialMotivoId).Igual()));
                                //cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialMotivoId1", DbType.Int32, entity.CredencialMotivoId1).Igual()));
                                //cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialMotivoId2", DbType.Int32, entity.CredencialMotivoId2).Igual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo).Diferente()));
                            }
                            else if (entity != null && entity.CredencialMotivoId > 0 && !entity.Impeditivo)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialMotivoId", DbType.Int32, entity.CredencialMotivoId).Igual()));
                            }
                            if (entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }
                            //Busca faixa de data
                            if (entity.Baixa != null || entity.BaixaFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Baixa", DbType.DateTime, entity.Baixa).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("BaixaFim", DbType.DateTime, entity.BaixaFim).MenorIgual()));
                            }

                            //Busca faixa de data
                            if (entity.DataStatus != null || entity.DataStatusFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataStatus", DbType.DateTime, entity.DataStatus).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataStatusFim", DbType.DateTime, entity.DataStatusFim).MenorIgual()));
                            }

                            //if (entity.CredencialMotivoId == 15 && entity.Baixa == null)
                            //{
                            //    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Baixa", DbType.DateTime, entity.Baixa).IsNull()));
                            //}

                            if (entity.TipoRel > 0)
                            {
                                if (!entity.Impeditivo && !entity.flaTodasDevolucaoEntregaBO)
                                {
                                    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo).Igual()));
                                }
                            }
                            else
                            {
                                if (!entity.Impeditivo && entity.flaDevolucaoEntregaBO != null)
                                {
                                    cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo).Igual()));
                                }
                            }


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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - impressoes 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialImpressoesView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialImpressaoView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.EmpresaId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaId", DbType.Int32, entity.EmpresaId).Igual()));
                            }

                            if (entity.DataImpressao != null || entity.DataImpressaoFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataImpressao", DbType.DateTime, entity.DataImpressao).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataImpressaoFim", DbType.DateTime, entity.DataImpressaoFim).MenorIgual()));
                            }
                            if (entity.Impressa != null && entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - permanentes ativos por área
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialPermanentePorAreaView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialPorAreaAcessoView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.AreaAcessoId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("AreaAcessoID", DbType.Int32, entity.AreaAcessoId).Igual()));
                            }
                            if (entity != null && entity.CredencialStatusId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusId", DbType.Int32, entity.CredencialStatusId).Igual()));
                            }
                            if (entity.Impressa != null && entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }
                            if (entity != null && entity.TipoCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialId", DbType.Int32, entity.TipoCredencialId).Igual()));
                            }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialDestruidasView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.ColaboradorCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, entity.ColaboradorCredencialId).Igual()));
                            }
                            if (entity != null && entity.TipoCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialId", DbType.Int32, entity.TipoCredencialId).Igual()));
                            }
                            if (entity != null && entity.CredencialStatusId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusId", DbType.Int32, entity.CredencialStatusId).Igual()));
                            }
                            if (entity.Impeditivo)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo).Diferente()));
                            }
                            else if (entity != null && entity.CredencialMotivoId > 0 && !entity.Impeditivo)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialMotivoId", DbType.Int32, entity.CredencialMotivoId).Igual()));
                            }
                            if (entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }
                            //Busca faixa de data
                            if (entity.Baixa != null || entity.BaixaFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Baixa", DbType.DateTime, entity.Baixa).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("BaixaFim", DbType.DateTime, entity.BaixaFim).MenorIgual()));
                            }

                            //Busca faixa de data
                            if (entity.DataStatus != null || entity.DataStatusFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataStatus", DbType.DateTime, entity.DataStatus).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataStatusFim", DbType.DateTime, entity.DataStatusFim).MenorIgual()));
                            }
                            if (entity.DevolucaoEntregaBo)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo).Igual()));
                            }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///    Listar Colaboradores credenciais - extraviadas 
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public List<ColaboradoresCredenciaisView> ListarColaboradorCredencialExtraviadasView(FiltroReportColaboradoresCredenciais entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("RelatorioColaboradorCredencialView", conn))
                    {
                        try
                        {
                            if (entity != null && entity.ColaboradorCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, entity.ColaboradorCredencialId).Igual()));
                            }
                            if (entity != null && entity.TipoCredencialId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialId", DbType.Int32, entity.TipoCredencialId).Igual()));
                            }
                            if (entity != null && entity.CredencialStatusId > 0)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusId", DbType.Int32, entity.CredencialStatusId).Igual()));
                            }
                            if (entity.Impressa != null && entity.Impressa)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Impressa", DbType.Boolean, entity.Impressa).Igual()));
                            }
                            //Busca faixa de data
                            if (entity.DataStatus != null || entity.DataStatusFim != null)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataStatus", DbType.DateTime, entity.DataStatus).MaiorIgual()));
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DataStatusFim", DbType.DateTime, entity.DataStatusFim).MenorIgual()));
                            }

                            if (!entity.Impeditivo && !entity.flaTodasDevolucaoEntregaBO)
                            {
                                cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("DevolucaoEntregaBO", DbType.Boolean, entity.DevolucaoEntregaBo).Igual()));
                            }

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Obter credencial
        /// </summary>
        /// <param name="colaboradorCredencialId"></param>
        /// <returns></returns>
        public CredencialView ObterCursosColaboradorView(int colaboradorCredencialId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresCursosView", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, colaboradorCredencialId).Igual()));
                            var reader = cmd.ExecuteReaderSelect();
                            var d1 = reader.MapToList<CredencialView>();
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
        #endregion
    }
}