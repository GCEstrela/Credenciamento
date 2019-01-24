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
}