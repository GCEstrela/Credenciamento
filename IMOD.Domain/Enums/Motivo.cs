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
    public enum Inativo
    {
        EXPIRADA = 6,
        DESLIGAMENTO = 8,
        EXTRAVIDA = 9,
        ROUBADA = 10,
        NÃO_EMITIDA = 11,
        INDEFERIDAS = 12,
        CANCELADA = 15
    }

    public enum Ativo
    {
        PRIMEIRA_EMISSÃO = 1,
        SEGUNDA_EMISSÃO = 2,
        TERCEIRA_EMISSÃO = 3,
        RENOVAÇÃO = 4,
        ALTERAÇÃO_DE_ACESSO = 5
    }
}
