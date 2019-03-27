using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosAutorizacoes.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoes : Window
    {
        public PopUpFiltrosAutorizacoes()
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
            int tipo;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;

            if (permanente_rb.IsChecked.Value)
            {
                tipo = 1;
            }
            else
                tipo = 2;

            ((RelatoriosViewModel)DataContext).OnFiltroRelatorioAutorizacoesCommand(tipo, DataIni, DataFim);

            Close();
        }
    }
}
