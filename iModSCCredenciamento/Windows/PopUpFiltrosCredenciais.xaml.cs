using iModSCCredenciamento.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace iModSCCredenciamento.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciais.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciais : Window
    {
        public PopUpFiltrosCredenciais()
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
            bool tipo;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;

            if (permanente_rb.IsChecked.Value)
            {
                tipo = true;
            }
            else
            {
                tipo = false;
            } ((RelatoriosViewModel)this.DataContext).OnFiltroRelatorioCredencialCommand(tipo, DataIni, DataFim);

            this.Close();
        }
    }
}
