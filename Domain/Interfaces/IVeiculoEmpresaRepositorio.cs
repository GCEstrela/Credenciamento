// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using System.Collections.Generic;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IVeiculoEmpresaRepositorio : IRepositorioBaseAdoNet<VeiculoEmpresa>
    {
        /// <summary>
        ///    Listar Veículos e seus contratos
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<VeiculoEmpresaView> ListarContratoView(params object[] o);

    }
}