using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisDestruidas.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisDestruidas : Window
    {
        public PopUpFiltrosCredenciaisDestruidas()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(2);//Carregar os motivos do status 2 - inativo 
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
            bool flaDevolucaoEntregue = (bool)chkDevolucaoEntregue.IsChecked; 

            var checkTipo = (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value ? false : true);

            if (lstMotivoCredencial.SelectedItems.Count > 0 )
            {
                motivoCredencialSelecionados = (IEnumerable<object>)lstMotivoCredencial.SelectedItems;

                var teste = lstMotivoCredencial.SelectedItems;
            }
           
            ((RelatoriosViewModel)DataContext).OnRelatorioCredenciaisDestruidasFiltroCommand(checkTipo,
                                                                                            (IEnumerable<object>)motivoCredencialSelecionados, dataIni, dataFim, 
                                                                                                                        flaDevolucaoEntregue);

            Close();
        }
    }
}
