
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
    public class EmpresaRepositorio :IEmpresaRepositorio 
    {
	    private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public  EmpresaRepositorio()
        {
          _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Empresa entity)
        {
             using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText ("Empresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Sigla", entity.Sigla, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNPJ", entity.Cnpj, false)));
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
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Excluida", entity.Excluida, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente11", entity.Pendente11, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente12", entity.Pendente12, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente13", entity.Pendente13, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente14", entity.Pendente14, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente15", entity.Pendente15, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente16", entity.Pendente16, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente17", entity.Pendente17, false)));


                        var key = Convert.ToInt32 (cmd.ExecuteScalar());

                        entity.EmpresaId = key;
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
        public Empresa BuscarPelaChave(int id)
        {
           using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("Empresa", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("EmpresaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Empresa>();

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
        ///     Listar
        /// </summary>
        /// <param name="predicate">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<Empresa> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("Empresa", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome",DbType.String, objects, 0)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Apelido", DbType.String, objects, 1)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Sigla", DbType.String, objects, 2)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CNPJ", DbType.String, objects, 3)));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Email2", DbType.String, objects, 4)));
                        
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Empresa>();

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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Empresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText ("Empresa", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Sigla", entity.Sigla, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNPJ", entity.Cnpj, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CEP", entity.Cep, false)));
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
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Excluida", entity.Excluida, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente11", entity.Pendente11, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente12", entity.Pendente12, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente13", entity.Pendente13, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente14", entity.Pendente14, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente15", entity.Pendente15, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente16", entity.Pendente16, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente17", entity.Pendente17, false)));

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
        ///     Deletar registro
        /// </summary>
        /// <param name="predicate"></param>
        public void Remover(Empresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText ("Empresa", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamDelete ("EmpresaId", entity.EmpresaId).Igual()));

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

 