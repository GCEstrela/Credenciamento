using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class RelColaboradoresCredenciaisView
    {
        public int ColaboradorCredencialId { get; set; }
        public int ColaboradorEmpresaId { get; set; }
        public int TecnologiaCredencialId { get; set; }
        public int TipoCredencialId
        {
            get;
            set;
        }
        public int LayoutCrachaId { get; set; }
        public int FormatoCredencialId { get; set; }
        public string NumeroCredencial { get; set; }
        public int Fc { get; set; }
        private string _emissao;
       
        public string Emissao
        {
            get { return string.IsNullOrEmpty(_emissao) ? "" : System.DateTime.Parse(_emissao).ToString("dd/MM/yyyy"); }
            set { _emissao = value; }
        }
        private string _validade;
        public string Validade
        {
            get { return string.IsNullOrEmpty(_validade) ? "" : System.DateTime.Parse(_validade).ToString("dd/MM/yyyy"); }
            set { _validade = value; }
        }
        public int CredencialStatusId { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        public int ColaboradorPrivilegio1Id { get; set; }
        public int ColaboradorPrivilegio2Id { get; set; }
        public string Colete { get; set; }
        public int CredencialMotivoId { get; set; }
        public string CredencialMotivoId1 { get; set; }
        public string CredencialMotivoId2 { get; set; } 
        public string CredencialMotivoDescricao { get; set; }
        public bool Impressa { get; set; }
        public bool Ativa { get; set; }
        private string _baixa;
        public string Baixa
        {
            get { return string.IsNullOrEmpty(_baixa) ? "" : System.DateTime.Parse(_baixa).ToString("dd/MM/yyyy"); }
            set { _baixa = value; }
        }
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
        private string _matricula;
        public string Matricula
        {
            get
            {
                if (TipoCredencialId == 1)
                {
                    return _matricula;
                }
                else
                {
                    return "----";
                }
            }
            set { _matricula = value; }
        }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public bool IsencaoCobranca { get; set; }
        public string DataImpressao { get; set; }
        public string DataImpressaoFim { get; set; }
        public decimal Valor { get; set; }
        public string TiposCobrancaNome { get; set; }
        public string EmpresasContratosNome { get; set; }
        public string Identificacao { get; set; }
        public int AreaAcessoId { get; set; }
        public string IdentificacaoDescricao { get; set; }
        public bool DevolucaoEntregaBo { get; set; }
        private string _dataStatus;
        public string DataStatus
        {
            get { return string.IsNullOrEmpty(_dataStatus) ? "" : System.DateTime.Parse(_dataStatus).ToString("dd/MM/yyyy"); }
            set { _dataStatus = value; }
        }

        public int CredencialVia { get; set; }
        public int CredencialMotivoIDAnterior { get; set; }
        public string CredencialMotivoIDAnteriorDescricao { get; set; }
        public int CredencialMotivoViaAdicionalID { get; set; }
        public string CredencialMotivoViaAdicionalDescricao { get; set; }

    }
}
