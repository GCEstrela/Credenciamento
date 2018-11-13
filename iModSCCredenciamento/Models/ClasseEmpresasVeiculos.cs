using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.ViewModels
{
    public class ClasseEmpresasVeiculos
    {
        public ObservableCollection<EmpresaVeiculo> EmpresasVeiculos { get; set; }

        public class EmpresaVeiculo
        {


            public int EmpresaVeiculoID { get; set; }
            public string Validade { get; set; }
            public string Descricao { get; set; }
            public string Tipo { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Ano { get; set; }
            public string Cor { get; set; }
            public string Placa { get; set; }
            public string Renavam { get; set; }
            public int EstadoID { get; set; }
            public int MunicipioID { get; set; }
            public string Seguro { get; set; }
            public int EmpresaID { get; set; }
            public bool Ativo { get; set; }
            public int LayoutCrachaID { get; set; }
            public string LayoutCrachaNome { get; set; }
            public int FormatoCredencialID { get; set; }
            public string NumeroCredencial { get; set; }
            public string FC { get; set; }

            public EmpresaVeiculo CriaCopia(EmpresaVeiculo empresasVeiculos)
            {
                return (EmpresaVeiculo)empresasVeiculos.MemberwiseClone();
            }
        }
    }
}
