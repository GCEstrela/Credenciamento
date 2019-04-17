// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class EmpresaRepositorio : IEmpresaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Empresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("Empresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Sigla", entity.Sigla.Trim(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cnpj", entity.Cnpj.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CEP", entity.Cep, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Endereco", entity.Endereco, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Numero", entity.Numero, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Complemento", entity.Complemento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Bairro", entity.Bairro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Telefone", entity.Telefone, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Email1", entity.Email1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Contato1", entity.Contato1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Telefone1", entity.Telefone1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Celular1", entity.Celular1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Email2", entity.Email2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Contato2", entity.Contato2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Telefone2", entity.Telefone2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Celular2", entity.Celular2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Obs", entity.Obs, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Responsavel", entity.Responsavel, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Logo", entity.Logo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("InsEst", entity.InsEst, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("InsMun", entity.InsMun, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", true, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente11", entity.Pendente11, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente12", entity.Pendente12, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente13", entity.Pendente13, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente14", entity.Pendente14, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente15", entity.Pendente15, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente16", entity.Pendente16, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente17", entity.Pendente17, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("PraVencer", entity.PraVencer, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Senha", entity.Senha, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaId = key;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601)
                            throw new InvalidOperationException("CNPJ já existente.");
                        throw;
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
        public Empresa BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Empresas", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Empresa>();

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



        public ICollection<Empresa> ListarEmpresasPendentes(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Empresas", conn))
                {
                    try
                    {

                        var empresasPendencias = ListarPendencias();

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 0).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Apelido", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cnpj", DbType.String, objects, 2).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Empresa>();
                        
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
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Empresa> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Empresas", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 0).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Apelido", DbType.String, objects, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cnpj", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("PraVencer", DbType.String, objects, 3).MenorIgual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Empresa>();

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
        public void Alterar(Empresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("Empresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Sigla", entity.Sigla, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cnpj", entity.Cnpj.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CEP", entity.Cep.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Endereco", entity.Endereco, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Numero", entity.Numero, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Complemento", entity.Complemento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Bairro", entity.Bairro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Telefone", entity.Telefone, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Email1", entity.Email1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Contato1", entity.Contato1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Telefone1", entity.Telefone1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Celular1", entity.Celular1, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Email2", entity.Email2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Contato2", entity.Contato2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Telefone2", entity.Telefone2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Celular2", entity.Celular2, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Obs", entity.Obs, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Responsavel", entity.Responsavel, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Logo", entity.Logo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("InsEst", entity.InsEst, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("InsMun", entity.InsMun, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente11", entity.Pendente11, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente12", entity.Pendente12, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente13", entity.Pendente13, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente14", entity.Pendente14, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente15", entity.Pendente15, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente16", entity.Pendente16, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente17", entity.Pendente17, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PraVencer", entity.PraVencer, false)));
                        

                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601)
                            throw new InvalidOperationException("CNPJ já existente.");
                        throw;
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
        public void Remover(Empresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("Empresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaId", entity.EmpresaId).Igual()));

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
        /// Buscar empresa por CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public Empresa BuscarEmpresaPorCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return null;
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Empresas", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("Cnpj", DbType.String, cnpj.RetirarCaracteresEspeciais()).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Empresa>();

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
        /// Buscar empresa por Sigla
        /// </summary>
        /// <param name="sigla"></param>
        /// <returns></returns>
        public Empresa BuscarEmpresaPorSigla(string sigla)
        {
            if (string.IsNullOrWhiteSpace(sigla)) return null;
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Empresas", conn))

                {
                    try
                    {
                        //cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, EmpresaId).Igual()));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("Sigla", DbType.String, sigla).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Empresa>();

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
        /// Listar Pendencias
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<EmpresaPendenciaView> ListarPendencias(int empresaId = 0)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresaPendenciasView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaId", DbType.Int32, empresaId).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaPendenciaView>();
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
        public ICollection<EmpresaTipoCredencialView> ListarTipoCredenciaisEmpresa(int empresaId = 0)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("EmpresaCredenciaisView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaId", DbType.Int32, empresaId).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var objResult = reader.MapToList<EmpresaTipoCredencialView>();
                        return objResult;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        //public Empresa BuscarEmpresaPorSigla(string sigla)
        //{
        //    throw new NotImplementedException();
        //}


        #endregion
    }
}