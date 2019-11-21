// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace IMOD.Domain.Entities
{
    public class ColaboradorEmpresa
    {
        #region  Propriedades

        public int ColaboradorEmpresaId { get; set; }
        public string CardHolderGuid { get; set; }
        public int ColaboradorId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaSigla { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }
        public string NomeAnexo { get; set; }
        public string Anexo { get; set; }
        public DateTime? Validade { get; set; }
        public bool ManuseioBagagem { get; set; }
        public bool OperadorPonteEmbarque { get; set; }
        public bool FlagCcam { get; set; }
        public bool Motorista { get; set; }
        public bool FlagAuditoria { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Usuario { get; set; }
        
        public override bool Equals(object obj)
        {
            var empresa = obj as ColaboradorEmpresa;
            return empresa != null &&
                   ColaboradorEmpresaId == empresa.ColaboradorEmpresaId;
        }

        public override int GetHashCode()
        {
            return -2141522505 + ColaboradorEmpresaId.GetHashCode();
        }

        public ColaboradorEmpresa(int colaboradorEmpresaId)
        {
            ColaboradorEmpresaId = colaboradorEmpresaId;
        }

        public ColaboradorEmpresa()
        {
        }
        public bool Terceirizada { get; set; }
        public string TerceirizadaSigla { get; set; }
        public List<Guid> listadeGrupos { get; set; }
        public bool grupoAlterado { get; set; }
        #endregion


    }
}