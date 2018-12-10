using System.Collections.ObjectModel;

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
