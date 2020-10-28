// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorAnexoWebService : IColaboradorAnexoWebRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}