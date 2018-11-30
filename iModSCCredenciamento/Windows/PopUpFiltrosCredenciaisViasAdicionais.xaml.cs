using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Interaction logic for PopUpFiltrosCredenciaisViasAdicionais.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisViasAdicionais : Window
    {
        public PopUpFiltrosCredenciaisViasAdicionais()
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

        private void button_ClickFiltrar(object sender, RoutedEventArgs e)
        {
            int tipo = 0;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;

            if (segundaemissao_rb.IsChecked.Value)
            {
                tipo = 2;
            }
            else if (terceiraemissao_rb.IsChecked.Value)
            {
                tipo = 3;
            }
            else
            {
                tipo = 0;
            }

            ((RelatoriosViewModel)this.DataContext).OnFiltroCredencialViasAdicionaisCommand(tipo, DataIni, DataFim);


            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
