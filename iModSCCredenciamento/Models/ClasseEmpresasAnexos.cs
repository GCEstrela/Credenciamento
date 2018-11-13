using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasAnexos
    {
        public ObservableCollection<EmpresaAnexo> EmpresasAnexos { get; set; }

        public class EmpresaAnexo
        {
            public int EmpresaAnexoID { get; set; }
            public int EmpresaID { get; set; }
            public string Descricao { get; set; }
            public string NomeAnexo { get; set; }
            public string Anexo { get; set; }

            public EmpresaAnexo CriaCopia(EmpresaAnexo _EmpresasAnexos)
            {
                return (EmpresaAnexo)_EmpresasAnexos.MemberwiseClone();
            }


        }
    }
}
