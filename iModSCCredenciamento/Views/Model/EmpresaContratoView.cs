﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace iModSCCredenciamento.Views.Model
{
    public class EmpresaContratoView
    {
        #region  Propriedades

        public int EmpresaContratoId { get; set; }
        public int EmpresaId { get; set; }
        public string NumeroContrato { get; set; }
        public string Descricao { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Validade { get; set; }
        public bool Terceirizada { get; set; }
        public string Contratante { get; set; }
        public bool IsencaoCobranca { get; set; }
        public int TipoCobrancaId { get; set; }
        public int CobrancaEmpresaId { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public string NomeResp { get; set; }
        public string TelefoneResp { get; set; }
        public string CelularResp { get; set; }
        public string EmailResp { get; set; }
        public int StatusId { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }
        public int TipoAcessoId { get; set; }

        #endregion
    }
}