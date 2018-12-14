using System.Collections.ObjectModel;

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
