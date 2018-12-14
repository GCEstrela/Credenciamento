// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace iModSCCredenciamento.Views.Model
{
    public class CredencialView
    {
        #region  Propriedades

        public int ColaboradorCredencialId { get; set; }
        public string Colete { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaApelido { get; set; }
        public string Cnpj { get; set; }
        public string Sigla { get; set; }
        public byte[] Logo { get; set; }
        public string ColaboradorNome { get; set; }
        public string ColaboradorApelido { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string RgOrgLocal { get; set; }
        public string RgOrgUf { get; set; }
        public string TelefoneEmergencia { get; set; }
        public byte[] Foto { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string CnhCategoria { get; set; }
        public string LayoutRpt { get; set; }

        #endregion
    }
}