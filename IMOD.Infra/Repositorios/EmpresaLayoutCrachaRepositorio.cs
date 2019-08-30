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
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Ado;
using IMOD.Infra.Ado.Interfaces;
using IMOD.Infra.Ado.Interfaces.ParamSql;

#endregion

namespace IMOD.Infra.Repositorios
{
    public class EmpresaLayoutCrachaRepositorio : IEmpresaLayoutCrachaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public EmpresaLayoutCrachaRepositorio()
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
        public void Criar(EmpresaLayoutCracha entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("EmpresasLayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaLayoutCrachaID", entity.EmpresaLayoutCrachaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutCrachaID", entity.LayoutCrachaId, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.EmpresaLayoutCrachaId = key;

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
        public EmpresaLayoutCracha BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("EmpresasLayoutsCrachas", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("EmpresaLayoutCrachaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<EmpresaLayoutCracha>();

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
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCracha> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("LayoutCrachaView", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaLayoutCrachaID", DbType.Int32, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.Int32, objects, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("LayoutCrachaID", DbType.Int32, objects, 2).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 2).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaLayoutCracha>();

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
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EmpresaLayoutCracha entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("EmpresasLayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaLayoutCrachaID", entity.EmpresaLayoutCrachaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("EmpresaID", entity.EmpresaId, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID", entity.LayoutCrachaId, false)));

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
        public void Remover(EmpresaLayoutCracha entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasLayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("EmpresaLayoutCrachaID", entity.EmpresaLayoutCrachaId).Igual()));
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
        /// Remover tipo de atividades por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        public void RemoverPorEmpresa(int empresaId)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("EmpresasLayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("empresaId", empresaId).Igual()));

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaView(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("LayoutCrachaView", conn))
                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("EmpresaID", DbType.String, objects, 0).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("LayoutCrachaID", DbType.String, objects, 1).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 2).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("LayoutCrachaGUID", DbType.Int32, objects, 3).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoCracha", DbType.Int32, objects, 4).Igual()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("TipoValidade", DbType.Int32, objects, 5).Igual()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<EmpresaLayoutCrachaView>();
                        return d1;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaPorEmpresaView(int idEmpresa, int tipoChacha)
        {
            try
            {
                return ListarLayoutCrachaView(idEmpresa, null, null, null, tipoChacha);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}