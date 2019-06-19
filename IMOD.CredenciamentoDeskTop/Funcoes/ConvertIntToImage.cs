using IMOD.CredenciamentoDeskTop.Views.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class ConvertIntToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (value.ToString() == "0")
            {
                //BitmapImage impressa = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/CardPrinterAzul.png"));
                BitmapImage imagem = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/red_alerta.png"));
                return imagem;
            }
            else if ((int)value == 5)
            {
                BitmapImage imagem = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/orange_alerta.png"));
                return imagem;
            }
            else if ((int)value == 15)
            {
                BitmapImage imagem = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/yellow_alerta.png"));
                return imagem;
            }
            else if ((int)value == 30)
            {
                BitmapImage imagem = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/green_alerta.png"));
                return imagem;
            }
            else
            {
                return null;
            }
            //BitmapImage imgOk = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Ok.ico"));
            //BitmapImage imgPend = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Pendencia.ico"));
            //BitmapImage imgNull = new BitmapImage(new Uri("pack://application:,,,/IMOD.CredenciamentoDeskTop;component/Resources/Proibido.ico"));

            //if (value != null)
            //{
            //    bool isTrue = (bool)value;

            //    return isTrue ? imgPend : imgOk;
            //}
            //return imgNull;
            //return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
