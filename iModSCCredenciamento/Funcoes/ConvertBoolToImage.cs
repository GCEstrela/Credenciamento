using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iModSCCredenciamento.Funcoes
{
    public class ConvertBoolToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)parameter == "Impressa")
            {
                BitmapImage impressa = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/CardPrinterAzul.png"));
                BitmapImage naoimpressa = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/CardPrinterAmarelo.png"));

                if (value != null)
                {
                    bool isTrue = (bool)value;

                    return isTrue ? impressa : naoimpressa;
                }
                return null;
            }
            else
            {
                BitmapImage imgOk = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Ok.ico"));
                BitmapImage imgPend = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Pendencia.ico"));
                BitmapImage imgNull = new BitmapImage(new Uri("pack://application:,,,/iModSCCredenciamento;component/Resources/Proibido.ico"));

                if (value != null)
                {
                    bool isTrue = (bool)value;               

                    return isTrue?imgPend:imgOk;
                }
                return imgNull;
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
