using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMOD.Domain.Interfaces;

namespace IMOD.Application.Interfaces
{
    public interface IVeiculoAnexoWebService : IVeiculoAnexoWebRepositorio
    {
        /// <summary>
        ///     Pendência serviços
        /// </summary>
        IPendenciaService Pendencia { get; }
    }
}
