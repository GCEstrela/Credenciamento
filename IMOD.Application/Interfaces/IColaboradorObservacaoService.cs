using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IColaboradorObservacaoService : IColaboradorObservacaoRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

    }
}
