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
        ///     Obtém a menor data de entre um curso do tipo controlado e uma data de validade do contrato
        /// </summary>
        /// <param name="equipamentoVeiculoId">Identificador</param>
        /// <param name="numContrato">Número do contrato</param>
        /// <returns></returns>
        private DateTime? ObterMenorData(int equipamentoVeiculoId, string numContrato)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.CreateCommand ("Select dbo.fnc_Veiculo_Obter_Menor_Data (@equipamentoVeiculoId,@NumContrato)", conn))
                {
                    try
                    {
                        var param1 = _dataBase.CreateParameter ("@equipamentoVeiculoId", DbType.Int32, ParameterDirection.Input, equipamentoVeiculoId);
                        var param2 = _dataBase.CreateParameter ("@numContrato", DbType.String, ParameterDirection.Input, numContrato);
                        cmd.Parameters.Add (param1);
                        cmd.Parameters.Add (param2);

                        var returns = cmd.ExecuteScalar();
                        var dt = returns == null ? (DateTime?) null : Convert.ToDateTime (returns);

                        return dt;
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
        ///     Obtem um único numero de contrato vinculado ao colaborador
        ///     <para>Não é permitido vincular mais de uma número de codntrato igual para o colaborador.</para>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="equiapmentoVeiculoId"></param>
        /// <returns></returns>
        private EmpresaContratoCredencialView ObterNumeroContrato(VeiculoCredencial entity, int equiapmentoVeiculoId)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("EmpresaContratoCredencialVeiculoView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoEmpresaID", entity.VeiculoEmpresaId).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoID", equiapmentoVeiculoId).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaContratoCredencialView>();
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
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Emissao", DbType.DateTime, entity.Emissao, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Validade", DbType.DateTime, entity.Validade, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialStatusID", entity.CredencialStatusId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CardHolderGUID", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialGUID", entity.CredencialGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoPrivilegio1ID", entity.VeiculoPrivilegio1Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoPrivilegio2ID", entity.VeiculoPrivilegio2Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Ativa", entity.Ativa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Colete", entity.Colete, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialmotivoID", entity.CredencialMotivoId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Baixa", DbType.DateTime, entity.Baixa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Impressa", entity.Impressa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataStatus", DbType.DateTime, entity.DataStatus, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DevolucaoEntregaBOID", DbType.Int32, entity.DevolucaoEntregaBoId, false)));

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
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoEmpresaID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialID", DbType.Int32, objects, 1).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("CredencialStatusID", DbType.Int32, objects, 2).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("FormatoCredencialID", DbType.Int32, objects, 3).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("NumeroCredencial", objects, 4).Like()));

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
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Validade", DbType.DateTime, entity.Validade, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialStatusID", entity.CredencialStatusId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CardHolderGUID", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialGUID", entity.CredencialGuid, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoPrivilegio1ID", entity.VeiculoPrivilegio1Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoPrivilegio2ID", entity.VeiculoPrivilegio2Id, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Ativa", DbType.Boolean, entity.Ativa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Colete", entity.Colete, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialmotivoID", entity.CredencialMotivoId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Baixa", DbType.DateTime, entity.Baixa, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Impressa", entity.Impressa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataStatus", DbType.DateTime, entity.DataStatus, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DevolucaoEntregaBOID", DbType.Int32, entity.DevolucaoEntregaBoId, false)));

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
        /// <param name="entity"></param>
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

        public ICollection<VeiculosCredenciaisView> ListarView(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciaisView", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialID", DbType.Int32, objects, 1).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("CredencialStatusID", DbType.Int32, objects, 2).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("FormatoCredencialID", DbType.Int32, objects, 3).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("NumeroCredencial", objects, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculosCredenciaisView>();

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

        public ICollection<AutorizacaoView> ListarAutorizacaoView(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("AutorizacaoView", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("Nome", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("PlacaIdentificador", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("Marca", DbType.String, objects, 3).Like()));
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("Cor", DbType.String, objects, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<AutorizacaoView>();

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
        ///     Obter credencial
        /// </summary>
        /// <param name="veiculoCredencialId"></param>
        /// <returns></returns>
        public AutorizacaoView ObterCredencialView(int veiculoCredencialId)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("AutorizacaoView", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialID", DbType.Int32, veiculoCredencialId).Igual()));
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<AutorizacaoView>();
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
        ///     Obtem a data de validade de uma credencial
        ///     <para>
        ///         Verificar se o contrato é temporário ou permanente,
        ///         sendo permanente, então vale obter a menor data entre
        ///         um seguro e uma data de validade do contrato, caso contrario, será concedido prazo de 90 dias a
        ///         partir da data atual
        ///     </para>
        /// </summary>
        /// <param name="tipoCredencialId"></param>
        /// <param name="equiapmentoVeiculoId"></param>
        /// <param name="numContrato"></param>
        /// <param name="credencialRepositorio"></param>
        /// <returns></returns>
        public DateTime? ObterDataValidadeCredencial(int tipoCredencialId, int equiapmentoVeiculoId, string numContrato, ITipoCredencialRepositorio credencialRepositorio)
        {
            if (credencialRepositorio == null) throw new ArgumentNullException (nameof (credencialRepositorio));

            //Verificar se o contrato é temporário ou permanente
            var tipoCredencial = credencialRepositorio.BuscarPelaChave (tipoCredencialId);
            if (tipoCredencial == null) throw new InvalidOperationException ("Um tipo de credencial é necessário.");
            if (tipoCredencial.CredPermanente) //Sendo uma credencial do tipo permanente, então vale a regra da menor data
                return ObterMenorData (equiapmentoVeiculoId, numContrato);
            //Caso contrario, trata-se de uma credencial temporária, então vale o prazo de 90 dias em relação a data atual
            return DateTime.Today.AddDays (90);
        }

        /// <summary>
        ///     Criar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <param name="equiapmentoVeiculoId">Identificador</param>
        /// <param name="credencialRepositorio"></param>
        public void Criar(VeiculoCredencial entity, int equiapmentoVeiculoId, ITipoCredencialRepositorio credencialRepositorio)
        {
            Criar (entity);
            //Setar a data de validade, caso Status Ativo
            if (!entity.Ativa) return;
            //Obter contrato
            var contrato = ObterNumeroContrato (entity, equiapmentoVeiculoId);
            var numContrato = contrato.NumeroContrato;
            //Obter uma data de validade (menor data entre um curso do tipo controlado e uma data de vencimento de um determinado contrato
            var dataCredencial = ObterDataValidadeCredencial (entity.TipoCredencialId, equiapmentoVeiculoId, numContrato, credencialRepositorio);
            //Setando a data de vencimento uma credencial
            entity.Validade = dataCredencial;
            Alterar (entity);
        }

        /// <summary>
        ///     Alterar registro credencial obtendo a data de validade da credencial
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="equiapmentoVeiculoId"></param>
        /// <param name="credencialRepositorio"></param>
        public void Alterar(VeiculoCredencial entity, int equiapmentoVeiculoId, ITipoCredencialRepositorio credencialRepositorio)
        {
            Alterar (entity);
            //Setar a data de validade, caso Status Ativo
            if (!entity.Ativa) return;
            //Obter contrato
            var contrato = ObterNumeroContrato (entity, equiapmentoVeiculoId);
            var numContrato = contrato.NumeroContrato;
            //Obter uma data de validade (menor data entre um curso do tipo controlado e uma data de vencimento de um determinado contrato
            var dataCredencial = ObterDataValidadeCredencial (entity.TipoCredencialId, equiapmentoVeiculoId, numContrato, credencialRepositorio);
            //Setando a data de vencimento uma credencial
            entity.Validade = dataCredencial;
            Alterar (entity);
        }

        /// <summary>
        ///     Obter dados da credencial
        /// </summary>
        /// <param name="veiculoCredencialId">Identificador</param>
        /// <returns></returns>
        public VeiculosCredenciaisView BuscarCredencialPelaChave(int veiculoCredencialId)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciaisView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialID", DbType.Int32, veiculoCredencialId, 0).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculosCredenciaisView>();
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
        ///     Obter dados da credencial pelo numero da credencial
        /// </summary>
        /// <param name="numCredencial"></param>
        /// <returns></returns>
        public VeiculoCredencial ObterCredencialPeloNumeroCredencial(string numCredencial)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciais", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("NumeroCredencial", numCredencial).Igual()));
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

        #endregion
    }
}