using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Views.Model
{

    public class EmpresaTipoCredencialView

    {

        #region  Propriedades

        public int ColaboradorCredencialId { get; set; }
        public int ColaboradorEmpresaId { get; set; }
        public int TipoCredencialId { get; set; }
        public bool Ativa { get; set; }
        public int EmpresaId { get; set; }
        public int Colaborador { get; set; }
        public string Descricao { get; set; }

        #endregion

    }
}
