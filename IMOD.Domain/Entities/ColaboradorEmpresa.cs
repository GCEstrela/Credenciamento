// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class ColaboradorEmpresa
    {
        #region  Propriedades

        public int ColaboradorEmpresaId { get; set; }
        public int ColaboradorId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaSigla { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }

        #endregion
    }
}