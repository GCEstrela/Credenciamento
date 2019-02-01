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
        [Required(ErrorMessage = "O campo descrição é requerido!")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O campo Nome Arquivo é requerido!")]
        public string NomeArquivo { get; set; }
        [Required(ErrorMessage = "O campo Arquivo é requerido!")]
        public string Arquivo { get; set; }

        #endregion
    }
}