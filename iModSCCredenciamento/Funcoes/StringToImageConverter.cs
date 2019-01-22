// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#endregion

namespace iModSCCredenciamento.Funcoes
{
    public class StringToImageConverter : IValueConverter
    {
        #region  Metodos

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string str = value as string;
            //if (string.IsNullOrWhiteSpace(str)) return null;
            //var img =  str.ConverterBase64StringToBitmap();
            var str = value as string;
            if (string.IsNullOrWhiteSpace (str)) return null;

            var o = new MemoryStream (System.Convert.FromBase64String (str));
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = o;
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}