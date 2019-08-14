// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
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
    public class EmpresaContratoRepositorio : IEmpresaContratoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaContratoRepositorio()
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
        public void Criar(EmpresaContrato entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EmpresasContratos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaContratoID", entity.EmpresaContratoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NumeroContrato", entity.NumeroContrato, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Emissao", entity.Emissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Terceirizada", entity.Terceirizada, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Contratante", entity.Contratante, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("IsencaoCobranca", entity.IsencaoCobranca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoCobrancaID", entity.TipoCobrancaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CobrancaEmpresaID", entity.CobrancaEmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CEP", entity.Cep, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Endereco", entity.Endereco, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Complemento", entity.Complemento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Numero", entity.Numero, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Bairro", entity.Bairro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeResp", entity.NomeResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TelefoneResp", entity.TelefoneResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CelularResp", entity.CelularResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmailResp", entity.EmailResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Arquivo", entity.Arquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ContratoBasico", entity.ContratoBasico, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ArquivoBlob", DbType.Binary, entity.ArquivoBlob, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TerceirizadaNome", entity.TerceirizadaNome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("PraVencer", entity.PraVencer, false)));


                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaContratoId = key;

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
        public EmpresaContrato BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasContratos", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaContratoId", DbType.Int32, id).Igual()));


                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaContrato>();

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
        public ICollection<EmpresaContrato> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasContratosView", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroContrato", DbType.String, objects, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Emissao", DbType.Date, objects, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Validade", DbType.Date, objects, 4).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Contratante", DbType.String, objects, 5).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCobrancaID", DbType.Int32, objects, 6).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmailResp", DbType.String, objects, 7).Like()));
                        //cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NumeroContrato", DbType.String, objects, 8).MaiorIgual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaContrato>();

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
        public void Alterar(EmpresaContrato entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EmpresasContratos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaContratoID", entity.EmpresaContratoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NumeroContrato", entity.NumeroContrato, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Emissao", entity.Emissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Terceirizada", entity.Terceirizada, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Contratante", entity.Contratante, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("IsencaoCobranca", entity.IsencaoCobranca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoCobrancaID", entity.TipoCobrancaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CobrancaEmpresaID", entity.CobrancaEmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CEP", entity.Cep, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Endereco", entity.Endereco, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Complemento", entity.Complemento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Numero", entity.Numero, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Bairro", entity.Bairro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeResp", entity.NomeResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TelefoneResp", entity.TelefoneResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CelularResp", entity.CelularResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmailResp", entity.EmailResp, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Arquivo", entity.Arquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ContratoBasico", entity.ContratoBasico, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ArquivoBlob", DbType.Binary, entity.ArquivoBlob, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TerceirizadaNome", entity.TerceirizadaNome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PraVencer", entity.PraVencer, false)));

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
        /// <param name="entity"></param>
        public void Remover(EmpresaContrato entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasContratos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaContratoId", entity.EmpresaContratoId).Igual()));

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
        ///     Listar contratos por número
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorNumeroContrato(string numContrato)
        {
            try
            {
                return Listar(null, numContrato, null, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Buscar numero do contrato
        /// </summary>
        /// <param name="numContrato"></param>
        /// <returns></returns>
        public EmpresaContrato BuscarContrato(string numContrato)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasContratos", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("NumeroContrato", numContrato).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaContrato>();

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
        ///     Listar contratos por descrição
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorDescricao(string desc)
        {
            try
            {
                return Listar(null, null, $"%{desc}%", null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///     Listar contratos por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<EmpresaContrato> ListarPorEmpresa(int empresaId)
        {
            try
            {
                return Listar(empresaId, null, null, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}