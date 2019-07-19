using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisExtraviadas.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisExtraviadas : Window
    {
        public PopUpFiltrosCredenciaisExtraviadas()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel(); 
            MouseDown += Window_MouseDown;
            List<int> status = new List<int> { 9, 10, 18 }; 
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciaisListaSelecionada(status); 
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

            string dataIni = dp_dataInicial.Text; 
            string dataFim = dp_dataFinal.Text; 

            bool flaTodasDevolucaoEntregue = (bool)RbtnTodasDevolucaoEntregue.IsChecked.Value;
            bool flaSimNaoDevolucaoEntregue = (bool)RbtnSimDevolucaoEntregue.IsChecked.Value ? true : (bool)RbtnNaoDevolucaoEntregue.IsChecked.Value ? false : true;
            
            var checkTipo = (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value ? false : true);

            if (lstMotivoCredencial.SelectedItems.Count > 0 )
            {
                motivoCredencialSelecionados = (IEnumerable<object>)lstMotivoCredencial.SelectedItems;

                var teste = lstMotivoCredencial.SelectedItems;
            }
           
            ((RelatoriosViewModel)DataContext).OnRelatorioCredenciaisExtraviadasFiltroCommand(checkTipo,
                                                                                            (IEnumerable<object>)motivoCredencialSelecionados, dataIni, dataFim,
                                                                                                                        flaTodasDevolucaoEntregue, flaSimNaoDevolucaoEntregue);

            Close();
        }
    }
}
