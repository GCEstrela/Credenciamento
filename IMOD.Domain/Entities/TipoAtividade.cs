// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class TipoAtividade
    {        
        #region  Propriedades

        public int TipoAtividadeId { get; set; }
        public string Descricao { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TipoAtividade atividade &&
                   TipoAtividadeId == atividade.TipoAtividadeId;
        }

        public override int GetHashCode()
        {
            return -2130805795 + TipoAtividadeId.GetHashCode();
        }

        #endregion



    }
}