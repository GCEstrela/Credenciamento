using System.Windows;
using System.Windows.Input;
using IMOD.CredenciamentoDeskTop.ViewModels;

namespace IMOD.CredenciamentoDeskTop.Windows
{
    /// <summary>
    /// Lógica interna para PopUpFiltrosAutorizacoesInvalidas.xaml
    /// </summary>
    public partial class PopUpFiltrosAutorizacoesInvalidas : Window
    {
        public PopUpFiltrosAutorizacoesInvalidas()
        {
            InitializeComponent();
            DataContext = new RelatoriosViewModel();
            MouseDown += Window_MouseDown;
            ((RelatoriosViewModel)DataContext).CarregaMotivoCredenciais(2);//Carregar os motivos do status 2 - inativo 
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
            int status = 0;
            IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView motivoCredencialSelecionado = null; 
            string dataIni = dp_dataInicial.Text; 
            string dataFim = dp_dataFinal.Text;
            bool flaDevolucaoEntregue = (bool)chkDevolucaoEntregue.IsChecked;

            if (lstMotivoCredencial.SelectedItem != null)
            {
                motivoCredencialSelecionado = (IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem;
                status = ((IMOD.CredenciamentoDeskTop.Views.Model.CredencialMotivoView)lstMotivoCredencial.SelectedItem).CredencialMotivoId;
            }

             var checkTipo =   (RbtnPermanente.IsChecked.Value ? true : RbtnTemporario.IsChecked.Value? false : true); 

            ((RelatoriosViewModel)DataContext).OnRelatorioAutorizacoesInvalidasFiltroCommand(checkTipo, status, motivoCredencialSelecionado, dataIni, dataFim, flaDevolucaoEntregue);

            Close();
        }

       
    }
}
