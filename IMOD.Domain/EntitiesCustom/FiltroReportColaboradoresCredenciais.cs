using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class FiltroReportColaboradoresCredenciais
    {

        public int ColaboradorCredencialId { get; set; }
        public int ColaboradorEmpresaId { get; set; }
        public int TecnologiaCredencialId { get; set; }
        public int TipoCredencialId { get; set; }
        public int LayoutCrachaId { get; set; }
        public int FormatoCredencialId { get; set; }
        public string NumeroCredencial { get; set; }
        public int Fc { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? EmissaoFim { get; set; }
        public DateTime? Validade { get; set; }
        public DateTime? ValidadeFim { get; set; }
        public int CredencialStatusId { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        public int ColaboradorPrivilegio1Id { get; set; }
        public int ColaboradorPrivilegio2Id { get; set; }
        public string Colete { get; set; }
        public int CredencialMotivoId { get; set; }
        public int CredencialMotivoId1 { get; set; }
        public int CredencialMotivoId2 { get; set; }
        public string CredencialMotivoDescricao { get; set; }
        public bool Impressa { get; set; }
        public bool Ativa { get; set; }
        public DateTime? Baixa { get; set; }
        public DateTime? BaixaFim { get; set; }
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

        public DateTime? DataImpressao { get; set; }
        public DateTime? DataImpressaoFim { get; set; }
        public decimal Valor { get; set; }
        public string TiposCobrancaNome { get; set; }
        public string EmpresasContratosNome { get; set; }
        public int Periodo { get; set; }
        public int Identificacao { get; set; }
        public int AreaAcessoId { get; set; }

        public DateTime? DataStatus { get; set; }
        public bool Impeditivo { get; set; }
        public DateTime? DataStatusFim { get; set; }
        public bool DevolucaoEntregaBo { get; set; } 
        public bool? flaTodasDevolucaoEntregaBO { get; set; }

        public bool flaTodasAtivosInativos { get; set; }
        public int emissaoValidade { get; set; }
        public int CredencialVia { get; set; }
        public int CredencialMotivoViaAdicionalId{ get; set; }
        public int CredencialMotivoIdAnterior{ get; set; }

    }


}
