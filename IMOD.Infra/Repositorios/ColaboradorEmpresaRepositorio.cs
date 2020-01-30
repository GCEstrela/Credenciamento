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
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class ColaboradorEmpresaRepositorio : IColaboradorEmpresaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorEmpresaRepositorio()
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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorEmpresa entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("ColaboradoresEmpresas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorEmpresaId", entity.ColaboradorEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("CardHolderGuid", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaId", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaContratoId", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeAnexo", entity.NomeAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Anexo", entity.Anexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ManuseioBagagem", entity.ManuseioBagagem, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FlagCcam", entity.FlagCcam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("OperadorPonteEmbarque", entity.OperadorPonteEmbarque, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Motorista", entity.Motorista, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("FlagAuditoria", entity.FlagAuditoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataInicio", entity.DataInicio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("DataFim", entity.DataFim, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Usuario", entity.Usuario, false)));

                        cmd.ExecuteNonQuery();

                        ////Gerar matricula
                        //if (entity.Matricula == null)
                        //    CriarNumeroMatricula(entity);

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
        public ColaboradorEmpresa BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresEmpresas", conn))

                    {

                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        ///     Criar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Criar(ColaboradorEmpresa entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("ColaboradoresEmpresas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorEmpresaId", entity.ColaboradorEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("CardHolderGuid", entity.CardHolderGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaId", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaContratoId", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", entity.Ativo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeAnexo", entity.NomeAnexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Anexo", entity.Anexo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Validade", entity.Validade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ManuseioBagagem", entity.ManuseioBagagem, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FlagCcam", entity.FlagCcam, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("OperadorPonteEmbarque", entity.OperadorPonteEmbarque, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Motorista", entity.Motorista, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("FlagAuditoria", entity.FlagAuditoria, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataInicio", entity.DataInicio, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("DataFim", entity.DataFim, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Usuario", entity.Usuario, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorEmpresaId = key;
                        //Gerar matricula
                        CriarNumeroMatricula(entity);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Criar numero de matricula
        /// </summary>
        /// <param name="entity"></param>
        public void CriarNumeroMatricula(ColaboradorEmpresa entity)
        {
            try
            {
                var codigo = entity.ColaboradorEmpresaId.ToString("N0");
                var data = DateTime.Now.ToString("yy");
                entity.Matricula = $"{codigo}-{data}";//entity
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
        public ICollection<ColaboradorEmpresa> Listar(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradorEmpresaView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativo", DbType.Boolean, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cargo", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Matricula", DbType.String, o, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 4).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, o, 5).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaContratoId", DbType.Int32, o, 6).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaId", DbType.Int32, o, 7).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        ///     Listar
        /// </summary>
        /// <returns></returns>
        public ICollection<RelatorioColaboradorEmpresaView> ListarColaboradorEmpresaView(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradorEmpresaRelatorioView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CNH", DbType.Int32, o, 1).IsNotNull()));


                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<RelatorioColaboradorEmpresaView>();

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
        ///     Deletar registro
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void Remover(ColaboradorEmpresa entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("ColaboradoresEmpresas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorEmpresaId", entity.ColaboradorEmpresaId).Igual()));

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
        ///     Listar View
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorEmpresa> ListarView(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradorEmpresaView", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativo", DbType.Boolean, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cargo", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Matricula", DbType.String, o, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

                        return d1;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ICollection<ColaboradorEmpresa> BuscarListaIntegracao(int nomedaTabela)
        {
            try
            {
                var select = string.Empty;                
                switch (nomedaTabela)
                {
                    case TipoSelectColaboradorEmpresa.integracao:
                        select = "Select * From ColaboradorEmpresaView Where CardHolderGUID is null And UsuarioDB is not null";
                        break;
                    case TipoSelectColaboradorEmpresa.insercao:
                        select = "Select * From ColaboradorEmpresaView Where CardHolderGUID is null And UsuarioDB is not null";
                        break;
                    case TipoSelectColaboradorEmpresa.delecao:
                        select = "Select * From ColaboradorEmpresaView Where CardHolderGUID is null And UsuarioDB is not null";
                        break;
                    default:
                        break;
                }
                

                using (var conn = _dataBase.CreateOpenConnection())
                {
                   
                    
                    using (var cmd = _dataBase.SelectSQL(select, conn))
                    {

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

                        return d1;

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