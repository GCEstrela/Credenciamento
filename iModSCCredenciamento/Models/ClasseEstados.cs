using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseEstados
    {
        public ObservableCollection<Estado> Estados { get; set; }
        public class Estado
        {
            public int EstadoID { get; set; }
            public string Nome { get; set; }
            public string UF { get; set; }

        }
    }
}
