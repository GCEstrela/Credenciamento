// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class Relatorios
    {
        #region  Propriedades

        public int RelatorioId { get; set; }
        public string Nome { get; set; }
        public string NomeArquivoRpt { get; set; }
        public string ArquivoRpt { get; set; }
        public bool Ativo { get; set; }

        #endregion
    }
}