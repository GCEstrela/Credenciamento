using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
   public class ClasseClaboradoresPrivilegios
    {
        public ObservableCollection<ColaboradorPrivilegio> ColaboradoresPrivilegios { get; set; }
        public class ColaboradorPrivilegio
        {
            public int ColaboradorPrivilegioID { get; set; }
            public string Descricao { get; set; }

        }
    }
}
