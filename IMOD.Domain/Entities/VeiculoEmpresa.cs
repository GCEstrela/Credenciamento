// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class VeiculoEmpresa
    {
        #region  Propriedades

        public int VeiculoEmpresaId { get; set; }
        public string CardHolderGuid { get; set; }
        public int VeiculoId { get; set; }
        public int? EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }
        public bool AreaManobra { get; set; }


        public override bool Equals(object obj)
        {
            var contrato = obj as VeiculoEmpresa;
            return contrato != null &&
                   EmpresaContratoId == contrato.EmpresaContratoId;
        }

        //public override int GetHashCode()
        //{
        //    return 1502971449 + EmpresaContratoId.GetHashCode();
        //}

        public VeiculoEmpresa(int empresaContratoId)
        {
            EmpresaContratoId = empresaContratoId;
        }

        public VeiculoEmpresa()
        {
        }
        #endregion
    }
}