using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Interaction logic for PopUpFiltrosCredenciaisViasAdicionais.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisViasAdicionais : Window
    {
        public PopUpFiltrosCredenciaisViasAdicionais()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciaisViaAdicional(true);
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas();
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
            IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView motivoCredencialSelecionado = null;
            string dataIni = dp_dataInicial.Text;  
            string dataFim = dp_dataFinal.Text;
            string empresa;
            bool? flaAtivoInativo;
            IEnumerable<object> motivoCredencialSelecionados = new List<object>();

            if (RbtnTodosStatus != null && RbtnTodosStatus.IsChecked.Value)
            {
                flaAtivoInativo = null;
            }
            else
            {
                flaAtivoInativo = (bool)RbtnStatusAtiva.IsChecked.Value ? true : (bool)RbtnStatusInativa.IsChecked.Value ? false : true;
            }

            if (EmpresaRazaoSocial_cb.SelectedItem == null)
            {
                empresa = "0";
            }
            else
            {
                empresa = ((IMOD.CredenciamentoDeskTop.Views.Model.EmpresaView)EmpresaRazaoSocial_cb.SelectedItem).EmpresaId.ToString();
            }

            var checkTipo = (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value ? false : true);

            if (lstMotivoCredencial.SelectedItems.Count > 0)
            {
                motivoCredencialSelecionados = (IEnumerable<object>)lstMotivoCredencial.SelectedItems;

                var teste = lstMotivoCredencial.SelectedItems;
            }

            ((RelatoriosViewModel)DataContext).OnFiltroCredencialViasAdicionaisCommand(checkTipo, empresa, motivoCredencialSelecionados, dataIni, dataFim, flaAtivoInativo);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
