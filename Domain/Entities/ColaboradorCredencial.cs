// ***********************************************************************
// Project: Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace Domain.Entities
{
    public class ColaboradorCredencial
    {
        public int ColaboradorCredencialId { get; set; }
        public int ColaboradorEmpresaId { get; set; }
        public int ColaboradorId { get; set; }
        public string ContratoDescricao { get; set; }
        public string EmpresaNome { get; set; }
        public string ColaboradorNome { get; set; }
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
        public DateTime? Validade { get; set; }
        public int CredencialStatusId { get; set; }
        public string CredencialStatusDescricao { get; set; }
        public Guid? CredencialGuid { get; set; }
        public Guid? CardHolderGuid { get; set; }
        public int ColaboradorPrivilegio1Id { get; set; }
        public int ColaboradorPrivilegio2Id { get; set; }
        public string PrivilegioDescricao1 { get; set; }
        public string PrivilegioDescricao2 { get; set; }
        public bool Ativa { get; set; }
        public string ColaboradorApelido { get; set; }
        public bool Motorista { get; set; }
        public string Cpf { get; set; }
        public string ColaboradorFoto { get; set; }
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
        public bool Impressa { get; set; }
        public DateTime? Baixa { get; set; }
    }
}