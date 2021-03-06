﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class VeiculoEmpresaView : ValidacaoModel
    {
        #region  Propriedades

        public int VeiculoEmpresaId { get; set; }
        public int VeiculoId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Empresa é requerida.")] 
        public int EmpresaId { get; set; } 
        public int EmpresaContratoId { get; set; }
        public string CardHolderGuid { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        [Required(ErrorMessage = "Ativo é requerida.")]
        public bool Ativo { get; set; }
        public bool AreaManobra { get; set; }
        #endregion
    }
}