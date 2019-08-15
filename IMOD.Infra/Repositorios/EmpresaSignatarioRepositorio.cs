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
        public void Criar(EmpresaSignatario entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EmpresasSignatarios", conn))
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
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RG", entity.RG, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("OrgaoExp", entity.OrgaoExp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RGUF", entity.RGUF, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoRepresentanteID", entity.TipoRepresentanteId, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaSignatarioId = key;

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
        public EmpresaSignatario BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasSignatarios", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaSignatarioId", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaSignatario>();

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
        public ICollection<EmpresaSignatario> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasSignatariosView", conn))

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
        public void Alterar(EmpresaSignatario entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EmpresasSignatarios", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaSignatarioID", entity.EmpresaSignatarioId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CPF", entity.Cpf.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Email", entity.Email, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Telefone", entity.Telefone, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Celular", entity.Celular, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Principal", entity.Principal, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Assinatura", entity.Assinatura, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RG", entity.RG, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("OrgaoExp", entity.OrgaoExp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RGUF", entity.RGUF, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoRepresentanteID", entity.TipoRepresentanteId, false)));

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
        /// <param name="objects"></param>
        public void Remover(EmpresaSignatario entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasSignatarios", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaSignatarioId", entity.EmpresaSignatarioId).Igual()));
                        cmd.ExecuteNonQuery();

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