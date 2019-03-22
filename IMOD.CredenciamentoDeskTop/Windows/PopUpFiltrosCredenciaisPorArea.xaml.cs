using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisPorArea.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisPorArea : Window
    {
        public PopUpFiltrosCredenciaisPorArea()
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

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            bool check;
            string area;

            if (AreaAcesso_cb.SelectedItem == null)
            {
                area = "";
            }
            else
            {
                area = ((IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView)AreaAcesso_cb.SelectedItem).AreaAcessoId.ToString();
            }
                


            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
            }
            else
                check = false;

            ((RelatoriosViewModel)DataContext).OnRelatorioFiltroPorAreaCommand(area, check);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
