﻿// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 21 - 2018
// ***********************************************************************

#region

using System;
using System.Windows.Input;

#endregion

namespace iModSCCredenciamento.ViewModels.Commands
{
    public class CommandBase2 : ICommand
    {
        private readonly bool _podeExecutar;

        private readonly Action<object> _action;

        public CommandBase2(Action<object> action, bool podeExecutar)
        {
            _action = action;
            _podeExecutar = podeExecutar;
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
            return _podeExecutar;
        }

        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            _action (parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}