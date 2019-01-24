// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Data;

#endregion

namespace IMOD.Infra.Ado.Interfaces.ParamSql
{
    /// <summary>
    ///     Interface serve de modelo para criação de parametros relacionados a montagem de instrução SQL nativa
    ///     de um determinado banco de dados
    /// </summary>
    public interface IParametros
    {
        #region  Propriedades

        /// <summary>
        ///     Nome do campo
        /// </summary>
        string Campo { get; set; }

        /// <summary>
        ///     Tipo do campo
        /// </summary>
        DbType Tipo { get; set; }

        /// <summary>
        ///     Resultado da concatenaçao de um campo e sua referencia
        ///     <para>Por exemplo CampoA=@CampoA</para>
        /// </summary>
        string Concatene { get; set; }

        /// <summary>
        ///     Valor do campo
        /// </summary>
        object Valor { get; set; }

        /// <summary>
        ///     Indica se o campo é uma primary key
        /// </summary>
        bool Key { get; set; }

        #endregion
    }
}