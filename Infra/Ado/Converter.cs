// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region

using System;
using System.ComponentModel;

#endregion

namespace IMOD.Infra.Ado
{
    public static class Converter
    {
        #region  Metodos

        public static T ChangeType<T>(object value)
        {
            return (T) ChangeType (typeof(T), value);
        }

        public static object ChangeType(Type t, object value)
        {
            var tc = TypeDescriptor.GetConverter (t);
            return tc.ConvertFrom (value);
        }

        #endregion
    }
}