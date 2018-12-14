using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using CrystalDecisions.CrystalReports.Engine;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupCredencial.xaml
    /// </summary>
    public partial class PopupCredencial : Window
    {
        public bool Result;
        ReportDocument Cracha = new ReportDocument();
        public PopupCredencial(ReportDocument reportDocument)
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
                // GenericReportViewer.ShowToolbar = false;
                GenericReportViewer.ShowOpenFileButton = false;
                GenericReportViewer.ShowLogo = false;
                GenericReportViewer.ViewerCore.Zoom(150);

                GenericReportViewer.ViewerCore.ReportSource = reportDocument;

            }
            catch (Exception ex)
            {

            }
            MouseDown += Window_MouseDown;
        }


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
                //Cracha report1 = new Cracha();
                PrintDialog dialog1 = new PrintDialog();


                //report1.SetDatabaseLogon("imod", "imod");

                dialog1.AllowSomePages = true;
                dialog1.AllowPrintToFile = false;

                if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int copies = dialog1.PrinterSettings.Copies;
                    int fromPage = dialog1.PrinterSettings.FromPage;
                    int toPage = dialog1.PrinterSettings.ToPage;
                    bool collate = dialog1.PrinterSettings.Collate;

                    Cracha.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
                    Cracha.PrintToPrinter(copies, collate, fromPage, toPage);
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

                //return null;
            }
        }
        bool firstPage;
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

               // throw;
            }
        }


    }
}
