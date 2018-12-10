using System;

namespace IMOD.Domain.EntitiesCustom
{
    public class VeiculosCredenciaisView
    {
        public int VeiculoCredencialId { get; set; }
        public int VeiculoEmpresaId { get; set; }
        public int TecnologiaCredencialId { get; set; }
        public int TipoCredencialId { get; set; }
        public int LayoutCrachaId { get; set; }
        public int FormatoCredencialId { get; set; }
        public string NumeroCredencial { get; set; }
        public int Fc { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }
        public int CredencialStatusId { get; set; }
        public string CredencialGuid { get; set; }
        public string CardHolderGuid { get; set; }
        public int VeiculoPrivilegio1Id { get; set; }
        public int VeiculoPrivilegio2Id { get; set; }
        public string Colete { get; set; }
        public int CredencialMotivoId { get; set; }
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

    }
}