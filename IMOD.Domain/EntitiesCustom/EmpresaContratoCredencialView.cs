// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 07 - 2019
// ***********************************************************************

namespace IMOD.Domain.EntitiesCustom
{
    public class EmpresaContratoCredencialView
    {
        #region  Propriedades

        public int ColaboradorId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string Descricao { get; set; }
        public int ColaboradorEmpresaID { get; set; }
        public bool Terceirizada { get; set; }
        public string TerceirizadaSigla { get; set; }
        #endregion
    }
}