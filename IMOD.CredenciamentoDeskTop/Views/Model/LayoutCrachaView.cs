// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

using System;
using IMOD.CredenciamentoDeskTop.Enums;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class LayoutCrachaView
    {
        #region  Propriedades

        public int LayoutCrachaId { get; set; }
        public string Nome { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public decimal Valor { get; set; }
        public string LayoutRpt { get; set; }
        public int TipoCracha { get; set; }

        public string TipoCrachaDescricao { get { return ((IMOD.CredenciamentoDeskTop.Enums.TipoLayoutCracha)TipoCracha).ToString(); }  }

        #endregion
    }
}