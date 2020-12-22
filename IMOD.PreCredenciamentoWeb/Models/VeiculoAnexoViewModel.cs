using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class VeiculoAnexoViewModel
    {
        public VeiculoAnexoViewModel()
        {

        }

        #region  Propriedades
        [Key]
        public int VeiculoAnexoId { get; set; }
        public int VeiculoAnexoWebId { get; set; }
        public int VeiculoId { get; set; }
        public string Descricao { get; set; }
        public string NomeArquivo { get; set; }
        public string Arquivo { get; set; }
        #endregion
    }
}
