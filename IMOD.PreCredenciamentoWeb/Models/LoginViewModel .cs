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
        public string Senha { get; set; }
        public string ConfimacaoSenha { get; set; }
        public bool Lembreme { get; set; }
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "Informe o CNPJ", AllowEmptyStrings = false)]
        public string Cnpj { get; set; }
        public string Email1 { get; set; }        


    }
}