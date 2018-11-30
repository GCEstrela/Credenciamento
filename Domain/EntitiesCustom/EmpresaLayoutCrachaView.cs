﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMOD.Domain.EntitiesCustom
{
    public class EmpresaLayoutCrachaView
    {
        #region  Propriedades

        public int LayoutCrachaId { get; set; }
        public string Nome { get; set; }
        public string LayoutCrachaGuid { get; set; }
        public decimal? Valor { get; set; }
        public int EmpresaLayoutCrachaId { get; set; }
        public int EmpresaId { get; set; }

        #endregion
    }
}
