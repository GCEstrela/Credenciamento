using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
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
            int status;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;

            //EXTRAVIADAS
            if (extraviadas_rb.IsChecked.Value)
            {
                status = 9;
            }
            //ROUBADAS
            else if (roubadas_rb.IsChecked.Value)
            {
                status = 10;
            }
            //DESTRUIDAS
            else if (destruidas_rb.IsChecked.Value)
            {
                status = 13;
            }
            //NÂO DEVOLVIDA
            else if (naodevolvidas_rb.IsChecked.Value)
            {
                status = 14;
            }
            //INDEFERIDAS
            else if (indeferidas_rb.IsChecked.Value)
            {
                status = 2;
            }
            //TUDO
            else
            {
                status = 0;
            }

            ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesInvalidasFiltroCommand(status, DataIni, DataFim);

            Close();
        }

       
    }
}
