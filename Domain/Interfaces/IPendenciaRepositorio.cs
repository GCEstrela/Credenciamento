// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IPendenciaRepositorio : IRepositorioBaseAdoNet<Pendencia>
    {

        /// <summary>
        ///     Buscar por EmpresaID
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        Pendencia BuscarPorEmpresa(int id);
    }
}