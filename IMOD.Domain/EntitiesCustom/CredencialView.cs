using System;

namespace IMOD.Domain.EntitiesCustom
{
    public class CredencialView
    {
        public int ColaboradorCredencialID { get; set; }
        public string Colete { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaApelido { get; set; }
        public string CNPJ { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        public string ColaboradorNome { get; set; }
        public string ColaboradorApelido { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string RGOrgLocal { get; set; }
        public string RGOrgUF { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string Foto { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string CNHCategoria { get; set; }
        public string LayoutRPT { get; set; }
        public string CrachaCursos { get; set; }
        public string ImpressaoMotivo { get; set; }
        public string TerceirizadaNome { get; set; }
        public string RNE { get; set; }
        public string Passaporte { get; set; }
        public DateTime PassaporteValidade { get; set; }
        public bool ManuseioBagagem { get; set; }
        public bool Motorista { get; set; }
        public string Cnh { get; set; }
        public bool Ativa { get; set; }
         
    }
}
