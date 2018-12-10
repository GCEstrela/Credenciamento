using System.Collections.ObjectModel;

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
