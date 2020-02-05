using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("Aguardando Aprovação Inclusão")]
        AGUARDANDO_APROVACAO_INCLUSAO,
        [Description("Aguardando Revisçao")]
        AGUARDANDO_REVISAO,
        [Description("Aguardando Aprovação Revisão")]
        AGUARDANDO_APROVACAO_REVISAO,
        [Description("Aprovado")]
        APROVADO
    }

}
