using System;
using System.Collections.Generic;
using System.Globalization; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iModSCCredenciamento.Funcoes
{
    public class ConvertDescToBool : IValueConverter
 
    { 
 
    {
   
 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //comentarios
            //Mais uma modificação
            if (value != null)
            {
               //Esse trecho de codigo converte uma descrição para boleano
                string desc = (string)value;

                if (desc == "ATIVA")
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
