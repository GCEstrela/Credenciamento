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
            var checkTipo = (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value ? false : true);
            bool? flaAtivoInativo;

            if (RbtnTodosStatus != null && RbtnTodosStatus.IsChecked.Value)
            {
                flaAtivoInativo = null;
            } else
            {
               flaAtivoInativo = (bool)RbtnStatusAtiva.IsChecked.Value ? true : (bool)RbtnStatusInativa.IsChecked.Value ? false : true;
            }

            if (AreaAcesso_cb.SelectedItem == null)
            {
                area = "";
            }
            else
            {
                area = ((IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView)AreaAcesso_cb.SelectedItem).AreaAcessoId.ToString();
                objAreaSelecionado = (IMOD.CredenciamentoDeskTop.Views.Model.AreaAcessoView)AreaAcesso_cb.SelectedItem;
            }

            if (credenciais_rb.IsChecked.Value)
            {
                check = true;
                ((RelatoriosViewModel)DataContext).OnRelatorioCredencialPorAreaCommand(checkTipo, area, check, objAreaSelecionado, flaAtivoInativo);
            }
            else
            {
                check = false;
                ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesPorAreaCommand(checkTipo, area, check, objAreaSelecionado, flaAtivoInativo);
            }


            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
