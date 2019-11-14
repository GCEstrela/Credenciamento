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
    public class EmpresaSeguroView
    {
        #region  Propriedades

        public int EmpresaSeguroId { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public decimal ValorCobertura { get; set; }
        public int? EmpresaId { get; set; }
        public string Arquivo { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }
        public int EmpresaContratoId { get; set; }
        #endregion
    }
}