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
    public class ColaboradorCursoView:ValidacaoModel
    {
        #region  Propriedades

        public int ColaboradorCursoId { get; set; }
        public int ColaboradorId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Curso é requerido.")]
        public int CursoId { get; set; }
        public string CursoNome { get; set; }
        [Required(ErrorMessage = "O Nome do arquivo é requerido.")]
        public string NomeArquivo { get; set; }
        [Required(ErrorMessage = "O Arquivo é requerido.")]
        public string Arquivo { get; set; }
        public DateTime? Validade { get; set; }
        public bool Controlado { get; set; }

        #endregion
    }
}