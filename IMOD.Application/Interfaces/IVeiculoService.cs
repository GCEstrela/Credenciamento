using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IVeiculoService : IVeiculoRepositorio
    {
        /// <summary>
        /// Seguros
        /// </summary>
        IVeiculoSeguroService Seguro { get; }

        /// <summary>
        /// Anexos
        /// </summary>
        IVeiculoAnexoService Anexo { get; }

        /// <summary>
        /// Veiculos
        /// </summary>
        IVeiculoempresaService Veiculo { get; }
    }
}
