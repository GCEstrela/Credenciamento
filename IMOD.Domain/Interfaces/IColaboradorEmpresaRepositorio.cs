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
    public interface IColaboradorEmpresaRepositorio : IRepositorioBaseAdoNet<ColaboradorEmpresa>
    {
        /// <summary>
        ///     Listar Colaboradores e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradorEmpresa> ListarView(params object[] o);
    }
}