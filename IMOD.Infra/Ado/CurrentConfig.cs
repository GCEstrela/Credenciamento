﻿// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region

#endregion

using IMOD.Infra.Properties;

namespace IMOD.Infra.Ado
{
    /// <summary>
    ///     Obtem configurações correntes
    /// </summary>
    public static class CurrentConfig
    {
        #region  Propriedades

        /// <summary>
        ///     String de conexao com o banco de dados
        /// </summary>
        public static string ConexaoString => Settings.Default.Credenciamento;

        #endregion
    }
}