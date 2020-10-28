using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorEmpresaWebService : IColaboradorEmpresaWebRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

    }
}
