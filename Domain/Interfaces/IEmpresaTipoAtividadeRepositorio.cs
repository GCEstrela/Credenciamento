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
    public interface IEmpresaTipoAtividadeRepositorio : IRepositorioBaseAdoNet<EmpresaTipoAtividade>
    {
        /// <summary>
        /// Listar layout Crachas
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        ICollection<EmpresaTipoAtividadeView> ListarEmpresaTipoAtividadeView(params object[] objects);
    }
}