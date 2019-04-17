using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class EmpresaViewModel
    {

        public int Codigo { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe a senha", AllowEmptyStrings = false)]
        public string Senha { get; set; }
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Informe o CNPJ da empresa", AllowEmptyStrings = false)]
        public string CNPJ { get; set; }
        public Byte[] Logo { get; set; }
        public bool Lembreme { get; set; }
    }
}