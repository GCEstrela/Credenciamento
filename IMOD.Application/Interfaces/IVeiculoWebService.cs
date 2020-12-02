using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IVeiculoWebService : IVeiculoWebRepositorio
    {

        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }

        /// <summary>
        /// Serviços de credenciais
        /// </summary>
        IVeiculoCredencialService Credencial { get; }

        /// <summary>
        /// Serviços de Empresas
        /// Author: Mihai
        /// </summary>
        IVeiculoempresaWebService Empresa { get; }

        /// <summary>
        /// Serviços de Seguros
        /// </summary>
        IVeiculoSeguroWebService Seguro { get; }

        /// <summary>
        /// Serviços de Anexos
        /// </summary>
        IVeiculoAnexoWebService Anexo { get; }

        /// <summary>
        /// Serviços Equipamentos
        /// </summary>
        IEquipamentoVeiculoTipoServicoService Equipamento { get; }


    }
}
