// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorEmpresaView:ValidacaoModel
    {
        #region  Propriedades
       
        public int ColaboradorEmpresaId { get; set; }
        public int ColaboradorId { get; set; } 
        [Range(1, int.MaxValue,ErrorMessage = "A Razão Social é requerida.")]
        public int EmpresaId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "O Contrato é requerido.")]
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }

        #endregion
    }
}