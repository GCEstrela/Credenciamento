using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseTiposCredenciais
    {
        public ObservableCollection<TipoCredencial> TiposCredenciais { get; set; }
        public class TipoCredencial
        {
            public int TipoCredencialID { get; set; }
            public string Descricao { get; set; }

        }
    }
}
