using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.ViewModels
{
    /// <summary>
    /// Provides common functionality for ViewModel classes
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //protected bool SetProperty<T>(ref T field, T value, [CallerMemberName]string name = null)
        //{
        //    if (Equals(field, value))
        //    {
        //        return false;
        //    }
        //    field = value;
        //    this.OnPropertyChanged(name);
        //    return true;
        //}


        ////public event PropertyChangedEventHandler PropertyChanged;

        ////protected void OnPropertyChanged(string propertyName)
        ////{
        ////    PropertyChangedEventHandler handler = PropertyChanged;

        ////    if (handler != null)
        ////    {
        ////        handler(this, new PropertyChangedEventArgs(propertyName));
        ////    }
        ////}

        #endregion 

    }
}
