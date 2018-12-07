// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 06 - 2018
// ***********************************************************************

#region

using System;
using System.IO;
using System.Windows.Forms;
using iModSCCredenciamento.Windows;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

#endregion

namespace iModSCCredenciamento.Helpers
{
    /// <summary>
    ///     Mátodos utilizados somente no domínio da aplicação WPF
    /// </summary>
    internal static class WpfHelp
    {
        #region  Metodos

        /// <summary>
        ///     Caixa de Menssagem Customizada
        /// </summary>
        /// <param name="msg">Mensagem</param>
        public static void Mbox(string msg)
        {
            Mbox (msg, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     Mensagem de erro
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void MboxError(Exception ex)
        {
            var innerMsg = "";
            if (ex.InnerException != null)
                innerMsg = ex.InnerException.Message;
            var detalhe = string.IsNullOrWhiteSpace (innerMsg) ? string.Empty : $"\nDetalhe: {innerMsg}";
            Mbox ("Um erro ocorreu.\nRazão: " + ex.Message + detalhe, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     Mensagem de erro
        /// </summary>
        /// <param name="msg">Mensagem de erro</param>
        /// <param name="ex">Exception</param>
        public static void MboxError(string msg, Exception ex)
        {
            var innerMsg = "";
            if (ex.InnerException != null)
                innerMsg = ex.InnerException.Message;
            var detalhe = string.IsNullOrWhiteSpace (innerMsg) ? string.Empty : $"\nDetalhe: {innerMsg}";
            Mbox (msg + "\nRazão: " + ex.Message + detalhe, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     Caixa de Menssagem Customizada
        /// </summary>
        /// <param name="msg">Mensagem</param>
        /// <param name="pIcon">Um icone para apresentacao</param>
        public static void Mbox(string msg, MessageBoxIcon pIcon)
        {
            MessageBox.Show (msg, Caption (pIcon), MessageBoxButtons.OK, pIcon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        ///     Define titulo do messagebox
        /// </summary>
        /// <param name="pIcon"></param>
        /// <returns></returns>
        private static string Caption(MessageBoxIcon pIcon)
        {
            var caption = "Nota Importante";
            switch (pIcon)
            {
                case MessageBoxIcon.Error:
                    caption = "Erro Crítico";
                    break;
                case MessageBoxIcon.Information:
                    caption = "Informação Importante";
                    break;
                case MessageBoxIcon.Warning:
                    caption = "Atenção";
                    break;
            }
            return caption;
        }

        /// <summary>
        ///     Mensagem por Popup
        /// </summary>
        /// <param name="msg">Mensagem a ser exibida</param>
        /// <param name="icone">1-Informação, 2-Interrogação, 3-Exclamação, 4-Proibido</param>
        /// <returns></returns>
        public static bool PopupBox(string msg, int icone)
        {
            var popupBox = new PopupBox (msg, icone);
            popupBox.ShowDialog();
            return popupBox.Result;
        }

        /// <summary>
        ///     Upload de arquivo
        ///     <para>Retorna dados de um arquivo</para>
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="tamMax">Tamanho máximo do arquivo, informe 0 para não limitar o tamnho do arquivo</param>
        /// <returns></returns>
        public static ArquivoInfo UpLoadArquivoDialog(string filtro, int tamMax = 0)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = filtro;
            openFileDialog.InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
            var result = openFileDialog.ShowDialog();
            if (result != true) return null;

            var path = openFileDialog.FileName;
            var tam = new FileInfo (path).Length;
            var arq = new ArquivoInfo
            {
                Nome = openFileDialog.SafeFileName
            };

            if (tamMax == 0)
            {
                arq.ArrayBytes = File.ReadAllBytes (path);
                arq.FormatoBase64 = Convert.ToBase64String (arq.ArrayBytes);
                return arq;
            }
               

            if (tamMax < tam)
                throw new Exception ($"{tamMax} Kbytes é o tamanho máximo permitido para upload.");

            arq.ArrayBytes = File.ReadAllBytes(path);
            arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);
            return arq;


        }

        /// <summary>
        ///     Salvar um arquivo
        ///     <para>Apresenta uma modal dialog onde usuário seleciona caminho de gravação do arquivo</para>
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="arrayBytes">Array de Bytes relativo ao arquivo</param>
        public static void DownloadArquivoDialog(string nomeArquivo, byte[] arrayBytes)
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