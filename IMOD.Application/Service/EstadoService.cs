// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
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
    public class EstadoService : IEstadoService
    {
        private readonly IEstadosRepositorio _repositorio = new EstadoRepositorio();

        #region  Metodos

        /// <summary>
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        public ICollection<Estados> Listar()
        {
            return _repositorio.Listar();
        }

        /// <summary>
        ///     Buscar Estado por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public Estados BuscarEstadoPorUf(string uf)
        {
            return _repositorio.BuscarEstadoPorUf (uf);
        }

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public EstadoView BuscarEstadoMunicipiosPorUf(string uf)
        {
            return _repositorio.BuscarEstadoMunicipiosPorUf (uf);
        }

        #endregion
    }
}