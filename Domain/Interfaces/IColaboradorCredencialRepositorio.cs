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
    public interface IColaboradorCredencialRepositorio : IRepositorioBaseAdoNet<ColaboradorCredencial>
    {
        #region  Metodos

        /// <summary>
        ///     Listar Colaboradores e suas credenciais
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradoresCredenciaisView> ListarView(params object[] o);

        /// <summary>
        ///     Listar dados de Credencial (Impressão)
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<CredencialView> ListarCredencialView(int id);

        #endregion
    }
}