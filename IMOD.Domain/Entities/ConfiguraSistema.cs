// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 03 - 2019
// ***********************************************************************

using System.Collections.Generic;
using System.Data;

namespace IMOD.Domain.Entities
{
    public class ConfiguraSistema
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
        public string GrupoPadrao { get; set; }
        public string UrlSistemaPreCadastro { get; set; }
        public bool AssociarGrupos { get; set; }
        public int diasContencao { get; set; }
        public int PortaSMTP { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string Licenca { get; set; }
        #endregion
    }
}
