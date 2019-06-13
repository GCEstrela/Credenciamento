using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class ColaboradorViewModel
    {
        #region  Propriedades

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ColaboradorId { get; set; }
        [Required(ErrorMessage = "O Nome é requerido.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Apelido é requerido.")]
        [Display(Name = "Apelido")]
        public string Apelido { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data Nascimento")]
        public DateTime? DataNascimento { get; set; }
        [Display(Name = "Nome Pai")]
        public string NomePai { get; set; }
        [Display(Name = "Nome Mãe")] 
        public string NomeMae { get; set; }
        public string Nacionalidade { get; set; }
        public string Foto { get; set; }
        [Display(Name = "Estado Civil")]
        public string EstadoCivil { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-##-####}")]
        public string Cpf { get; set; }
        public string Rg { get; set; }
        [Display(Name = "Emissão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? RgEmissao { get; set; }
        [Display(Name = "Órgão Emissor")]
        public string RgOrgLocal { get; set; }
        [Display(Name = "UF")]
        public string RgOrgUf { get; set; }
        public string Passaporte { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Validade Passaporte")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PassaporteValidade { get; set; }
        public string Rne { get; set; }
        [Display(Name = "Fone")]
        public string TelefoneFixo { get; set; }
        [Display(Name = "Celular")]
        public string TelefoneCelular { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um Email válido...")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O Contato de Emergencia é requerido.")]
        [Display(Name = "Contato Emergência")]
        public string ContatoEmergencia { get; set; }
        [Required(ErrorMessage = "O Telefone de Emergencia é requerido.")]
        [Display(Name = "Telefone Emergência")]
        public string TelefoneEmergencia { get; set; }
        [Required(ErrorMessage = "O Cep é requerido.")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O Endereco é requerido.")]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "O Número é requerido.")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        [Required(ErrorMessage = "O Bairro é requerido.")]
        public string Bairro { get; set; }
        public bool Ativo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Município é requerido.")]
        [Display(Name = "Municipio")]
        public int MunicipioId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Estado é requerido.")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Motorista é requerido.")]
        public bool Motorista { get; set; }
        [Display(Name = "Categoria Habilitação")]
        public string CnhCategoria { get; set; }
        [Display(Name = "Número Habilitação")]
        public string Cnh { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Validade Habilitação")]
        public DateTime? CnhValidade { get; set; }

        public string CnhEmissor { get; set; }
        [Display(Name = "UF")]
        public string Cnhuf { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataEmissao { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataValidade { get; set; }

        public bool Estrangeiro { get; set; }

        //public IEnumerable<Estados> Estados { get; set; }
        [Display(Name = "Constrato Empresa")]
        public string ContratoEmpresaID { get; set; }
        public bool Precadastro { get; set; }

        public string CaminhoArquivo { get; set; } 

        [Display(Name = "Documento Anexo")]
        public HttpPostedFileBase FileUpload { get; set; }

        #endregion
    }
}