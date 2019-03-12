// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorAnexoView:ValidacaoModel
    {
        #region  Propriedades

        public int ColaboradorAnexoId { get; set; }
        public int ColaboradorId { get; set; }
        //[Required(ErrorMessage = "A Descrição é requerida.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O Nome do arquivo é requerido.")]
        public string NomeArquivo { get; set; }
        //[Required(ErrorMessage = "O Arquivo do arquivo é requerido.")]
        public string Arquivo { get; set; }

        #endregion
    }
}