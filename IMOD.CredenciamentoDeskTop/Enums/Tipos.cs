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

    /// <summary>
    ///     Tipo Layout Crachá: 1-Credencial e 2-Autorização
    /// </summary>
    public enum TipoLayoutCracha
    {
        [Description("Credencial")] Credencial = 1,
        [Description("Autorização")] Autorizacao = 2
    }

    /// <summary>
    ///     Tipo 1 - Veículo e 2 - Equipamento
    /// </summary>
    public enum TipoVeiculoEquipamento
    {
        [Description("Veiculo")] Veiculo = 1,
        [Description("Equipamento")] Equipamento = 2
    }

    /// <summary>
    ///     Tipo Layout Crachá: 1-Credencial e 2-Autorização
    /// </summary>
    public enum TipoValidade
    {
        [Description("Permanente")] Permanente = 1,
        [Description("Temporária")] Temporária = 2
    }

}