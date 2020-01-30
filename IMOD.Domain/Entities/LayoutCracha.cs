// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class LayoutCracha
    {
     

        #region  Propriedades

        public int LayoutCrachaId { get; set; }
        public string Nome { get; set; }
        public string Modelo { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public decimal Valor { get; set; }
        public string LayoutRpt { get; set; }
        public int TipoCracha { get; set; }
        public int TipoValidade { get; set; }
        public string Cor { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LayoutCracha cracha &&
                   LayoutCrachaId == cracha.LayoutCrachaId;
        }

        public override int GetHashCode()
        {
            return -1486030884 + LayoutCrachaId.GetHashCode();
        }
        #endregion



    }
}