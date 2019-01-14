using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class DadosLicencaViewModel
    {

        /// <summary>
        /// Identificador da licença
        /// </summary>
        [Required(ErrorMessage = "Um usuário é requerido.")]
        public int IdUser { get; set; }

        /// <summary>
        ///     Identificador
        /// </summary>
        public int IdLicenca { get; set; }

        /// <summary>
        ///     Numero sequencial do hardware
        /// </summary>
        [Required(ErrorMessage = "O serial ID da computador é requerido.")]
        public string HardwareInfo { get; set; }

        /// <summary>
        ///     CNPJ do licenciado
        /// </summary>
        [Required(ErrorMessage = "O CNPJ é requerido (sem caracteres especiais")]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        /// <summary>
        ///     Data expira
        /// </summary>
        [Required(ErrorMessage = "A data de expiração da licença é requerido.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")] 
        public DateTime DataExpira { get; set; }

        /// <summary>
        ///     Nome do licenciado
        /// </summary>
        [Required(ErrorMessage = "O nome do cliente é requerido.")]
        [MaxLength(50,ErrorMessage = "Informe até 50 caracteres")]
        public string LicenciadoPara { get; set; }

        /// <summary>
        ///     Numero de usuario simultaneos
        /// </summary>
        [Required(ErrorMessage = "Informe a quantidade máxima")]
        [Range(5,10, ErrorMessage="Informe números entre 5 e 10")]
        [DefaultValue(5)]
        public int NumUsuariosSimultaneos { get; set; }

        /// <summary>
        /// Data da renovação
        /// </summary>
        public DateTime? DataRenovacao { get; set; }
    }
}