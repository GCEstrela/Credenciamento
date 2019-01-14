using System;

namespace IMOD.CredenciamentoWeb.ViewModel
{
    public class LicencasViewModel
    {
        /// <summary>
        /// Idenntificador da licença
        /// </summary>
        public int IdLicenca { get; set; }
        /// <summary>
        /// CNPJ
        /// </summary>
        public string Cnpj { get; set; }
        /// <summary>
        /// String contendo XML com dados da licença
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Identificador do usuario
        /// </summary>
        public int IdUser { get; set; }
        /// <summary>
        /// Data em que se expira a licença
        /// </summary>
        public DateTime DataExpira { get; set; }
        /// <summary>
        /// Nomeidetificador da licença
        /// <para>Usar o HardWareInfo </para>
        /// </summary>
        public string NomeIdentificador { get; set; }

    }
}