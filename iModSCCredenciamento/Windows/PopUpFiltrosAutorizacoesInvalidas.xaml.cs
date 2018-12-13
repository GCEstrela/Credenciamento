using System.Windows;
using System.Windows.Input;
using iModSCCredenciamento.ViewModels;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosAutorizacoesInvalidas.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoesInvalidas : Window
    {
        public PopUpFiltrosAutorizacoesInvalidas()
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
            int check;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;

            //NAO DEVOLVIDAS
            if (naodevolvidas_rb.IsChecked.Value)
            {
                check = 0;
            }
            //INDEFERIDAS
            else if (indeferidas_rb.IsChecked.Value)
            {
                check = 3;
            }
            //EXTRAVIADAS
            else if (extraviadas_rb.IsChecked.Value)
            {
                check = 4;
            }
            //DESTRUIDAS
            else if (destruidas_rb.IsChecked.Value)
            {
                check = 6;
            }
            //TUDO
            else
            {
                check = 10;
            }

            ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesInvalidasFiltroCommand(check, DataIni, DataFim);

            Close();
        }
    }
}
