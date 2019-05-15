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
            ((RelatoriosViewModel)DataContext).CarregaColecaoAreasAcessos();
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
            IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView objAreaSelecionado = new IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView();

            if (AreaAcesso_cb.SelectedItem == null)
            {
                area = "0";
            }
            else
            {
                area = ((IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView)AreaAcesso_cb.SelectedItem).AreaAcessoId.ToString();
                objAreaSelecionado = (IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView)AreaAcesso_cb.SelectedItem;
            }

            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
                ((RelatoriosViewModel)DataContext).OnRelatorioCredencialPorAreaCommand(area, check, objAreaSelecionado);
            }
            else
            {
                check = false;
                ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesPorAreaCommand(area, check, objAreaSelecionado);
            }


            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
