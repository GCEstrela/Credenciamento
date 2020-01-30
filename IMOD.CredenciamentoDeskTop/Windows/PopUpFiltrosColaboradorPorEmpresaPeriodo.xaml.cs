using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosColaboradorPorEmpresaPeriodo.xaml
    /// </summary>
    public partial class PopUpFiltrosColaboradorPorEmpresaPeriodo : Window
    {
        public PopUpFiltrosColaboradorPorEmpresaPeriodo(int tipoRelatorio)
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas();
            MouseDown += Window_MouseDown;
            lblTipoRelatorio.Content = tipoRelatorio;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            string empresa = null;
            string dataInicio = Dataini_tb.Text;
            string dataFim = Datafim_tb.Text;
            int tipoRelatorio = int.Parse(lblTipoRelatorio.Content.ToString());

            if (EmpresaRazaoSocial_cb.SelectedItem != null)
            {
                empresa = ((IMOD.CredenciamentoDeskTop.Views.Model.EmpresaView)EmpresaRazaoSocial_cb.SelectedItem).EmpresaId.ToString();
            }

            ((RelatoriosViewModel)DataContext).OnRelatorioFiltroColaboradorPorEmpresaCommand(empresa, null, null);

            Close();
        }
    }
}
