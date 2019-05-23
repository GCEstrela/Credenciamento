using System;
using System.Globalization;
using System.Windows.Data;

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
   public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool _valor = (bool)value;
                if (_valor)
                {
                    return "Visible";
                }
                return "Collapsed";

            }
            return "Collapsed";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value != null)
            //{
            //    BitmapImage image = value as BitmapImage;
            //    return Conversores.IMGtoSTR(image);
            //}
            return null;
        }
    }
}
