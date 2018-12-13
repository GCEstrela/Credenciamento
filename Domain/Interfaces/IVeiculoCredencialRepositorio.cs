// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IVeiculoCredencialRepositorio : IRepositorioBaseAdoNet<VeiculoCredencial>
    {
        #region  Metodos

        /// <summary>
        ///     Listar Veículos e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<VeiculosCredenciaisView> ListarView(params object[] objects);

        #endregion
    }
}