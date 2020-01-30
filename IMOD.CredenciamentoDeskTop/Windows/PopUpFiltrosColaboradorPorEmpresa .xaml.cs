using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosColaboradorPorEmpresa.xaml
    /// </summary>
    public partial class PopUpFiltrosColaboradorPorEmpresa : Window
    {
        public PopUpFiltrosColaboradorPorEmpresa()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas();
            MouseDown += Window_MouseDown;
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
            string dataIni = Dataini_tb.Text;
            string dataFim = Datafim_tb.Text;            
            string empresa = null;

            if (EmpresaRazaoSocial_cb.SelectedItem != null)
            {
                empresa = ((IMOD.CredenciamentoDeskTop.Views.Model.EmpresaView)EmpresaRazaoSocial_cb.SelectedItem).EmpresaId.ToString();
            }

            ((RelatoriosViewModel)DataContext).OnRelatorioFiltroColaboradorPorEmpresaCommand(empresa, dataIni, dataFim);

            Close();
        }
    }
}
