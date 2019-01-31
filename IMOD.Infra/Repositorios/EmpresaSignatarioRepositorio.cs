// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    public class EmpresaSignatarioRepositorio : IEmpresaSignatarioRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaSignatarioRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EmpresaSignatario entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("EmpresasSignatarios", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaSignatarioID", entity.EmpresaSignatarioId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CPF", entity.Cpf.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Email", entity.Email, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Telefone", entity.Telefone, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Celular", entity.Celular, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Principal", entity.Principal, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Assinatura", entity.Assinatura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivo", entity.NomeArquivo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaSignatarioId = key;
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
        public EmpresaSignatario BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresasSignatarios", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaSignatarioId", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaSignatario>();

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
        public ICollection<EmpresaSignatario> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresasSignatarios", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CPF", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Email", DbType.String, objects, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Telefone", DbType.String, objects, 4).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Celular", DbType.String, objects, 5).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Principal", DbType.String, objects, 6).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaSignatario>();

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
        public void Alterar(EmpresaSignatario entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("EmpresasSignatarios", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaSignatarioID", entity.EmpresaSignatarioId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome.Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CPF", entity.Cpf.RetirarCaracteresEspeciais().Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Email", entity.Email.Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Telefone", entity.Telefone.Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Celular", entity.Celular.Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Principal", entity.Principal, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Assinatura", entity.Assinatura.Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo.Trim(), false)));

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
        public void Remover(EmpresaSignatario entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("EmpresasSignatarios", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaSignatarioId", entity.EmpresaSignatarioId).Igual()));

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