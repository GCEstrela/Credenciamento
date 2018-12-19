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
    public class Pendencia
    {
        #region  Propriedades

        public int PendenciaId { get; set; }
        public int CodPendencia { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataLimite { get; set; }
        public bool Impeditivo { get; set; }
        public int? ColaboradorId { get; set; }
        public int? EmpresaId { get; set; }
        public int? VeiculoId { get; set; }

        #endregion
    }
}