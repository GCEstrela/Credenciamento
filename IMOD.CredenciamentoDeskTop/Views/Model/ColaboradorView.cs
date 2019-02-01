﻿// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.ViewModels;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorView: ValidacaoModel
    {
       
        #region  Propriedades

        public int ColaboradorId {get; set; }
        [Required(ErrorMessage = "O Nome é requerido.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Apelido é requerido.")]
        public string Apelido { get; set; }
        public DateTime? DataNascimento { get; set; } 
        public string NomePai { get; set; } 
        public string NomeMae { get; set; } 
        public string Nacionalidade { get; set; } 
        public string Foto { get; set; } 
        public string EstadoCivil { get; set; }
        [Required(ErrorMessage = "O CPF é requerido.")]
        public string Cpf { get; set; } 
        public string Rg { get; set; }
        public DateTime? RgEmissao { get; set; }
        public string RgOrgLocal { get; set; } 
        public string RgOrgUf { get; set; }
        public string Passaporte { get; set; }
        public DateTime? PassaporteValidade { get; set; }
        public string Rne { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        public string ContatoEmergencia { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public bool Ativo { get; set; }
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public bool Motorista { get; set; }
        public string CnhCategoria { get; set; }
        public string Cnh { get; set; }
        public DateTime? CnhValidade { get; set; }
        public string CnhEmissor { get; set; }
        public string Cnhuf { get; set; }
        public string Bagagem { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        

        #endregion
        
    }
}