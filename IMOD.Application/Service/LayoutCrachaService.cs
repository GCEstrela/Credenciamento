// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class LayoutCrachaService : ILayoutCrachaService
    {
        #region Variaveis Globais

        private readonly ILayoutCrachaRepositorio _repositorio = new LayoutCrachaRepositorio();

        #endregion

        #region Construtor

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
                _repositorio.Criar(entity);
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
                return _repositorio.BuscarPelaChave(id);
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
                return _repositorio.Listar(objects);
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
                _repositorio.Alterar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(LayoutCracha entity)
        {
            try
            {
                _repositorio.Remover(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Listar layout Crachas
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaView(params object[] objects)
        {
            try
            {
                return _repositorio.ListarLayoutCrachaView(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Listar layout Cracha por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaPorEmpresaView(int idEmpresa)
        {
            try
            {
                return _repositorio.ListarLayoutCrachaPorEmpresaView(idEmpresa);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}