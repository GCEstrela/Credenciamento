// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

namespace iModSCCredenciamento.Views.Model
{
    public class EmpresaSignatarioView
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

        #endregion
    }
}