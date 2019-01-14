 

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class UsuarioProfileViewModel
    {
        /// <summary>
        /// Identificador do usuário
        /// </summary>
        public int IdUser { get; set; } 
        /// <summary>
        /// Nome do usuario
        /// </summary>
        public string Nome { get; set; }
        /// <summary>
        /// /Senha
        /// </summary>
        public string Senha { get; set; }
        /// <summary>
        /// Perfil do usuario
        /// </summary>
        public string Perfil { get; set; }
        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Endereco
        /// </summary>
        public string Endereco { get; set; }
        /// <summary>
        /// CEP
        /// </summary>
        public string Cep { get; set; } 
    }
}