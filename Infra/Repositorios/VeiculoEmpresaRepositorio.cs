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
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class VeiculoEmpresaRepositorio : IVeiculoEmpresaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculoEmpresaRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro VeiculoEmpresa
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText("VeiculosEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoEmpresaID", entity.VeiculoEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaContratoID", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", entity.Ativo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.VeiculoEmpresaId = key;
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
        ///     Buscar pela chave primaria VeiculoEmpresa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoEmpresa BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("VeiculosEmpresas", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamSelect("VeiculoEmpresaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoEmpresa>();

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
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta VeiculoEmpresa</param>
        /// <returns></returns>
        public ICollection<VeiculoEmpresa> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("VeiculoEmpresaView", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", DbType.Int16, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Ativo", DbType.Boolean, objects, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Cargo", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Matricula", DbType.String, objects, 3).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, objects, 4).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoEmpresa>();

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
        ///     Alterar registro VeiculoEmpresa
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText("VeiculosEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoEmpresaID", entity.VeiculoEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("VeiculoID", entity.VeiculoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaContratoID", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Ativo", entity.Ativo, false)));

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
        ///     Deletar registro VeiculoEmpresa
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(VeiculoEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText("VeiculosEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamDelete("VeiculoEmpresaId", entity.VeiculoEmpresaId).Igual()));
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException(ex);
                    }
                }
            }
        }
        /// <summary>
        ///    Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<VeiculosCredenciaisView> ListarView(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("VeiculosCredenciaisView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoCredencialID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCredencialID", DbType.String, o, 2).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("CredencialStatusID", DbType.String, o, 3).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", DbType.Int32, o, 4).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculosCredenciaisView>();
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
        ///    Listar Veículos e seus contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        public ICollection<VeiculoEmpresaView> ListarContratoView(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("VeiculoEmpresaView", conn))
                {
                    try
                    {
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("VeiculoID", DbType.Int32, o, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaNome", DbType.String, o, 1).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Matricula", DbType.Int32, o, 2).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoEmpresaView>();
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


        #endregion
    }
}