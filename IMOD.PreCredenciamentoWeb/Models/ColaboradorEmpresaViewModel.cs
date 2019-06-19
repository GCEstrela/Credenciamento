using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class ColaboradorEmpresaViewModel
    {
        #region  Propriedades

        public int ColaboradorEmpresaId { get; set; }
        public string CardHolderGuid { get; set; }
        public int ColaboradorId { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaSigla { get; set; }
        public string Cargo { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }
        public string NomeAnexo { get; set; }
        public string Anexo { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Estado")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Validade { get; set; }
        public bool ManuseioBagagem { get; set; }
        public bool OperadorPonteEmbarque { get; set; }
        public bool FlagCcam { get; set; }
        #endregion

        public override bool Equals(object obj)
        {
            var empresa = obj as ColaboradorEmpresaViewModel;
            return empresa != null &&
                   ColaboradorEmpresaId == empresa.ColaboradorEmpresaId;
        }

        public override int GetHashCode()
        {
            return -2141522505 + ColaboradorEmpresaId.GetHashCode();
        }

        public ColaboradorEmpresaViewModel(int colaboradorEmpresaId)
        {
            ColaboradorEmpresaId = colaboradorEmpresaId;
        }

        public ColaboradorEmpresaViewModel()
        {
        }
    }
}
