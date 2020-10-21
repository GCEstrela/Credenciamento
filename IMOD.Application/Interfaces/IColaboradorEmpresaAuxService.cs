using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorEmpresaAuxService : IColaboradorEmpresaAuxRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

    }
}
