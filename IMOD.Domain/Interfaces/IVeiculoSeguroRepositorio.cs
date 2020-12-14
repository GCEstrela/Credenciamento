// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;
using System.Collections.Generic;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IVeiculoSeguroRepositorio : IRepositorioBaseAdoNet<VeiculoSeguro>
    {
        ICollection<VeiculoSeguro> ListarComAnexo(params object[] objects);
    }
}