// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 17 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface ITipoPendenciaRepositorio
    {
        #region  Metodos

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<TipoPendencia> Listar();

        /// <summary>
        ///     Obter Tipo Pendencia por codigo
        /// </summary>
        /// <param name="codPendencia"></param>
        /// <returns></returns>
        TipoPendencia BuscarPorCodigo(string codPendencia);

        #endregion
    }
}