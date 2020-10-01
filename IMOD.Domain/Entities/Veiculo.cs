// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

using System;
using System.Web;

namespace IMOD.Domain.Entities
{
    public class Veiculo
    {
        #region  Propriedades

        public int EquipamentoVeiculoId { get; set; }
        public string Descricao { get; set; }
        public string PlacaIdentificador { get; set; }
        public string Frota { get; set; }
        public string Patrimonio { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }
        public string Cor { get; set; }
        public string Ano { get; set; }
        public int? EstadoId { get; set; }
        public int? MunicipioId { get; set; }
        public string SerieChassi { get; set; }
        public int? CombustivelId { get; set; }
        public string Altura { get; set; }
        public string Comprimento { get; set; }
        public string Largura { get; set; }
        public int? TipoEquipamentoVeiculoId { get; set; }
        public string Renavam { get; set; }
        public string Foto { get; set; }
        public bool Ativo { get; set; }
        public int? StatusId { get; set; }
        public int? TipoAcessoId { get; set; }
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
        public DateTime? DataLicenciameno { get; set; }
        public DateTime? DataVistoria { get; set; }
        public string Observacao { get; set; }
        public string NomeSeguradora { get; set; }
        public string NumeroApolice { get; set; }
        public decimal ValorCobertura { get; set; }
        public DateTime Emissao { get; set; }
        public DateTime Validade { get; set; }
        public string Arquivo { get; set; }
        public string NomeArquivo { get; set; }
        public int? StatusCadastro { get; set; }
        #endregion
    }
}