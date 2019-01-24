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
    public class EmpresaAnexoView : ValidacaoModel
    {
        #region  Propriedades

        public int EmpresaAnexoId { get; set; }
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "A Descrição do Anexo é requerido.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A Descrição do Arquivo Anexo é requerido.")]
        public string NomeAnexo { get; set; }
        public string Anexo { get; set; }

        #endregion
    }
}