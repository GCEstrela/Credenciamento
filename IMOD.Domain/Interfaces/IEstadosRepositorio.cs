// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEstadosRepositorio
    {
        #region  Metodos

        /// <summary>
        ///     Listar Estados
        /// </summary>
        /// <returns></returns>
        ICollection<Estados> Listar();

        /// <summary>
        ///     Buscar Estado por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        Estados BuscarEstadoPorUf(string uf);

        /// <summary>
        ///     Buscar municipios por UF
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        EstadoView BuscarEstadoMunicipiosPorUf(string uf);

        #endregion
    }
}