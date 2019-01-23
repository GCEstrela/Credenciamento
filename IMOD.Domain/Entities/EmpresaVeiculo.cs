// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

namespace IMOD.Domain.Entities
{
    public class EmpresaVeiculo
    {
        #region  Propriedades

        public int EmpresaVeiculoId { get; set; }
        public string Validade { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Ano { get; set; }
        public string Cor { get; set; }
        public string Placa { get; set; }
        public string Renavam { get; set; }
        public int? EstadoId { get; set; }
        public int? MunicipioId { get; set; }
        public string Seguro { get; set; }
        public int? EmpresaId { get; set; }
        public bool Ativo { get; set; }
        public int? LayoutCrachaId { get; set; }
        public string NumeroCredencial { get; set; }
        public string Fc { get; set; }
        public int? FormatoCredencialId { get; set; }

        #endregion
    }
}