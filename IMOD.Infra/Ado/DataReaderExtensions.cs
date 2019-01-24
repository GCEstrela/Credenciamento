// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 20 - 2018
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

#endregion

namespace IMOD.Infra.Ado
{
    public static class DataReaderExtensions
    {
        #region  Metodos

        /// <Summary>
        ///     Map data from DataReader to list of objects
        /// </Summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="dr">Data Reader</param>
        /// <returns>List of objects having data from data reader</returns>
        public static List<T> MapToList<T>(this IDataReader dr) where T : new()
        {
            List<T> retVal = null;
            var entity = typeof(T);
            var propDict = new Dictionary<string, PropertyInfo>();
            string nomeCampo;
            string valorCampo;
            if (dr != null)
            {
                retVal = new List<T>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                while (dr.Read())
                {
                    var newObject = new T();
                    for (var index = 0; index < dr.FieldCount; index++)
                        if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                        {
                            var info = propDict[dr.GetName(index).ToUpper()];
                            nomeCampo = info.Name;
                            if (info != null && info.CanWrite)
                            {
                                var val = dr.GetValue(index);
                                valorCampo = val.ToString().Trim();
                                info.SetValue(newObject, val == DBNull.Value ? null : val, null);
                            }
                        }

                    retVal.Add(newObject);
                }
            }

            return retVal;
        }

        /// <Summary>
        ///     Map data from DataReader to an object
        /// </Summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="dr">Data Reader</param>
        /// <returns>Object having data from Data Reader</returns>
        public static T MapToSingle<T>(this IDataReader dr) where T : new()
        {
            var retVal = new T();
            var entity = typeof(T);
            var propDict = new Dictionary<string, PropertyInfo>();
            if (dr != null)
            {
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                dr.Read();
                for (var index = 0; index < dr.FieldCount; index++)
                    if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                    {
                        var info = propDict[dr.GetName(index).ToUpper()];
                        if (info != null && info.CanWrite)
                        {
                            var val = dr.GetValue(index);
                            info.SetValue(retVal, val == DBNull.Value ? null : val, null);
                        }
                    }
            }

            return retVal;
        }

        #endregion
    }
}