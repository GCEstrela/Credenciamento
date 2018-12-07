using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class VeiculoTipoServicoView
    {
        #region  Propriedades

        public int VeiculoTipoServicoId { get; set; }
        public int VeiculoId { get; set; }
        public int TipoServicoId { get; set; }
        public string Descricao { get; set; }

        #endregion
    }
}
