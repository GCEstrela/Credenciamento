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
    public interface IEquipamentoVeiculoTipoServicoRepositorio : IRepositorioBaseAdoNet<EquipamentoVeiculoTipoServico>
    {
        /// <summary>
        /// Listar layout Crachas
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        ICollection<EquipamentoVeiculoTipoServicoView> ListarEquipamentoVeiculoTipoServicoView(params object[] objects);
    }
}