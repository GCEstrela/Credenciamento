using System;
using System.ComponentModel.DataAnnotations;
using IMOD.Domain.EntitiesCustom.Funcoes;

namespace IMOD.Domain.EntitiesCustom
{
    public class VeiculosCredenciaisView : ValidacaoModel
    {
        
        public int VeiculoCredencialId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Empresa é requerida.")]
        public int VeiculoEmpresaId { get; set; }
        public int TecnologiaCredencialId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Tipo de credencial é requerida.")]
        public int TipoCredencialId { get; set; }
        public string TipoCredencialDescricao { get; set; }
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
        public string CredencialStatusDescricao { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        public int VeiculoPrivilegio1Id { get; set; }
        public int VeiculoPrivilegio2Id { get; set; }
        public string Colete { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A motivação é requerida.")]
        public int CredencialMotivoId { get; set; }
        public int CredencialMotivoId1 { get; set; }
        public string CredencialMotivoDescricao { get; set; }
        public bool Impressa { get; set; }
        public bool Ativa { get; set; }
        public DateTime? Baixa { get; set; }
        public string LayoutCrachaNome { get; set; }
        public string FormatoCredencialDescricao { get; set; }
        public string VeiculoNome { get; set; }
        public string EmpresaNome { get; set; }
        public string ContratoDescricao { get; set; }
        public int EmpresaId { get; set; }
        public int VeiculoId { get; set; }
        public string VeiculoFoto { get; set; }
        public string PlacaIdentificador { get; set; }
        public string EmpresaLogo { get; set; }
        public string EmpresaSigla { get; set; }
        public string EmpresaApelido { get; set; }
        public string Cnpj { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public string FormatIdguid { get; set; }
        public string Identificacao { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string IdentificacaoDescricao { get; set; }
        public bool IsencaoCobranca { get; set; }
        
        /// <summary>
        /// True, possue pendencia impeditiva
        /// </summary>
        public bool PendenciaImpeditiva { get; set; }

        public DateTime? DataStatus { get; set; } 
        public bool DevolucaoEntregaBo { get; set; }
        public string Email { get; set; }


    }
}