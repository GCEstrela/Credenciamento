// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorCursoView
    {
        #region  Propriedades

        public int ColaboradorCursoId { get; set; }
        public int ColaboradorId { get; set; }
        public int CursoId { get; set; }
        public string CursoNome { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }
        public DateTime? Validade { get; set; }
        public bool Controlado { get; set; }

        #endregion
    }
}