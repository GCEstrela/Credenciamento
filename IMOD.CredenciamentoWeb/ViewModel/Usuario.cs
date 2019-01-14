// ***********************************************************************
// Project: IMOD.CredenciamentoWeb
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 10 - 2019
// ***********************************************************************

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class Usuario
    {
        #region  Propriedades

        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Cep { get; set; }

        #endregion
    }
}