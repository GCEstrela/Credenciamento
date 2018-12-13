// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class VeiculoEquipTipoServico
    {
        #region  Propriedades

        public int EquipamentoVeiculoTipoServicoId { get; set; }
        public int EquipamentoVeiculoId { get; set; }
        public int? TipoServicoId { get; set; }
        public string Descricao { get; set; }

        #endregion
    }
}