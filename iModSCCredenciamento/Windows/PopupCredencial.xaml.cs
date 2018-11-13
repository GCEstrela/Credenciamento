using CrystalDecisions.CrystalReports.Engine;
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

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopupCredencial.xaml
    /// </summary>
    public partial class PopupCredencial : Window
    {
        public bool Result = false;
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
            this.Close();
        }

        private void ImprimirCredencial_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cracha report1 = new Cracha();
                System.Windows.Forms.PrintDialog dialog1 = new System.Windows.Forms.PrintDialog();


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
                    this.Close();
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
        bool firstPage = false;
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
