// ***********************************************************************
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
    public class ColaboradorView : ValidacaoModel, ICloneable
    {

        #region  Propriedades

        public int ColaboradorId { get; set; }
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
        //[Required(ErrorMessage = "O CPF é requerido.")]
        [RequiredIf("Estrangeiro", false, ErrorMessage = "O CPF é requerido.")]
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime? RgEmissao { get; set; }
        public string RgOrgLocal { get; set; }
        public string RgOrgUf { get; set; }
        [RequiredIf("ValidaEstrangeiro", true, ErrorMessage = "O passaporte ou RNE são obrigatórios para estrangeiros.")]
        public string Passaporte { get; set; }
        [RequiredIf("ValidaEstrangeiro", true, ErrorMessage = "O passaporte ou RNE são obrigatórios para estrangeiros.")]
        public DateTime? PassaporteValidade { get; set; }
        [RequiredIf("ValidaEstrangeiro", true, ErrorMessage = "O passaporte ou RNE são obrigatórios para estrangeiros.")]
        public string Rne { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "O Contato de Emergencia é requerido.")]
        public string ContatoEmergencia { get; set; }
        [Required(ErrorMessage = "O Telefone de Emergencia é requerido.")]
        public string TelefoneEmergencia { get; set; }
        [Required(ErrorMessage = "O Cep é requerido.")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O Endereco é requerido.")]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "O Numero é requerido.")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        [Required(ErrorMessage = "O Numero é requerido.")]
        public string Bairro { get; set; }
        public bool Ativo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Município é requerido.")]
        public int MunicipioId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Estado é requerido.")]
        public int EstadoId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Motorista é requerido.")]
        public bool Motorista { get; set; }
        [RequiredIf("Motorista", true, ErrorMessage = "Categoria da CNH é obrigatória para motorista.")]
        public string CnhCategoria { get; set; }
        [RequiredIf("Motorista", true, ErrorMessage = "Número da CNH é obrigatório para motorista.")]
        public string Cnh { get; set; }
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        [RequiredIf("Motorista", true, ErrorMessage = "Validade da CNH é obrigatório para motorista.")]
        public DateTime? CnhValidade { get; set; }

        public string CnhEmissor { get; set; }
        public string Cnhuf { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }

        public bool Estrangeiro { get; set; }

        public bool ValidaEstrangeiro
        {
            get
            {
                return (Estrangeiro == true && (string.IsNullOrEmpty(Rne) & !PassaportePreenchido()));
            }
        }
        //public bool Pendente21 { get; set; }
        //public bool Pendente22 { get; set; }
        //public bool Pendente23 { get; set; }
        //public bool Pendente24 { get; set; }
        //public bool Pendente25 { get; set; }

        private bool PassaportePreenchido()
        {
            return (!string.IsNullOrEmpty(Passaporte) && PassaporteValidade != null);
        }
        public bool Policiafederal { get; set; }
        public bool Receitafederal { get; set; }
        public bool Segurancatrabalho { get; set; }

        #endregion

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return (ColaboradorView)MemberwiseClone();
        }
    }





}