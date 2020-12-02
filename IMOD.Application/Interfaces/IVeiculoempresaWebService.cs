using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IVeiculoempresaWebService : IVeiculoEmpresaWebRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}
