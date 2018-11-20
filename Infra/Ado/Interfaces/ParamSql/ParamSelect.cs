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
    ///     Classe usada para receber e tratar parametros adequados a instruçao Select
    ///     <para>Independe do tipo de banco de dados, CrossDataBase</para>
    /// </summary>
    public class ParamSelect : IParametros
    {
        /// <summary>
        ///     Ordenacao do campo
        /// </summary>
        public enum OrderBy
        {
            Asc,
            Desc
        }

        #region  Propriedades

        /// <summary>
        ///     Ordenacao do campo
        /// </summary>
        public OrderBy? Order { get; set; }

        #endregion

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
        ///     <para>Valores com dados nulo ou vazio seraõ descartados na montagem</para>
        /// </param>
        /// <param name="idx">Indice do array do objeto</param>
        public ParamSelect(string campo, DbType tipo, object[] o, int idx)
        {
            SetDados(campo, tipo, o, idx, null);
        }

        /// <summary>
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Select nativa
        /// </summary>
        /// <param name="campo">Nome do campo</param>
        /// <param name="o">
        ///     Array de objeto
        ///     <para>
        ///         Valores com dados nulo ou vazio seraõ descartados na montagem.
        ///         O tipo será definido automaticamente
        ///     </para>
        /// </param>
        /// <param name="idx">Indice do array do objeto</param>
        public ParamSelect(string campo, object[] o, int idx)
        {
            SetDados(campo, o, idx, null);
        }

        /// <summary>
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Select nativa
        /// </summary>
        /// <param name="campo">>Nome do campo</param>
        /// <param name="tipo">Tipo do campo</param>
        /// <param name="o">
        ///     Valor do campo
        ///     <para>Valores com dados nulo ou vazio seraõ descartados na montagem</para>
        /// </param>
        public ParamSelect(string campo, DbType tipo, object o)
        {
            SetDados(campo, tipo,o, null);
        }

        /// <summary>
        ///     Parametro de dados para auxiliar na montagem de uma instruçao SQL Select nativa
        /// </summary>
        /// <param name="campo">>Nome do campo</param>
        /// <param name="o">
        ///     Valor do campo
        ///     <para>
        ///         Valores com dados nulo ou vazio seraõ descartados na montagem.
        ///         O tipo será definido automaticamente
        ///     </para>
        /// </param>
        public ParamSelect(string campo, object o)
        {
            SetDados(campo, o, null);
        }

        /// <summary>
        ///     Construtor, vazio, não haverá montagem de clausula where
        /// </summary>
        public ParamSelect()
        {
        }

        public ParamSelect(string campo, DbType tipo, object[] o, int idx, OrderBy? order)
        {
            SetDados(campo, tipo, o, idx, order);
        }

        public ParamSelect(string campo, object[] o, int idx, OrderBy? order)
        {
            SetDados(campo, o, idx, order);
        }

        public ParamSelect(string campo, DbType tipo, object o, OrderBy? order)
        {
            SetDados(campo, tipo, o, order);
        }

        public ParamSelect(string campo, object o, OrderBy? order)
        {
            SetDados(campo, o, order);
        }

        #endregion

        #region  Métodos

        /// <summary>
        ///     Set dados
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="o"></param>
        /// <param name="order"></param>
        private void SetDados(string campo, object o, OrderBy? order)
        {
            Campo = campo;

            if (o != null)
            {
                Valor = o;
                Tipo = ParamType.ObterTipo(Valor);
            }
            Order = order;
        }

        /// <summary>
        ///     Set dados
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="o"></param>
        /// <param name="order"></param>
        private void SetDados(string campo, DbType tipo, object o, OrderBy? order)
        {
            Campo = campo;

            if (o != null)
            {
                Valor = o;
                Tipo = tipo;
            }
            Order = order;
        }

        /// <summary>
        ///     Set dados
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="o"></param>
        /// <param name="idx"></param>
        /// <param name="order"></param>
        private void SetDados(string campo, object[] o, int idx, OrderBy? order)
        {
            Campo = campo;

            if (o != null)
                if (o.Length > idx)
                {
                    Valor = o.GetValue(idx);
                    Tipo = ParamType.ObterTipo(Valor);
                }
            Order = order;
        }

        /// <summary>
        ///     Set dados
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="tipo"></param>
        /// <param name="o"></param>
        /// <param name="idx"></param>
        /// <param name="order"></param>
        private void SetDados(string campo, DbType tipo, object[] o, int idx, OrderBy? order)
        {
            Campo = campo;
            Tipo = tipo;
            if (o != null)
                if (o.Length > idx)
                    Valor = o.GetValue(idx);
            Order = order;
        }

        /// <summary>
        ///     Monta instrução [Where] da clausula SQL pesquisando pelo valor exato (igual) informado no respestivo campo
        ///     <para>Ex: CampoA=@CampoA, </para>
        /// </summary>
        /// <returns></returns>
        public ParamSelect Igual()
        {
            var sintaxe = " {0}=@{0} ".ToLower();

            return Fill(sintaxe);
        }

        /// <summary>
        ///     Parse no campo
        /// </summary>
        /// <param name="sintaxe"></param>
        /// <returns></returns>
        private ParamSelect Fill(string sintaxe)
        {
            return new ParamSelect
            {
                Concatene = string.Format(sintaxe, Campo),
                Tipo = Tipo,
                Campo = Campo,
                Valor = Valor,
                Order = Order
            };
        }

        /// <summary>
        ///     Monta instrução [Where] da clausula SQL pesquisando pelo valor aproximado informado no respestivo campo
        ///     <para>Ex: CampoA Like (@CampoA), caracteres curinga devem ser concatenado com o valor informado </para>
        /// </summary>
        /// <returns></returns>
        public ParamSelect Like()
        {
            var sintaxe = " {0} like(@{0}) ".ToLower();
            return Fill(sintaxe);
        }

        /// <summary>
        ///     Monta instrução [Where] da clausula SQL pesquisando pelo valor não nulo informado no respestivo campo
        ///     <para>Ex: is @CampoA not null </para>
        /// </summary>
        /// <returns></returns>
        public ParamSelect IsNotNull()
        {
            var sintaxe = " is @{0} not null ".ToLower();
            return Fill(sintaxe);
        }

        /// <summary>
        ///     Monta instrução [Where] da clausula SQL pesquisando pelo valor aproximado informado no respestivo campo
        ///     <para>Ex: CampoA ILike (@CampoA), caracteres curinga devem ser concatenado com o valor informado </para>
        ///     <para>Ilike não é Case Sensitive </para>
        /// </summary>
        /// <returns></returns>
        public ParamSelect ILike()
        {
            var sintaxe = " {0} Ilike(@{0}) ".ToLower();
            return Fill(sintaxe);
        }

        /// <summary>
        ///     Monta instrução [Where] da clausula SQL pesquisando pelo valor maior ou igual informado no respestivo campo
        ///     <para>Ex: Campo >= @CampoA</para>
        /// </summary>
        /// <returns></returns>
        public ParamSelect MaiorIgual()
        {
            var sintaxe = " {0} >=@{0} ".ToLower();
            return Fill(sintaxe);
        }

        #endregion
    }
}