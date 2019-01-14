// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

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
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EquipamentoVeiculoTipoServico BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<EquipamentoVeiculoTipoServico> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(EquipamentoVeiculoTipoServico entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(EquipamentoVeiculoTipoServico entity)
        {
            _repositorio.Remover(entity);
        }

        /// <summary>
        /// Remover 
        /// </summary>
        /// <param name="equipamentoVeiculoId"></param>
        public void RemoverPorVeiculo(int equipamentoVeiculoId)
        {
            _repositorio.RemoverPorVeiculo(equipamentoVeiculoId);
        }

        /// <summary>
        /// Listar layout Cracha por empresa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public ICollection<EquipamentoVeiculoTipoServicoView> ListarEquipamentoVeiculoTipoServicoView(int id)
        {
            return _repositorio.ListarEquipamentoVeiculoTipoServicoView(id);
        }

        #endregion
    }
}