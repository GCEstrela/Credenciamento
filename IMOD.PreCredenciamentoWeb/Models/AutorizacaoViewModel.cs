using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class AutorizacaoViewModel
    {

        public int VeiculoCredencialId { get; set; }
        public string EmpresaNome { get; set; } 
        public string Descricao { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string PlacaIdentificador { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string Categoria { get; set; }
        public string Frota { get; set; }

    }
}
