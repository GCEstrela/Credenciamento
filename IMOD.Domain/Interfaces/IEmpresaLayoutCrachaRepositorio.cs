// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IEmpresaLayoutCrachaRepositorio : IRepositorioBaseAdoNet<EmpresaLayoutCracha>
    {
        /// <summary>
        /// Remover tipo de atividades por empresa
        /// </summary>
        /// <param name="empresaId"></param>
        void RemoverPorEmpresa(int empresaId);

        /// <summary>
        /// Listar layout Crachas
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaView(params object[] objects);

        /// <summary>
        /// Listar layout Cracha por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        ICollection<EmpresaLayoutCrachaView> ListarLayoutCrachaPorEmpresaView(int idEmpresa,int tipoCracha);
        
    }
}