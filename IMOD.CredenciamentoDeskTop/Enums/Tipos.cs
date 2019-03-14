// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 18 - 2018
// ***********************************************************************

#region

using System.ComponentModel;

#endregion

namespace IMOD.CredenciamentoDeskTop.Enums
{
    /// <summary>
    ///     Tipo de pendencia
    /// </summary>
    public enum PendenciaTipo
    {
        [Description("Empresa")] Empresa,
        [Description("Veiculo")] Veiculo,
        [Description("Colaborador")] Colaborador
    }

    /// <summary>
    ///     Devolução Credencial
    /// </summary>
    public enum DevoluçãoCredencial
    {
        [Description("Devolução")] Devolucao = 1,
        [Description("Entrega BO")] EntregaBO = 2
    }
}