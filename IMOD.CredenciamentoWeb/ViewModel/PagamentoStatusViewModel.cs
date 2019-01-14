using System;

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class PagamentoStatusViewModel
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int IdPagamento { get; set; }
        /// <summary>
        /// Identifiador da licença
        /// </summary>
        public int IdLicenca { get; set; }
        /// <summary>
        /// Forma de pagamento
        /// </summary>
        public string FormaPagamento { get; set; }
        /// <summary>
        /// Valor da licença
        /// </summary>
        public decimal ValorLicenca { get; set; }
        /// <summary>
        /// Numero de parcela
        /// </summary>
        public int NumeroParcela { get; set; }

        public DateTime DataInicioParcela { get; set; }
        /// <summary>
        /// Nome do usuario responsavel pela operação
        /// </summary>
        public string NomeOperador { get; set; }
        /// <summary>
        /// Status do pagamento
        /// </summary>
        public string Status { get; set; }
    }
}