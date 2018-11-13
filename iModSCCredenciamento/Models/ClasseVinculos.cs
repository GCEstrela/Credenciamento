using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseVinculos
    {
        public ObservableCollection<Vinculo> Vinculos { get; set; }

        public class Vinculo
        {
            public int VinculoID { get; set; }
            public int ColaboradorID { get; set; }
            public string ColaboradorNome { get; set; }
            public string ColaboradorApelido { get; set; }
            public bool Motorista { get; set; }
            public string CPF { get; set; }
            public string ColaboradorFoto { get; set; }
            public int EmpresaID { get; set; }
            public string EmpresaNome { get; set; }
            public string EmpresaApelido { get; set; }
            public string CNPJ { get; set; }
            public string Cargo { get; set; }
            public string LayoutCrachaGUID { get; set; }
            public string FormatIDGUID { get; set; }
            public string NumeroCredencial { get; set; }
            public int FC { get; set; }
            public DateTime? Validade { get; set; }
            public Guid CredencialGuid { get; set; }
            public Guid CardHolderGuid { get; set; }

            public Vinculo CriaCopia(Vinculo _Vinculo)
            {
                return (Vinculo)_Vinculo.MemberwiseClone();
            }


        }

    }
}
