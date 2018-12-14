using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasLayoutsCrachas
    {
        public ObservableCollection<EmpresaLayoutCracha> EmpresasLayoutsCrachas { get; set; }
        public class EmpresaLayoutCracha
        {
            public int EmpresaLayoutCrachaID { get; set; }
            public int EmpresaID { get; set; }
            public int LayoutCrachaID { get; set; }
            //public string LayoutCrachaGUID { get; set; }
            public string Nome { get; set; }

            public EmpresaLayoutCracha CriaCopia(EmpresaLayoutCracha empresaLayoutsCrachas)
            {
                return (EmpresaLayoutCracha)empresaLayoutsCrachas.MemberwiseClone();
            }

        }
    }
}

