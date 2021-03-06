﻿// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using System.Collections.Generic;
using IMOD.Domain.Entities;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IMunicipioRepositorio
    {
        /// <summary>
        ///     Buscar pela chave primaria
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns></returns>
        Municipio BuscarPelaChave(int id);

        /// <summary>
        ///     Listar
        /// </summary>
        /// <returns></returns>
        ICollection<Municipio> Listar(params object[] objects);


    }
}