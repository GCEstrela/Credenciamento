using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;
using IMOD.CredenciamentoDeskTop.Views.Model;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosEmpresaPorArea.xaml
    /// </summary>
    public partial class PopUpFiltrosEmpresaPorArea : Window
    {
        public PopUpFiltrosEmpresaPorArea()
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
            IMOD.CredenciamentoDeskTop.Views.Model.EmpresaAreaAcessoView objAreaSelecionado = null;
            if (AreaAcesso_cb.SelectedItem != null)
            {
             AreaAcessoView area = (AreaAcessoView)AreaAcesso_cb.SelectedItem;
             objAreaSelecionado = new IMOD.CredenciamentoDeskTop.Views.Model.EmpresaAreaAcessoView();
             objAreaSelecionado.Area = area.Identificacao;
            }

           ((RelatoriosViewModel)DataContext).OnRelatorioEmpresaPorAreaCommand(objAreaSelecionado);
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
