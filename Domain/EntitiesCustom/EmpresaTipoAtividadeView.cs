using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class EmpresaTipoAtividadeView
    {
        #region  Propriedades

        public int EmpresaTipoAtividadeId { get; set; }
        public int EmpresaId { get; set; }
        public int TipoAtividadeId { get; set; }
        public string Descricao { get; set; }

        #endregion
    }
}
