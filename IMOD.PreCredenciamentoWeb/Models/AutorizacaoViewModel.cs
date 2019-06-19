using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class AutorizacaoViewModel
    {

        public int VeiculoCredencialId { get; set; }
        [Display(Name = "Empresa Nome")]
        public string EmpresaNome { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Emissão")]
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        [Display(Name = "Placa / Identificador")]
        public string PlacaIdentificador { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        [Display(Name = "Área Acesso 1")]
        public string Identificacao1 { get; set; }
        [Display(Name = "Área Acesso 2")]
        public string Identificacao2 { get; set; }
        public string Categoria { get; set; }
        public string Frota { get; set; }
        [Display(Name = "Status")]
        public bool Ativa { get; set; }
        public string Foto { get; set; }
        public string FotoFormatada { get { return string.Format("data:image/png;base64,{0}", Foto); } }
        

    }
}
