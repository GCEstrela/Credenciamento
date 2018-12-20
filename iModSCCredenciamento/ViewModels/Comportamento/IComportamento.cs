// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 18 - 2018
// ***********************************************************************

#region

using System.Windows.Input;

#endregion

namespace iModSCCredenciamento.ViewModels.Comportamento
{
    public interface IComportamento
    {
        #region  Propriedades

        ComportamentoBasico Comportamento { get;}

        /// <summary>
        ///     Novo
        /// </summary>
        ICommand PrepareCriarCommand { get;}

        /// <summary>
        ///     Novo
        /// </summary>
        ICommand PrepareAlterarCommand { get;}

        /// <summary>
        ///     Novo
        /// </summary>
        ICommand PrepareCancelarCommand { get;}

        /// <summary>
        ///     Novo
        /// </summary>
        ICommand PrepareSalvarCommand { get;}

        /// <summary>
        ///     Novo
        /// </summary>
        ICommand PrepareRemoverCommand { get;}

        #endregion
    }
}