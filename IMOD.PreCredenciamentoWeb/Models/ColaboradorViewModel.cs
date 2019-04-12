﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class ColaboradorViewModel
    {
        #region  Propriedades

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ColaboradorId { get; set; }
        [Required(ErrorMessage = "O Nome é requerido.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Apelido é requerido.")]
        [Display(Name = "Apelido")]
        public string Apelido { get; set; }
        [Display(Name = "Data Nascimento")]
        public DateTime? DataNascimento { get; set; }
        [Display(Name = "Nome Pai")]
        public string NomePai { get; set; }
        [Display(Name = "Nome Mãe")] 
        public string NomeMae { get; set; }
        public string Nacionalidade { get; set; }
        public string Foto { get; set; }
        [Display(Name = "Estado Civil")]
        public string EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        [Display(Name = "Emissão RG")]
        public DateTime? RgEmissao { get; set; }
        [Display(Name = "Órgão Emissor RG")]
        public string RgOrgLocal { get; set; }
        [Display(Name = "UF RG")]
        public string RgOrgUf { get; set; }
        public string Passaporte { get; set; }
        [Display(Name = "Validade Passaporte")]
        public DateTime? PassaporteValidade { get; set; }
        public string Rne { get; set; }
        [Display(Name = "Telefone Fixo")]
        public string TelefoneFixo { get; set; }
        [Display(Name = "Telefone Celular")]
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "O Contato de Emergencia é requerido.")]
        [Display(Name = "Contato Emergência")]
        public string ContatoEmergencia { get; set; }
        [Required(ErrorMessage = "O Telefone de Emergencia é requerido.")]
        [Display(Name = "Telefone Emergência")]
        public string TelefoneEmergencia { get; set; }
        [Required(ErrorMessage = "O Cep é requerido.")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O Endereco é requerido.")]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "O Número é requerido.")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        [Required(ErrorMessage = "O Bairro é requerido.")]
        public string Bairro { get; set; }
        public bool Ativo { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "O Município é requerido.")]
        [Display(Name = "Municipio")]
        public int MunicipioId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Estado é requerido.")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Motorista é requerido.")]
        public bool Motorista { get; set; }
        [Display(Name = "Categoria Habilitação")]
        public string CnhCategoria { get; set; }
        [Display(Name = "Número Habilitação")]
        public string Cnh { get; set; }
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        [Display(Name = "Validade Habilitação")]
        public DateTime? CnhValidade { get; set; }

        public string CnhEmissor { get; set; }
        public string Cnhuf { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }

        public bool Estrangeiro { get; set; }

       //public IEnumerable<Estados> Estados { get; set; }

        #endregion
    }
}