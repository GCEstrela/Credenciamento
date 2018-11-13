using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosAutorizacoes.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoes : Window
    {
        public PopUpFiltrosAutorizacoes()
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
            bool check;

            if (permanente_rb.IsChecked.Value)
            {
                check = true;
            }
            else
                check = false;

            ((RelatoriosViewModel)this.DataContext).OnFiltroRelatorioAutorizacoesCommand(check);

            this.Close();
        }
    }
}
