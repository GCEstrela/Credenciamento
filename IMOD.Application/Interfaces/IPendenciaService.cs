// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 12 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IPendenciaService : IPendenciaRepositorio
    {
        /// <summary>
        ///     Tipo
        /// </summary>
        ITipoPendenciaService TipoPendenciaService { get; }
    }
}