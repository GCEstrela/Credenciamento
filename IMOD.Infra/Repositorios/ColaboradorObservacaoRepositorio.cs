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
    public class ColaboradorObservacaoRepositorio : IColaboradorObservacaoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorObservacaoRepositorio()
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
        public void Alterar(ColaboradorObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("ColaboradoresObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorObservacaoId", entity.ColaboradorObservacaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Impeditivo", entity.Impeditivo, false)));

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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorObservacao BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresObservacoes", conn))

                    {

                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamSelect("ColaboradorObservacaoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorObservacao>();

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
        public void Criar(ColaboradorObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("ColaboradoresObservacoes", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorObservacaoId", entity.ColaboradorObservacaoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Observacao", entity.Observacao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Impeditivo", entity.Impeditivo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorObservacaoId = key;
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
        public ICollection<ColaboradorObservacao> Listar(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradorObservacaoView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorObservacao>();

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
        public void Remover(ColaboradorObservacao entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("ColaboradoresEmpresas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("ColaboradorEmpresaId", entity.ColaboradorObservacaoId).Igual()));

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
        public ICollection<ColaboradorObservacao> ListarView(params object[] o)
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
                        var d1 = reader.MapToList<ColaboradorObservacao>();

                        return d1;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ICollection<ColaboradorObservacao> BuscarListaIntegracao(int nomedaTabela)
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
                        var d1 = reader.MapToList<ColaboradorObservacao>();

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