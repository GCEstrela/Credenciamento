using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class ConvertBoolToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)parameter == "Impressa")
            {
                BitmapImage impressa = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/CardPrinterAzul.png"));
                BitmapImage naoimpressa = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/CardPrinterAmarelo.png"));

                if (value != null)
                {
                    bool isTrue = (bool)value;

                    return isTrue ? impressa : naoimpressa;
                }
                return null;
            }
            
            BitmapImage imgOk = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Ok.ico"));
            BitmapImage imgPend;
            if (value.ToString() == "True")
            {
                imgPend = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/PendenciaImpeditiva.gif"));                                                                                                                         
            }
            
            else
            {
                imgPend = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Pendencia.ico"));
            }

            BitmapImage imgNull = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Proibido.ico"));

            if (value != null)
            {
                bool isTrue = (bool)value;               

                return isTrue?imgPend:imgOk;
            }
            return imgNull;

            //return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
