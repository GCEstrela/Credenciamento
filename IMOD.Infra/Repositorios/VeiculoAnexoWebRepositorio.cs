// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
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
    public class VeiculoAnexoWebRepositorio : IVeiculoAnexoWebRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculoAnexoWebRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro VeiculoAnexo
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoAnexoWeb entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("VeiculosAnexosWeb", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoAnexoWebID", entity.VeiculoAnexoWebId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoAnexoID", entity.VeiculoAnexoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Arquivo", entity.Arquivo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.VeiculoAnexoWebId = key;

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria VeiculoAnexo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoAnexoWeb BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosAnexosWeb", conn))

                    {


                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("VeiculoAnexoId", DbType.Int32, id).Igual()));

                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoAnexoWeb>();

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
        ///     Listar VeiculoAnexo
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoAnexoWeb> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("VeiculosAnexosWeb", conn))

                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoAnexoID", objects, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NomeArquivo", objects, 2).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoAnexoWeb>();

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
        ///     Alterar registro VeiculoAnexo
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoAnexoWeb entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("VeiculosAnexosWeb", conn))
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoAnexoWebID", entity.VeiculoAnexoWebId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoAnexoID", entity.VeiculoAnexoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Arquivo", entity.Arquivo, false)));

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
        public void Remover(VeiculoAnexoWeb entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("VeiculosAnexosWeb", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("VeiculoAnexoId", entity.VeiculoAnexoId).Igual()));

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