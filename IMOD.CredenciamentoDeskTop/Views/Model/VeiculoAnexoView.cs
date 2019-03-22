// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;


#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class VeiculoAnexoView : ValidacaoModel
    {
        #region  Propriedades

        public int VeiculoAnexoId { get; set; }
        public int VeiculoId { get; set; }
        //[Required(ErrorMessage = "A Descrição é requerida.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O Nome do arquivo é requerido.")]
        public string NomeArquivo { get; set; }
        //[Required(ErrorMessage = "O Arquivo do arquivo é requerido.")]
        public string Arquivo { get; set; }

        #endregion
    }
}