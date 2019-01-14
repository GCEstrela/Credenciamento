 
#region

using System;

#endregion

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class FaturasViewModel
    {
        #region Propriedades

        public int IdUser { get; set; }
        public string Nome { get; set; }
        public int IdLicenca { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorLicenca { get; set; }
        public string Status { get; set; }
        public string NomeIdentificador { get; set; }

        #endregion
    }
}