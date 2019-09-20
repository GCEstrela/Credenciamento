using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMOD.PreCredenciamentoWeb.Models
{
    public class VeiculoEmpresaViewModel
    {
        #region  Propriedades

        public int VeiculoEmpresaId { get; set; }
        public string CardHolderGuid { get; set; }
        public int EmpresaId { get; set; }
        public int EmpresaContratoId { get; set; }
        public string Descricao { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaSigla { get; set; }
        public string Matricula { get; set; }
        public bool Ativo { get; set; }
        public string NomeAnexo { get; set; }
        public string Anexo { get; set; }
        public bool FlagAreaManobra { get; set; }
        #endregion

        public override bool Equals(object obj)
        {
            var veiculo = obj as VeiculoEmpresaViewModel;
            return veiculo != null &&
                   VeiculoEmpresaId == veiculo.VeiculoEmpresaId;
        }

        //public override int GetHashCode()
        //{
        //    return -2141522505 + ColaboradorEmpresaId.GetHashCode();
        //}

        public VeiculoEmpresaViewModel(int veiculoEmpresaId)
        {
            VeiculoEmpresaId = veiculoEmpresaId;
        }

        public VeiculoEmpresaViewModel()
        {
        }
    }
}
