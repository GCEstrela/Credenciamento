using System;
using System.Globalization;
using System.Windows.Data;

namespace iModSCCredenciamento.Funcoes
{
    class AtualizaDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //Isso foi um teste (Github)
            try
            {
                string _data0 = (string)values[0];
                string _data1 = (string)values[1];

                if (_data0 != null)
                {
                    return _data0.Trim();
                }
                if (_data1 != null)
                {
                    return _data1.Trim();
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
