using System.Collections.ObjectModel;

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
