// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System;
using System.Data;

#endregion

namespace IMOD.Infra.Ado.Interfaces.ParamSql
{
    /// <summary>
    ///     Classe usada para receber e tratar parametros adequados a instruçao Insert
    ///     <para>Independe do tipo de banco de dados, CrossDataBase</para>
    /// </summary>
    public class ParamInsert : IParametros
    {
        /// <summary>
        ///     Nome do campo
        /// </summary>
        public string Campo { get; set; }

        /// <summary>
        ///     Tipo do dados
        /// </summary>
        public DbType Tipo { get; set; }

        /// <summary>
        ///     Resultado da concatenaçao de um campo e sua referencia
        ///     <para>Ex: CampoA=@CampoA</para>
        /// </summary>
        public string Concatene { get; set; }

        /// <summary>
        ///     Valor do campo
        /// </summary>
        public object Valor { get; set; }

        /// <summary>
        ///     Indica se o campo é uma primary key
        /// </summary>
        public bool Key { get; set; }

        #region Construtor

        /// <summary>
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Insert nativa
        /// </summary>
        /// <param name="campo">Nome do campo</param>
        /// <param name="tipo">Tipo do campo</param>
        /// <param name="valor">Valor do campo</param>
        /// <param name="key">True, o campo é uma primary key</param>
        public ParamInsert(string campo, DbType tipo, object valor, bool key)
        {
            Campo = campo;
            Tipo = tipo;
            Key = key;
            Valor = valor ?? DBNull.Value;
        }

        /// <summary>
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Insert nativa
        /// </summary>
        /// <param name="campo">Nome do campo</param>
        /// <param name="valor">Valor do campo
        ///     <para>O tipo será definido automaticamente</para>
        /// </param>
        /// <param name="key">True, o campo é uma primary key</param>
        public ParamInsert(string campo, object valor, bool key)
        {
            Campo = campo;
            Tipo = ParamType.ObterTipo(valor);
            Key = key;
            Valor = valor ?? DBNull.Value;
        }

        public ParamInsert()
        {
        }

        #endregion
    }
}