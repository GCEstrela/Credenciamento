﻿// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Funcoes
{
    public class FormateTel : IValueConverter
    {
        #region  Metodos

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return "";
                var str = value.ToString();
                var strNew = str.RetirarCaracteresEspeciais().Replace (" ", "");
                switch (strNew.Length)
                {
                    case 7:
                        return Regex.Replace (strNew, @"(\d{3})(\d{4})", "$1-$2");
                    case 10:
                        return Regex.Replace (strNew, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3");
                    case 11:
                        return Regex.Replace (strNew, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");
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

        #endregion
    }
}