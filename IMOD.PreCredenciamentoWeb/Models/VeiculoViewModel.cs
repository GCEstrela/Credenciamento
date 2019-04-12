using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class VeiculoViewModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "A Definição é requerida.")]
        public int EquipamentoVeiculoId { get; set; }
        [Required(ErrorMessage = "A Descrição é requerida.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A Placa/Identificador é requerida.")]
        [Display(Name = "Placa/Identificador")]
        public string PlacaIdentificador { get; set; }
        [Required(ErrorMessage = "A Frota é requerida.")]
        public string Frota { get; set; }
        [Required(ErrorMessage = "O Patrimonio é requerida.")]
        [Display(Name = "Patrimônio")]
        public string Patrimonio { get; set; }
        [Required(ErrorMessage = "A Marca é requerida.")]
        public string Marca { get; set; }
        [Required(ErrorMessage = "O Modelo é requerida.")]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "O Tipo é requerido.")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "A Cor é requerida.")]
        public string Cor { get; set; }
        [Required(ErrorMessage = "O Ano é requerida.")]
        public string Ano { get; set; }
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        [Display(Name = "Municipio")]
        public int MunicipioId { get; set; }
        [Required(ErrorMessage = "A Série/Chassi é requerido.")]
        [Display(Name = "Série/Chassi")]
        public string SerieChassi { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Tipo de Combustível é requerido.")]
        public int CombustivelId { get; set; }
        [Required(ErrorMessage = "A Altura é requerida.")]
        public string Altura { get; set; }
        [Required(ErrorMessage = "O Comprimento é requerida.")]
        public string Comprimento { get; set; }
        [Required(ErrorMessage = "A Largura é requerida.")]
        public string Largura { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Natureza do Equipamento/Veículo é requerido.")]
        [Display(Name = "Natureza")]
        public int TipoEquipamentoVeiculoId { get; set; }
        public string Renavam { get; set; }
        public string Foto { get; set; }
        public bool Ativo { get; set; }
        public int StatusId { get; set; }
        public int TipoAcessoId { get; set; }
        public string DescricaoAnexo { get; set; }
        public string NomeArquivoAnexo { get; set; }
        public string ArquivoAnexo { get; set; }
        public bool Pendente31 { get; set; }
        public bool Pendente32 { get; set; }
        public bool Pendente33 { get; set; }
        public bool Pendente34 { get; set; }
        public string DescricaoAlias { get { return Descricao; } }


    }
}