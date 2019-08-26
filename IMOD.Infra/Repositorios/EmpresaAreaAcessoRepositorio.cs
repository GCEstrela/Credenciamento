// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
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
    public class EmpresaAreaAcessoRepositorio : IEmpresaAreaAcessoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaAreaAcessoRepositorio()
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
        public void Criar(EmpresaAreaAcesso entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EmpresasAreasAcessos", conn))
                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaAreaAcessoID", entity.EmpresaAreaAcessoId, true)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("AreaAcessoID", entity.AreaAcessoId, false)));

                            var key = Convert.ToInt32(cmd.ExecuteScalar());

                            entity.EmpresaAreaAcessoId = key;
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

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmpresaAreaAcesso BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasAreasAcessos", conn))

                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaAreaAcessoId", DbType.Int32, id).Igual()));
                            var reader = cmd.ExecuteReader();
                            var d1 = reader.MapToList<EmpresaAreaAcesso>();

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
        public ICollection<EmpresaAreaAcesso> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasAreasAcessos", conn))

                    {
                        try
                        {
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaAreaAcessoID", DbType.Int32, objects, 0).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, objects, 1).Igual()));
                            cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("AreaAcessoID", DbType.Int32, objects, 2).Igual()));

                            var reader = cmd.ExecuteReaderSelect();
                            var d1 = reader.MapToList<EmpresaAreaAcesso>();

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaAreaAcesso entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EmpresasAreasAcessos", conn))
                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaAreaAcessoID", entity.EmpresaAreaAcessoId, true)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("AreaAcessoID", entity.AreaAcessoId, false)));

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(EmpresaAreaAcesso entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasAreasAcessos", conn))
                    {
                        try
                        {
                            cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaAreaAcessoId", entity.EmpresaAreaAcessoId).Igual()));

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Utils.TraceException(ex);
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