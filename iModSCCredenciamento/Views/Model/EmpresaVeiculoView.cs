// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region



#endregion

namespace iModSCCredenciamento.Views.Model
{
    public class EmpresaVeiculoView
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
        public int EstadoId { get; set; }
        public int MunicipioId { get; set; }
        public string Seguro { get; set; }
        public int EmpresaId { get; set; }
        public bool Ativo { get; set; }
        public int LayoutCrachaId { get; set; }
        public string LayoutCrachaNome { get; set; }
        public int FormatoCredencialId { get; set; }
        public string NumeroCredencial { get; set; }
        public string Fc { get; set; }

        #endregion
    }
}