using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

