// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class EmpresaSignatarioView : ValidacaoModel
    {
        #region  Propriedades

        public int EmpresaSignatarioId { get; set; }
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "O Nome é requerido.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Cpf é requerido.")]
        public string Cpf { get; set; }
        //[Required(ErrorMessage = "O E-Mail é requerido.")]
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        //[Required(ErrorMessage = "O Principal é requerido.")]
        public bool Principal { get; set; }
        //[Required(ErrorMessage = "A Ficha Cadastral é requerida.")]
        public string Assinatura { get; set; }
        [Required(ErrorMessage = "O Nome do Arquivo é requerido.")]
        public string NomeArquivo { get; set; }
        #endregion
    }
}