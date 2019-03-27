using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisPorEmpresa.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisPorEmpresa : Window
    {
        public PopUpFiltrosCredenciaisPorEmpresa()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
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
            bool check;
            string empresa;

            if (EmpresaRazaoSocial_cb.SelectedItem == null)
            {
                empresa = "0";
            }
            else
            {
                empresa = ((IMOD.CredenciamentoDeskTop.Views.Model.EmpresaView)EmpresaRazaoSocial_cb.SelectedItem).EmpresaId.ToString();
            }

            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
                ((RelatoriosViewModel)DataContext).OnRelatorioFiltroCredencialPorEmpresaCommand(empresa, check, dataIni, dataFim);
            } 
            else
            {
                check = false; 
                ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesPorEmpresaCommand(empresa, check, dataIni, dataFim);
            } 


            Close();
        }
    }
}
