﻿// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;
using System.Collections.Generic;

#endregion

namespace IMOD.Domain.Interfaces
{
    public interface IColaboradorCursoRepositorio : IRepositorioBaseAdoNet<ColaboradorCurso>
    {
        /// <summary>
        ///     Listar Colaboradores cursos 
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<ColaboradorCurso> ListarView(params object[] objects);
    }
}