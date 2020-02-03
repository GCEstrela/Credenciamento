// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class EmpresaAreaAcesso
    {
        #region  Propriedades

        public int EmpresaAreaAcessoId { get; set; }
        public int EmpresaId { get; set; }
        public int AreaAcessoId { get; set; }
        public string Descricao { get; set; }
        public string Identificacao { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public string Area { get; set; }
        public string Cnpj { get; set; }

        #endregion
    }
}