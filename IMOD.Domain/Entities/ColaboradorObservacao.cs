// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace IMOD.Domain.Entities
{
    public class ColaboradorObservacao
    {
        #region  Propriedades

        public int ColaboradorObservacaoId { get; set; }
        public int ColaboradorId { get; set; }
        public string Observacao { get; set; }
        public bool Impeditivo { get; set; }
        public bool Resolvido { get; set; }

        public override bool Equals(object obj)
        {
            var observacao = obj as ColaboradorObservacao;
            return observacao != null &&
                   ColaboradorObservacaoId == observacao.ColaboradorObservacaoId;
        }
        public override int GetHashCode()
        {
            return -2141522505 + ColaboradorObservacaoId.GetHashCode();
        }
        public ColaboradorObservacao(int colaboradorObservacaoId)
        {
            ColaboradorObservacaoId = colaboradorObservacaoId;
        }
        public ColaboradorObservacao()
        {
        }
        
        #endregion
    }
}