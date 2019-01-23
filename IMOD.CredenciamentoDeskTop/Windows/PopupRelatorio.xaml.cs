using System;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopupRelatorio.xaml
    /// </summary>
    public partial class PopupRelatorio : Window
    {
        public PopupRelatorio(ReportDocument reportDocument)
        {
            InitializeComponent();
            try
            {
                GenericReportViewer.ShowRefreshButton = false;
                GenericReportViewer.ShowToggleSidePanelButton = false;
                GenericReportViewer.ShowOpenFileButton = false;
                GenericReportViewer.ShowLogo = false;

                GenericReportViewer.ViewerCore.ReportSource = reportDocument;

            }
            catch (Exception ex)
            {

            }
        }
    }
}
