using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IMOD.Domain.Entities;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class VeiculoSeguroViewModel
    {
        [Key]
        public int VeiculoSeguroId { get; set; }
        [Required(ErrorMessage = "A Seguradora é requerida.")]
        [Display(Name = "Seguradora")]
        public string NomeSeguradora { get; set; }
        [Required(ErrorMessage = "O Número da Apólice é requerido.")]
        [Display(Name = "Número da Apólice")]
        public int NumeroApolice { get; set; }
        [Required(ErrorMessage = "O Valor Da Cobertura é requerido.")]
        [Display(Name = "Valor da Cobertura")]
        public double ValorCobertura { get; set; }
        public int VeiculoId { get; set; }
        [Required(ErrorMessage = "O Arquivo é requerido.")]
        [Display(Name = "Documento Anexo")]
        public HttpPostedFileBase FileUpload { get; set; }
        [Display(Name = "Documento Anexo")]
        public string NomeArquivo { get; set; }
        [Required(ErrorMessage = "A Emissão é requerida.")]
        [Display(Name = "Data Emissão")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Emissao { get; set; }

        [Display(Name = "Data Validade")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Validade { get; set; }
    }
}