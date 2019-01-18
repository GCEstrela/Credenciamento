using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CrystalDecisions.CrystalReports.Engine;
using IMOD.CrossCutting;
using PrintDialog = System.Windows.Forms.PrintDialog;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interaction logic for PopupAutorizacao.xaml
    /// </summary>
    public partial class PopupAutorizacao : Window
    {
        public bool Result;
        ReportDocument Cracha = new ReportDocument();

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
                // GenericReportViewer.ShowToolbar = false;
                GenericReportViewer.ShowOpenFileButton = false;
                GenericReportViewer.ShowLogo = false;
                GenericReportViewer.ViewerCore.Zoom(150);

                GenericReportViewer.ViewerCore.ReportSource = reportDocument;

            }
            catch (Exception ex)
            {
                Utils.TraceException(ex);
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
                System.Windows.Forms.PrintDialog dialog1 = new PrintDialog();


                //report1.SetDatabaseLogon("imod", "imod");

                dialog1.AllowSomePages = true;
                dialog1.AllowPrintToFile = false;

                if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int copies = dialog1.PrinterSettings.Copies;
                    int fromPage = dialog1.PrinterSettings.FromPage;
                    int toPage = dialog1.PrinterSettings.ToPage;
                    bool collate = dialog1.PrinterSettings.Collate;

                    dialog1.PrinterSettings.PrinterName = dialog1.PrinterSettings.PrinterName;

                    //Cracha.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
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
                Utils.TraceException(ex);
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
                Utils.TraceException(ex);
            }
        }
    }
}
