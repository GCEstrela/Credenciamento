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
    public class VeiculoCredencialimpressaoRepositorio : IVeiculoCredencialimpressaoRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public VeiculoCredencialimpressaoRepositorio()
        {
            _dataBase = _dataWorkerFactory.ObterDataBaseSingleton (TipoDataBase.SqlServer, _connection);
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro VeiculoCredencialimpressao
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoCredencialimpressao entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.InsertText ("VeiculosCredenciaisImpressoes", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("CredencialImpressaoID", entity.CredencialImpressaoId, true)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("VeiculoCredencialID", entity.VeiculoCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("DataImpressao", entity.DataImpressao, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamInsert ("Cobrar", entity.Cobrar, false)));

                        var key = Convert.ToInt32 (cmd.ExecuteScalar());

                        entity.CredencialImpressaoId = key;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Buscar pela chave primaria VeiculoCredencialimpressao
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoCredencialimpressao BuscarPelaChave(int id)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciaisImpressoes", conn))

                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamSelect ("CredencialImpressaoId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<VeiculoCredencialimpressao>();

                        return d1.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Listar VeiculoCredencialimpressao
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoCredencialimpressao> Listar(params object[] objects)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.SelectText ("VeiculosCredenciaisImpressoes", conn))

                {
                    try
                    {
                        cmd.CreateParameterSelect (_dataBase.CreateParameter (new ParamSelect ("VeiculoCredencialID", objects, 0).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<VeiculoCredencialimpressao>();

                        return d1;
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Alterar registro VeiculoCredencialimpressao
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoCredencialimpressao entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.UpdateText ("VeiculosCredenciaisImpressoes", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("CredencialImpressaoID", entity.CredencialImpressaoId, true)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("VeiculoCredencialID", entity.VeiculoCredencialId, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("DataImpressao", entity.DataImpressao, false)));
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamUpdate ("Cobrar", entity.Cobrar, false)));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Deletar registro VeiculoCredencialimpressao
        /// </summary>
        /// <param name="objects"></param>
        public void Remover(VeiculoCredencialimpressao entity)
        {
            using (var conn = _dataBase.CreateOpenConnection())
            {
                using (var cmd = _dataBase.DeleteText ("VeiculosCredenciaisImpressoes", conn))
                {
                    try
                    {
                        cmd.Parameters.Add (_dataBase.CreateParameter (new ParamDelete ("CredencialImpressaoId", entity.CredencialImpressaoId).Igual()));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Utils.TraceException (ex);
                    }
                }
            }
        }

        #endregion
    }
}