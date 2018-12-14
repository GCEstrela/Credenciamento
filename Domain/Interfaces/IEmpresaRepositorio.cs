// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

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



        /// <summary>
        /// Listar Pendencias
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        ICollection<EmpresaPendenciaView> ListarPendencias(int empresaId = 0);
    }
}