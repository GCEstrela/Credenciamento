// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 15 - 2019
// ***********************************************************************

#region

using IMOD.Infra.Ado;

#endregion

namespace IMOD.Infra.Interfaces
{
    public interface IInfoDataBase
    {
        #region  Propriedades

        /// <summary>
        ///     Obter informações do banco de dados
        /// </summary>
        DataBaseInfo ObterInformacaoBancoDeDados { get; }

        #endregion
    }
}