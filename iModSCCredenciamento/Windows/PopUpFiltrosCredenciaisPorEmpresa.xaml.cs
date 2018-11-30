using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisPorEmpresa.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisPorEmpresa : Window
    {
        public PopUpFiltrosCredenciaisPorEmpresa()
        {
            InitializeComponent();
            this.DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            string DataIni = Dataini_tb.Text;
            string DataFim = Datafim_tb.Text;
            bool check;
            string empresa;


            if (EmpresaRazaoSocial_cb.SelectedValue == null)
            {
                empresa = "";
            }
            else
                empresa = EmpresaRazaoSocial_cb.SelectedValue.ToString();


            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
            }
            else
                check = false;


            ((RelatoriosViewModel)this.DataContext).OnRelatorioFiltroPorEmpresaCommand(empresa, check, DataIni, DataFim);

            this.Close();
        }
    }
}
