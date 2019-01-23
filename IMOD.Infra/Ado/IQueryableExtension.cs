// ***********************************************************************
// Project: IMOD.Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 10 - 2018
// ***********************************************************************

#region

using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

namespace IMOD.Infra.Ado
{
    public static class QueryableExtension
    {
        #region  Metodos

        /// <summary>
        ///     For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceQuery<T>(this IQueryable<T> query)
        {
            var objectQuery = GetQueryFromQueryable (query);
            if (objectQuery == null) return "";
            var result = objectQuery.ToTraceString();
            foreach (var parameter in objectQuery.Parameters)
            {
                var name = "@" + parameter.Name;
                var value = "'" + parameter.Value + "'";
                result = result.Replace (name, value);
            }

            return result;
        }

        /// <summary>
        ///     For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IQueryable<T> query)
        {
            var objectQuery = GetQueryFromQueryable (query);
            if (objectQuery == null) return "";
            var traceString = new StringBuilder();

            traceString.AppendLine (objectQuery.ToTraceString());
            traceString.AppendLine();

            foreach (var parameter in objectQuery.Parameters)
            {
                traceString.AppendLine (parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return traceString.ToString();
        }

        private static ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields (BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault (f => f.Name.Equals ("_internalQuery"));
            if (internalQueryField == null) return null;
            {
                var internalQuery = internalQueryField.GetValue (query);
                var objectQueryField = internalQuery.GetType().GetFields (BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault (f => f.Name.Equals ("_objectQuery"));
                if (objectQueryField != null) return objectQueryField.GetValue (internalQuery) as ObjectQuery<T>;
            }

            return null;
        }

        #endregion
    }
}