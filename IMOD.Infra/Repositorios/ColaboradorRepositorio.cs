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
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class ColaboradorRepositorio : IColaboradorRepositorio
    {
        private readonly IColaboradorAnexoRepositorio _anexoRepositorio = new ColaboradorAnexoRepositorio();
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Criar colaboradore e anexo
        /// </summary>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="anexos">Anexos</param>
        public void CriarAnexos(Colaborador colaborador, IList<ColaboradorAnexo> anexos)
        {
            Criar(colaborador); //Criar colaborador
            foreach (var item in anexos)
            {
                item.ColaboradorId = colaborador.ColaboradorId;
                _anexoRepositorio.Criar(item); //Criar Anexos
            }
        }

        /// <summary>
        ///     Listar Colaborador por status
        /// </summary>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        public ICollection<Colaborador> ListarPorStatus(int idStatus)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Colaboradores", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("StatusID", DbType.Int32, idStatus).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Colaborador>();

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
        ///     Obter colaborador por CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public Colaborador ObterPorCpf(string cpf)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Colaboradores", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("Cpf", DbType.String, cpf.RetirarCaracteresEspeciais()).Igual())); 
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Colaborador>();
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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(Colaborador entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("Colaboradores", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorId", entity.ColaboradorId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataNascimento", DbType.Date, entity.DataNascimento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomePai", entity.NomePai, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeMae", entity.NomeMae, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nacionalidade", entity.Nacionalidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Foto", entity.Foto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoCivil", entity.EstadoCivil, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CPF", entity.Cpf.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RG", entity.Rg, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RGEmissao", DbType.Date, entity.RgEmissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RGOrgLocal", entity.RgOrgLocal, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RGOrgUF", entity.RgOrgUf, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Passaporte", entity.Passaporte, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PassaporteValidade", DbType.Date, entity.PassaporteValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("RNE", entity.Rne, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TelefoneFixo", entity.TelefoneFixo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TelefoneCelular", entity.TelefoneCelular, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Email", entity.Email, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ContatoEmergencia", entity.ContatoEmergencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TelefoneEmergencia", entity.TelefoneEmergencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cep", entity.Cep, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Endereco", entity.Endereco, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Numero", entity.Numero, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Complemento", entity.Complemento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Bairro", entity.Bairro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Motorista", entity.Motorista, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNHCategoria", entity.CnhCategoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNH", entity.Cnh, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNHValidade", DbType.Date, entity.CnhValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNHEmissor", entity.CnhEmissor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNHUF", entity.Cnhuf, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Bagagem", entity.Bagagem, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataEmissao", DbType.Date, entity.DataEmissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataValidade", DbType.Date, entity.DataValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente21", entity.Pendente21, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente22", entity.Pendente22, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente23", entity.Pendente23, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente24", entity.Pendente24, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Pendente25", entity.Pendente25, false)));

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
        public Colaborador BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Colaboradores", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Colaborador>();

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
        public void Criar(Colaborador entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("Colaboradores", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorId", entity.ColaboradorId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataNascimento", DbType.Date, entity.DataNascimento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomePai", entity.NomePai, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeMae", entity.NomeMae, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nacionalidade", entity.Nacionalidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Foto", entity.Foto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoCivil", entity.EstadoCivil, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CPF", entity.Cpf.RetirarCaracteresEspeciais(), false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RG", entity.Rg, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RGEmissao", DbType.Date, entity.RgEmissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RGOrgLocal", entity.RgOrgLocal, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RGOrgUF", entity.RgOrgUf, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Passaporte", entity.Passaporte, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("PassaporteValidade", DbType.Date, entity.PassaporteValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("RNE", entity.Rne, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TelefoneFixo", entity.TelefoneFixo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TelefoneCelular", entity.TelefoneCelular, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Email", entity.Email, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ContatoEmergencia", entity.ContatoEmergencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TelefoneEmergencia", entity.TelefoneEmergencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cep", entity.Cep, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Endereco", entity.Endereco, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Numero", entity.Numero, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Complemento", entity.Complemento, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Bairro", entity.Bairro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("MunicipioID", entity.MunicipioId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EstadoID", entity.EstadoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Motorista", entity.Motorista, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNHCategoria", entity.CnhCategoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNH", entity.Cnh, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNHValidade", DbType.Date, entity.CnhValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNHEmissor", entity.CnhEmissor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNHUF", entity.Cnhuf, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Bagagem", entity.Bagagem, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataEmissao", DbType.Date, entity.DataEmissao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataValidade", DbType.Date, entity.DataValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", true, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("StatusID", entity.StatusId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoAcessoID", entity.TipoAcessoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente21", entity.Pendente21, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente22", entity.Pendente22, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente23", entity.Pendente23, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente24", entity.Pendente24, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Pendente25", entity.Pendente25, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());
                        entity.ColaboradorId = key;
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
        public ICollection<Colaborador> Listar(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("Colaboradores", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cpf", DbType.String, o, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, o, 2).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Colaborador>();

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
        public void Remover(Colaborador entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("Colaboradores", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorId", DbType.Int32, entity.ColaboradorId).Igual()));
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