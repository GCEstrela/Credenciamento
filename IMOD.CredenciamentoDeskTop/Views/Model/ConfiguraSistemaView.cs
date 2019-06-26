﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 03 - 2019
// ***********************************************************************

using System.Collections.Generic;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ConfiguraSistemaView
    {
        #region  Propriedades

        public int ConfiguraSistemaId { get; set; }
        public string CNPJ { get; set; }
        public string NomeEmpresa { get; set; }
        public string Apelido { get; set; }
        public string EmpresaLOGO { get; set; }
        public bool Contrato { get; set; }
        public bool Colete { get; set; }
        public bool Regras { get; set; }
        public string Email { get; set; }
        public string Responsavel { get; set; }
        public string SMTP { get; set; }
        public string EmailUsuario { get; set; }
        public string EmailSenha { get; set; }
        public string UrlSistema { get; set; }
        public int imagemResolucao { get; set; }
        public int imagemTamanho { get; set; }
        public int arquivoTamanho { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string NomeAeroporto { get; set; }
        public List<string> Grupos { get; set; }
        #endregion
    }
}
