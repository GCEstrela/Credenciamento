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
    public class EmpresaSeguro
    {
        #region  Propriedades

        public int EmpresaSeguroId { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public string ValorCobertura { get; set; }
        public int? EmpresaId { get; set; }
        public string Arquivo { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }

        #endregion
    }
}