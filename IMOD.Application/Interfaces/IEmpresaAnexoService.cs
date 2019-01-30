using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaAnexoService : IEmpresaAnexoRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}
