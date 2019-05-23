using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMOD.ValidacaoCredencialWeb.Models
{
    public class CredencialViewModel
    {
        public int ColaboradorCredencialID { get; set; }
        public string Colete { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string ValidadeFormatada { get { return Validade.ToShortDateString(); } }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        [Display(Name = "Empresa")]
        public string EmpresaNome { get; set; }
        [Display(Name = "Nome Fantasia")]
        public string EmpresaApelido { get; set; }
        public string CNPJ { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        [Display(Name = "Colaborador")]
        public string ColaboradorNome { get; set; }
        [Display(Name = "Apelido")]
        public string ColaboradorApelido { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string RGOrgLocal { get; set; }
        public string RGOrgUF { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string Foto { get; set; }
        public byte[] Foto2 { get; set; }
        public byte[] Logo2 { get; set; }
        [Display(Name = "Área de Acesso 1")] 
        public string Identificacao1 { get; set; }
        [Display(Name = "Área de Acesso 2")] 
        public string Identificacao2 { get; set; }
        [Display(Name = "Categoria CNH")]
        public string CNHCategoria { get; set; }
        public string LayoutRPT { get; set; }
        public string CrachaCursos { get; set; }
        public string ImpressaoMotivo { get; set; }
        public string TerceirizadaNome { get; set; }
        public string RNE { get; set; }
        public string Passaporte { get; set; }
        [Display(Name = "Validade Passaporte")]
        public DateTime PassaporteValidade { get; set; }
        [Display(Name = "Manuseio Bagagem")]
        public bool ManuseioBagagem { get; set; }
        public bool Motorista { get; set; }
        [Display(Name = "Número CNH")]
        public string Cnh { get; set; }
        [Display(Name = "Ponte Embarque")]
        public bool OperadorPonteEmbarque { get; set; }
        [Display(Name = "CCAM")]
        public bool FlagCcam { get; set; }
        [Display(Name = "Status")]
        public bool Ativa { get; set; }
        [Display(Name = "Foto Formatada")]
        public string FotoFormatada { get { return string.Format("data:image/png;base64,{0}", Foto); } }

    }
}