// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 26 - 2018
// ***********************************************************************

#region

using IMOD.Application.Interfaces;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using IMOD.Domain.Interfaces;
using IMOD.Infra.Repositorios;
using System.Collections.Generic;

#endregion

namespace IMOD.Application.Service
{
    public class VeiculoEmpresaService : IVeiculoEmpresaService
    {
        #region Variaveis Globais

        private readonly IVeiculoEmpresaRepositorio _repositorio = new VeiculoEmpresaRepositorio();

        #endregion

        #region Construtor

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoEmpresa entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoEmpresa BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoEmpresa> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoEmpresa entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoEmpresa entity)
        {
            _repositorio.Remover(entity);
        }

        public ICollection<VeiculoEmpresa> ListarEmpresas(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        public ICollection<VeiculoEmpresaView> ListarContratoView(params object[] o)
        {
            return _repositorio.ListarContratoView(o);
        } 

        #endregion
    }
}