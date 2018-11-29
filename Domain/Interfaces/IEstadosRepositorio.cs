// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEstadosRepositorio //: IRepositorioBaseAdoNet<Estados>
    {
        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        Estados BuscarPelaChave(int id);

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<Estados> Listar(params object[] objects);
    }
}