using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Foolproof;
using IMOD.Domain.Enums;
using System.ComponentModel;
using System.Reflection;

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
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data Nascimento")]
        public DateTime? DataNascimento { get; set; }
        [Display(Name = "Nome Pai")]
        public string NomePai { get; set; }
        [Display(Name = "Nome Mãe")]
        public string NomeMae { get; set; }
        public string Nacionalidade { get; set; }
        [Display(Name = "Foto do Colaborador")]
        public HttpPostedFileBase FotoColaborador { get; set; }
        public string Foto { get; set; }
        [Display(Name = "Estado Civil")]
        public string EstadoCivil { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-##-####}")]
        //[Required(ErrorMessage = "O CPF é requerido.")]
        [RequiredIf("Estrangeiro", false, ErrorMessage = "O CPF é requerido.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        [RequiredIf("Estrangeiro", false, ErrorMessage = "O RG é requerido.")]
        [Display(Name = "RG")]
        public string Rg { get; set; }
        
        [Display(Name = "Emissão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? RgEmissao { get; set; }
        [Display(Name = "Órgão Emissor")]
        public string RgOrgLocal { get; set; }
        [Display(Name = "UF")]
        public string RgOrgUf { get; set; }
        [RequiredIf("ValidaEstrangeiro", true, ErrorMessage = "O passaporte ou RNE são obrigatórios para estrangeiros.")]
        public string Passaporte { get; set; }
        [RequiredIf("ValidaEstrangeiro", true, ErrorMessage = "O passaporte ou RNE são obrigatórios para estrangeiros.")]
        [Display(Name = "Validade Passaporte")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PassaporteValidade { get; set; }
        [RequiredIf("ValidaEstrangeiro", true, ErrorMessage = "O passaporte ou RNE são obrigatórios para estrangeiros.")]
        public string Rne { get; set; }
        [Display(Name = "Telefone")]
        public string TelefoneFixo { get; set; }
        [Display(Name = "Celular")]
        public string TelefoneCelular { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um Email válido...")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O Contato de Emergência é requerido.")]
        [Display(Name = "Contato Emergência")]
        public string ContatoEmergencia { get; set; }
        [Required(ErrorMessage = "O Telefone de Emergência é requerido.")]
        [Display(Name = "Telefone Emergência")]
        public string TelefoneEmergencia { get; set; }
        [Required(ErrorMessage = "O CEP é requerido.")]
        [Display(Name = "CEP")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O Endereço é requerido.")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "O Número é requerido.")]
        [Display(Name = "Número")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        [Required(ErrorMessage = "O Bairro é requerido.")]
        public string Bairro { get; set; }
        //public bool Ativo { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Município é requerido.")]
        [Display(Name = "Município")]
        public int MunicipioId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Estado é requerido.")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Motorista é requerido.")]
        public bool Motorista { get; set; }
        [RequiredIf("Motorista", true, ErrorMessage = "Categoria da CNH é obrigatória para motorista.")]
        [Display(Name = "Categoria Habilitação")]
        public string CnhCategoria { get; set; }
        [RequiredIf("Motorista", true, ErrorMessage = "Número da CNH é obrigatório para motorista.")]
        [Display(Name = "Número Habilitação")]
        public string Cnh { get; set; }
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        [RequiredIf("Motorista", true, ErrorMessage = "Validade da CNH é obrigatório para motorista.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Validade Habilitação")]
        public DateTime? CnhValidade { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Validade")]
        public DateTime? Vigencia { get; set; }

        public string CnhEmissor { get; set; }
        [Display(Name = "UF")]
        public string Cnhuf { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataEmissao { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data de Validade")]
        public DateTime? DataValidade { get; set; }
        
        public bool Estrangeiro { get; set; }

        public bool ValidaEstrangeiro
        {
            get
            {
                return (Estrangeiro == true && (string.IsNullOrEmpty(Rne) & !PassaportePreenchido()));
            }
        }

        private bool PassaportePreenchido()
        {
            return (!string.IsNullOrEmpty(Passaporte) && PassaporteValidade != null);
        }

        //public IEnumerable<Estados> Estados { get; set; }
        [RequiredIf("ContratoEmpresaID", 0, ErrorMessage = "Necessário adicionar pelo menos um contrato!")]
        [Display(Name = "Contrato Empresa")]
        public string ContratoEmpresaID { get; set; }
        public bool Precadastro { get; set; }
        [Display(Name = "Observação")]
        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        [Display(Name = "Documento Anexo")]
        public string NomeAnexoVinculo { get; set; }
        [Display(Name = "Documento Anexo")]
        public string NomeAnexoCurso { get; set; }
        [Display(Name = "Anexo")]
        public HttpPostedFileBase AnexoVinculo { get; set; }
        [Display(Name = "Anexo")]
        public HttpPostedFileBase AnexoCurso { get; set; }

        public HttpPostedFileBase Aceite { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "É necessário aceitar o termo!")]
        public bool chkAceite { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Status")]
        public string Status 
        {
            get
            {
                if (StatusCadastro != null)
                {
                    return Funcoes.GetDescription((StatusCadastro)StatusCadastro); 
                }
                else
                {
                    return "Aprovado";
                }
            }
        }
        public int? StatusCadastro { get; set; }
        
        #endregion
    }
}
