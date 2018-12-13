using System;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;

namespace iModSCCredenciamento.Windows
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

                //var sidepanel = GenericReportViewer.FindName("btnToggleSidePanel") as ToggleButton;
                //if (sidepanel != null)
                //{
                //    GenericReportViewer.ViewChange += (x, y) => sidepanel.IsChecked = false;
                //}
                // GenericReportViewer.ShowStatusbar = false;
                GenericReportViewer.ShowRefreshButton = false;
                GenericReportViewer.ShowToggleSidePanelButton = false;
               // GenericReportViewer.ShowToolbar = false;
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
