using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisPorArea.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisPorArea : Window
    {
        public PopUpFiltrosCredenciaisPorArea()
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

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            bool check;
            string area;

            if (AreaAcesso_cb.SelectedValue == null)
            {
                area = "";
            }
            else
                area = AreaAcesso_cb.SelectedValue.ToString();


            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
            }
            else
                check = false;

            ((RelatoriosViewModel)this.DataContext).OnRelatorioFiltroPorAreaCommand(area, check);

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
