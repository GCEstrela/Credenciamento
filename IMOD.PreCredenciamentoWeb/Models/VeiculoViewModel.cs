﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IMOD.Domain.Entities;

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
        [Display(Name = "Identificador")]
        public string PlacaIdentificador { get; set; }
        public string Frota { get; set; }
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
        [Required(ErrorMessage = "O Ano é requerido.")]
        public string Ano { get; set; }
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        [Display(Name = "Municipio")]
        public int MunicipioId { get; set; }
        [Required(ErrorMessage = "A Série/Chassi é requerido.")]
        [Display(Name = "Série/Chassi")]
        public string SerieChassi { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Tipo de Combustível é requerido.")]
        [Display(Name = "Combustível")]
        public int CombustivelId { get; set; }
        [Required(ErrorMessage = "A Altura é requerida.")]
        [Display(Name = "Altura (cm)")]
        public string Altura { get; set; }
        [Required(ErrorMessage = "O Comprimento é requerida.")]
        [Display(Name = "Comprimento (cm)")]
        public string Comprimento { get; set; }
        [Required(ErrorMessage = "A Largura é requerida.")]
        [Display(Name = "Largura (cm)")]
        public string Largura { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Natureza é requerido.")]
        [Display(Name = "Natureza")]
        public int TipoEquipamentoVeiculoId { get; set; }
        public string Renavam { get; set; }
        public string Foto { get; set; }
        public bool Ativo { get; set; }
        public int StatusId { get; set; }
        public int[] idEmpresaContratoVinculados { get; set; }
        public int[] idTipoServicoVinculados { get; set; }
        public MultiSelectList EmpresasContrato { get; set; }

        public string CaminhoArquivo { get; set; }

        [Display(Name = "Documento Anexo")] 
        public HttpPostedFileBase FileUpload { get; set; }

        public string Licenciamento { get; set; }
        [Display(Name = "Veículo")]
        public string RadioVeiculoEquip { get; set; }
        [Display(Name = "Equipamento")]
        public string RadioEquipamento { get; set; }
        [Display(Name = "Vigência")]
        public DateTime? Vigencia { get; set; }
        [Display(Name = "Serviço")]
        public int TipoServicoId { get; set; }
        public IList<TipoServico> Servicos { get; set; }
        [Display(Name = "Contrato Empresa")]
        public string ContratoEmpresaID { get; set; }
    }
}