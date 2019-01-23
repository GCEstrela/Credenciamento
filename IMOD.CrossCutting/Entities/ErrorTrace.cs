// ***********************************************************************
// Project: CrossCutting
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

#endregion

namespace IMOD.CrossCutting.Entities
{
    public class ErrorTrace
    {
        #region  Propriedades

        /// <summary>
        ///     Data do erro
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///     Detalhe do erro
        /// </summary>
        public string Detalhe { get; set; }

        /// <summary>
        ///     Rastreamento do Erro
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        ///     Mensagem customizada
        /// </summary>
        public string Mensagem { get; set; }

        #endregion
    }
}