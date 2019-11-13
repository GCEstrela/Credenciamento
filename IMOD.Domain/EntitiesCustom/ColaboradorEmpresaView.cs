using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class ColaboradorEmpresaView
    {
        public int VeiculoEmpresaId { get; set; }
        public int VeiculoId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }
        public string EmpresaNome { get; set; }
        public string Descricao { get; set; }
        public string NumeroContrato { get; set; }
        public bool Terceirizada { get; set; }
        public string TerceirizadaSigla { get; set; }


    }
}
