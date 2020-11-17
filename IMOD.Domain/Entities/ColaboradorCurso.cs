// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace IMOD.Domain.Entities
{
    public class ColaboradorCurso
    {
        #region  Propriedades

        public int ColaboradorCursoId { get; set; }
        public int ColaboradorCursoWebId { get; set; }
        public int ColaboradorId { get; set; }
        public int? CursoId { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }
        public DateTime? Validade { get; set; }
        public bool Controlado { get; set; }
        public string Descricao { get; set; }
        public bool Cracha { get; set; }
        #endregion
    }
}