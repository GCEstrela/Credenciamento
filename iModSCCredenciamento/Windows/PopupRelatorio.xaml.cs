using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
