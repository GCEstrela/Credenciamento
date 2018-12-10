using System;
using System.Globalization;
using System.Windows.Data;

namespace iModSCCredenciamento.Funcoes
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string imagename = value as string;
                return Conversores.STRtoIMG(imagename);
            }
            return null;
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
