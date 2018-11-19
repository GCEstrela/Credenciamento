// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Data;

#endregion

namespace Infra.Ado.Interfaces.ParamSql
{
    /// <summary>
    ///     Define o tipo do parametro
    ///     CrossDataBase
    /// </summary>
    public static class ParamType
    {
        #region  Metodos

        #region  Métodos

        public static DbType ObterTipo(object valor)
        {
            if (valor == null)
                return DbType.String; //retorna nullo
            //throw   new  ArgumentNullException("Parâmetro nulo não pode identificar o tipo da variável.");
            var tipo2 = valor.GetType();
            if (tipo2 == typeof(long))
                return DbType.Int64;
            if (tipo2 == typeof(int))
                return DbType.Int64;
            if (tipo2 == typeof(string))
                return DbType.String;
            if (tipo2 == typeof(bool))
                return DbType.Boolean;
            if (tipo2 == typeof(DateTime))
                return DbType.DateTime;
            if (tipo2 == typeof(byte[]))
                return DbType.Binary;
            if (tipo2 == typeof(char))
                return DbType.StringFixedLength;
            if (tipo2 == typeof(decimal))
                return DbType.Decimal;
            if (tipo2 == typeof(double))
                return DbType.Double;
            if (tipo2 == typeof(float))
                return DbType.Double;
            if (tipo2 == typeof(short))
                return DbType.Int32;
            //Caso o metodo nao encontre um tipo de variavel
            throw new ArgumentException("Não foi possível identificar o tipo da variável", "valor");
        }

        #endregion

        #endregion
    }
}