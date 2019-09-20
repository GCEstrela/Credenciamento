using IMOD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class LoginViewModel
    {

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Informe a senha", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "A senha deve ter entre 4 e 10 caracteres")]
        public string Senha { get; set; }
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 10 caracteres")]
        public string NovaSenha { get; set; }
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 10 caracteres")]
        [Compare(nameof(NovaSenha), ErrorMessage ="A confirmação deve ser igual a nova senha informada")]
        public string ConfimacaoSenha { get; set; }
        public bool Lembreme { get; set; }
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "Informe o CNPJ", AllowEmptyStrings = false)]
        public string Cnpj { get; set; }
        public string Email1 { get; set; }        


    }
}