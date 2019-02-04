// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  02 - 04 - 2019
// ***********************************************************************

#region

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class FormatCurrency : IValueConverter
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
                //if (!value.ToString().All (char.IsNumber)) return 0;
                var num = value.ToString();
                var strNew = num.RetirarCaracteresEspeciais().Replace (" ", "");
                return strNew.FormatarMoeda();
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