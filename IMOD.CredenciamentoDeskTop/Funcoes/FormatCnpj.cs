// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 04 - 2019
// ***********************************************************************

#region

using System;
using System.Globalization;
using System.Windows.Data;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class FormatCnpj : IValueConverter
    {
        #region  Metodos

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return "";
                var str = value.ToString();
                var strNew = str.RetirarCaracteresEspeciais().Replace (" ", "");
                switch (strNew.Length)
                {
                    case 14:
                        return strNew.FormatarCnpj();
                        //break;
                    default:
                        return strNew;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}