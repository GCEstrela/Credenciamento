// ***********************************************************************
// Project: IMOD.CredenciamentoDeskTop
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 24 - 2019
// ***********************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using IMOD.CredenciamentoDeskTop.Windows;
using IMOD.CrossCutting;
using IMOD.Infra.Ado;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

#endregion

namespace IMOD.CredenciamentoDeskTop.Helpers
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
        ///     Exibe messageBox contendo uma lista de erros
        ///     <para>Usada para exibir mensagens formatadas numa única messageBox</para>
        /// </summary>
        /// <param name="msg">Lista de mensagens</param>
        public static void Summary(IList<string> msg)
        {
            if (msg == null) return;
            var text = "";
            foreach (var item in msg)
            {
                text = text + "- " + item + "\n";
            }
            if (!string.IsNullOrWhiteSpace (text))
                Mbox ($"Não foi possível continuar\nRazão:\n{text}", MessageBoxIcon.Exclamation);
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
        ///     Exibe Mensagem Solicitando Confirmação Sim ou Não
        /// </summary>
        /// <param name="msg">Uma string  contendo a mensagem</param>
        /// <param name="button1">True: Opção Sim recebe focus</param>
        /// <returns></returns>
        public static DialogResult MboxDialogYesNo(string msg, bool button1)
        {
            var button = button1 ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2;
            var result = MessageBox.Show (msg, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question, button);
            return result;
        }

        /// <summary>
        ///     Exibe Mensagem Solicitando Confirmação Sim ou Não para remoção de dados
        /// </summary>
        /// <returns></returns>
        public static DialogResult MboxDialogRemove()
        {
            return MboxDialogYesNo ("Deseja realmente excluir o item?", false);
        }

        /// <summary>
        ///     Exibe Mensagem Solicitando Confirmação Sim ou Não para desativação de dados
        /// </summary>
        /// <returns></returns>
        public static DialogResult MboxDialogDesativar()
        {
            return MboxDialogYesNo ("Deseja realmente desativar o item?", false);
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
        ///     Converte um dado em Base64 para um array de bytes
        /// </summary>
        /// <param name="strFormatBase64">String no formato Base64</param>
        /// <param name="nomeDocumento">
        ///     Um nome qualificado para identificar o nome a ser exibido como causa de uma execao se
        ///     houver
        /// </param>
        /// <returns></returns>
        public static byte[] ConverterBase64(string strFormatBase64, string nomeDocumento)
        {
            try
            {
                return Convert.FromBase64String (strFormatBase64);
            }
            catch (Exception)
            {
                throw new Exception ($"Não foi possível converter o {nomeDocumento}");
            }
        }

        public static bool PopupBox(Exception ex)
        {
            //var msg = $"Não foi possível executar a operação solicitada.\n{ex.Message}";
            //return PopupBox (msg, 3);
            return PopupBox(ex.Message, 3);
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
            var tamBytes = new FileInfo (path).Length;
            var tam = decimal.Divide (tamBytes, 1024);
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
            {
                WpfHelp.Mbox($"{tamMax} Kbytes é o tamanho máximo permitido para upload. Seu arquivo tem " + Convert.ToInt32(tam), MessageBoxIcon.Information);
                //throw new Exception ($"{tamMax} Kbytes é o tamanho máximo permitido para upload.");
                return null;
            }
            arq.ArrayBytes = File.ReadAllBytes (path);
            arq.FormatoBase64 = Convert.ToBase64String (arq.ArrayBytes);
            return arq;
        }

        /// <summary>
        ///     Abrir arquivo
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static void AbrirArquivo(string caminho, string nomeArquivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (caminho)) throw new ArgumentNullException (nameof (caminho));
                if (string.IsNullOrWhiteSpace (caminho)) throw new ArgumentNullException (nameof (nomeArquivo));
                var path = Path.Combine (caminho, nomeArquivo);
                Process.Start (path);
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        /// <summary>
        ///     Abrir arquivo Pdf dentro do programa associado à extensão .pdf
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="arrayBytes"></param>
        public static void AbrirArquivoPdf(string nomeArquivo, byte[] arrayBytes)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (nomeArquivo)) return;
                if (arrayBytes == null) return;
                var fileName = "";
                var tempArea = Path.GetTempPath();
                fileName = Path.ChangeExtension (nomeArquivo, ".pdf");
                var path = Path.Combine (tempArea, fileName);
                File.WriteAllBytes (path, arrayBytes); //Save file on temp area

                var tsk = Task.Factory.StartNew (() => { System.Windows.Application.Current.Dispatcher.Invoke (() => { Process.Start (path); }); });
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
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

        /// <summary>
        ///     Exibir o relatório
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="arrayBytes">Array de Bytes relativo ao arquivo</param>
        /// <param name="formula">Formula do relatório</param>
        /// <param name="mensagem"></param>
        [Obsolete("Usado apenas para relatórios do tipo .rpt onde tinha conexao com o banco de dados internamente.")]
        public static void ShowRelatorio(byte[] arrayBytes, string nomeArquivo, string formula, string mensagem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (nomeArquivo)) return;
                if (arrayBytes == null) return;

                var tempArea = Path.GetTempPath();

                var idx = nomeArquivo.IndexOf (".", StringComparison.Ordinal);
                var nameFile = idx == -1 ? nomeArquivo : nomeArquivo.Substring (idx);

                var fileName = Path.Combine (tempArea, $"{nameFile}.rpt");

                
                File.WriteAllBytes (fileName, arrayBytes); //Save file on temp area

                var reportDoc = new ReportDocument();
                reportDoc.Load (fileName, OpenReportMethod.OpenReportByTempCopy);

                var crtableLogoninfo = new TableLogOnInfo();
                var crConnectionInfo = new ConnectionInfo();
                Tables CrTables;
                //Obter string de conexao
                var  db = new SqlConnectionStringBuilder (CurrentConfig.ConexaoString);

                crConnectionInfo.ServerName = db.DataSource;
                crConnectionInfo.DatabaseName = db.InitialCatalog;
                crConnectionInfo.UserID = db.UserID;
                crConnectionInfo.Password = db.Password;

                CrTables = reportDoc.Database.Tables;
                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo (crtableLogoninfo);
                }

                var tsk = Task.Factory.StartNew (() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke (() =>
                    {
                        //your code here, formulas...
                        if (!string.IsNullOrWhiteSpace (formula))
                        {
                            reportDoc.RecordSelectionFormula = formula;
                        }
                        if (!string.IsNullOrWhiteSpace (mensagem))
                        {
                            var txt = (TextObject) reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                            txt.Text = mensagem;
                        }
                        reportDoc.Refresh();
                        var _popupRelatorio = new PopupRelatorio (reportDoc);
                        _popupRelatorio.Show();
                    });
                });
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        /// <summary>
        ///     Retorna um objeto Crystal Report
        /// </summary>
        /// <param name="arrayBytes">Array de bytes de um arquivo rpt</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <returns></returns>
        public static ReportDocument ShowRelatorioCrystalReport(byte[] arrayBytes, string nomeArquivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace (nomeArquivo)) throw new ArgumentNullException ("O nome do arquivo rpt é requerido");
                if (arrayBytes == null) throw new ArgumentNullException ("Um array de bytes do arquivo rpt é requerido");

                var tempArea = Path.GetTempPath();

                var idx = nomeArquivo.IndexOf (".", StringComparison.Ordinal);
                var nameFile = idx == -1 ? nomeArquivo : nomeArquivo.Substring (idx);
                //Criar um arquivo rpt na maquina local numa área temporária
                var fileName = Path.Combine (tempArea, $"{nameFile}.rpt");
                File.WriteAllBytes (fileName, arrayBytes); //Save file on temp area

                var reportDoc = new ReportDocument();
                reportDoc.Load (fileName, OpenReportMethod.OpenReportByTempCopy);
                return reportDoc;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
                return null;
            }
        }

        /// <summary>
        /// Exibir o relatório
        /// </summary>
        /// <param name="documentoReport">Documento do crystal report</param> 
        public static void ShowRelatorio(ReportDocument documentoReport)
        {
            try
            {
                var _popupRelatorio = new PopupRelatorio(documentoReport);
                _popupRelatorio.Show();
            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }
        public static string IMGtoSTR(BitmapSource img)
        {
            try
            {
                MemoryStream o = new MemoryStream();
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(img));
                png.Save(o);
                string str;
                str = Convert.ToBase64String(o.GetBuffer());
                o = null;
                png = null;
                return str;
            }
            catch (Exception)
            {
                return null;
            }


        }
        /// <summary>
        /// Salva imagem tipo BitmapSource
        /// </summary>
        /// <param BitmapSource="Imagem da WebCam">Documento do crystal report</param> 
        public static void SaveImageCapture(BitmapSource bitmap)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.QualityLevel = 100;


            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Image"; // Default file name
            dlg.DefaultExt = ".Jpg"; // Default file extension
            dlg.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save Image
                string filename = dlg.FileName;
                FileStream fstream = new FileStream(filename, FileMode.Create);
                encoder.Save(fstream);
                fstream.Close();
            }

        }
        #endregion
    }
}