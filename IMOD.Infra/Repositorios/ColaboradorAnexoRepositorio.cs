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
    public class ColaboradorAnexoRepositorio : IColaboradorAnexoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorAnexoRepositorio()
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
        public void Alterar(ColaboradorAnexo entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("ColaboradoresAnexos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorAnexoId", entity.ColaboradorAnexoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorID", entity.ColaboradorId, false)));
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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorAnexo BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresAnexos", conn))

                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorAnexoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorAnexo>();

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
        public void Criar(ColaboradorAnexo entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("ColaboradoresAnexos", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorAnexoId", entity.ColaboradorAnexoId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Descricao", entity.Descricao, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("NomeArquivo", entity.NomeArquivo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorID", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Arquivo", entity.Arquivo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorAnexoId = key;

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
        public ICollection<ColaboradorAnexo> Listar(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresAnexosView", conn))

                    {


                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorAnexoId", DbType.Int32, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NomeArquivo", DbType.String, o, 2).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorAnexo>();

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
        ///     ListarComAnexo
        /// </summary>
        /// <returns></returns>
        public ICollection<ColaboradorAnexo> ListarComAnexo(params object[] o)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("ColaboradoresAnexos", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("ColaboradorAnexoId", DbType.Int32, o, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("NomeArquivo", DbType.String, o, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Descricao", DbType.String, o, 3).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorAnexo>();

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
        public void Remover(ColaboradorAnexo entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("ColaboradoresAnexos", conn))
                    {

                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamDelete("ColaboradorAnexoId", entity.ColaboradorAnexoId).Igual()));

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