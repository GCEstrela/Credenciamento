using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseTecnologiasCredenciais
    {
        public ObservableCollection<TecnologiaCredencial> TecnologiasCredenciais { get; set; }
        public class TecnologiaCredencial
        {
            public int TecnologiaCredencialID { get; set; }
            public string Descricao { get; set; }

        }
    }
}
