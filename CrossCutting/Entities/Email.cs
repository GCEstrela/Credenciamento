// ***********************************************************************
// Project: CrossCutting
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;

#endregion

namespace IMOD.CrossCutting.Entities
{
    public class Email
    {
        #region  Propriedades

        public string TimeOut { get; set; }
        public string Porta { get; set; }
        public string ServidorEmail { get; set; }
        public bool UsarSsl { get; set; }
        public bool UsarTls { get; set; }
        public bool UsarAutenticacao { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string EmailRemetente { get; set; }
        public string NomeRemetente { get; set; }
        public List<string> EmailDestinatario { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
        public string Titulo { get; set; }

        public List<byte[]> Anexos { get; set; }

        #endregion
    }
}