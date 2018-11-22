using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMOD.CrossCutting;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

namespace IMOD.Infra.Repositorios
{
    public class ColaboradorEmpresaRepositorio : IColaboradorEmpresaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        public ColaboradorEmpresaRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton(TipoDataBase.SqlServer, _connection);
        }

        #region  Metodos

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(ColaboradorEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {

                using (var cmd = _dataBase.UpdateText("ColaboradoresEmpresas", conn))
                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorEmpresaId", entity.ColaboradorEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaId", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaContratoId", entity.EmpresaContratoId, false)));
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
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        public ColaboradorEmpresa BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresEmpresas", conn))

                {
                    try
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorEmpresaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        public void Criar(ColaboradorEmpresa entity)
        {

            using (var conn = _dataBase.CreateOpenConnection())
            {

                using (var cmd = _dataBase.InsertText("ColaboradoresEmpresas", conn))
                {
                    try
                    {



                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorEmpresaId", entity.ColaboradorEmpresaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("ColaboradorId", entity.ColaboradorId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaId", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaContratoId", entity.EmpresaContratoId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cargo", entity.Cargo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Matricula", entity.Matricula, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Ativo", entity.Ativo, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.ColaboradorEmpresaId = key;
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
        public ICollection<ColaboradorEmpresa> Listar(params object[] o)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText("ColaboradoresEmpresas", conn))

                {
                    try
                    {
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("ColaboradorCredencialID", DbType.Int32, o, 0).Igual()));
                        
                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<ColaboradorEmpresa>();

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
        public void Remover(ColaboradorEmpresa entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {

                using (var cmd = _dataBase.DeleteText("ColaboradoresEmpresas", conn))
                {
                    try
                    {

                        cmd.Parameters.Add(
                            _dataBase.CreateParameter(new ParamDelete("ColaboradorEmpresaId", entity.ColaboradorEmpresaId).Igual()));

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
