using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class RelatorioColaboradorEmpresaView
    {
        public int ColaboradorEmpresaId { get; set; }
        public int EmpresaId { get; set; }
        public string EmpresaNome { get; set; }
        public string CNPJ { get; set; }
        public bool Ativo { get; set; }
        public DateTime Validade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int ColaboradorId { get; set; }
        public string ColaboradorNome { get; set; }
        public string CPF { get; set; }
        public string Foto { get; set; }
        public string TelefoneCelular { get; set; }
        public string CNH { get; set; }
        public string CNHCategoria { get; set; }
        public DateTime CNHValidade { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }

    }
}
