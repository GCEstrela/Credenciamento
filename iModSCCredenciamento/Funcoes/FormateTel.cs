using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using IMOD.CrossCutting;

namespace iModSCCredenciamento.Funcoes
{
    public class FormateTel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

                try
                {
                    if (value == null) return "";
                    var str = value.ToString();
                    var strNew = str.RetirarCaracteresEspeciais().Replace(" ", "");
                    switch (strNew.Length)
                    {
                        case 7:
                            return Regex.Replace(strNew, @"(\d{3})(\d{4})", "$1-$2");
                        case 10:
                        return Regex.Replace(strNew, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3");
                        case 11:
                            return Regex.Replace(strNew, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");
                        default:
                            return strNew;
                    }

                }
                catch (Exception)
                {
                    return "";
                }

                
            }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
