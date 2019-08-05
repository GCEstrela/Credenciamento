using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class FiltroReportVeiculoCredencial
    {
        #region  Propriedades

        public int VeiculoCredencialId { get; set; }
        public int VeiculoEmpresaId { get; set; }
        public int VeiculoId { get; set; }
        public string ContratoDescricao { get; set; }
        public string EmpresaNome { get; set; }
        public string VeiculoNome { get; set; }
        public string PlacaIdentificador { get; set; }
        public string Matricula { get; set; }
        public int TecnologiaCredencialId { get; set; }
        public string TecnologiaCredencialDescricao { get; set; }
        public int TipoCredencialId { get; set; }
        public string TipoCredencialDescricao { get; set; }
        public int LayoutCrachaId { get; set; }
        public int EmpresaLayoutCrachaId { get; set; }
        public string LayoutCrachaNome { get; set; }
        public int FormatoCredencialId { get; set; }
        public string FormatoCredencialDescricao { get; set; }
        public string NumeroCredencial { get; set; }
        public int Fc { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? EmissaoFim { get; set; }
        public DateTime? Validade { get; set; }
        public DateTime? ValidadeFim { get; set; }
        public int CredencialStatusId { get; set; }
        public string CredencialStatusDescricao { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        public int VeiculoPrivilegio1Id { get; set; }
        public int VeiculoPrivilegio2Id { get; set; }
        public string PrivilegioDescricao1 { get; set; }
        public string PrivilegioDescricao2 { get; set; }
        public bool Ativa { get; set; }
        public string ColaboradorApelido { get; set; }
        public bool Motorista { get; set; }
        public string Placa { get; set; }
        public string VeiculoFoto { get; set; }
        public string EmpresaLogo { get; set; }
        public int EmpresaId { get; set; }
        public string EmpresaApelido { get; set; }
        public string Cnpj { get; set; }
        public string Cargo { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public string FormatIdguid { get; set; }
        public string Colete { get; set; }
        public string EmpresaSigla { get; set; }
        public int CredencialMotivoId { get; set; }
        public int CredencialMotivoId1 { get; set; }
        public int CredencialMotivoId2 { get; set; }
        public string CredencialMotivoDescricao { get; set; }
        public bool Impressa { get; set; }
        public DateTime? Baixa { get; set; }
        public DateTime? BaixaFim { get; set; }
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
        public bool ? DevolucaoEntregaBo { get; set; }
        public bool Impeditivo { get; set; }
        public DateTime? DataStatusFim { get; set; }
        public bool? flaTodasDevolucaoEntregaBO { get; set; }
        public int emissaoValidade { get; set; }
        #endregion


    }


}
