using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculosEmpresas
    {
        public ObservableCollection<VeiculoEmpresa> VeiculosEmpresas { get; set; }

        public class VeiculoEmpresa
        {
            public int VeiculoEmpresaID { get; set; }
            public int VeiculoID { get; set; }
            public int EmpresaID { get; set; }
            public int EmpresaContratoID { get; set; }
            public string Descricao { get; set; }
            public string EmpresaNome { get; set; }
            public string Cargo { get; set; }
            public string Matricula { get; set; }
            public bool Ativo { get; set; }

            public VeiculoEmpresa CriaCopia(VeiculoEmpresa _VeiculosEmpresas)
            {
                return (VeiculoEmpresa)_VeiculosEmpresas.MemberwiseClone();
            }


        }
    }
}
