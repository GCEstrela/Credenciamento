using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseMunicipios
    {
        public ObservableCollection<Municipio> Municipios { get; set; }
        public class Municipio
        {
            public int MunicipioID { get; set; }
            public string Nome { get; set; }
            public string UF { get; set; }
        }
    }
}
