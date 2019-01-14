using System;

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class RenovarLicencaViewModel
    {
        #region Propriedades
        /// <summary>
        /// Identificador da licença
        /// </summary>
        public int IdLicenca { get; set; }

        /// <summary>
        ///     Data termino da renovação
        /// </summary>
        public DateTime DataTerminoRenovacao { get; set; }

        #endregion
    }
}