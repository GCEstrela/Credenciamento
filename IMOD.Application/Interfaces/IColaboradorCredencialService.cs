using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorCredencialService : IColaboradorCredencialRepositorio
    {
        /// <summary>
        ///     TecnologiaCredencial serviços
        /// </summary>
        ITecnologiaCredencialService TecnologiaCredencial { get; }

        /// <summary>
        ///     TipoCredencial serviços
        /// </summary>
        ITipoCredencialService TipoCredencial { get; }

        /// <summary>
        ///     LayoutCracha serviços
        /// </summary>
        ILayoutCrachaService LayoutCracha { get; }

        /// <summary>
        ///     FormatoCredencial serviços
        /// </summary>
        IFormatoCredencialService FormatoCredencial { get; }


        /// <summary>
        ///     CredencialStatus serviços
        /// </summary>
        ICredencialStatusService CredencialStatus { get; }

        /// <summary>
        ///     CredencialMotivo serviços
        /// </summary>
        ICredencialMotivoService CredencialMotivo { get; }


    }
}
