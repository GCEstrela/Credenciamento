// ***********************************************************************
// Project: iModSCCredenciamento
// Crafted by: Grupo Estrela by Genetec
// Date:  01 - 21 - 2019
// ***********************************************************************

#region

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.CrossCutting;

#endregion

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    ///     Interaction logic for PopupAutorizacao.xaml
    /// </summary>
    public partial class PopupAutorizacao : Window
    {
        private readonly ReportDocument Cracha = new ReportDocument();
        private bool firstPage;
        public bool Result;

        public PopupAutorizacao(ReportDocument reportDocument)
        {
            InitializeComponent();
            try
            {
                Cracha = reportDocument;

                GenericReportViewer.Background = Brushes.Transparent;
                GenericReportViewer.ShowSearchTextButton = false;
                GenericReportViewer.ShowExportButton = false;
                GenericReportViewer.ShowCopyButton = false;
                GenericReportViewer.ShowRefreshButton = false;
                GenericReportViewer.ShowToggleSidePanelButton = false;
                GenericReportViewer.ShowOpenFileButton = false;
                GenericReportViewer.ShowLogo = false;
                GenericReportViewer.ViewerCore.Zoom (150);
                GenericReportViewer.ViewerCore.ReportSource = reportDocument;
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
            MouseDown += Window_MouseDown;
        }

        #region  Metodos

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImprimirCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog1 = new PrintDialog();
                dialog1.AllowSomePages = true;
                dialog1.AllowPrintToFile = false;

                if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int copies = dialog1.PrinterSettings.Copies;
                    var fromPage = dialog1.PrinterSettings.FromPage;
                    var toPage = dialog1.PrinterSettings.ToPage;
                    var collate = dialog1.PrinterSettings.Collate;

                    dialog1.PrinterSettings.PrinterName = dialog1.PrinterSettings.PrinterName;
                    Cracha.PrintToPrinter (copies, collate, fromPage, toPage);
                    Result = true;
                    Close();
                }
                else
                {
                    Result = false;
                }

                //Cracha.Dispose();
                dialog1.Dispose();
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        private void ChangePage_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (firstPage)
                {
                    GenericReportViewer.ViewerCore.ShowFirstPage();
                    firstPage = false;
                }
                else
                {
                    GenericReportViewer.ViewerCore.ShowLastPage();
                    firstPage = true;
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException (ex);
            }
        }

        #endregion
    }
}