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
    public class VeiculoCredencialimpressao
    {
        #region  Propriedades

        public int CredencialImpressaoId { get; set; }
        public int? VeiculoCredencialId { get; set; }
        public DateTime? DataImpressao { get; set; }
        public bool Cobrar { get; set; }

        #endregion
    }
}