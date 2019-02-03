// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class VeiculoAnexo
    {
        #region  Propriedades

        public int VeiculoAnexoId { get; set; }
        public int VeiculoId { get; set; }
        public string Descricao { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }

        #endregion
    }
}