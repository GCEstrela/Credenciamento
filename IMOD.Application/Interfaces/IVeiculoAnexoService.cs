using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IVeiculoAnexoService : IVeiculoAnexoRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}
