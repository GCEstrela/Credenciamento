// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 07 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;

#endregion

namespace IMOD.Application.Service
{
    public class PendenciaService : IPendenciaService
    {
        #region Variaveis Globais

        private readonly IPendenciaRepositorio _repositorio = new PendenciaRepositorio();

        #endregion

        #region  Propriedades

        /// <summary>
        ///     Tipo
        /// </summary>
        public ITipoPendenciaService TipoPendenciaService
        {
            get { return new TipoPendenciaService(); }
        }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(Pendencia entity)
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
        public Pendencia BuscarPelaChave(int id)
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
        public ICollection<Pendencia> Listar(params object[] objects)
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
        public void Alterar(Pendencia entity)
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
        public void Remover(Pendencia entity)
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
        ///     Desativar Pendência
        /// </summary>
        /// <param name="entity"></param>
        public void Desativar(Pendencia entity)
        {
            try
            {
                _repositorio.Desativar(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Listar Pendencia por Empresa
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorEmpresa(int empresaId)
        {
            try
            {
                return _repositorio.ListarPorEmpresa(empresaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Listar Pendencia por Colaborador
        /// </summary>
        /// <param name="colaboradorId"></param> 
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorColaborador(int colaboradorId)
        {
            try
            {
                return _repositorio.ListarPorColaborador(colaboradorId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Listar Pendencia por Veiculo
        /// </summary>
        /// <param name="veiculoId"></param> 
        /// <returns></returns>
        public ICollection<Pendencia> ListarPorVeiculo(int veiculoId)
        {
            try
            {
                return _repositorio.ListarPorVeiculo(veiculoId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Criar pendência de sistema
        /// </summary>
        /// <param name="entity"></param>
        public void CriarPendenciaSistema(Pendencia entity)
        {
            try
            {
                _repositorio.CriarPendenciaSistema(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///     Alterar pendência de sistema
        /// </summary>
        /// <param name="entity"></param>
        public void AlterarPendenciaSistema(Pendencia entity)
        {
            try
            {
                _repositorio.AlterarPendenciaSistema(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}