using System;

namespace IMOD.Domain.Entities
{
    public class VeiculoObservacao
    {
        #region  Propriedades
        public int VeiculoObservacaoId { get; set; }
        public int VeiculoId { get; set; }
        public string Observacao { get; set; }
        public bool Impeditivo { get; set; }
        public bool Resolvido { get; set; }
        public int? UsuarioRevisao { get; set; }
        public DateTime DataRevisao { get; set; }
        public int? VeiculoObservacaoRespostaId { get; set; }
        public string ObservacaoResposta { get; set; }
        #endregion
    }
}