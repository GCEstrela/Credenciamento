using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IMOD.Domain.Entities;
using System.Text.RegularExpressions;
using System.Globalization;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class VeiculoSeguroViewModel
    {
        [Key]
        public int VeiculoSeguroId { get; set; }
        public int VeiculoSeguroWebId { get; set; }
        [Required(ErrorMessage = "A Seguradora é requerida.")]
        [Display(Name = "Seguradora")]
        public string NomeSeguradora { get; set; }
        [Required(ErrorMessage = "O Número da Apólice é requerido.")]
        [Display(Name = "Número da Apólice")]
        public string NumeroApolice { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ValorCobertura { get; set; }
        [Required(ErrorMessage = "O Valor Da Cobertura é requerido.")]
        [Display(Name = "Valor da Cobertura")]
        public string ValorCoberturaMask {
            get { return ValorCobertura.ToString("0.00", CultureInfo.InvariantCulture); }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _valorcoberturamask = Regex.Replace(value, "\\.", "");
                    _valorcoberturamask = Regex.Replace(_valorcoberturamask, ",", ".");

                    ValorCobertura = Double.Parse(_valorcoberturamask, CultureInfo.InvariantCulture);
                }
            } 
        }
        private string _valorcoberturamask;
        public int VeiculoId { get; set; }
        public string Arquivo { get; set; }
        [Display(Name = "Apólice Digitalizada")]
        public string NomeArquivo { get; set; }
        [Required(ErrorMessage = "A Emissão é requerida.")]
        [Display(Name = "Data Emissão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Emissao { get; set; }

        [Display(Name = "Data Validade")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Validade { get; set; }
        public int EmpresaSeguroId { get; set; }
        [Display(Name = "Selecionar Apólice")]
        public HttpPostedFileBase SeguroArquivo { get; set; }
    }
}