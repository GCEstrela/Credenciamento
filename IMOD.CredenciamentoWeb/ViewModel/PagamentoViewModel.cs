//  *********************************************************************************************************
// Empresa: DSBR - Empresa de Desenvolvimento de Sistemas
// Sistema: Automação Comercial
// Projeto: DSBR.ServicesWeb
// Autores: 
// Valnei Filho e-mail: vbatistas@devsysbrasil.com.br;
// Vagner Marcelo e-mail: vmarcelo@devsysbrasil.com.br
// Data Criação:07/09/2018
// Todos os direitos reservados
//  *********************************************************************************************************

#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class PagamentoViewModel
    {
        #region Propriedades

        /// <summary>
        ///     Id da licenca criptografada
        /// </summary>
        public string IdLicencaCript { get; set; }

        /// <summary>
        ///     Identificador da licença
        /// </summary>
        public int IdLicenca { get; set; }

        /// <summary>
        ///     Forma de pagamento
        /// </summary>
        public string FormaPagamento { get; set; }

        /// <summary>
        ///     Valor da licença
        /// </summary>
        public decimal ValorLicenca { get; set; }

        /// <summary>
        ///     Numero de parcelas
        /// </summary>
        public int NumeroParcela { get; set; }

        /// <summary>
        ///     Intervalo entre parcelas
        /// </summary>
        public int IntervaloParcela { get; set; }

        /// <summary>
        ///   Quantidade de dias para a 1 parcela
        /// </summary>
        public int QtdDiasPrimeriParcela { get; set; }



        /// <summary>
        ///     Data inicial da parcela
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInicioParcela { get; set; }

        #endregion
    }
}