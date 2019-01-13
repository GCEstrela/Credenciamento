using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IVeiculoService : IVeiculoRepositorio
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
        IVeiculoEmpresaService Empresa { get; }

        /// <summary>
        /// Serviços de Seguros
        /// </summary>
        IVeiculoSeguroService Seguro { get; }

        /// <summary>
        /// Serviços de Anexos
        /// </summary>
        IVeiculoAnexoService Anexo { get; }

        /// <summary>
        /// Serviços Equipamentos
        /// </summary>
        IEquipamentoVeiculoTipoServicoService Equipamento { get; }


    }
}
