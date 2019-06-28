// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace IMOD.Domain.Entities
{
    public class VeiculoCredencial
    {
        #region  Propriedades

        public int VeiculoCredencialId { get; set; }
        public int? VeiculoEmpresaId { get; set; }
        public int? TecnologiaCredencialId { get; set; }
        public int TipoCredencialId { get; set; }
        public int? LayoutCrachaId { get; set; }
        public int? FormatoCredencialId { get; set; }
        public string NumeroCredencial { get; set; }
        public int? Fc { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }
        public int CredencialStatusId { get; set; }
        public string CardHolderGuid { get; set; }
        public string CredencialGuid { get; set; }
        public int? VeiculoPrivilegio1Id { get; set; }
        public int? VeiculoPrivilegio2Id { get; set; }
        public bool Ativa { get; set; }
        public string Colete { get; set; }
        public int? CredencialMotivoId { get; set; }
        public DateTime? Baixa { get; set; }
        public bool Impressa { get; set; }

        public DateTime? DataStatus { get; set; } 
        public bool DevolucaoEntregaBo { get; set; }
        public string Email { get; set; }
        public string IdentificacaoDescricao { get; set; }
        #endregion
    }
}