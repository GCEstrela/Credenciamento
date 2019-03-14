// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;
#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class EmpresaContratoView : ValidacaoModel
    {
        #region  Propriedades

        public int EmpresaContratoId { get; set; }
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "O Número do Contrato é requerido.")]
        public string NumeroContrato { get; set; }
        [Required(ErrorMessage = "A Descrição do Contrato é requerido.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A Data de Emissão do Contrato é requerido.")]
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        public DateTime? Emissao { get; set; }
        [Required(ErrorMessage = "A Data de Validade do Contrato é requerido.")]
        [Range(typeof(DateTime), "11/11/2000", "1/1/2200", ErrorMessage = "Data inválida")]
        public DateTime? Validade { get; set; } 
        public bool Terceirizada { get; set; }
        public string Contratante { get; set; }
        public bool IsencaoCobranca { get; set; } 
        public int TipoCobrancaId { get; set; }
        public int CobrancaEmpresaId { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        //[Required(ErrorMessage = "O Nome do Responsável do Contrato é requeirido.")]
        public string NomeResp { get; set; }
        public string TelefoneResp { get; set; }
        public string CelularResp { get; set; }
        //[Required(ErrorMessage = "O E-Mail do Responsável do Contrato é requeirido.")]
        public string EmailResp { get; set; }
        [Required(ErrorMessage = "O Ststus do Contrato é requerido.")]
        public int StatusId { get; set; }
        public string NomeArquivo { get; set; }
        //[Required(ErrorMessage = "O Contrato Digitalizado é requeirido.")]
        public string Arquivo { get; set; }
        public int TipoAcessoId { get; set; }
        public bool ContratoBasico { get; set; }

        #endregion
    }
}