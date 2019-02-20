// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class CredencialMotivo
    {
        #region  Propriedades

        public int CredencialMotivoId { get; set; }
        public string Descricao { get; set; }
        public string CodigoStatus { get; set; }
        public bool Impeditivo { get; set; }
        public int CredencialStatusId { get; set; }

        #endregion
    }
}