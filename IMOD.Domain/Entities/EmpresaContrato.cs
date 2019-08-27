// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace IMOD.Domain.Entities
{
    public class EmpresaContrato
    {
        #region  Propriedades

        public int EmpresaContratoId { get; set; }
        public int  EmpresaId { get; set; }
        public string NumeroContrato { get; set; }
        public string Descricao { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime  Validade { get; set; }
        public bool Terceirizada { get; set; }
        public string Contratante { get; set; }
        public bool IsencaoCobranca { get; set; }
        public int? TipoCobrancaId { get; set; }
        public int? CobrancaEmpresaId { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public int? MunicipioId { get; set; }
        public int? EstadoId { get; set; }
        public string NomeResp { get; set; }
        public string TelefoneResp { get; set; }
        public string CelularResp { get; set; }
        public string EmailResp { get; set; }
        public int? StatusId { get; set; }
        public string Arquivo { get; set; }
        public int? TipoAcessoId { get; set; }
        public string NomeArquivo { get; set; }
        public  byte [] ArquivoBlob { get; set; }
        public bool ContratoBasico { get; set; }
        public string TerceirizadaNome { get; set; }
        public int? PraVencer { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public override bool Equals(object obj)
        {
            var contrato = obj as EmpresaContrato;
            return contrato != null &&
                   EmpresaContratoId == contrato.EmpresaContratoId;
        }

        public override int GetHashCode()
        {
            return 1502971449 + EmpresaContratoId.GetHashCode();
        }

        public EmpresaContrato(int empresaContratoId)
        {
            EmpresaContratoId = empresaContratoId;
        }

        public EmpresaContrato()
        {
        }


        #endregion
    }
}