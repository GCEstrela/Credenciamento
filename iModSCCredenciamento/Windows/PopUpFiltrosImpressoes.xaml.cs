using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;


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


            ((RelatoriosViewModel)this.DataContext).OnFiltrosImpressoesCommand(empresa, area, check, DataIni, DataFim);

            this.Close();
        }
    }
}
