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
    public class EquipamentoVeiculoTipoServicoService : IEquipamentoVeiculoTipoServicoService
    {
        #region Variaveis Globais

        private readonly IEquipamentoVeiculoTipoServicoRepositorio _repositorio = new EquipamentoVeiculoTipoServicoRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(EquipamentoVeiculoTipoServico entity)
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
        public EquipamentoVeiculoTipoServico BuscarPelaChave(int id)
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
        public ICollection<EquipamentoVeiculoTipoServico> Listar(params object[] objects)
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
        public void Alterar(EquipamentoVeiculoTipoServico entity)
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
        public void Remover(EquipamentoVeiculoTipoServico entity)
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
        /// Remover 
        /// </summary>
        /// <param name="equipamentoVeiculoId"></param>
        public void RemoverPorVeiculo(int equipamentoVeiculoId)
        {
            try
            {
                _repositorio.RemoverPorVeiculo(equipamentoVeiculoId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Listar layout Cracha por empresa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public ICollection<EquipamentoVeiculoTipoServicoView> ListarEquipamentoVeiculoTipoServicoView(int id)
        {
            try
            {
                return _repositorio.ListarEquipamentoVeiculoTipoServicoView(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}