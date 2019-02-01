// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class VeiculoView : ValidacaoModel
    {
        #region  Propriedades

        
        public int EquipamentoVeiculoId { get; set; }
        [Required(ErrorMessage = "O campo Descrição é requerido!")]
        public string Descricao { get; set; }
        public string PlacaIdentificador { get; set; }
        public string Frota { get; set; }
        public string Patrimonio { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        [Required(ErrorMessage = "O campo Tipo é requerido!")]
        public string Tipo { get; set; }
        public string Cor { get; set; }
        public string Ano { get; set; }
        public int EstadoId { get; set; }
        public int MunicipioId { get; set; }
        public string SerieChassi { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O campo combustível requerido.")]
        public int CombustivelId { get; set; }
        public string Altura { get; set; }
        public string Comprimento { get; set; }
        public string Largura { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O campo tipo equipamento requerido.")]
        public int TipoEquipamentoVeiculoId { get; set; }
        public string Renavam { get; set; }
        public string Foto { get; set; }
        public int Excluida { get; set; }
        public int StatusId { get; set; }
        public int TipoAcessoId { get; set; }
        public string DescricaoAnexo { get; set; }
        public string NomeArquivoAnexo { get; set; }
        public string ArquivoAnexo { get; set; }
        public bool Pendente31 { get; set; }
        public bool Pendente32 { get; set; }
        public bool Pendente33 { get; set; }
        public bool Pendente34 { get; set; }
        public string DescricaoAlias { get { return Descricao; } }

        #endregion
    }
}