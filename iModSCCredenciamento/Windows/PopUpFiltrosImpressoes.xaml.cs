using System.Windows;
using System.Windows.Input;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosImpressoes.xaml
    /// </summary>
    public partial class PopUpFiltrosImpressoes : Window
    {
        public PopUpFiltrosImpressoes()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            EmpresaRazaoSocial_cb.Focus();
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
            string DataIni = Dataini_tb.Text;
            string DataFim = Datafim_tb.Text;
            bool check;
            string empresa, area;

            if (Area_cb.SelectedValue == null)
            {
                area = "";
            }
            else
                area = Area_cb.SelectedValue.ToString();

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


            ((RelatoriosViewModel)DataContext).OnFiltrosImpressoesCommand(empresa, area, check, DataIni, DataFim);

            Close();
        }
    }
}
