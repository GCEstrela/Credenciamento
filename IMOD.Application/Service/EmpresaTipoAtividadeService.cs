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
    public class EmpresaTipoAtividadeService : IEmpresaTipoAtividadeService
    {
        #region Variaveis Globais

        private readonly IEmpresaTipoAtividadeRepositorio _repositorio = new EmpresaTipoAtividadeRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EmpresaTipoAtividade entity)
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
        public EmpresaTipoAtividade BuscarPelaChave(int id)
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
        public ICollection<EmpresaTipoAtividade> Listar(params object[] objects)
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
        public void Alterar(EmpresaTipoAtividade entity)
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
        public void Remover(EmpresaTipoAtividade entity)
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
        /// Remover tipo de atividades por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        public void RemoverPorEmpresa(int empresaId)
        {
            try
            {
                _repositorio.RemoverPorEmpresa(empresaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ICollection<EmpresaTipoAtividadeView> ListarEmpresaTipoAtividadeView(params object[] objects)
        {
            try
            {
                return _repositorio.ListarEmpresaTipoAtividadeView(objects);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}