﻿// ***********************************************************************
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
    public class LayoutCrachaRepositorio : ILayoutCrachaRepositorio
    {
        private readonly string _connection = CurrentConfig.ConexaoString;
        private readonly IDataBaseAdo _dataBase;
        private readonly IDataWorkerFactory _dataWorkerFactory = new DataWorkerFactory();

        #region Construtor

        public LayoutCrachaRepositorio()
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
        public void Criar(LayoutCracha entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.InsertText("LayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutCrachaID", entity.LayoutCrachaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutCrachaGUID", entity.LayoutCrachaGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Valor", entity.Valor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("LayoutRPT", entity.LayoutRpt, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoCracha", entity.TipoCracha, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("TipoValidade", entity.TipoValidade, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamInsert("Cor", entity.Cor, false)));

                        var key = Convert.ToInt32(cmd.ExecuteScalar());

                        entity.LayoutCrachaId = key;

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
        public LayoutCracha BuscarPelaChave(int id)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("LayoutsCrachas", conn))

                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamSelect("LayoutCrachaId", DbType.Int32, id).Igual()));
                        var reader = cmd.ExecuteReader();
                        var d1 = reader.MapToList<LayoutCracha>();

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
        public ICollection<LayoutCracha> Listar(params object[] objects)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.SelectText("LayoutsCrachas", conn))

                    {

                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Nome", DbType.String, objects, 0).Like()));
                        cmd.CreateParameterSelect(_dataBase.CreateParameter(new ParamSelect("Valor", DbType.String, objects, 1).Like()));

                        var reader = cmd.ExecuteReaderSelect();
                        var d1 = reader.MapToList<LayoutCracha>();

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
        public void Alterar(LayoutCracha entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.UpdateText("LayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaID", entity.LayoutCrachaId, true)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Nome", entity.Nome, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Modelo", entity.Modelo, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutCrachaGUID", entity.LayoutCrachaGuid, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("Valor", entity.Valor, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("LayoutRPT", entity.LayoutRpt, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoCracha", entity.TipoCracha, false)));
                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamUpdate("TipoValidade", entity.TipoValidade, false)));

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
        public void Remover(LayoutCracha entity)
        {
            try
            {
                using (var conn = _dataBase.CreateOpenConnection())
                {
                    using (var cmd = _dataBase.DeleteText("LayoutsCrachas", conn))
                    {

                        cmd.Parameters.Add(_dataBase.CreateParameter(new ParamDelete("LayoutCrachaId", entity.LayoutCrachaId).Igual()));

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

        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaPorEmpresaView(int idEmpresa)
        {
            try
            {
                return ListarLayoutCrachaView(idEmpresa, null, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion
    }
}