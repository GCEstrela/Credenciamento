using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisInvalidas.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisInvalidas : Window
    {
        public PopUpFiltrosCredenciaisInvalidas()
        {
            InitializeComponent();
            this.DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            int status;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;


            //EXTRAVIADAS
            if (extraviadas_rb.IsChecked.Value)
            {
                status = 1;
            }
            //ROUBADAS
            else if (roubadas_rb.IsChecked.Value)
            {
                status = 2;
            }
            //DESTRUIDAS
            else if (destruidas_rb.IsChecked.Value)
            {
                status = 3;
            }
            //NÂO DEVOLVIDA
            else if (naodevolvidas_rb.IsChecked.Value)
            {
                status = 4;
            }
            //INDEFERIDAS
            else if (indeferidas_rb.IsChecked.Value)
            {
                status = 5;
            }
            //TUDO
            else
            {
                status = 0;
            }

            ((RelatoriosViewModel)this.DataContext).OnRelatorioCredenciaisInvalidasFiltroCommand(status, DataIni, DataFim);

            this.Close();
        }
    }
}
