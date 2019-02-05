// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 13 - 2018
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using IMOD.CredenciamentoDeskTop.Funcoes;

namespace IMOD.CredenciamentoDeskTop.Views.Model
{
    public class EmpresaView:ValidacaoModel 
    {
        #region  Propriedades

        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "A Razão Social é requerida.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Apelido é requerido.")]
        public string Apelido { get; set; }
        [Required(ErrorMessage = "O Cnpj é requerido.")]
        public string Cnpj { get; set; }       
        public string InsEst { get; set; }
        public string InsMun { get; set; }
        public string Responsavel { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int EstadoId { get; set; }
        public int MunicipioId { get; set; }
        public string Email1 { get; set; }
        public string Contato1 { get; set; }
        public string Telefone1 { get; set; }
        public string Celular1 { get; set; }
        public string Email2 { get; set; }
        public string Contato2 { get; set; }
        public string Telefone2 { get; set; }
        public string Celular2 { get; set; }
        public string Obs { get; set; }
        public string Logo { get; set; } 
        public bool Ativo { get; set; }
        public bool Pendente { get; set; }
        public bool Pendente11 { get; set; }
        public bool Pendente12 { get; set; }
        public bool Pendente13 { get; set; }
        public bool Pendente14 { get; set; }
        public bool Pendente15 { get; set; }
        public bool Pendente16 { get; set; }
        public bool Pendente17 { get; set; }
        public string Sigla { get; set; }
        public int TotalPermanente { get; set; }
        public int TotalTemporaria { get; set; }

        #endregion
    }
}