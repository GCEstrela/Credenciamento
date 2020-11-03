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
    ///     Tipo de revisão do usuário
    /// </summary>
    public enum UsuarioRevisao
    {        
        [Description("Cadastro Web")]
        CADASTRO_WEB,
        [Description("Credenciamento")]
        CREDENCIAMENTO,
        [Description("Colaborador")]
        COLABORADOR
    }
}
