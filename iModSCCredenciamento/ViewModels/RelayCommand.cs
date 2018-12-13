// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System;
using System.Windows.Input;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class RelayCommand : ICommand

    {
        private readonly Action _mAtAction;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public RelayCommand(Action mAtAction)
        {
            _mAtAction = mAtAction;
        }

        #region  Metodos

        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            if (parameter == null) return true;

            return (bool)parameter;
        }

        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            _mAtAction();
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion
    }
}