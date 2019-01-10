// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace iModSCCredenciamento.Views.Model
{
    public class PendenciaView
    {
        #region  Propriedades
        public int PendenciaId { get; set; }
        public int CodPendencia { get; set; }
        public string CodPendenciaDescricao { get; set; }
        [Required(ErrorMessage = @"Informe o motivo da pendência")]
        public string Descricao { get; set; }
        public DateTime? DataLimite { get; set; }
        public bool Impeditivo { get; set; }
        public int? ColaboradorId { get; set; }
        public int? EmpresaId { get; set; }
        public int? VeiculoId { get; set; }

        #endregion
    }
}