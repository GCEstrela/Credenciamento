// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 27 - 2018
// ***********************************************************************

#region

using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace iModSCCredenciamento.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region  Metodos

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}