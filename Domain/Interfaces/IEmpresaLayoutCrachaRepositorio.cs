// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEmpresaLayoutCrachaRepositorio //: IRepositorioBaseAdoNet<EmpresaLayoutCracha>
    {
        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        EmpresaLayoutCracha BuscarPelaChave(int id);

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<EmpresaLayoutCracha> Listar(params object[] objects);
    }
}