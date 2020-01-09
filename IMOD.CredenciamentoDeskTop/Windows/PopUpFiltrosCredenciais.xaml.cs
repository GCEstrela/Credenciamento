using System.Collections.Generic;
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
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas();
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(1);//Carregar os motivos do status 2 - ativo 
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
            IEnumerable<object> motivoCredencialSelecionados = new List<object>();

            bool tipo;
            string DataIni = dp_dataInicial.Text;
            string DataFim = dp_dataFinal.Text;
            string empresa;

            if (permanente_rb.IsChecked.Value)
            {
                tipo = true;
            }
            else
            {
                tipo = false;
            }

            if (lstMotivoCredencial.SelectedItems.Count > 0)
            {
                motivoCredencialSelecionados = (IEnumerable<object>)lstMotivoCredencial.SelectedItems;

                var teste = lstMotivoCredencial.SelectedItems;
            }

            if (EmpresaRazaoSocial_cb.SelectedItem == null)
            {
                empresa = "0";
            }
            else
            {
                empresa = ((IMOD.CredenciamentoDeskTop.Views.Model.EmpresaView)EmpresaRazaoSocial_cb.SelectedItem).EmpresaId.ToString();
            }

            ((RelatoriosViewModel)DataContext).OnFiltroRelatorioCredenciaisCommand(tipo, empresa, (IEnumerable<object>)motivoCredencialSelecionados, DataIni, DataFim);

            Close();
        }
    }
}
