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
        /// Remover 
        /// </summary>
        /// <param name="equipamentoVeiculoId"></param>
        void RemoverPorVeiculo(int equipamentoVeiculoId);

        /// <summary>
        /// Listar layout Crachas
        /// </summary>
        /// <param name="equipamentoVeiculoId"></param>
        /// <returns></returns>
        ICollection<EquipamentoVeiculoTipoServicoView> ListarEquipamentoVeiculoTipoServicoView(int equipamentoVeiculoId);
    }
}