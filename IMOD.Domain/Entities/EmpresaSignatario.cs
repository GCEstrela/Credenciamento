// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class EmpresaSignatario
    {
        #region  Propriedades

        public int EmpresaSignatarioId { get; set; }
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public bool Principal { get; set; }
        public string Assinatura { get; set; }
        public string NomeArquivo { get; set; }
        #endregion
    }
}