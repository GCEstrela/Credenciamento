 
#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

#endregion

namespace IMOD.CredenciamentoWeb.Helpers
{
    public static class HtmlHelpers
    {
        #region Propriedades

        /// <summary>
        ///     Suporte Técnico
        /// </summary>
        private const string MSG_SUP_TECNICO = "Suporte técnico: Tel:() - e-mail:";

        /// <summary>
        ///     Nome sistema
        /// </summary>
        public const string MSG_NOME_APLICACAO = "DSBR - Desenvolvimento de Sistemas";

        /// <summary>
        ///     Mensagem de erro Não é uma requisição Ajax
        /// </summary>
        public const string MSG_NAO_REQ_AJAX = "Não é uma requisição Ajax.Acesso negado.";

        /// <summary>
        ///     Mensagem usuario não autenticado
        /// </summary>
        public const string MSG_USUARIO_NAO_AUTENTICADO = "Usuário não autenticado.";

        /// <summary>
        ///     Mensagem registro não encontrado
        /// </summary>
        public const string MSG_REGISTRO_NAO_ENCONTRADO = "Registro não encontrado.";

        #endregion

        #region Metodos


        /// <summary>
        ///     Retorna uma lista a partir de um Enum
        ///     <para>
        ///         Instrução de uso na controller: ViewBag.DropDownList = HtmlHelpers.SelectListFor<Processo.TipoBusca />();
        ///     </para>
        ///     <para>
        ///         Instrução de uso na controller: ViewBag.DropDownList = HtmlHelpers.SelectListFor<Processo.TipoBusca />("2");
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SelectList SelectListFor<T>() where T : struct
        {
            var t = typeof(T);
            return !t.IsEnum
                    ? null
                    : new SelectList(ConstruirItens(t), "Value", "Text");
        }

        /// <summary>
        ///     Retorna uma lista a partir de um Enum
        ///     Para setar valor default: ViewBag.DropDownList = HtmlHelpers.SelectListFor<Processo.TipoBusca />("2");
        /// </summary>
        /// <typeparam name="T">um tipo Enum</typeparam>
        /// <param name="valueEnum">Valor Enum a ser selecionado</param>
        /// <returns></returns>
        public static SelectList SelectListFor<T>(string valueEnum)
        {
            var t = typeof(T);
            return !t.IsEnum
                    ? null
                    : new SelectList(ConstruirItens(t), "Value", "Text", valueEnum);
        }

        private static IEnumerable<SelectListItem> ConstruirItens(Type t)
        {
            return Enum.GetValues(t)
                    .Cast<Enum>()
                    .Select(
                            e =>
                                    new SelectListItem
                                    {
                                        Value = Convert.ToInt32(e).ToString(),
                                        Text = e.ObterDescricao()
                                    });
        }

        /// <summary>
        ///     Formatar mensagem de erro padrão
        /// </summary>
        /// <param name="msg">Mensagem de erro</param>
        /// <param name="visiblesuport">Exibe mensagem de suporte técnico</param>
        /// <returns></returns>
        public static string MensagenErroFormat(string msg, bool visiblesuport)
        {
            if (string.IsNullOrWhiteSpace(msg)) return string.Empty;
            var str = "Detalhe do erro:<br />{0}";
            if (!visiblesuport)
                return string.Format(str, msg);

            var str2 = "Detalhe do erro:<br />{0}<br />{1}";
            return string.Format(str2, msg, MSG_SUP_TECNICO);
        }

        /// <summary>
        ///     Formatar mensagem de erro padrão
        /// </summary>
        /// <param name="msg">Mensagem de erro</param>
        /// <param name="visiblesuport">Exibe mensagem de suporte técnico</param>
        /// <returns></returns>
        public static string MensagenErroFormat(List<string> msg, bool visiblesuport)
        {
            if (msg.Count == 0) return string.Empty;
            var str = "Detalhe do erro:</br>{0}";
            var msgs = msg.Aggregate("- {0}.</br>",
                    (current, item) => msg + string.Format(current, item));

            if (!visiblesuport)
                return string.Format(str, msgs);

            var str2 = "Detalhe do erro:</br>{0}</br>{1}";
            return string.Format(str2, msgs, MSG_SUP_TECNICO);
        }

        #endregion

        #region Metodos

        /// <summary>
        ///     Tratar nome da URL
        /// </summary>
        /// <param name="nomeservico">Nome do serviço</param>
        /// <returns></returns>
        public static string TratarNomeUrl(string nomeservico)
        {
            Uri uriResult;
            var result = Uri.TryCreate(nomeservico, UriKind.Absolute, out uriResult) &&
                         (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result) throw new InvalidOperationException("Url Inválida");
            return uriResult.ToString();
        }

        /// <summary>
        ///     Retira caracteres especias da string
        /// </summary>
        /// <param name="str">Uma string</param>
        /// <returns></returns>
        public static string RetirarCaracteresEspeciais(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "";
            var novastring = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return novastring.Replace(str, string.Empty);
        }

        /// <summary>
        ///     Obter a descrição do Enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ObterDescricao(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Formatar máscaraCNPJ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatarCnpj(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return "";
            if (str.Length < 14) return "";
            var str2=  str.RetirarCaracteresEspeciais();
            return Convert.ToUInt64(str2).ToString(@"00\.000\.000\/0000\-00");
        }

        /// <summary>
        ///     Obter um tipo Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        #endregion
    }
}