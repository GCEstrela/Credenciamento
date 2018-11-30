using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iModSCCredenciamento.Funcoes
{
   public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                bool _valor = (bool)value;
                if (_valor)
                {
                    return "Visible";
                }
                return "Hidden";

            }
            return "Hidden";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
