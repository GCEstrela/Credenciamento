// ***********************************************************************
// Project: Infra
// Crafted by: Grupo Estrela by Genetec
// Date:  11 - 19 - 2018
// ***********************************************************************

#region

using System.Data;

#endregion

namespace Infra.Ado.Interfaces.ParamSql
{
    /// <summary>
    ///     Classe usada para receber e tratar parametros adequados a instruçao Delete
    ///     <para>Independe do tipo de banco de dados, CrossDataBase</para>
    /// </summary>
    public class ParamDelete : IParametros
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
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Select nativa
        /// </summary>
        /// <param name="campo">Nome do campo</param>
        /// <param name="tipo">Tipo do dado</param>
        /// <param name="o">
        ///     Array de objeto
        ///     <para>Valores com dados nulo ou vazio NÂO seraõ descartados na montagem</para>
        /// </param>
        /// <param name="idx">Indice do array do objeto</param>
        public ParamDelete(string campo, DbType tipo, object o)
        {
            Campo = campo;
            Tipo = tipo;
            Valor = o;
        }

        /// <summary>
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Select nativa
        /// </summary>
        /// <param name="campo">Nome do campo</param>
        /// <param name="o">
        ///     Array de objeto
        ///     <para>Valores com dados nulo ou vazio NÂO seraõ descartados na montagem.</para>
        ///     <para>O tipo será definido automaticamente</para>
        /// </param>
        /// <param name="idx">Indice do array do objeto</param>
        public ParamDelete(string campo, object o)
        {
            Campo = campo;
            Valor = o;
            Tipo = ParamType.ObterTipo(Valor);
        }

        public ParamDelete()
        {
        }

        #endregion

        #region  Métodos

        public ParamDelete Igual()
        {
            var sintaxe = " {0}=@{0} ".ToLower();

            return Fill(sintaxe);
        }

        private ParamDelete Fill(string sintaxe)
        {
            return new ParamDelete
            {
                Concatene = string.Format(sintaxe, Campo),
                Tipo = Tipo,
                Campo = Campo,
                Valor = Valor
            };
        }

        public ParamDelete Like()
        {
            var sintaxe = " {0} like(@{0}) ".ToLower();
            return Fill(sintaxe);
        }

        public ParamDelete IsNotNull()
        {
            var sintaxe = " is @{0} not null ".ToLower();
            return Fill(sintaxe);
        }

        public ParamDelete ILike()
        {
            var sintaxe = " {0} Ilike(@{0}) ".ToLower();
            return Fill(sintaxe);
        }

        public ParamDelete MaiorIgual()
        {
            var sintaxe = " {0} >=@{0} ".ToLower();
            return Fill(sintaxe);
        }

        #endregion
    }
}