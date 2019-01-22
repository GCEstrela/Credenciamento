// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged,INotifyDataErrorInfo
    {
        #region  Metodos

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }

        /// <summary>Gets the validation errors for a specified property or for the entire entity.</summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets a value that indicates whether the entity has validation errors. </summary>
        /// <returns>true if the entity currently has validation errors; otherwise, false.</returns>
        public bool HasErrors { get; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}