// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEmpresaRepositorio : IRepositorioBaseAdoNet<Empresa>
    {
        /// <summary>
        /// Buscar empresa por CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        Empresa BuscarEmpresaPorCnpj(string cnpj);
    }
}