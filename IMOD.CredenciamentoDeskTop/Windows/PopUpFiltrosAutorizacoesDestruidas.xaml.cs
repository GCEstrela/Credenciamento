using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosAutorizacoesDestruidas.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoesDestruidas : Window
    {
        public PopUpFiltrosAutorizacoesDestruidas()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            List<int> status = new List<int> { 6, 8, 15 };
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciaisViaAdicionais(status);
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

            string dataIni = dp_dataInicial.Text; 
            string dataFim = dp_dataFinal.Text;
            
            IEnumerable<object> motivoCredencialSelecionados = new List<object>();

            if (lstMotivoCredencial.SelectedItems.Count > 0)
            {
                motivoCredencialSelecionados = (IEnumerable<object>)lstMotivoCredencial.SelectedItems;

                var teste = lstMotivoCredencial.SelectedItems;
            }

            var checkTipo =   (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value? false : true);

            bool flaTodasDevolucaoEntregue = (bool)RbtnTodasDevolucaoEntregue.IsChecked.Value;
            bool flaSimNaoDevolucaoEntregue = (bool)RbtnSimDevolucaoEntregue.IsChecked.Value ? true : (bool)RbtnNaoDevolucaoEntregue.IsChecked.Value ? false : true;


            ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesDestruidasFiltroCommand(checkTipo, motivoCredencialSelecionados, dataIni, dataFim, flaTodasDevolucaoEntregue, flaSimNaoDevolucaoEntregue);

            Close();
        }

       
    }
}
