// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 30 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.EntitiesCustom
{
    public class EstadoView
    {
        #region  Propriedades

        public int EstadoId { get; set; }
        public string Nome { get; set; }
        public string Uf { get; set; }
        public ICollection<Municipio> Municipios { get; set; }

        #endregion
    }
}