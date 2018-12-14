using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseColaboradoresEmpresas
    {
        public ObservableCollection<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }

        public class ColaboradorEmpresa
        {
            public int ColaboradorEmpresaID { get; set; }
            public int ColaboradorID { get; set; }
            public int EmpresaID { get; set; }
            public int EmpresaContratoID { get; set; }
            public string Descricao { get; set; }
            public string EmpresaNome { get; set; }
            public string Cargo { get; set; }
            public string Matricula { get; set; }
            public bool Ativo { get; set; }

            public ColaboradorEmpresa CriaCopia(ColaboradorEmpresa _ColaboradoresEmpresas)
            {
                return (ColaboradorEmpresa)_ColaboradoresEmpresas.MemberwiseClone();
            }


        }

    }
}
