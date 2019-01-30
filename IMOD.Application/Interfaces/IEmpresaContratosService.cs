// ***********************************************************************
// Project: IMOD.Application
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Interfaces;

#endregion

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaContratosService : IEmpresaContratoRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

        /// <summary>
        ///     AreaAceesso serviços
        /// </summary>
        IAreaAcessoService AreaAceesso { get; }

        /// <summary>
        ///     TipoAcesso serviços
        /// </summary>
        ITiposAcessoService TipoAcesso { get; }

        /// <summary>
        ///     Status serviços
        /// </summary>
        IStatusService Status { get; }

        /// <summary>
        ///     TipoCobranca serviços
        /// </summary>
        ITipoCobrancaService TipoCobranca { get; }

        /// <summary>
        ///     Estado serviços
        /// </summary>
        IEstadoService Estado { get; }

        /// <summary>
        ///     Municipio serviços
        /// </summary>
        IMunicipioService Municipio { get; }
    }
}