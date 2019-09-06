using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IMOD.Domain.EntitiesCustom.Funcoes;

namespace IMOD.Domain.EntitiesCustom
{
    public class ColaboradoresCredenciaisView : ValidacaoModel
    {
        public int ColaboradorCredencialId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Empresa é requerida.")]
        public int ColaboradorEmpresaId { get; set; }
        public int TecnologiaCredencialId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Tipo de credencial é requerida.")]
        public int TipoCredencialId { get; set; }
        public string NumeroContrato { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Layout do crachá é requerido.")]
        public int LayoutCrachaId { get; set; }
        public int FormatoCredencialId { get; set; }
        //[Required(ErrorMessage = "O Número da credencial é requerida.")]
        public string NumeroCredencial { get; set; }
        public int Fc { get; set; }
        public DateTime? Emissao { get; set; }
        [Range(typeof(DateTime), "1/1/1880", "1/1/2200", ErrorMessage = "Data inválida")]
        [Required(ErrorMessage = "A Data de Validade é requerido.")]
        public DateTime? Validade { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "O status da credencial é requerida.")]
        public int CredencialStatusId { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "A Regra é requerido.")]
        public int ColaboradorPrivilegio1Id { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "A Regra é requerido.")]
        public int ColaboradorPrivilegio2Id { get; set; }
        //private string _colete;
        //public string Colete { get { return _colete = ColaboradorId > 0 ? EmpresaSigla.Trim().ToString() + Convert.ToString(ColaboradorId) : _colete; } set { _colete = value; } }
        public string Colete { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O tipo é requerido.")]
        public int CredencialMotivoId { get; set; }
        public bool Impressa { get; set; }
        public bool Ativa { get; set; }
        public DateTime? Baixa { get; set; }
        public string LayoutCrachaNome { get; set; }
        public string FormatoCredencialDescricao { get; set; }
        public string ColaboradorNome { get; set; }
        public string EmpresaNome { get; set; }
        public string ContratoDescricao { get; set; }
        public string TipoCredencialDescricao { get; set; }
        public string CredencialStatusDescricao { get; set; }
        public int EmpresaId { get; set; }
        public int ColaboradorId { get; set; }
        public string ColaboradorFoto { get; set; }
        public string Cpf { get; set; }
        public bool Motorista { get; set; }
        public string ColaboradorApelido { get; set; }
        public string EmpresaLogo { get; set; }
        public string EmpresaSigla { get; set; }
        public string EmpresaApelido { get; set; }
        public string Cargo { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public string Cnpj { get; set; }
        public string CnhCategoria { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string Rg { get; set; }
        public string RgOrgLocal { get; set; }
        public string RgOrgUf { get; set; }
        public string Matricula { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public bool IsencaoCobranca { get; set; }
        /// <summary>
        /// True, possue pendencia impeditiva
        /// </summary>
        public bool PendenciaImpeditiva { get; set; }
        public string CredencialMotivoDescricao { get; set; }
        public DateTime? DataImpressao { get; set; }
        public decimal Valor { get; set; }
        public string TiposCobrancaNome { get; set; }
        public string EmpresasContratosNome { get; set; }
        public string Identificacao { get; set; }
        public int AreaAcessoId { get; set; }
        public string IdentificacaoDescricao { get; set; }
        public DateTime? DataStatus { get; set; }
        public string NumeroColete { get; set; }
        public bool Policiafederal { get; set; }
        public bool Receitafederal { get; set; }
        public bool Segurancatrabalho { get; set; }
        public string Email { get; set; }
        public string Obs { get; set; }
        public bool DevolucaoEntregaBo { get; set; }
        public bool Regras { get; set; }
        public string GrupoPadrao { get; set; }
        public int? CredencialVia { get; set; }        
        [RequiredIf("MotivoObrigatorio", true, ErrorMessage = "Informe o motivo da Via Adicional.")]
        public int? CredencialmotivoViaAdicionalID { get; set; }
        public int? CredencialmotivoIDanterior { get; set; }       
        public string CredencialmotivoViaAdicionalDescricao{ get; set; } 
        public string CredencialmotivoIdAnteriorDescricao { get; set; }
        public bool Estrangeiro { get; set; }
        public string RNE { get; set; }
        public string Usuario { get; set; }
        public string listadeGrupos { get; set; }
        public bool MotivoObrigatorio
        {
            get
            {
                return (CredencialMotivoId == 2 && CredencialmotivoViaAdicionalID == null);
            }
        }

    }
}
