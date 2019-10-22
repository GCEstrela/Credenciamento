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
    public class VeiculoView : ValidacaoModel
    {
        #region  Propriedades

        [Required(ErrorMessage = "A Definição é requerida.")]
        public int EquipamentoVeiculoId { get; set; }
        [Required(ErrorMessage = "A Descrição é requerida.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A Placa/Identificador é requerida.")]
        public string PlacaIdentificador { get; set; }
        public string Frota { get; set; }
        public string Patrimonio { get; set; }
        [Required(ErrorMessage = "A Marca é requerida.")]
        public string Marca { get; set; }
        [Required(ErrorMessage = "O Modelo é requerida.")]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "O Tipo é requerido.")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "A Cor é requerida.")]
        public string Cor { get; set; }
        [Required(ErrorMessage = "O Ano é requerida.")]
        public string Ano { get; set; }
        public int EstadoId { get; set; }
        public int MunicipioId { get; set; }
        [Required(ErrorMessage = "A Série/Chassi é requerido.")]
        public string SerieChassi { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O Tipo de Combustível é requerido.")]
        public int CombustivelId { get; set; }
        [Required(ErrorMessage = "A Altura é requerida.")]
        public string Altura { get; set; }
        [Required(ErrorMessage = "O Comprimento é requerida.")]
        public string Comprimento { get; set; }
        [Required(ErrorMessage = "A Largura é requerida.")]
        public string Largura { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A Natureza do Equipamento/Veículo é requerido.")]
        public int TipoEquipamentoVeiculoId { get; set; }
        public string Renavam { get; set; }
        public string Foto { get; set; }
        public bool Ativo { get; set; }
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
        public bool Precadastro { get; set; }
        public string Categoria { get; set; }
        //[Required(ErrorMessage = "A Data do Licenciamento é Obrigatória.")]
        public DateTime? DataLicenciameno { get; set; }
        //[Required(ErrorMessage = "A Data da Vistoria é Obrigatória.")]
        public DateTime? DataVistoria { get; set; }
        #endregion
    }
}