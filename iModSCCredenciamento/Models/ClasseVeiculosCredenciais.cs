using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseVeiculosCredenciais
    {
        public ObservableCollection<VeiculoCredencial> VeiculosCredenciais { get; set; }

        public class VeiculoCredencial
        {

            public int VeiculoCredencialID { get; set; }
            public int VeiculoEmpresaID { get; set; }
            public int VeiculoID { get; set; }
            //public int EmpresaID { get; set; }
            //public int EmpresaContratoID { get; set; }
            public string ContratoDescricao { get; set; }
            public string EmpresaNome { get; set; }
            public string VeiculoNome { get; set; }
            public int TecnologiaCredencialID { get; set; }
            public string TecnologiaCredencialDescricao { get; set; }
            public int TipoCredencialID { get; set; }
            public string TipoCredencialDescricao { get; set; }
            public int LayoutCrachaID { get; set; }
            public string LayoutCrachaNome { get; set; }
            public int FormatoCredencialID { get; set; }
            public string FormatoCredencialDescricao { get; set; }
            public string NumeroCredencial { get; set; }
            public int FC { get; set; }
            public DateTime? Emissao { get; set; }
            public DateTime? Validade { get; set; }
            public int CredencialStatusID { get; set; }
            public string CredencialStatusDescricao { get; set; }
            public Guid? CredencialGuid { get; set; }
            public Guid? CardHolderGuid { get; set; }


            public string Placa { get; set; }
            public string VeiculoFoto { get; set; }
            public int EmpresaID { get; set; }
            public string EmpresaApelido { get; set; }
            public string CNPJ { get; set; }
            public string Cargo { get; set; }
            public string LayoutCrachaGUID { get; set; }
            public string FormatIDGUID { get; set; }

            public VeiculoCredencial CriaCopia(VeiculoCredencial _VeiculosCredenciais)
            {
                return (VeiculoCredencial)_VeiculosCredenciais.MemberwiseClone();
            }


        }
    }
}
