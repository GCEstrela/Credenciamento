using IMOD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class VeiculoObservacaoViewModel
    {
        public int VeiculoObservacaoId { get; set; }
        public int VeiculoId { get; set; }
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        public bool Impeditivo { get; set; }
        public bool Resolvido { get; set; }
        public int UsuarioRevisao { get; set; }
        public DateTime DataRevisao { get; set; }
        public int? VeiculoObservacaoRespostaId { get; set; }
        public string ObservacaoResposta { get; set; }

        public string UsuarioRevisaoInfo 
        {   get 
            {                
                return Funcoes.GetDescription((UsuarioRevisao)UsuarioRevisao);
            } 
        }
    }
}