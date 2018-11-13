using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseEmpresasSignatarios
    {
        public ObservableCollection<EmpresaSignatario> EmpresasSignatarios { get; set; }

        public class EmpresaSignatario
        {
            public int EmpresaSignatarioID { get; set; }
            public int EmpresaId { get; set; }
            public string Nome { get; set; }
            public string CPF { get; set; }
            public string Email { get; set; }
            public string Telefone { get; set; }
            public string Celular { get; set; }
            public bool Principal { get; set; }
            public string Assinatura { get; set; }

            public EmpresaSignatario CriaCopia(EmpresaSignatario _EmpresaSegnatario)
            {
                return (EmpresaSignatario)_EmpresaSegnatario.MemberwiseClone();
            }
        }
    }
}
