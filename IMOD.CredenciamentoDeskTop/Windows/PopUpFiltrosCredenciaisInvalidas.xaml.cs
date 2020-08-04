using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosCredenciaisInvalidas.xaml
    /// </summary>
    public partial class PopUpFiltrosCredenciaisInvalidas : Window
    {
        public PopUpFiltrosCredenciaisInvalidas()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(2);//Carregar os motivos do status 2 - inativo 
            ((RelatoriosViewModel)DataContext).CarregaColecaoEmpresas();
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
            string empresa;
            int tipodeData = 0;

            bool flaTodasDevolucaoEntregue = (bool)RbtnTodasDevolucaoEntregue.IsChecked.Value;
            bool flaSimNaoDevolucaoEntregue = (bool)RbtnSimDevolucaoEntregue.IsChecked.Value ? true : (bool)RbtnNaoDevolucaoEntregue.IsChecked.Value ? false : true;
            
            var checkTipo = (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value ? false : true);

            if (lstMotivoCredencial.SelectedItems.Count > 0 )
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
            if (DataValidade_rb.IsChecked == true)
            {
                tipodeData = 1;
            }

            ((RelatoriosViewModel)DataContext).OnRelatorioCredenciaisInvalidasFiltroCommand(checkTipo, empresa,
                                                                                            (IEnumerable<object>)motivoCredencialSelecionados, dataIni, dataFim,
                                                                                                                        flaTodasDevolucaoEntregue, flaSimNaoDevolucaoEntregue, tipodeData);

            Close();
        }
    }
}
