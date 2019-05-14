using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosImpressoes.xaml
    /// </summary>
    public partial class PopUpFiltrosImpressoes : Window
    {
        public PopUpFiltrosImpressoes()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas(); 
            ((RelatoriosViewModel)DataContext).CarregaColecaoAreasAcessos();
            MouseDown += Window_MouseDown;

            EmpresaRazaoSocial_cb.IsEditable = true;
            EmpresaRazaoSocial_cb.Text = "--IMPRESSÕES DE TODAS EMPRESAS--";
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
            string dataIni = Dataini_tb.Text;
            string dataFim = Datafim_tb.Text;
            bool check;
            string empresa;

            if (EmpresaRazaoSocial_cb.SelectedItem == null)
            {
                empresa = "0";
            }
            else
            {
                empresa = ((IMOD.CredenciamentoDeskTop.Views.Model.EmpresaView)EmpresaRazaoSocial_cb.SelectedItem).EmpresaId.ToString();
            }

            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
                ((RelatoriosViewModel)DataContext).OnFiltrosColaboradorCredencialImpressoesCommand(empresa, check, dataIni, dataFim);
            }
            else
            {
                check = false;
                ((RelatoriosViewModel)DataContext).OnFiltrosImpressoesAutorizacoesCommand(empresa, check, dataIni, dataFim);
            }

            Close();
        }
    }
}
