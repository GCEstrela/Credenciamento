using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculosSeguros
    {
        public ObservableCollection<VeiculoSeguro> VeiculosSeguros { get; set; }

        public class VeiculoSeguro
        {
            public int VeiculoSeguroID { get; set; }
            public string NomeSeguradora { get; set; }
            public string NumeroApolice { get; set; }
            public string ValorCobertura { get; set; }
            public DateTime? Emissao { get; set; }
            public DateTime? Validade { get; set; }
            public int VeiculoID { get; set; }
            public string NomeArquivo { get; set; }
            public string Arquivo { get; set; }

            public VeiculoSeguro CriaCopia(VeiculoSeguro _VeiculoSeguro)
            {
                return (VeiculoSeguro)_VeiculoSeguro.MemberwiseClone();
            }


        }
    }
}
