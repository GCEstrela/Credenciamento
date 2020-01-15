// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 03 - 2019
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
    public class ConfiguraSistemaRepositorio : IConfiguraSistemaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();
        #region Construtor

        public ConfiguraSistemaRepositorio()
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

        public void Alterar(Domain.Entities.ConfiguraSistema entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("ConfiguraSistema", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ConfiguraSistemaID", entity.ConfiguraSistemaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNPJ", entity.CNPJ, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeEmpresa", entity.NomeEmpresa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaLOGO", entity.EmpresaLOGO, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Contrato", entity.Contrato, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Colete", entity.Colete, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Regras", entity.Regras, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("AssociarGrupos", entity.AssociarGrupos, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Email", entity.Email, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Responsavel", entity.Responsavel, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("SMTP", entity.SMTP, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmailUsuario", entity.EmailUsuario, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmailSenha", entity.EmailSenha, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("UrlSistema", entity.UrlSistema, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("imagemResolucao", entity.imagemResolucao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("imagemTamanho", entity.imagemTamanho, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("arquivoTamanho", entity.arquivoTamanho, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeAeroporto", entity.NomeAeroporto, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TelefoneEmergencia", entity.TelefoneEmergencia, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("UrlSistemaPreCadastro", entity.UrlSistemaPreCadastro, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DiasContencao", entity.diasContencao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("PortaSMTP", entity.PortaSMTP, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EnableSsl", entity.EnableSsl, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("UseDefaultCredentials", entity.UseDefaultCredentials, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Licenca", entity.Licenca, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("AssociarRegras", entity.AssociarRegras, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VisibleGruposRegras", entity.VisibleGruposRegras, false)));
                        if (entity.GrupoPadrao != null)
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("GrupoPadrao", entity.GrupoPadrao.Trim(), false)));
                        }

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Domain.Entities.ConfiguraSistema BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ConfiguraSistema", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ConfiguraSistemaID", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Domain.Entities.ConfiguraSistema>();

                        return d1.FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Criar(Domain.Entities.ConfiguraSistema entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("ConfiguraSistema", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ConfiguraSistemaID", entity.ConfiguraSistemaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CNPJ", entity.CNPJ, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeEmpresa", entity.NomeEmpresa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaLOGO", entity.EmpresaLOGO, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Contrato", entity.Contrato, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ConfiguraSistemaId = key;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ICollection<Domain.Entities.ConfiguraSistema> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ConfiguraSistema", conn))
                    {           

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CNPJ", objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Domain.Entities.ConfiguraSistema>();

                        return d1;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remover(Domain.Entities.ConfiguraSistema entity)
        {
            try
            {
                //using (var conn = _dataBase.CreateOpenConnection())
                //{
                //    using (var cmd = _dataBase.DeleteText("ConfiguraSistema", conn))
                //    {

                //        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ConfiguraSistemaID", entity.ConfiguraSistemaId).Igual()));
                //        cmd.ExecuteNonQuery();

                //    }
                //}

                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.CreateCommand("[dbo].[Contencao]", conn))
                    {
                        try
                        {
                            var returns = cmd.ExecuteScalar();

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
        private void Contencao(Domain.Entities.ConfiguraSistema entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.CreateCommand("Select dbo.Contencao", conn))
                    {
                        try
                        {
                            var returns = cmd.ExecuteScalar();

                            //return true;
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
