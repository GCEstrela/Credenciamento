using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciais.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciais : Window
    {
        public PopUpFiltrosCredenciais()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
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
            Close();
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
            }
            ((RelatoriosViewModel)DataContext).OnFiltroRelatorioCredenciaisCommand(tipo, DataIni, DataFim);

            Close();
        }
    }
}
