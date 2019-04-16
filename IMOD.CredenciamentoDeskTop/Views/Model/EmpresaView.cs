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
    public class EmpresaView:ValidacaoModel ,ICloneable
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
        [Required(ErrorMessage = "O Cep é requerido.")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O Endereço é requerido.")]
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        [Required(ErrorMessage = "O Bairro é requerido.")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "O Estado é requerido.")]
        public int EstadoId { get; set; }
        [Required(ErrorMessage = "O Município é requerido.")]
        public int MunicipioId { get; set; }
        [Required(ErrorMessage = "O E-Mail é requerido.")]
        public string Email1 { get; set; }
        [Required(ErrorMessage = "O Contato é requerido.")]
        public string Contato1 { get; set; }
        [Required(ErrorMessage = "O Telefone é requerido.")]
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
        [Required(ErrorMessage = "A Gigla é requerida.")]
        public string Sigla { get; set; }       
        public int TotalPermanente { get; set; }
        public int TotalTemporaria { get; set; }
        public int? PraVencer { get; set; }
        public string PraVencerTooltip { get { return "Vencimento de Contrato em.: " + PraVencer.ToString(); } }

        #endregion

        
        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return (EmpresaView)MemberwiseClone();
        }
    }
}