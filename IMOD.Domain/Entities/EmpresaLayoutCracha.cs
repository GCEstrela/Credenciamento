// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class EmpresaLayoutCracha
    {
        #region  Propriedades

        public int EmpresaLayoutCrachaId { get; set; }
        public int EmpresaId { get; set; }
        public int? LayoutCrachaId { get; set; }
        public string Nome { get; set; }
        public string Modelo { get; set; }

        #endregion


        public override bool Equals(object obj)
        {
            return obj is EmpresaLayoutCracha cracha &&
                   EmpresaLayoutCrachaId == cracha.EmpresaLayoutCrachaId;
        }

        public override int GetHashCode()
        {
            return -1878893795 + EmpresaLayoutCrachaId.GetHashCode();
        }


    }
}