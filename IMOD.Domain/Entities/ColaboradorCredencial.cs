// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;

#endregion

namespace IMOD.Domain.Entities
{
    public class ColaboradorCredencial
    {
        #region  Propriedades

        public int ColaboradorCredencialId { get; set; }
        public int ColaboradorEmpresaId { get; set; }
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
        public int ColaboradorPrivilegio1Id { get; set; }
        public int ColaboradorPrivilegio2Id { get; set; }
        public string Colete { get; set; }
        public int CredencialMotivoId { get; set; }
        public bool Impressa { get; set; }
        public bool Ativa { get; set; }
        public DateTime? Baixa { get; set; }
        public DateTime? DataStatus { get; set; }
        public int ColaboradorId { get; set; }
        public string ColaboradorNome { get; set; }
        public bool Policiafederal { get; set; }
        public bool Receitafederal { get; set; }
        public bool Segurancatrabalho { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string Email { get; set; }
        public string Obs { get; set; }
        public bool DevolucaoEntregaBo { get; set; }
        public bool Regras { get; set; }
        public int? CredencialVia { get; set; }
        public int? CredencialmotivoViaAdicionalID { get; set; }
        public int? CredencialmotivoIDanterior { get; set; }
        public string CredencialmotivoViaAdicionalDescricao { get; set; }
        public string CredencialmotivoIdAnteriorDescricao { get; set; }
        public bool Estrangeiro { get; set; }
        public string RNE { get; set; }
        public string Usuario { get; set; }
        public List<Guid> listadeGrupos { get; set; }
        #endregion
    }
}