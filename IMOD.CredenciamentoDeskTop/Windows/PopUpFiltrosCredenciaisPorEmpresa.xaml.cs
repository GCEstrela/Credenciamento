using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisPorEmpresa.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisPorEmpresa : Window
    {
        public PopUpFiltrosCredenciaisPorEmpresa()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas();
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
            string dataIni = Dataini_tb.Text;
            string dataFim = Datafim_tb.Text;
            bool check;
            string empresa;
            bool? flaAtivoInativo;

            var checkTipo = (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value ? false : true);
            var checkEmissao = RbtnEmissao.IsChecked.Value ? true : false;
            var checkValidade = RbtnValidade.IsChecked.Value ? true : false;

            if (RbtnTodasAtivosInativos != null && RbtnTodasAtivosInativos.IsChecked.Value)
            {
                flaAtivoInativo = null;
            }
            else
            {
                flaAtivoInativo = (bool)RbtnAtivos.IsChecked.Value ? true : (bool)RbtnInativos.IsChecked.Value ? false : true;
            }

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
                ((RelatoriosViewModel)DataContext).OnRelatorioFiltroCredencialPorEmpresaCommand(checkTipo, empresa, check, dataIni, dataFim, checkEmissao, checkValidade, flaAtivoInativo);
            } 
            else
            {
                check = false; 
                ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesPorEmpresaCommand(checkTipo, empresa, check, dataIni, dataFim, checkEmissao, checkValidade, flaAtivoInativo);
            } 


            Close();
        }
    }
}
