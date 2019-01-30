using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorEmpresaService : IColaboradorEmpresaRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}
