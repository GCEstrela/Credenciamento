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
    public class VeiculoSeguro
    {
        #region  Propriedades

        public int VeiculoSeguroId { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public decimal ValorCobertura { get; set; }
        public int VeiculoId { get; set; }
        public string Arquivo { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }
        public int? EmpresaSeguroid { get; set; }
        #endregion
    }
}