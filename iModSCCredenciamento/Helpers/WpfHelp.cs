// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 04 - 2018
// ***********************************************************************

#region

using System;
using System.IO;
using System.Windows.Forms;

#endregion

namespace iModSCCredenciamento.Helpers
{
    /// <summary>
    /// Mátodos utilizados somente no domínio da aplicação WPF
    /// </summary>
    internal static class WpfHelp
    {
        #region  Metodos

        /// <summary>
        ///     Salvar um arquivo
        ///     <para>Apresenta uma modal dialog onde usuário seleciona caminho de gravação do arquivo</para>
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="arrayBytes">Array de Bytes relativo ao arquivo</param>
        public static void SalvarArquivoDialog(string nomeArquivo, byte[] arrayBytes)
        {
            //Parâmetros necessários a execução do metodo
            if (string.IsNullOrWhiteSpace (nomeArquivo)) return;
            if (arrayBytes == null) return;

            var dlgSave = new SaveFileDialog();
            dlgSave.FileName = nomeArquivo; //Nome do arquivo
            //Obter extensão do arquivo, caso exista
            var ext = string.IsNullOrWhiteSpace (nomeArquivo) ? "" : nomeArquivo;
            if (ext.Contains ("."))
            {
                ext = ext.Substring (ext.IndexOf (".", StringComparison.CurrentCulture));
                dlgSave.DefaultExt = ext; //Extensão
                dlgSave.Filter = $"documents ({ext})|*{ext}";
            }
            //Usuario decidindo salvar...
            if (dlgSave.ShowDialog() == DialogResult.OK)
                File.WriteAllBytes (dlgSave.FileName, arrayBytes);
        }

        #endregion
    }
}