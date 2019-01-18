﻿// ***********************************************************************
// Project: IMOD.Domain
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 22 - 2018
// ***********************************************************************

#region

using IMOD.Domain.Entities;
using IMOD.Domain.EntitiesCustom;
using System.Collections.Generic;

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

        /// <summary>
        ///     Listar dados de Autorização de Veículo
        /// </summary>
        /// <param name="o">Arrays de Parametros</param>
        /// <returns></returns>
        ICollection<AutorizacaoView> ListarAutorizacaoView(params object[] objects);

        #endregion
    }
}