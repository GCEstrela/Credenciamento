using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class EmpresaRepresentanteViewModel
    {
        #region  Propriedades

        public int EmpresaSignatarioId { get; set; }
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public bool Principal { get; set; }
        public string Assinatura { get; set; }
        public string NomeArquivo { get; set; }
        public string RG { get; set; }
        public string OrgaoExp { get; set; }
        public string RGUF { get; set; }
        public int TipoRepresentanteId { get; set; }
        #endregion
    }
}