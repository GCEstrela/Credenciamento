// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 03 - 2019
// ***********************************************************************

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
        #endregion
    }
}
