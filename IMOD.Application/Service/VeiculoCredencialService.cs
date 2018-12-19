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
    public class VeiculoCredencialService : IVeiculoCredencialService
    {
        #region Variaveis Globais

        private readonly IVeiculoCredencialRepositorio _repositorio = new VeiculoCredencialRepositorio();

        #endregion

        #region Construtores

        public ITecnologiaCredencialService TecnologiaCredencial { get { return new TecnologiaCredencialService(); } }

        public ITipoCredencialService TipoCredencial { get { return new TipoCredencialService(); } }

        public ILayoutCrachaService LayoutCracha { get { return new LayoutCrachaService(); } }

        public IFormatoCredencialService FormatoCredencial { get { return new FormatoCredencialService(); } }

        public ICredencialStatusService CredencialStatus { get { return new CredencialStatusService(); } }

        public ICredencialMotivoService CredencialMotivo { get { return new CredencialMotivoService(); } }

        #endregion

        #region  Metodos

        /// <summary>
        ///     Criar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Criar(VeiculoCredencial entity)
        {
            _repositorio.Criar(entity);
        }

        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VeiculoCredencial BuscarPelaChave(int id)
        {
            return _repositorio.BuscarPelaChave(id);
        }

        /// <summary>
        ///     Listar
        /// </summary>
        /// <param name="objects">Expressão de consulta</param>
        /// <returns></returns>
        public ICollection<VeiculoCredencial> Listar(params object[] objects)
        {
            return _repositorio.Listar(objects);
        }

        /// <summary>
        ///     Alterar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Alterar(VeiculoCredencial entity)
        {
            _repositorio.Alterar(entity);
        }

        /// <summary>
        ///     Deletar registro
        /// </summary>
        /// <param name="entity"></param>
        public void Remover(VeiculoCredencial entity)
        {
            _repositorio.Remover(entity);
        }

        /// <summary>
        /// Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ICollection<VeiculosCredenciaisView> ListarView(params object[] objects)
        {
            return _repositorio.ListarView(objects);
        }

        #endregion


    }
}