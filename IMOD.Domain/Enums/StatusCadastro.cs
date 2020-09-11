using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
        [Description("Aguardando Revisão")]
        AGUARDANDO_REVISAO,
        [Description("Aguardando Aprovação")]
        AGUARDANDO_APROVACAO,
        [Description("Aprovado")]
        APROVADO
    }

    public static class Funcoes {
        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }
    }
}
