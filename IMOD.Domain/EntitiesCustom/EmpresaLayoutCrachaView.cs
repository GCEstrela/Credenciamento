using System.ComponentModel.DataAnnotations;

namespace IMOD.Domain.EntitiesCustom
{
    public class EmpresaLayoutCrachaView
    {
        #region  Propriedades

        public int EmpresaLayoutCrachaId { get; set; }
        public int EmpresaId { get; set; }
        public int LayoutCrachaId { get; set; }
        //[Required(ErrorMessage = "A Razão Social é requerida.")]
        public string Nome { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public decimal? Valor { get; set; }

        #endregion
    }
}
