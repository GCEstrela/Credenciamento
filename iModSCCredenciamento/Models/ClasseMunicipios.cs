using System.Collections.ObjectModel;

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
