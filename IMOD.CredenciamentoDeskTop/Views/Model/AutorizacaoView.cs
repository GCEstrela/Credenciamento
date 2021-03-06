﻿using System;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class AutorizacaoView
    {
        #region  Propriedades

        public int VeiculoCredencialId { get; set; }
        public string EmpresaNome { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string PlacaIdentificador { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string Identificacao1 { get; set; }
        public string Identificacao2 { get; set; }
        public string Categoria { get; set; }
        public string Frota { get; set; }
        public string Lacre { get; set; }
        public string SerieChassi { get; set; }
        public string TipoServico { get; set; }
        public string PlacaFrota { get { return PlacaIdentificador + "  " + Frota; } }
        public bool AreaManobra { get; set; }
        public string Registro { get; set; }
        public string Portao { get; set; }

        #endregion

    }
}
