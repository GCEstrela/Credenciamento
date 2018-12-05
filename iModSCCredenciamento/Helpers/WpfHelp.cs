// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  12 - 04 - 2018
// ***********************************************************************

#region

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iModSCCredenciamento.Funcoes;
using iModSCCredenciamento.Windows;
using System.Threading.Tasks;
using Utils = IMOD.CrossCutting.Utils;

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

                crConnectionInfo.ServerName = "172.16.190.108\\SQLEXPRESS";
                crConnectionInfo.DatabaseName = "D_iModCredenciamento";
                crConnectionInfo.UserID = "imod";
                crConnectionInfo.Password = "imod";

                CrTables = reportDoc.Database.Tables;
                foreach (Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                var tsk = Task.Factory.StartNew(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
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