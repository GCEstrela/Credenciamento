// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 06 - 2018
// ***********************************************************************

#region

using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iModSCCredenciamento.Windows;
using IMOD.CrossCutting;
using IMOD.Infra.Ado;
using Application = System.Windows.Application;
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
        /// ConnectionString
        /// </summary>
        public static SqlConnectionStringBuilder db = new SqlConnectionStringBuilder(CurrentConfig.ConexaoString);

        /// <summary>
        ///     Caixa de Menssagem Customizada
        /// </summary>
        /// <param name="msg">Mensagem</param>
        public static void Mbox(string msg)
        {
            Mbox(msg, MessageBoxIcon.Information);
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
            var detalhe = string.IsNullOrWhiteSpace(innerMsg) ? string.Empty : $"\nDetalhe: {innerMsg}";
            Mbox("Um erro ocorreu.\nRazão: " + ex.Message + detalhe, MessageBoxIcon.Error);
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
            var detalhe = string.IsNullOrWhiteSpace(innerMsg) ? string.Empty : $"\nDetalhe: {innerMsg}";
            Mbox(msg + "\nRazão: " + ex.Message + detalhe, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     Caixa de Menssagem Customizada
        /// </summary>
        /// <param name="msg">Mensagem</param>
        /// <param name="pIcon">Um icone para apresentacao</param>
        public static void Mbox(string msg, MessageBoxIcon pIcon)
        {
            MessageBox.Show(msg, Caption(pIcon), MessageBoxButtons.OK, pIcon, MessageBoxDefaultButton.Button1);
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
            var result = MessageBox.Show(msg, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question, button);
            return result;
        }

        /// <summary>
        /// Exibe Mensagem Solicitando Confirmação Sim ou Não para remoção de dados
        /// </summary>
        /// <returns></returns>
        public static DialogResult MboxDialogRemove()
        {
            return MboxDialogYesNo ("Deseja realmente excluir o item?", false);
        }

        /// <summary>
        ///     Mensagem por Popup
        /// </summary>
        /// <param name="msg">Mensagem a ser exibida</param>
        /// <param name="icone">1-Informação, 2-Interrogação, 3-Exclamação, 4-Proibido</param>
        /// <returns></returns>
        public static bool PopupBox(string msg, int icone)
        {
            var popupBox = new PopupBox(msg, icone);
            popupBox.ShowDialog();
            return popupBox.Result;
        }

        public static bool PopupBox(Exception ex)
        {
            var msg = $"Não foi possível executar a operação solicitada.\n{ex.Message}";
            return PopupBox(msg,3);
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
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var result = openFileDialog.ShowDialog();
            if (result != true) return null;

            var path = openFileDialog.FileName;
            var tamBytes = new FileInfo(path).Length;
            var tam = decimal.Divide(tamBytes, 1000);
            var arq = new ArquivoInfo
            {
                Nome = openFileDialog.SafeFileName
            };

            if (tamMax == 0)
            {
                arq.ArrayBytes = File.ReadAllBytes(path);
                arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);
                return arq;
            }


            if (tamMax < tam)
                throw new Exception($"{tamMax} Kbytes é o tamanho máximo permitido para upload.");

            arq.ArrayBytes = File.ReadAllBytes(path);
            arq.FormatoBase64 = Convert.ToBase64String(arq.ArrayBytes);
            return arq;


        }

        /// <summary>
        /// Abrir arquivo 
        /// </summary>
        /// <param name="caminho">Caminho do arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        public static void AbrirArquivo(string caminho, string nomeArquivo)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(caminho)) throw  new ArgumentNullException(nameof(caminho));
                if (string.IsNullOrWhiteSpace(caminho)) throw new ArgumentNullException(nameof(nomeArquivo));
                var path = Path.Combine (caminho, nomeArquivo);
                Process.Start(path);
            }
            catch (Exception ex)
            {
                   Utils.TraceException(ex);
            }
        }

        /// <summary>
        /// Abrir arquivo Pdf dentro do programa associado à extensão .pdf
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="arrayBytes"></param>
        public static void AbrirArquivoPdf(string nomeArquivo, byte[] arrayBytes)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeArquivo)) return;
                if (arrayBytes == null) return;
                string fileName = "";
                var tempArea = Path.GetTempPath(); 
                fileName = Path.ChangeExtension(nomeArquivo, ".pdf");
                var path = Path.Combine(tempArea, fileName);
                File.WriteAllBytes(path, arrayBytes);//Save file on temp area

                var tsk = Task.Factory.StartNew(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    { 
                        Process.Start(path);
                    });
                });

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
            if (string.IsNullOrWhiteSpace(nomeArquivo)) return;
            if (arrayBytes == null) return;

            var dlgSave = new SaveFileDialog();
            dlgSave.FileName = nomeArquivo; //Nome do arquivo
            //Obter extensão do arquivo, caso exista
            var ext = string.IsNullOrWhiteSpace(nomeArquivo) ? "" : nomeArquivo;
            if (ext.Contains("."))
            {
                ext = ext.Substring(ext.IndexOf(".", StringComparison.CurrentCulture));
                dlgSave.DefaultExt = ext; //Extensão
                dlgSave.Filter = $"documents ({ext})|*{ext}";
            }
            //Usuario decidindo salvar...
            if (dlgSave.ShowDialog() == DialogResult.OK)
                File.WriteAllBytes(dlgSave.FileName, arrayBytes);
        }

        /// <summary>
        /// Exibir o relatório
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo</param>
        /// <param name="arrayBytes">Array de Bytes relativo ao arquivo</param>
        /// <param name="formula">Formula do relatório</param>
        public static void ShowRelatorio(byte[] arrayBytes, string nomeArquivo, string formula, string mensagem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomeArquivo)) return;
                if (arrayBytes == null) return;

                var tempArea = Path.GetTempPath();

                var idx = nomeArquivo.IndexOf(".", StringComparison.Ordinal);
                var nameFile = idx == -1 ? nomeArquivo : nomeArquivo.Substring(idx);

                var fileName = Path.Combine(tempArea, $"{nameFile}.rpt");
                File.WriteAllBytes(fileName, arrayBytes);//Save file on temp area

                var reportDoc = new ReportDocument();
                reportDoc.Load(fileName, OpenReportMethod.OpenReportByTempCopy);

                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                Tables CrTables;

                crConnectionInfo.ServerName = db.DataSource;
                crConnectionInfo.DatabaseName = db.InitialCatalog;
                crConnectionInfo.UserID = db.UserID;
                crConnectionInfo.Password = db.Password;

                CrTables = reportDoc.Database.Tables;
                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                var tsk = Task.Factory.StartNew(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        //your code here, formulas...
                        if (!string.IsNullOrWhiteSpace(formula))
                        {
                            reportDoc.RecordSelectionFormula = formula;
                        }
                        if (!string.IsNullOrWhiteSpace(mensagem))
                        {
                            TextObject txt = (TextObject)reportDoc.ReportDefinition.ReportObjects["TextoPrincipal"];
                            txt.Text = mensagem;
                        }

                        var _popupRelatorio = new PopupRelatorio(reportDoc);
                        _popupRelatorio.Show();
                    });
                });

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
            }
        }


        #endregion
    }
}