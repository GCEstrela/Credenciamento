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
        public string Observacao { get; set; }
        public bool Impeditivo { get; set; }
        public bool Resolvido { get; set; }
        public int UsuarioRevisao { get; set; }
        public DateTime DataRevisao { get; set; }
        public int TipoSituacao { get; set; }
        public string ObservacaoResposta { get; set; }
        public int ColaboradorObservacaoRespostaID { get; set; }

    }
}
