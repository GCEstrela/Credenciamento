using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposPendencias
    {
        public ObservableCollection<TipoPendencia> TiposPendencias { get; set; }
        public class TipoPendencia
        {
            public int TipoPendenciaID { get; set; }
            public string Tipo { get; set; }
        }
    }
}
