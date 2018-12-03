using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.Entities
{
    public class EmpresaAnexo
    {
        #region  Propriedades

        public int EmpresaAnexoId { get; set; }
        public int? EmpresaId { get; set; }
        public string Descricao { get; set; }
        public string NomeAnexo { get; set; }
        public string Anexo { get; set; }

        #endregion
    }
}
