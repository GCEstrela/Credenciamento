using IMOD.Domain.Entities;
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
        //[Display(Name = "Senha")]       
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Informe a senha", AllowEmptyStrings = false)]
        public string Senha { get; set; }
        //[Display(Name = "Login")]
        //[Required(ErrorMessage = "Informe o CNPJ da empresa", AllowEmptyStrings = false)]
        public string CNPJ { get; set; }
        public string Logo { get; set; }
        //public string FotoFormatada { get { return string.Format("data:image/png;base64,{0}", Logo); } }
        public bool Lembreme { get; set; }
        public string Email1 { get; set; }
        public string Contato1 { get; set; }
        public IList<EmpresaContrato> Contratos { get; set; }
    }
}