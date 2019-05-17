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

        //public int EmpresaID { get; set; }
        //public string Nome { get; set; }
        //public string Apelido { get; set; }
        ////[Display(Name = "Senha")]       
        ////[DataType(DataType.Password)]
        ////[Required(ErrorMessage = "Informe a senha", AllowEmptyStrings = false)]
        public string Senha { get; set; }
        ////[Display(Name = "Login")]
        ////[Required(ErrorMessage = "Informe o CNPJ da empresa", AllowEmptyStrings = false)]
        //public string CNPJ { get; set; }
        //public string Logo { get; set; }
        ////public string FotoFormatada { get { return string.Format("data:image/png;base64,{0}", Logo); } }
        public bool Lembreme { get; set; }
        //public string Email1 { get; set; }
        //public string Contato1 { get; set; }
        public IList<EmpresaContrato> Contratos { get; set; }

        public int EmpresaId { get; set; }
        //[Required(ErrorMessage = "A Razão Social é requerida.")]
        public string Nome { get; set; }
       // [Required(ErrorMessage = "O Apelido é requerido.")]
        public string Apelido { get; set; }
        //[Required(ErrorMessage = "O Cnpj é requerido.")]
        public string Cnpj { get; set; }
        public string InsEst { get; set; }
        public string InsMun { get; set; }
        public string Responsavel { get; set; }
        //[Required(ErrorMessage = "O Cep é requerido.")]
        public string Cep { get; set; }
        //[Required(ErrorMessage = "O Endereço é requerido.")]
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        //[Required(ErrorMessage = "O Bairro é requerido.")]
        public string Bairro { get; set; }
        //[Required(ErrorMessage = "O Estado é requerido.")]
        public int EstadoId { get; set; }
        //[Required(ErrorMessage = "O Município é requerido.")]
        public int MunicipioId { get; set; }
        //[Required(ErrorMessage = "O E-Mail é requerido.")]
        public string Email1 { get; set; }
        //[Required(ErrorMessage = "O Contato é requerido.")]
        public string Contato1 { get; set; }
        //[Required(ErrorMessage = "O Telefone é requerido.")]
        public string Telefone1 { get; set; }
        public string Celular1 { get; set; }
        public string Email2 { get; set; }
        public string Contato2 { get; set; }
        public string Telefone2 { get; set; }
        public string Celular2 { get; set; }
        public string Obs { get; set; }
        public string Logo { get; set; }
        public bool Ativo { get; set; }
        public bool Pendente { get; set; }
        public bool Pendente11 { get; set; }
        public bool Pendente12 { get; set; }
        public bool Pendente13 { get; set; }
        public bool Pendente14 { get; set; }
        public bool Pendente15 { get; set; }
        public bool Pendente16 { get; set; }
        public bool Pendente17 { get; set; }
        //[Required(ErrorMessage = "A Gigla é requerida.")]
        [ScaffoldColumn(true)]
        [StringLength(3, ErrorMessage = "A Sigla não pode ser superior a 3 caracteres. ")]
        public string Sigla { get; set; }
        public int TotalPermanente { get; set; }
        public int TotalTemporaria { get; set; }



    }
}