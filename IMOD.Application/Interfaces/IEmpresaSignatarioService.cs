using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IEmpresaSignatarioService : IEmpresaSignatarioRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}
