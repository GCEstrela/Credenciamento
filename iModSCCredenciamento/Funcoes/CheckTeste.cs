using System;
using System.Globalization;
using System.Windows.Data;

namespace iModSCCredenciamento.Funcoes
{
    public class CheckTeste : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            if ((string)parameter == "Ativa_tw")
            {
                bool _CredencialAtiva = (bool)value;
                if (_CredencialAtiva)
                {
                    return "White";
                }

                return "Red";
            }

            if ((string)parameter == "Principal_tw")
            {
                bool _Principal = (bool)value;
                if (_Principal)
                {
                    return "White";
                }

                return "Gray";
            }
            //else if ((string)parameter == "Ativa_Checked")
            //{
            //    int _CredencialStatusID = (int)value;
            //    if (_CredencialStatusID==1)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            // throw new NotImplementedException();
        }
    }
}
