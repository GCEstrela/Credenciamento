// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorEmpresaView:ValidacaoModel
    {
        #region  Propriedades
       
        public int ColaboradorEmpresaId { get; set; }
        public int ColaboradorId { get; set; } 
        [Range(1, int.MaxValue,ErrorMessage = "A Razão Social é requerida.")]
        public int EmpresaId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "O Contrato é requerido.")]
        public int EmpresaContratoId { get; set; }
        public string CardHolderGuid { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
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
        #endregion
    }
}