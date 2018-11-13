using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
   public class ClasseEmpresasSeguros
    {
        public ObservableCollection<EmpresaSeguro> EmpresasSeguros { get; set; }

        public class EmpresaSeguro
        {
            public int EmpresaSeguroID { get; set; }
            public string NomeSeguradora { get; set; }
            public string NumeroApolice { get; set; }
            public string ValorCobertura { get; set; }
            public DateTime? Emissao { get; set; }
            public DateTime? Validade { get; set; }
            public int EmpresaID { get; set; }
            public string NomeArquivo { get; set; }
            public string Arquivo { get; set; }

            public EmpresaSeguro CriaCopia(EmpresaSeguro _EmpresaSeguro)
            {
                return (EmpresaSeguro)_EmpresaSeguro.MemberwiseClone();
            }


        }

    }
}
