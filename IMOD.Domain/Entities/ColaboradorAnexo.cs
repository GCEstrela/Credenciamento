// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

using System.Web;

namespace IMOD.Domain.Entities
{
    public class ColaboradorAnexo
    {
        #region  Propriedades

        public int ColaboradorAnexoId { get; set; }
        public string Descricao { get; set; }
        public string NomeArquivo { get; set; }
        public int ColaboradorId { get; set; }
        public string Arquivo { get; set; }
        public HttpPostedFileBase Anexo { get; set; }
        public ColaboradorAnexo(int colaboradorAnexoId)
        {
            ColaboradorAnexoId = colaboradorAnexoId;
        }

        public ColaboradorAnexo()
        {
        }

        #endregion
    }

    
}