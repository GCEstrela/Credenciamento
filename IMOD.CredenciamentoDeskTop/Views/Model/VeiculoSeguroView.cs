// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using IMOD.CredenciamentoDeskTop.Funcoes;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class VeiculoSeguroView : ValidacaoModel
    {
        #region  Propriedades

        public int VeiculoSeguroId { get; set; }
        [Required(ErrorMessage = "O Nome da Seguradora é requerido.")]
        public string NomeSeguradora { get; set; }
        [Required(ErrorMessage = "O Nùmero da Apólice é requerido.")]
        public string NumeroApolice { get; set; }
        [Required(ErrorMessage = "O Valor da Corbetura é requerido.")]
        public decimal ValorCobertura { get; set; }
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        public DateTime? Emissao { get; set; }
        [Required(ErrorMessage = "A Data de Validade é requerido.")]
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        public DateTime? Validade { get; set; }
        public int VeiculoId { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }

        #endregion
    }
}