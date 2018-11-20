// ***********************************************************************
// Project: Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using Domain.Entities;
using Domain.EntitiesCustom;

#endregion

namespace Domain.Interfaces
{
    public interface IColaboradorCredencialRepositorio : IRepositorioBaseAdoNet<ColaboradorCredencial>
    {
        /// <summary>
        /// Listar Colaboradores e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o);
    }
}