using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class RelVeiculosCredenciaisView
    {
        public int VeiculoCredencialId { get; set; }
        public int VeiculoEmpresaId { get; set; }
        public int TecnologiaCredencialId { get; set; }
        public int TipoCredencialId { get; set; }
        public int LayoutCrachaId { get; set; }
        public int FormatoCredencialId { get; set; }
        public string NumeroCredencial { get; set; }
        public string Matricula { get; set; }
        public int Fc { get; set; }
        private string _emissao;
        public string Emissao { get{ return System.DateTime.Parse(_emissao).ToString("dd/MM/yyyy"); } set { _emissao = value; } } 
        private string _validade;
        public string Validade { get { return System.DateTime.Parse(_validade).ToString("dd/MM/yyyy"); } set { _validade = value; } } 
        public string PlacaIdentificador { get; set; }
        public int CredencialStatusId { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        public int VeiculoPrivilegio1Id { get; set; }
        public int VeiculoPrivilegio2Id { get; set; }
        public string Colete { get; set; }
        public int CredencialMotivoId { get; set; }
        public int CredencialMotivoId1 { get; set; }
        
        public string CredencialMotivoDescricao { get; set; }
        public bool Impressa { get; set; }
        public bool Ativa { get; set; }
        private string _baixa;
        public string Baixa { get { return System.DateTime.Parse(_baixa).ToString("dd/MM/yyyy"); } set { _baixa = value; } } 
        public string LayoutCrachaNome { get; set; }
        public string FormatoCredencialDescricao { get; set; }
        public string VeiculoNome { get; set; }
        public string EmpresaNome { get; set; }
        public string ContratoDescricao { get; set; }
        public int EmpresaId { get; set; }
        public int VeiculoId { get; set; }
        public string VeiculoFoto { get; set; }
        public string Cpf { get; set; }
        public bool Motorista { get; set; }
        public string VeiculoApelido { get; set; }
        public string EmpresaLogo { get; set; }
        public string EmpresaSigla { get; set; }
        public string EmpresaApelido { get; set; }
        public string Cargo { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public string Cnpj { get; set; }
        public string FormatIdguid { get; set; }
        public bool IsencaoCobranca { get; set; }
        public string TipoCredencialDescricao { get; set; }
        public string CredencialStatusDescricao { get; set; }
        public string DataImpressao { get; set; }

        public decimal Valor { get; set; }
        public string TiposCobrancaNome { get; set; }
        public string EmpresasContratosNome { get; set; }
        public string Identificacao { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string IdentificacaoDescricao { get; set; }
        public bool DevolucaoEntregaBo { get; set; }
        public string  DataStatus { get; set; }

    }
}
