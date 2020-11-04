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
    ///     Tipo de situação
    /// </summary>
    public enum TipoSituacao
    {        
        [Description("1ª Emissão")]
        PRIMEIRA_EMISSAO,
        [Description("Danificado")]
        DANIFICADO,
        [Description("Alteração de Acesso")]
        ALTERACAO_ACESSO,
        [Description("Autorização para Dirigir")]
        AUTORIZACAO_DIRIGIR,
        [Description("Renovação")]
        RENOVACAO,
        [Description("Extraviado")]
        EXTRAVIADO,
        [Description("Alteração de Função")]
        ALTERACAO_FUNCAO,
        [Description("Credencial Temporária")]
        CREDENCIAL_TEMPORARIA,
        [Description("Outros")]
        OUTROS
    }
}
