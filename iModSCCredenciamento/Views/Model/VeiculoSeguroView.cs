// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace iModSCCredenciamento.Views.Model
{
    public class VeiculoSeguroView
    {
        #region  Propriedades

        public int VeiculoSeguroId { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public string ValorCobertura { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }
        public int VeiculoId { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }

        #endregion
    }
}