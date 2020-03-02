// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;
using IMOD.CredenciamentoDeskTop.ViewModels;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorObservacaoView : ValidacaoModel, ICloneable
    {

        #region  Propriedades
        public int ColaboradorObservacaoId { get; set; }
        public int ColaboradorId { get; set; }
        public string Observacao { get; set; }
        public bool Impeditivo { get; set; }
        public bool Resolvido { get; set; }
        #endregion

        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return (ColaboradorView)MemberwiseClone();
        }
    }





}