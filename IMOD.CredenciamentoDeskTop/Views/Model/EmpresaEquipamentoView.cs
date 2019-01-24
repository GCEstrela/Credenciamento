// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class EmpresaEquipamentoView
    {
        #region  Propriedades

        public int EmpresaEquipamentoId { get; set; }
        public string Descricao { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Ano { get; set; }
        public string Patrimonio { get; set; }
        public string Seguro { get; set; }
        public string ApoliceSeguro { get; set; }
        public string ApoliceValor { get; set; }
        public string ApoliceVigencia { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string Excluido { get; set; }
        public int TipoEquipamentoId { get; set; }
        public int StatusId { get; set; }
        public int TipoAcessoId { get; set; }
        public int EmpresaId { get; set; }

        #endregion
    }
}