// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

#region

using System;
using IMOD.CredenciamentoDeskTop.Funcoes;

#endregion

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class ColaboradorView: PropertyValidateModel
    {
        #region  Propriedades

        public int ColaboradorId { get; set; } 
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string NomePai { get; set; }
        public string NomeMae { get; set; }
        public string Nacionalidade { get; set; }
        public string Foto { get; set; }
        public string EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime? RgEmissao { get; set; }
        public string RgOrgLocal { get; set; }
        public string RgOrgUf { get; set; }
        public string Passaporte { get; set; }
        public DateTime? PassaporteValidade { get; set; }
        public string Rne { get; set; }
        public string TelefoneFixo { get; set; }
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        public string ContatoEmergencia { get; set; }
        public string TelefoneEmergencia { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public bool Motorista { get; set; }
        public string CnhCategoria { get; set; }
        public string Cnh { get; set; }
        public DateTime? CnhValidade { get; set; }
        public string CnhEmissor { get; set; }
        public string Cnhuf { get; set; }
        public string Bagagem { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public int Excluida { get; set; }
        public int StatusId { get; set; }
        public int TipoAcessoId { get; set; }
        public bool Pendente { get; set; }
        public bool Pendente21 { get; set; }
        public bool Pendente22 { get; set; }
        public bool Pendente23 { get; set; }
        public bool Pendente24 { get; set; }
        public bool Pendente25 { get; set; }
        public bool Pendente26 { get; set; }
        public bool Pendente27 { get; set; }

        #endregion
    }
}