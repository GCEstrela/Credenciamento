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
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion
        #region  Metodos

        public void Alterar(Domain.Entities.ConfiguraSistema entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("ConfiguraSistema", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ConfiguraSistemaID", entity.ConfiguraSistemaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CNPJ", entity.CNPJ, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeEmpresa", entity.NomeEmpresa, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Apelido", entity.Apelido, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaLOGO", entity.EmpresaLOGO, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Contrato", entity.Contrato, false)));

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Colete", entity.Colete, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Regras", entity.Regras, false)));
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

        public Domain.Entities.ConfiguraSistema BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ConfiguraSistema", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ConfiguraSistemaID", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<Domain.Entities.ConfiguraSistema>();

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

        public void Criar(Domain.Entities.ConfiguraSistema entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("ConfiguraSistema", conn))
                {
                    try
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
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                        throw;
                    }
                }
            }
        }

        public ICollection<Domain.Entities.ConfiguraSistema> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ConfiguraSistema", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CNPJ", objects, 0).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<Domain.Entities.ConfiguraSistema>();

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

        public void Remover(Domain.Entities.ConfiguraSistema entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("ConfiguraSistema", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ConfiguraSistemaID", entity.ConfiguraSistemaId).Igual()));

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
