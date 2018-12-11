using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class ColaboradorEmpresaView
    {
        public int VeiculoEmpresaID { get; set; }
        public int VeiculoID { get; set; }
        public int EmpresaID { get; set; }
        public int EmpresaContratoID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }

    }
}
