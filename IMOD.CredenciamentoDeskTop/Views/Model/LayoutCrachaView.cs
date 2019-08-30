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
        public string Modelo { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public decimal Valor { get; set; }
        public string LayoutRpt { get; set; }
        public int TipoCracha { get; set; }
        public int TipoValidade { get; set; }
        public string Cor { get; set; }

        public string TipoCrachaDescricao { get { return ((IMOD.CredenciamentoDeskTop.Enums.TipoLayoutCracha)TipoCracha).ToString(); }  }
        public string TipoValidadeDescricao { get { return ((IMOD.CredenciamentoDeskTop.Enums.TipoValidade)TipoValidade).ToString(); } }

        #endregion
    }
}