using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculosAnexos
    {
        public ObservableCollection<VeiculoAnexo> VeiculosAnexos { get; set; }
        public class VeiculoAnexo
        {

            public int VeiculoAnexoID { get; set; }
            public int VeiculoID { get; set; }
            public string Descricao { get; set; }
            public string NomeArquivo { get; set; }
            public string Arquivo { get; set; }

            public VeiculoAnexo CriaCopia(VeiculoAnexo _VeiculosAnexos)
            {
                return (VeiculoAnexo)_VeiculosAnexos.MemberwiseClone();
            }
        }
    }
}
