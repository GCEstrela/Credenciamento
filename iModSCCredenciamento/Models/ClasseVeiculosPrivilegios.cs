using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculosPrivilegios
    {
        public ObservableCollection<VeiculoPrivilegio> VeiculosPrivilegios { get; set; }
        public class VeiculoPrivilegio
        {
            public int VeiculoPrivilegioID { get; set; }
            public string Descricao { get; set; }

        }
    }
}
