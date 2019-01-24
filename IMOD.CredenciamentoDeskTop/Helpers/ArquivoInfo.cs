// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 06 - 2018
// ***********************************************************************

namespace IMOD.CredenciamentoDeskTop.Helpers
{
    public class ArquivoInfo
    {
        #region  Propriedades

        /// <summary>
        ///     Nome do arquivo
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        ///     Array de bytes do arquivo
        /// </summary>
        public byte[] ArrayBytes { get; set; }

        /// <summary>
        ///     Formato do arquivo em base 64
        /// </summary>
        public string FormatoBase64 { get; set; }

        #endregion
    }
}