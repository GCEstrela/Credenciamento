using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IMOD.Domain.EntitiesCustom.Funcoes;

namespace IMOD.Domain.EntitiesCustom
{
    public class ColaboradoresObservacoesView : ValidacaoModel
    {
        public int ColaboradorObservacaoId { get; set; }
        public int ColaboradorId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Observação é requerida.")]
        public string Observacao { get; set; }
        public bool Impeditivo { get; set; }
        public bool Resolvido { get; set; }

    }
}
