using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.Enums
{
    /// <summary>
    ///     Tipo de pendencia
    /// </summary>
    public enum StatusCadastro
    {        
        AGUARDANDO_APROVACAO_INCLUSAO,
        AGUARDANDO_REVISAO,
        AGUARDANDO_APROVACAO_REVISAO,
        APROVADO
    }

}
