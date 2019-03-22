using System;
using System.Globalization;
using System.Windows.Data;

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class CheckImpressao : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            if ((string)parameter == "Impressa_tw")
            {
                bool _CredencialImpressa = (bool)value;
                if (_CredencialImpressa)
                {
                    return "White";
                }

                return "Red";
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            // throw new NotImplementedException();
        }
    }
}
