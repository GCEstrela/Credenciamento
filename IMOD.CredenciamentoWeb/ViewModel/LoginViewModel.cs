using System.ComponentModel.DataAnnotations;

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class LoginViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Informe um e-mail")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válifo")]
        public string Email { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Required(ErrorMessage = "Informe uma senha")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        /// <summary>
        /// String Url de retorno
        /// </summary>
        public string UrlRetorno { get; set; }
    }
}