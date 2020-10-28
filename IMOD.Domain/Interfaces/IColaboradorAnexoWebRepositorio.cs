// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 21 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;
using System.Collections.Generic;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IColaboradorAnexoWebRepositorio : IRepositorioBaseAdoNet<ColaboradorAnexo>
    {
        #region  Metodos

        /// <summary>
        ///     ListarComAnexo
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        ICollection<ColaboradorAnexo> ListarComAnexo(params object[] objects);
        
        #endregion

    }
}