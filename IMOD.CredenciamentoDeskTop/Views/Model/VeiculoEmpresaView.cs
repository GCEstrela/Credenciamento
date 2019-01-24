// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class VeiculoEmpresaView
    {
        #region  Propriedades

        public int VeiculoEmpresaId { get; set; }
        public int VeiculoId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }

        #endregion
    }
}